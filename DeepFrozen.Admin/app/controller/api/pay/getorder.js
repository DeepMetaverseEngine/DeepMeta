'use strict';
const Controller = require('egg').Controller;

class GetOrderController extends Controller {
  async index() {
    var orders = this.ctx.request.body.orders;
    if(!orders){
      this.ctx.body = {code:0, message:'incorrect order number.'}
      return;
    }
    this.ctx.body = await this.ctx.service.pay.queryorder.getOrderStatus(orders);
  }

  async iap() {
    var result = await this.ctx.service.pay.appstore.parseOrder(this.ctx.request.body);
    this.ctx.logger.info('iap:' + JSON.stringify(result))
    this.ctx.body = result
  }
}

module.exports = GetOrderController;
