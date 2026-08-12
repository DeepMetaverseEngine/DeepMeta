const Service = require('egg').Service;
var crypto = require('crypto');

class XiaomiService extends Service {
	async parseOrder(params) {
		const ctx = this.ctx;
		ctx.logger.info(params)
		var orderId = params.orderId;
		var cpOrderId = params.cpOrderId;
		var sign = params.signature;
		var orderStatus = params.orderStatus;
		var payFee = params.payFee;
		if(!sign || !cpOrderId){
			ctx.status = 403;
			return 'Go away, robot.';
		}

		try{
			var order = await ctx.model.Order.findByOrderId(cpOrderId);
			if(ctx.helper.is_empty(order)){
				this.logger.error('order not exist orderId=' + orderId + ' cpOrderId=' + cpOrderId+' details=' + JSON.stringify(params))
				return {errcode:1506};
			}
			else {
				var signString =  this.sortParams(params);
				var local_sign = this.getSignature(signString ,order.channel.api_key)
				if(local_sign != sign){
					this.logger.error('signature verification failed. orderId=' + orderId + ' cpOrderId=' + cpOrderId+' details=' + JSON.stringify(params))
					return {errcode:1525};
				}

				if(order.status == ctx.app.orderStatus.Create 
					|| order.status == ctx.app.orderStatus.PaySuccess
					|| order.status == ctx.app.orderStatus.OrderInvalid){
					var status = orderStatus == 'TRADE_SUCCESS' ? ctx.app.orderStatus.OrderValid : ctx.app.orderStatus.OrderInvalid;
					await order.updateOrder(orderId, status, payFee);
				}
				if(orderStatus == 'TRADE_SUCCESS'){
					try{
						await this.orderStatusChangeNotify(order);
					}catch(err){
						this.logger.info("send orderStatusChangeNotify failed. remote not responding")
						this.logger.error(err);
					}
					
				}
				return {errcode:200};
			}

		}catch (err){
			 this.logger.error(err);
			 return {errcode:1000};
		}
	}

	async queryOrder(orders) {
		var valid_orders = {}
		for ( var i = 0; i <orders.length; i++){
			if(this.ctx.helper.getOrderValid(orders[i])){
				this.ctx.helper.object_add(valid_orders, orders[i].role_id, orders[i].cp_order_id)
			}else{
				if(await this.getNetOrderValid(orders[i])){
					this.ctx.helper.object_add(valid_orders, orders[i].role_id, orders[i].cp_order_id)
				}
			}
		}
		return valid_orders;
	}

	async getNetOrderValid(order){
		var params = {
	        appId: order.channel.game_id,
	        uid: order.platform_account,
	        cpOrderId: order.cp_order_id,
      	}
      	var code = 0
      	try{
	      	var signString = this.sortParams(params)
	      	var signature = this.getSignature(signString ,order.channel.api_key)
			const result = await this.ctx.curl(order.channel.query_url + '?' + signString + "&signature=" + signature, {
		      dataType: 'json',
		      timeout: 10000,
		    });
		    this.logger.info('queryOrder '+ order.cp_order_id + ' response: ' + JSON.stringify(result.data))
		    if(result.data.errcode){
		    	return false;
		    }else {
		    	if(result.data.orderStatus == 'TRADE_SUCCESS'){
		    		return true;
		    	}
		    	return false;
		    }
	    }catch(err){
	    	this.logger.error(err);
	    }
	}

	async orderStatusChangeNotify(order){
		const ctx = this.ctx;
		const realm = await this.service.realmselector.get_realm_by_id(order.realm_id)
		if(this.ctx.helper.is_empty(realm)){
			this.logger.error('realm not exist realm_id=' + order.realm_id + ' send notification failed.');
			return;
		}
		var orders = {};
		orders[order.role_id] = [order.cp_order_id];
		var command = {
	        code: 1,
	        order_list:orders
      	}
		var params = this.service.gmt.generate_cmd(command, realm.gmt_key);
	    const result = await ctx.curl(realm.pay_url + params, {
	      method: 'POST',
	      timeout: 3000,
	    });
	    this.logger.info(result.data.toString())
	}

	//参数排序
	sortParams(obj)
	{
		var sorted_keys = Object.keys(obj).sort();
		var sorted_signMap = {};
		var signedStr = '';
		for(var i=0;i<sorted_keys.length;i++){
			sorted_signMap[sorted_keys[i]] = obj[sorted_keys[i]]
			if(obj[sorted_keys[i]] !=='undefined' && sorted_keys[i] != 'signature'){
				signedStr += sorted_keys[i] + '=' + sorted_signMap[sorted_keys[i]] + '&';
			}
		}

		if(signedStr.endsWith('&')){
			signedStr = signedStr.substring(0,signedStr.length-1);
		}
		return signedStr;
	}

	getSignature(params, apiKey) {
		return crypto.createHmac('sha1', apiKey).update(params).digest('hex');
	}
}

module.exports = XiaomiService;