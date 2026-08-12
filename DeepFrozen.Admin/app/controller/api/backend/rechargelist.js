'use strict';
const Controller = require('egg').Controller;
//规则
const rule = {
    id: 'id',
    platform_id: 'id',
    type: 'id',
    name: 'string',
    price: 'id'
  };

const order_rule = {
    id: 'id',
    role: 'string'
  };

const order_relay_apply_rule = {
    server_id: 'id',
    role_id: 'id',
    product_id: 'id',
    original_order_id: 'string',
  };


class RechargelistController extends Controller {
  async index(){
    this.ctx.body = await this.ctx.service.rechargelist.show()
  };

  async create(){
    const ctx = this.ctx;
    let action = ctx.request.body.action;
    let data = ctx.get_request_primary_data(ctx.request.body.data)

    if(action == 'edit') {
      try {
        await ctx.validate(rule, data);
        //var update = await ctx.model.Recharge.find(data.id);
       await ctx.model.Recharge.update({
            platform_id: data.platform_id,
            type: data.type,
            name: data.name,
            price: data.price
          },
          { 
            where:{id: data.id
            }
        });

        result = await this.ctx.model.Recharge.findOne({
          where:{id: data.id}
        });

        //await ctx.write_log(ctx.app.action.update, {before: update, after: result, filter: ['gmt_key']})
       
        ctx.body = {data:[result]}
      } catch(err) {
        ctx.response.rsp_table_field_errors(err.errors);
      }
      

    }else if(action == 'remove') {
      var task = await ctx.model.Recharge.findOne({
         where: {id: data.id}
      });

      await task.destroy();
      //await ctx.write_log(ctx.app.action.destroy, {after: task, filter: ['gmt_key']})
      ctx.body = {data:[]};
    }else if(action == 'create') {
       try {
        await ctx.validate(rule, data);
      }catch(err){
        ctx.response.rsp_table_field_errors(err.errors);
        return;
      }
      try {
        var task = await ctx.model.Recharge.create({
            id: data.id,
            platform_id: data.platform_id,
            type: data.type, 
            name: data.name,
            price: data.price
          })
          var result = await ctx.model.Recharge.findOne({
                where:{id: data.id}
              });
          //await ctx.write_log(ctx.app.action.create, {filter:['pwd_encrypt','salt'], after:result})
          ctx.body = {data:[result]}
        }
      catch(err) {
        this.ctx.logger.error(err)
        ctx.response.rsp_table_error(err.errors);
      }
    }
  };

  async product_list(){
    this.ctx.body = await this.ctx.service.rechargelist.show();
  };

//  async deal_order() {
    // const request = this.ctx.request.body;
    // try {
    //   this.ctx.logger.info(request)
    //   await this.ctx.validate(order_rule, request);
      
    //   this.ctx.body = await this.ctx.service.rechargelist.deal_order(request);

    // } catch(err) {
    //   this.ctx.response.rsp_body_errors(err.errors);
    // }
//  }

  async reSendOrder() {
    const request = this.ctx.request.body;
    this.ctx.body = await this.ctx.service.pay.egls.reSendOrder(request)
    await this.ctx.write_log(this.ctx.app.action.info, {
      customType:'resend_order',
      command: request,
      result: this.ctx.body
    });
  }


  async resource_gm_apply() {

    const ctx = this.ctx
    const request = this.ctx.request.body;
    try {
     this.ctx.logger.info(request)
      var recharge_data = await ctx.model.Recharge.find(request.product_id);

      if(ctx.helper.is_empty(recharge_data)){
        this.ctx.body = {state:true,reason:this.ctx.__('common_instructions_send_failed')}
        return
      }
      var role = await ctx.model.Gmaccountlist.findOne({where: { id: request.id }})
      try {
        var task = await ctx.model.GmRechargeApply.create({
          realm_id: await ctx.service.realmselector.get_realm_by_server_id(role.server_id),
          server_id: role.server_id,
          role_id: role.role_id,
          department: role.department,
          owner: role.owner,
          product_id: request.product_id,
          product_name: recharge_data.name,
          price: recharge_data.price,
          platform_id: recharge_data.platform_id,
          signger: ctx.user.id,
          status: 0
        })

         this.ctx.body = {state:true,reason:this.ctx.__('common_instructions_send_success')}
        }
      catch(err) {
        this.ctx.logger.error(err)
        this.ctx.body = {state: false, reason: this.ctx.__('common_instructions_send_failed')}
        return
      }
    } catch(err) {
      this.ctx.logger.error(err)
      this.ctx.body = {state: false, reason: this.ctx.__('common_instructions_validation_failed')}
      return
    }
  }


  async resource_gm_review() {
    var review_list = await this.ctx.model.GmRechargeApply.findAll({where: {status: 0},
        include: [ { model: this.ctx.model.User, as: 'sign', attributes: ['username']}]
    });
    this.ctx.body = review_list
  }

  async review_act(){
    const ctx = this.ctx
    const request = this.ctx.request.body;
    this.ctx.logger.info(request)
    for (var i = 0; i < request.idx.length; i++) {
      var task = await ctx.model.GmRechargeApply.findOne({
         where: {id: request.idx[i]}
      });
      if(!ctx.helper.is_empty(task)){
        task.operator = ctx.user.id
        if(request.type == 1) {
          var order_id = this.service.rechargelist.generate_order();
          task.order_id = order_id
          var result = await this.deal_order(task, order_id, 'GM', 0)
          if(result.state){
            task.status = 1
          }else{
            task.status = -1
          }
          task.reason = JSON.stringify(result)
        }else {
          task.status = 3
        }
        await task.save()
      }
      
    }

    this.ctx.body = {state:true,reason:this.ctx.__('common_instructions_success')}

  }

  async deal_order(task, order_id, sdk_name, channel_id){
    try {
      var command = {
        cmd: "DigitIdToUUID",
        role: task.role_id,
        realm_id: task.realm_id
      }
      var result = await this.service.gmt.send_command(command,'success');
      var reason = 'success'
      this.logger.info(result)
      if(result.state){
        var ext_info = JSON.parse(result.ext);
        try {
          var order = await this.ctx.model.Order.create({
            realm_id: task.realm_id,
            server_id: task.server_id,
            platform_account: ext_info.account_uuid,
            digit_id: task.role_id,
            role_id: ext_info.uuid,
            platform_id: task.platform_id,
            cp_order_id: order_id,
            currency_type: '',
            price: task.price,
            count: 1,
            product_id: task.product_id,
            sdk_name: sdk_name,
            channel_id: channel_id,
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
              reason = "send orderStatusChangeNotify failed. remote not responding"
              this.logger.error(reason)
              this.logger.error(err);
              return {state: false, reason: reason}
            }

            return {state: true, reason: reason}
          }
        }catch(err){
          this.logger.error("order create faild.");
          this.logger.error(err);
          return {state: false, reason: 'faild.'}
        }
        return {state: true, reason: 'success'}
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


  async resource_gm_record() {
    const ctx = this.ctx
    const request = ctx.request.body;

    ctx.logger.info(request)
    var review_list = await ctx.model.GmRechargeApply.findBy(request.date1, request.date2, request.server_id, request.role_id, request.department, request.status);
    ctx.body = {state: true, info:review_list, reason: ctx.__('common_instructions_success')}
  }

  async resource_order_relay_apply() {

    const ctx = this.ctx
    const request = this.ctx.request.body;
    try {
     this.ctx.logger.info(request)
      await this.ctx.validate(order_relay_apply_rule, request);

      var recharge_data = await ctx.model.Recharge.find(request.product_id);

      if(ctx.helper.is_empty(recharge_data)){
        this.ctx.body = {state:true,reason:this.ctx.__('common_instructions_send_failed')}
        return
      }

      try {
        var task = await ctx.model.OrderRelayApply.create({
          realm_id: await ctx.service.realmselector.get_realm_by_server_id(request.server_id),
          server_id: request.server_id,
          role_id: request.role_id,
          old_order_id: request.original_order_id,
          product_id: request.product_id,
          product_name: recharge_data.name,
          price: recharge_data.price,
          platform_id: recharge_data.platform_id,
          signger: ctx.user.id,
          status: 0
        })

         this.ctx.body = {state:true,reason:this.ctx.__('common_instructions_send_success')}
        }
      catch(err) {
        this.ctx.logger.error(err)
        this.ctx.body = {state: false, reason: this.ctx.__('common_instructions_send_failed')}
        return
      }
    } catch(err) {
      this.ctx.logger.error(err)
      this.ctx.body = {state: false, reason: this.ctx.__('common_instructions_validation_failed')}
      return
    }
  }


  async resource_order_relay_review() {
    var review_list = await this.ctx.model.OrderRelayApply.findAll({where: {status: 0},
        include: [ { model: this.ctx.model.User, as: 'sign', attributes: ['username']}]
    });
    this.ctx.body = review_list
  }

  async resource_order_relay_review_act(){
    const ctx = this.ctx
    const request = this.ctx.request.body;
    this.ctx.logger.info(request)

    for (var i = 0; i < request.idx.length; i++) {
      var task = await ctx.model.OrderRelayApply.findOne({
         where: {id: request.idx[i]}
      });
      if(!ctx.helper.is_empty(task)){
        task.operator = ctx.user.id
        if(request.type == 1) {
          //开始处理流程
          var order = await ctx.model.Order.findByOrderId(task.old_order_id);
          if(!ctx.helper.is_empty(order)){
            var order_id = this.service.rechargelist.generate_order();
            task.new_order_id = order_id
            var result = await this.deal_order(task, order_id, order.sdk_name, order.channel_id)
            if(result.state){
              task.status = 1
            }else{
              task.status = -1
            }
            task.reason = JSON.stringify(result)
          }else {
            task.status = -1
            task.reason = ctx.__('page_resource_order_relay_apply_original_order_not_exist')
          }
        }else {
          task.status = 3
        }
        await task.save()
      }
      
    }

    this.ctx.body = {state:true,reason:this.ctx.__('common_instructions_success')}

  }

  async resource_order_relay_record() {
    const ctx = this.ctx
    const request = ctx.request.body;

    ctx.logger.info(request)
    var review_list = await ctx.model.OrderRelayApply.findBy(request.date1, request.date2, request.server_id, request.role_id, request.status);
    ctx.body = {state: true, info:review_list, reason: ctx.__('common_instructions_success')}
  }



  async edit(){};

  async update(){};

  async destroy(){};

  async new() {
    this.ctx.body = 'new';
  }
}

module.exports = RechargelistController;
