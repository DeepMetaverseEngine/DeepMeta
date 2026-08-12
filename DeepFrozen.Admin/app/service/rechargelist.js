const Service = require('egg').Service;
const uuid = require('uuid/v1');
const encode = require( 'hashcode' ).hashCode;
const moment = require('moment');

class RechargelistService extends Service {
  async show() {
    let list = await this.ctx.model.Recharge.findAll();
    return list;
  }

  async deal_order(request) {
    try {
      var command = {
        cmd: "RoleNameToUUID",
        role: request.role
      }
      var result = await this.service.gmt.send_command(command,'success');
      this.logger.info(result)
      if(result.state){
        var ext_info = JSON.parse(result.ext);
        var recharge_data = await this.ctx.model.Recharge.find(request.id);
        if(!this.ctx.helper.is_empty(recharge_data)){
          try {
            var order_id = this.generate_order();
            var order = await this.ctx.model.Order.create({
              realm_id: this.ctx.service.realmselector.get_session_realmid(),
              server_id: ext_info.server_id,
              platform_account: ext_info.account_uuid,
              role_id: ext_info.uuid,
              platform_id: 2101,
              cp_order_id: order_id,
              currency_type: '',
              price: recharge_data.price,
              count: 1,
              product_id: recharge_data.id,
              sdk_name: 'internal',
              channel_id: 0,
              order_id: order_id,
              status: 2,
            })

            var order_detail = await this.ctx.model.Order.findOne({
                where:{id: order.id}
              });

            if(!this.ctx.helper.is_empty(order_detail)){
              try{
                await this.ctx.service.pay.egls.orderStatusChangeNotify(order_detail);
              }catch(err){
                this.logger.error("send orderStatusChangeNotify failed. remote not responding")
                this.logger.error(err);
              }

              return {state: true, reason: this.ctx.__('common_instructions_success')}
            }
          }catch(err){
            this.logger.error("order create faild.");
            this.logger.error(err);
          }
        }
        return {state: false, reason: this.ctx.__('page_order_grant_product_not_found')}
      }else {
        if(result.reason == 'role_name_not_exist'){
          result.reason = this.ctx.__('role_name_not_exist');
        }
        return result;
      }
    } catch(err) {
      this.ctx.error(err)
    }
  }

  generate_order() {
    var time = moment().format('YYYYMMDDHHmmssSSS');
    var hash = Math.abs(encode().value(uuid()))
    var order_id = time + hash;
    return order_id;
  }
}

module.exports = RechargelistService;