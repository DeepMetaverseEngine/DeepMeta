const Service = require('egg').Service;
var crypto = require('crypto');

class OnegameService extends Service {
	async parseOrder(params) {
		const ctx = this.ctx;
		ctx.logger.info(params)

		try{
			var orderId = params.orderId;
			var amount = params.amount;
			var orderStatus = params.orderStatus;
			var cpOrderId = params.cpOrderId;

			if(cpOrderId){
				var order = await ctx.model.Order.findByOrderId(cpOrderId);
				if(ctx.helper.is_empty(order)){
					this.logger.error('order not exist orderId=' + orderId + ' cpOrderId=' + cpOrderId+' details=' + JSON.stringify(params))
					return 'FAILURE';
				}
				else {
					if(order.status == ctx.app.orderStatus.Create 
						|| order.status == ctx.app.orderStatus.PaySuccess
						|| order.status == ctx.app.orderStatus.OrderInvalid){
						var status = orderStatus == 'S' ? ctx.app.orderStatus.OrderValid : ctx.app.orderStatus.OrderInvalid;
						await order.updateOrder(orderId, status, amount);
					}
					if(orderStatus == 'S'){
						try{
							await this.orderStatusChangeNotify(order);
						}catch (err){
							this.logger.error("send orderStatusChangeNotify failed. remote not responding")
							this.logger.error(err);
						}
						
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

	async queryOrder(orders) {
		var valid_orders = {}
		for ( var i = 0; i <orders.length; i++){
			if(this.ctx.helper.getOrderValid(orders[i])){
				this.ctx.helper.object_add(valid_orders, orders[i].role_id, orders[i].cp_order_id)
			}
			// else if(this.ctx.helper.is_local_ip(this.ctx.ip)){
			// 	await orders[i].updateOrder(((new Date())/1).toString(), this.ctx.app.orderStatus.OrderValid, orders[i].price);
			// 	this.ctx.helper.object_add(valid_orders, orders[i].role_id, orders[i].cp_order_id)
			// }
		}
		return valid_orders;
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

		var signMap = {
			amount: params.amount / 100,
			notifyUrl: 'http://office.1gamesh.com:31000/api/public/callback/aligames',//params.notifyUrl,
			cpOrderId: params.cpOrderId,
			accountId: params.accountId,
		}

		var signature = this.signatureTool(signMap, '4789aef1222330c9443e22c3d82055e9');

		return {code:1, signature:signature}; 
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

module.exports = OnegameService;