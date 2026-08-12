const Service = require('egg').Service;
var crypto = require('crypto');

class AppstoreService extends Service {
	async parseOrder(params, sandbox_env) {
		const ctx = this.ctx;

		var code = 0
      	try{
      		var cpOrderId = Object.keys(params)[0];
			var receipt = params[cpOrderId];
			this.logger.error('received receipt orderId ' + cpOrderId +' receipt=' + receipt)
			const result = await this.ctx.curl(sandbox_env?'https://sandbox.itunes.apple.com/verifyReceipt':'https://buy.itunes.apple.com/verifyReceipt', {
				method: 'POST',
				contentType: 'json',
				dataType: 'json',
				data: {
					'receipt-data': receipt
				},
				timeout: 15000,
		    });

		    if(result.data.status > 0) {
		    	//如果收据环境不匹配
		    	if(result.data.status == 21007) {
		    		//sandbox订单重新发起收据验证
		    		return await this.parseOrder(params, true)
		    		return;
		    	}
		    	this.logger.error('order status = ' + result.data.status)
		    	return {code:0, message:'order status error.'}
		    }

		    var role_id = 0
		    var environment = result.data.environment
		    var valid_orders = {}
		    var receipts = result.data.receipt

		    //如果订单为数字则通过订单匹配
		    if(ctx.helper.isNumber(cpOrderId)) {
		    	var order = await ctx.model.Order.findByOrderId(cpOrderId);
		    	if(ctx.helper.is_empty(order)){
					this.logger.error('order not exist cpOrderId=' + cpOrderId+' receipt=' + JSON.stringify(params))
					return {code:0, message:'order not exist.'}
				}
				var purchase = this.getPurchaseByProductId(receipts, order.sell_id)
				ctx.logger.info(purchase)
				role_id = order.role_id;
				if(purchase != null) {
					if(order.status == ctx.app.orderStatus.Create 
						|| order.status == ctx.app.orderStatus.PaySuccess){
						order.sandbox = environment == 'Sandbox' ? 1 : 0
						await order.updateOrder(purchase.transaction_id, ctx.app.orderStatus.OrderValid);
					}
					this.removePurchaseByProductId(receipts, purchase.product_id);
					this.ctx.helper.object_add(valid_orders, order.role_id, cpOrderId)
				}
			}
				
			//如果订单无法识别
			if(role_id == 0) {
				if(cpOrderId.length != 36) {
					this.logger.error('order can not match cpOrderId = ' + cpOrderId)
					return {code:0, message:'order can not match.'}
				}else {
					role_id = cpOrderId;
				}
			}

			//粗放式验证
			if(receipts.in_app.length > 0) {
				this.logger.error('the order is role_id ' + role_id +' matching order....')
				this.logger.error('now receipts is ' + JSON.stringify(receipts))
				var orders = await ctx.model.Order.findByRoleId(role_id);
				for( var i = 0; i < receipts.in_app.length; i++) { 
					//过滤已验证的
					var order = this.getOrderByTransactionId(orders, receipts.in_app[i].transaction_id)
					if(order != null) {
						this.ctx.helper.object_add(valid_orders, order.role_id, order.cp_order_id)
						this.removePurchaseOrder(orders, receipts.in_app[i].transaction_id)
						continue;
					}

					order = this.getOrderByProductId(orders, receipts.in_app[i].product_id)
					if(order != null) {
						if(order.status == ctx.app.orderStatus.Create 
							|| order.status == ctx.app.orderStatus.PaySuccess){
							order.sandbox = environment == 'Sandbox' ? 1 : 0
							await order.updateOrder(receipts.in_app[i].transaction_id, ctx.app.orderStatus.OrderValid);
						}
						this.ctx.helper.object_add(valid_orders, order.role_id, order.cp_order_id)
						continue;
					}
					this.logger.error('can not matching ' + JSON.stringify(receipts.in_app[i]))
				}
			}


			if(Object.keys(valid_orders).length > 0){
				return {code:1, order_list:valid_orders}
				
			}else {
				return {code:0, message:'no valid order.'}
				
			}
	    }catch(err){
	    	this.logger.error(err);
	    	return {code:0, message:'error.'}
	    }
	}

	getOrderByProductId(orders, product) {
		if(!this.ctx.helper.is_empty(orders)){
			for( var i = 0; i < orders.length; i++) { 
				if (orders[i].sell_id == product) {
			     return orders[i]
			   }
			}
	    }
	    return null
	}

	removePurchaseOrder(orders, transaction_id) {
		if(!this.ctx.helper.is_empty(orders)){
			for( var i = 0; i < orders.length; i++) { 
				if (orders[i].order_id == transaction_id) {
			     orders.splice(i, 1); 
			   }
			}
		}
	}

	getOrderByTransactionId(orders, transaction_id) {
		if(!this.ctx.helper.is_empty(orders)){
			for( var i = 0; i < orders.length; i++) { 
	    		if(orders[i].order_id == transaction_id) {
	    			return orders[i]
	    		}
		    }
	    }
	    return null
	}

	getPurchaseByProductId(receipts, product) {
		if(receipts.in_app) {
			for( var i = 0; i < receipts.in_app.length; i++) { 
				if (receipts.in_app[i].product_id == product) {
			     return receipts.in_app[i]
			   }
			}
		}
	    return null
	}

	removePurchaseByProductId(receipts, product) {
		if(receipts != null) {
			var in_apps = receipts.in_app
			for( var i = 0; i < in_apps.length; i++) { 
				if (in_apps[i].product_id == product) {
			     in_apps.splice(i, 1); 
			   }
			}
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

module.exports = AppstoreService;