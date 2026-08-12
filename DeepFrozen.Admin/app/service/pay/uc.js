const Service = require('egg').Service;
var crypto = require('crypto');

class UcService extends Service {
	async parseOrder(params) {
		const ctx = this.ctx;
		ctx.logger.info(params)
		var data = params.data;
		var sign = params.sign;
		if(!sign || !data){
			ctx.status = 403;
			return 'Go away, robot.';
		}

		try{
			var orderId = data.orderId;
			var gameId = data.gameId;
			var accountId = data.accountId;
			var creator = data.creator;
			var amount = data.amount * 100;//单位元
			var callbackInfo = data.callbackInfo;
			var orderStatus = data.orderStatus;
			var failedDesc = data.failedDesc;
			var cpOrderId = data.cpOrderId;

			if(cpOrderId){
				var order = await ctx.model.Order.findByOrderId(cpOrderId);
				if(ctx.helper.is_empty(order)){
					this.logger.error('order not exist orderId=' + orderId + ' cpOrderId=' + cpOrderId+' details=' + JSON.stringify(params))
					return 'FAILURE';
				}
				else {
					var local_sign = this.signatureTool(params.data,order.channel.api_key);
					if(local_sign != sign){
						this.logger.error('signature verification failed. orderId=' + params.data.orderId + ' cpOrderId=' + params.data.cpOrderId+' details=' + JSON.stringify(params))
						return 'FAILURE';
					}
					
					if(order.status == ctx.app.orderStatus.Create 
						|| order.status == ctx.app.orderStatus.PaySuccess
						|| order.status == ctx.app.orderStatus.OrderInvalid){
						var status = orderStatus == 'S' ? ctx.app.orderStatus.OrderValid : ctx.app.orderStatus.OrderInvalid;
						await order.updateOrder(orderId, status, amount);
					}
					if(orderStatus == 'S'){
						await this.orderStatusChangeNotify(order);
					}
					return 'SUCCESS';
				}
			}else {
				this.logger.error('can not parse order orderId=' + orderId + ' cpOrderId=' + cpOrderId+' details=' + JSON.stringify(params))
				return 'FAILURE';
			}
		}catch (err){
			 this.logger.error(err);
			 return 'FAILURE';
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

	async getSignature(params) {

		this.logger.info('getSignature ' + JSON.stringify(params))

		if(params.cpOrderId) {
			var order = await this.ctx.model.Order.findByOrderId(params.cpOrderId);
				if(!this.ctx.helper.is_empty(order)){
					var signMap = {
						// callbackInfo: params.callbackInfo,
						amount: params.amount / 100,
						// notifyUrl: params.notifyUrl,
						cpOrderId: params.cpOrderId,
						accountId: params.accountId,
					}
					var signature = this.signatureTool(signMap, order.channel.api_key);
					this.logger.info(order.channel.api_key)
					return {code:1, signature:signature}; 
				}
		}

		return {code:0, message:'order id not exist.'}; 

		
	}

	signatureTool(obj, apiKey) {
		var sorted_keys = Object.keys(obj).sort();
		var sorted_signMap = {};
		var signedStr = '';
		for(var i=0;i<sorted_keys.length;i++){
			sorted_signMap[sorted_keys[i]] = obj[sorted_keys[i]]
			if(obj[sorted_keys[i]] !=='undefined'){
				signedStr += sorted_keys[i] + '=' + sorted_signMap[sorted_keys[i]];
			}
		}
		signedStr += apiKey;

		return crypto.createHash('md5').update(signedStr).digest("hex");
	}
}

module.exports = UcService;