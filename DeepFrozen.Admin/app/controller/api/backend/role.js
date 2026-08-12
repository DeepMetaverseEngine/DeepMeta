'use strict';
const Controller = require('egg').Controller;

//验证规则
const editRule = {
    //role: 'string',
    op_reason: 'string',
  };

class RoleController extends Controller {
  async role_info(request) {
    try {

      var role = this.ctx.request.body.role;
      var result;
      if(role.indexOf(':') < 0){
          var command = {
            cmd: "RoleNameToUUID",
            role: role
          }
        var result = await this.service.gmt.send_command(command,'success');
      }else {
        var command = {
            cmd: "ServerAccountQuery",
            account_id: role
          }
        var result = await this.service.gmt.send_command(command,'success');
        result.ext = JSON.parse(result.ext)
      }
      
      if(result.state){
        // var order = await this.ctx.model.Order.create({
        //   realm_id: this.ctx.service.realmselector.get_session_realmid(),
        //   server_id: result.ext.server_id,
        //   platform_account: result.ext.account_id,
        //   role_id: result.ext.role_uuid,
        //   platform_id: 2101,
        //   cp_order_id: order_id,
        //   currency_type: '',
        //   price: recharge_data.price,
        //   count: 1,
        //   product_id: recharge_data.id,
        //   sdk_name: 'internal',
        //   channel_id: 0,
        //   order_id: order_id,
        //   status: 2,
        // })
        var res = result.ext;
        //var res = 'account_id:' + result.ext.account_id + '\n' + 'role_uuid:' + result.ext.role_uuid + '\n' + 'server_id:' + result.ext.server_id;

      this.ctx.body = {state: true, info:res, reason: this.ctx.__('common_instructions_success')}

      }else {
        if(result.reason == 'role_name_not_exist'){
          result.reason = this.ctx.__('role_name_not_exist');
        }
        this.ctx.body = result;
      }
    } catch(err) {
      this.ctx.error(err)
    }
  }

  async role_bag(request) {
    try {
      var ctx = this.ctx;
      var bag_type = this.get_bag_type(ctx.request.body.type);
      var role = this.ctx.request.body.role.trim()
      if(ctx.request.body.query_type == 'role_id'){
        var command = {
          cmd: "ServerQueryRoleList",
          realm_id: await this.service.realmselector.get_realm_by_server_id(ctx.request.body.server_id),
          role_id: role,
          type: 1,
        }
        var qrole = await this.service.gmt.send_command(command,'success');
        if(! qrole.state){
          this.ctx.body = qrole;
          return
        }
        role = JSON.parse(qrole.ext)[0].role_name
      }
      var command = {
        cmd: "ServerRoleBagQuery",
        role: role,
        realm_id: await this.service.realmselector.get_realm_by_server_id(ctx.request.body.server_id),
        bagType: bag_type
      }
      var result = await this.service.gmt.send_command(command,'success');

      if(result.state){
        var item_json = JSON.parse(result.ext);
        var item_data = [];
        var item_indexs = Object.keys(item_json.Slots);
        item_indexs.forEach(function (id) {
          item_data.push({ID: id, TemplateID: item_json.Slots[id].TemplateID, CanTrade: item_json.Slots[id].CanTrade, Count: item_json.Slots[id].Count})
        });
        ctx.logger.info(item_data)
        var res = item_data;
      //   await this.ctx.write_log(this.ctx.app.action.info, {
      //   customType:'bag_query', 
      //   command: command, 
      //   result: result
      // });
      this.ctx.body = {state: true, info:res, reason: this.ctx.__('common_instructions_success')}

      }else {
        if(result.reason == 'role_name_not_exist'){
          result.reason = this.ctx.__('role_name_not_exist');
        }
        this.ctx.body = result;
      }
    } catch(err) {
      this.ctx.logger.error(err)
    }
  }


  async role_bag_edit(request) {
    try {
      var ctx = this.ctx;
      var role =  ctx.request.body.role;
      var bag_type = this.get_bag_type(ctx.request.body.type);
      var action = ctx.request.body.action;
      var data = ctx.get_request_primary_data(ctx.request.body.data)
    ctx.logger.info(action)
    ctx.logger.info(data)
    if(action == 'remove') {
      if(bag_type == 'RoleVirtualBag:') {
        this.ctx.response.rsp_table_error([{message: this.ctx.__('common_instructions_inputerr'), value: 'can not remove RoleVirtualBag item'}]);
        this.ctx.logger.error('try to remove RoleVirtualBag item. response error.')
        return;
      }
    }
    await this.ctx.validate(editRule, data);

      var command = {
        cmd: "ServerRoleBagModify",
        role: this.ctx.request.body.role,
        bagType: bag_type,
        action: action,
        entryKey: data.ID,
        value: data.Count,
        realm_id: await this.service.realmselector.get_realm_by_server_id(ctx.request.body.server_id),
      }
      var result = await this.service.gmt.send_command(command,'success');
      data['role'] = role,
      data['action'] = ctx.request.body.action;
      data['server_id'] = ctx.request.body.server_id;
      await this.ctx.write_log(this.ctx.app.action.info, {
        customType:'bag_edit', 
        command: data, 
        result: result
      });
      if(result.state){
        if(action == 'remove') {
          ctx.body = {data:[]};
        }else {
          ctx.body = {data:[data]}
        }

      }else {
        if(result.reason == 'role_name_not_exist'){
          result.reason = this.ctx.__('role_name_not_exist');
        }
        this.ctx.response.rsp_table_error([{message: this.ctx.__('common_instructions_modify_failed'), value: result.reason}]);
        this.ctx.logger.error('server response failed result.reason='+result.reason)
      }
    } catch(err) {
      this.ctx.response.rsp_table_field_errors(err.errors);
      this.ctx.logger.error(err)
    }
  }

  get_bag_type(index) {
    var bag_type = 'RoleVirtualBag:';

      if(index == 1){
        bag_type = 'RoleBag:'
      }

      return bag_type;
  }

  async role_email(request) {
    let ctx = this.ctx;
    var role_name = this.ctx.request.body.role.trim()

    var logs = await this.ctx.model.Log.findLogByType(7);

    var result = []

    for (var i = 0; i < logs.length; i++) {
      var params_json = JSON.parse(logs[i].operation);

      if(params_json.logs.role_list && params_json.logs.role_list.length > 0 && params_json.logs.role_list.indexOf(role_name) >= 0) {
        var mail_data = {
          server_id: params_json.logs.realm_id,
          role_name: params_json.logs.role_list,
          title: params_json.logs.mail.title,
          content: params_json.logs.mail.content,
          items: JSON.stringify(params_json.logs.mail.item),
          result:params_json.logs.reason,
          time: logs[i].created_at
        }

        if(!mail_data.server_id) {
          mail_data.server_id = 'NULL';
        }
        result.push(mail_data)
      }
    }

    this.ctx.body = this.ctx.body = {state: true, info:result, reason: this.ctx.__('common_instructions_success')};
  }
}

module.exports = RoleController;
