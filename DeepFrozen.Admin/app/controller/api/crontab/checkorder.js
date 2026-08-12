'use strict';
const Controller = require('egg').Controller;

class CheckordersController extends Controller {
  async index() {
    var orders = await this.ctx.model.Order.findUnverifiedOrder();

    for( var i = 0;i<orders.length;i++){
    	this.call_sdk_pay(orders[i].cp_order_id, orders[i].price);
    }

    this.ctx.body = 1
  }

  async call_sdk_pay(orderId, amount) {
	try{
		//随机成功
		var rate = Math.floor(Math.random()*(10+1));
		var status = rate <= 8 ? 'S' : 'F';

		const result = await this.ctx.curl('http://' + this.ctx.request.header.host + '/api/public/callback/onegame_test', {
			method: 'POST',
			dataType: 'text',
			timeout: 10000,
			data: {
		        orderId: ((new Date())/1).toString(),
		        cpOrderId: orderId,
		        amount:amount,
		        orderStatus:status
		    },
	    });
	    this.logger.info('call_sdk_pay '+ orderId + ' response: ' + JSON.stringify(result.data))
	}catch(err){
		this.logger.error(err);
	}
  }
}

module.exports = CheckordersController;
