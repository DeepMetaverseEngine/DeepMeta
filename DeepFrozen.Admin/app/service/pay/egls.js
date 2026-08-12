const Service = require('egg').Service;
var crypto = require('crypto');

class EglsService extends Service {
	async parseOrder(params) {
		const ctx = this.ctx;
		ctx.logger.info(params)
		var orderId = params.order;
		var cpOrderId = params.cpOrder;
		var currency = params.currency;
		var sign = params.sign;
		var payFee = params.money * 100;
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
					await order.updateOrder(orderId, ctx.app.orderStatus.OrderValid, payFee, currency);
				}

				order.status = ctx.app.orderStatus.OrderValid
				await order.save()

				try{
					await this.orderStatusChangeNotify(order);
				}catch(err){
					this.logger.error('send orderStatusChangeNotify failed. orderId=' + orderId + ' cpOrderId=' + cpOrderId + 'remote not responding')
					this.logger.error(err);
				}
				return "success";
			}

		}catch (err){
			 this.logger.error(err);
			 return {errcode:1000};
		}
	}

	async reSendOrder(params) {
		var order = await this.ctx.model.Order.findByOrderId(params.order_id);
		if(this.ctx.helper.is_empty(order)){
			this.logger.error('reSendOrder  order not exist.' + params.order_id)
			return {state:false, reason:'order not exist.'}
		}

		try{
			await this.orderStatusChangeNotify(order);
		}catch(err){
			this.logger.error(err);
			return {state:false, reason:'send orderStatusChangeNotify failed.'}
		}
		return {state:true, reason:'success.'}

	}


	async orderStatusChangeNotify(order){
		const ctx = this.ctx;
		const realm = await this.service.realmselector.get_realm_by_id(order.realm_id)
		if(this.ctx.helper.is_empty(realm)){
			this.logger.error('realm not exist realm_id =' + order.realm_id + ' send notification failed.');
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
			if(obj[sorted_keys[i]] !=='undefined' && sorted_keys[i] != 'sign'){
				signedStr += sorted_signMap[sorted_keys[i]] + '&';
			}
		}

		if(signedStr.endsWith('&')){
			signedStr = signedStr.substring(0,signedStr.length-1);
		}
		return signedStr;
	}

	getSignature(params, apiKey) {
		return crypto.createHash('md5').update(params + apiKey).digest("hex").toUpperCase();
	}
}

module.exports = EglsService;