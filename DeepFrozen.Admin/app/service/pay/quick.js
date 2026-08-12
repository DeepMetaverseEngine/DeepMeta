const Service = require('egg').Service;
var crypto = require('crypto');

class QuickService extends Service {
	async parseOrder(params, secret) {
		const ctx = this.ctx;
		var nt_data = params.nt_data;
		var sign = params.sign;
		var md5_sign = params.md5Sign;

		if(!nt_data || !sign || !md5_sign){
			ctx.status = 403;
			return 'Go away, robot.';
		}

		var local_sign = this.getSignature(nt_data + sign, secret.md5_key);

		if(local_sign != md5_sign){
			this.logger.error('signature verification failed. details=' + JSON.stringify(params))
			return {errcode:1525};
		}




		//调用解密
		var xml = this.decode(nt_data,secret.product_code);
		var result = await this.ctx.helper.xmlToJs(xml)
		var pay_result = result.quicksdk_message.message[0];


		var orderId = pay_result.order_no[0];
		var cpOrderId = pay_result.game_order;
		var payFee = pay_result.amount * 100;

		try{
			var order = await ctx.model.Order.findByOrderId(cpOrderId);
			if(ctx.helper.is_empty(order)){
				this.logger.error('order not exist orderId=' + orderId + ' cpOrderId=' + cpOrderId+' details=' + JSON.stringify(pay_result))
				return {errcode:1506};
			}
			else {

				if(pay_result.is_test == 1){
					this.logger.error('the order is a test order ' + orderId + ' cpOrderId=' + cpOrderId+' details=' + JSON.stringify(pay_result))
					return "SUCCESS";
				}

				if(order.status == ctx.app.orderStatus.Create 
					|| order.status == ctx.app.orderStatus.PaySuccess
					|| order.status == ctx.app.orderStatus.OrderInvalid){
					await order.updateOrder(orderId, ctx.app.orderStatus.OrderValid, payFee);
				}

				try{
					 await this.orderStatusChangeNotify(order);
				}catch(err){
					this.logger.error('send orderStatusChangeNotify failed. orderId=' + orderId + ' cpOrderId=' + cpOrderId + ' remote not responding')
					this.logger.error(err);
				}
				return "SUCCESS";
			}

		}catch (err){
			 this.logger.error(err);
			 return {errcode:1000};
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


	//QuickSDK参数同步解码方法
	//输入密文、callbackKey
	//成功返回解密后的xml字符
	//失败会空字符串,长度为0
	decode(str,key){

		if(str.length <= 0){
			return '';
		}

		var list = new Array();
		var resultMatch = str.match(/\d+/g);
		for(var i= 0;i<resultMatch.length;i++){
			list.push(resultMatch[i]);
		}

		if(list.length <= 0){
			return '';
		}
		
		var keysByte = this.stringToBytes(key);
		var dataByte = new Array();
		for(var i = 0 ; i < list.length ; i++){
			dataByte[i] = parseInt(list[i]) - (0xff & parseInt(keysByte[i % keysByte.length]));
		}

		if(dataByte.length <= 0){
			return '';
		}

		var parseStr = this.bytesToString(dataByte);
		return parseStr;
	}



	stringToBytes (str) {  
		var ch, st, re = [];  
	  	for (var i = 0; i < str.length; i++ ) {  
	    	ch = str.charCodeAt(i);
	    	st = []; 
	    	do {  
	      		st.push( ch & 0xFF );
	      		ch = ch >> 8;
	    	}while ( ch );  
	    	re = re.concat( st.reverse() );  
		}  
	  	return re;  
	} 


	bytesToString(array) {
	  return String.fromCharCode.apply(String, array);
	}

	getSignature(params, apiKey) {
		return crypto.createHash('md5').update(params + apiKey).digest("hex");
	}
}

module.exports = QuickService;