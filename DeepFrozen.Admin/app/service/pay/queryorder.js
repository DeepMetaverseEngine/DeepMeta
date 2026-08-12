const Service = require('egg').Service;
var crypto = require('crypto');

class queryOrderService extends Service {
  async getOrderStatus(orders) {
    let ctx = this.ctx;
    var order_list = await this.ctx.model.Order.findMany(orders.split(','));

      if(order_list.length == 0){
        return {code:0, message:'order not exis'}
      }

      var order = order_list[0];
      var valid_orders = {}
      //如果订单所属渠道支持查询接口则单独处理
      if(order.channel.query_order){
        if(order.sdk_name == 'xiaomi'){
          valid_orders = await this.ctx.service.pay.xiaomi.queryOrder(order_list);
        }else if(order.sdk_name == 'OneGame'){
          valid_orders = await this.ctx.service.pay.onegame.queryOrder(order_list);
        }else {
          return {code:0, message:'incorrect queryorder interface.'}
        }
      }else {
        valid_orders = this.ctx.helper.getManyOrderValid(order_list);
      }

      if(Object.keys(valid_orders).length > 0){
        return {code:1, order_list:valid_orders}
      }else {
        return {code:0, message:'no valid order.'}
      }
    }
}

module.exports = queryOrderService;