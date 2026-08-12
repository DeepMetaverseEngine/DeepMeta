const Service = require('egg').Service;
var crypto = require('crypto');

class EWANService extends Service {
	async parseOrder(params) {
		const ctx = this.ctx;
		ctx.logger.info(params)
		var orderId = params.ordernum;
		var cpOrderId = params.custominfo;
		var sign = params.sign;
		var amount = params.amount;
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
				var local_sign = this.getSignature(params,order.channel.api_key)
				if(local_sign != sign){
					this.logger.error('signature verification failed. orderId=' + orderId + ' cpOrderId=' + cpOrderId+' details=' + JSON.stringify(params))
					return {errcode:100};
				}


				if(order.status == ctx.app.orderStatus.Create 
					|| order.status == ctx.app.orderStatus.PaySuccess
					|| order.status == ctx.app.orderStatus.OrderInvalid){
					await order.updateOrder(orderId, ctx.app.orderStatus.OrderValid, amount);
				}

				order.status = ctx.app.orderStatus.OrderValid
				await order.save()

				try{
					await this.orderStatusChangeNotify(order);
				}catch(err){
					this.logger.error('send orderStatusChangeNotify failed. orderId=' + orderId + ' cpOrderId=' + cpOrderId + 'remote not responding')
					this.logger.error(err);
				}
				return "1";
			}

		}catch (err){
			 this.logger.error(err);
			 return {errcode:103};
		}
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

	getSignature(params, apiKey) {
		return crypto.createHash('md5').update(params.serverid 
			+ '|' + params.custominfo 
			+ '|' + params.openid 
			+ '|' + params.ordernum 
			+ '|' + params.status 
			+ '|' + params.paytype 
			+ '|' + params.amount 
			+ '|' + params.errdesc 
			+ '|' + params.paytime 
			+ '|' + apiKey).digest("hex").toLowerCase();
	}
}

module.exports = EWANService;