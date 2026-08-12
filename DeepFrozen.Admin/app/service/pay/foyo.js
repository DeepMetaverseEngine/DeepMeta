const Service = require('egg').Service;
var crypto = require('crypto');

class FoyoService extends Service {
	async parseOrder(params) {
		const ctx = this.ctx;
		ctx.logger.info(params)
		var sign = params.sign;
		if(!sign){
			ctx.status = 403;
			return 'Go away, robot.';
		}

		try{
			var orderId = params.oid;
			var username = params.username;
			var amount = params.amount * 100;
			var gold = params.gold;//人民币ratio
			var cp_ext = params.cp_ext;
			var server_id = params.server_id;
			var role_id = params.role_id;
			var is_test = params.is_test;
			var timestamp = params.tm;
			var failedDesc = params.failedDesc;
			var cpOrderId = params.cp_oid;

			if(cpOrderId){
				var order = await ctx.model.Order.findByOrderId(cpOrderId);
				if(ctx.helper.is_empty(order)){
					this.logger.error('order not exist orderId=' + orderId + ' cpOrderId=' + cpOrderId+' details=' + JSON.stringify(params))
					return {code:0, message:'order not exist'};
				}
				else {
					var local_sign = this.signatureTool(params,order.channel.api_key);
					if(local_sign != sign){
						this.logger.error('signature verification failed. orderId=' + orderId + ' cpOrderId=' + cpOrderId+' details=' + JSON.stringify(params))
						return {code:0, message:'signature verification failed'};
					}
					
					if(order.status == ctx.app.orderStatus.Create 
						|| order.status == ctx.app.orderStatus.PaySuccess
						|| order.status == ctx.app.orderStatus.OrderInvalid){
						await order.updateOrder(orderId, ctx.app.orderStatus.OrderValid, amount);
					}
					await this.orderStatusChangeNotify(order);
					return {code:1};
				}
			}else {
				this.logger.error('can not parse order orderId=' + orderId + ' cpOrderId=' + cpOrderId+' details=' + JSON.stringify(params))
				return {code:0, message:'can not parse order'};
			}
		}catch (err){
			 this.logger.error(err);
			 return {code:0, message:'error'};
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


	signatureTool(obj, apiKey) {
		var sorted_keys = Object.keys(obj).sort();
		var sorted_signMap = {};
		var signedStr = '';
		for(var i=0;i<sorted_keys.length;i++){
			sorted_signMap[sorted_keys[i]] = obj[sorted_keys[i]]
			if(sorted_keys[i] != 'sign' && obj[sorted_keys[i]] !=='undefined'){
				signedStr += sorted_keys[i] + '=' + sorted_signMap[sorted_keys[i]] + '&';;
			}
		}
		if(signedStr.endsWith('&')){
			signedStr = signedStr.substring(0,signedStr.length-1);
		}
		signedStr += apiKey;

		return crypto.createHash('md5').update(signedStr).digest("hex");
	}
}

module.exports = FoyoService;