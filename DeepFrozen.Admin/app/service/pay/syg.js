const Service = require('egg').Service;
var crypto = require('crypto');

class SygService extends Service {
	async parseOrder(params) {
		const ctx = this.ctx;
		ctx.logger.info(params)
		var orderId = params.orderId;
		var cpOrderId = params.billno;
		var sign = params.sign;
		var payFee = params.amount * 100;
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
				var local_sign = this.getSignature(orderId + params.uid + params.serverId + params.amount + params.extraInfo + params.orderTime + cpOrderId + params.test,order.channel.api_key)
				if(local_sign != sign){
					this.logger.error('signature verification failed. orderId=' + orderId + ' cpOrderId=' + cpOrderId+' details=' + JSON.stringify(params))
					return {errcode:1525};
				}

				if(params.test == 1){
					this.logger.error('the order is a test order ' + orderId + ' cpOrderId=' + cpOrderId+' details=' + JSON.stringify(pay_result))
					// return "SUCCESS";
				}

				if(order.status == ctx.app.orderStatus.Create 
					|| order.status == ctx.app.orderStatus.PaySuccess
					|| order.status == ctx.app.orderStatus.OrderInvalid){
					await order.updateOrder(orderId, ctx.app.orderStatus.OrderValid, payFee);
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


	getSignature(params, apiKey) {
		return crypto.createHash('md5').update(params + apiKey).digest("hex").toLowerCase();
	}
}

module.exports = SygService;