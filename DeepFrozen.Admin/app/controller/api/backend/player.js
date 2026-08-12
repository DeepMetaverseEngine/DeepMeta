'use strict';

const moment = require('moment');
const Controller = require('egg').Controller;

class PlayerController extends Controller {

  async player_charge(){
    try{
      const ctx = this.ctx;
      let where
      if(ctx.request.body.query_type == 'cp_order_id'){
        where = {}
      }else{
        where = {
          updated_at:  {[ctx.model.Op.between]:[moment(ctx.request.body.date1).format('YYYY-MM-DD HH:mm:ss'),moment(ctx.request.body.date2).add(1, 'd').format('YYYY-MM-DD HH:mm:ss')]}
        }
      };
      where[ctx.request.body.query_type] = ctx.request.body.input_field;
      const orderlist = await this.ctx.model.Order.findAll({
        where: where,
        order: [['updated_at', 'DESC']],
      });
      ctx.body = orderlist;
    }catch(err){
      this.ctx.logger.error(err);
      this.ctx.response.rsp_body_errors(err.errors);
    }
  };

	async player_query(){
    try {
      var server_id = this.ctx.request.body.server_id;
      var role_name = this.ctx.request.body.role_name.trim();
      var query_type = this.ctx.request.body.query_type;
      var type = this.ctx.request.body.type;
      var result;
      if(type == '0'){
        var command = {
          cmd: "ServerQueryRoleSnap",
          realm_id: await this.service.realmselector.get_realm_by_server_id(server_id),
          account_id: role_name,
        }
      }else{
        //if(query_type == '0'){
          var command = {
            cmd: "ServerQueryRoleList",
            realm_id: await this.service.realmselector.get_realm_by_server_id(server_id),
            type: 1,
          }
        //}else{
        //if(! role_name.match(/^\d+$/)){
        //  this.ctx.body = { 'state': false, 'reason': this.ctx.__('common_instructions_role_id') + ' error' };
        //  return
        //}
          command[query_type] = role_name;
      }
      result = await this.service.gmt.send_command(command,'success');

      if(result.state){
        var res = JSON.parse(result.ext)
        this.ctx.body = {state: true, info: res, reason: this.ctx.__('common_instructions_success')}
      }else {
        if(result.reason == 'role_name_not_exist'){
          result.reason = this.ctx.__('role_name_not_exist');
        }
        this.ctx.body = result;
      }
    } catch(err){
      this.ctx.logger.error(err);
      this.ctx.response.rsp_body_errors(err.errors);
    }
	}

  async player_ability() {
    try{
      var role = this.ctx.request.body.role.trim();
      var leixing = this.ctx.request.body.leixing;
      var server_id = this.ctx.request.body.server_id
      var date1 = this.ctx.request.body.date1;
      var date2 = this.ctx.request.body.date2;
      var abchangelist = await this.ctx.biModel.AbilityChange.getabchangelist( leixing, role, server_id, date1, date2)
      this.ctx.body = { info: abchangelist, state: true , reason: 'success'}
    }catch(err){
      this.ctx.body = { state: false , reason: err.toString()};
      this.ctx.logger.error(err);
    }
  }

  async player_item(){
    try{
      var role = this.ctx.request.body.role.trim();
      var leixing = this.ctx.request.body.leixing;
      var server_id = this.ctx.request.body.server_id
      var query_type = this.ctx.request.body.query_type;
      var item_id = this.ctx.request.body.item_to;
      var date1 = this.ctx.request.body.date1;
      var date2 = this.ctx.request.body.date2;
      var itemlist = null
      itemlist = await this.ctx.biModel[query_type].getroleitem(leixing, role, server_id, item_id, date1, date2)
      this.ctx.body = { info: itemlist, state: true , reason: 'success'}
    }catch(err){
      this.ctx.body = { reason: error, state: false }
      this.ctx.logger.error(err);
    }
  };

  async player_yuanbao(){
    try{
      var role = this.ctx.request.body.role.trim();
      var leixing = this.ctx.request.body.leixing;
      var server_id = this.ctx.request.body.server_id
      var query_type = this.ctx.request.body.query_type;
      var date1 = this.ctx.request.body.date1;
      var date2 = this.ctx.request.body.date2;
      var options = this.ctx.request.body.options;
      var querylist = await this.ctx.biModel[query_type].queryrolelist(leixing, role, server_id, options, date1, date2)
      this.ctx.body = { info: querylist, state: true , reason: 'success'}
    }catch(err){
      this.ctx.body = { state: false , reason: err.toString()};
      this.ctx.logger.error(err);
    }
  };
  async player_tongbi(){
    try{
      var role = this.ctx.request.body.role.trim();
      var leixing = this.ctx.request.body.leixing;
      var query_type = this.ctx.request.body.query_type;
      var server_id = this.ctx.request.body.server_id;
      var date1 = this.ctx.request.body.date1;
      var date2 = this.ctx.request.body.date2;
      var options = this.ctx.request.body.options;
      var querylist = await this.ctx.biModel[query_type].queryrolelist(leixing, role, server_id, options, date1, date2)
      this.ctx.body = { info: querylist, state: true , reason: 'success'}
    }catch(err){
      this.ctx.body = { state: false , reason: err.toString()};
      this.ctx.logger.error(err);
    }
  };
  async player_yinliang(){
    try{
      var role = this.ctx.request.body.role.trim();
      var leixing = this.ctx.request.body.leixing;
      var query_type = this.ctx.request.body.query_type;
      var server_id = this.ctx.request.body.server_id;
      var date1 = this.ctx.request.body.date1;
      var date2 = this.ctx.request.body.date2;
      var options = this.ctx.request.body.options;
      var querylist = await this.ctx.biModel[query_type].queryrolelist(leixing, role, server_id, options, date1, date2)
      this.ctx.body = { info: querylist, state: true , reason: 'success'}
    }catch(err){
      this.ctx.body = { state: false , reason: err.toString()};
      this.ctx.logger.error(err);
    }
  };

  async player_levelup(){
    try{
      var role = this.ctx.request.body.role.trim();
      var leixing = this.ctx.request.body.leixing;
      var server_id = this.ctx.request.body.server_id;
      var date1 = this.ctx.request.body.date1;
      var date2 = this.ctx.request.body.date2;
      var querylist = await this.ctx.biModel.LogLvup.lvuplog(leixing, role, server_id, date1, date2)
      this.ctx.body = { info: querylist, state: true , reason: 'success'}
    }catch(err){
      this.ctx.body = { state: false , reason: err.toString()};
      this.ctx.logger.error(err);
    }
  };

  async player_online(){
    try{
      var role = this.ctx.request.body.role.trim();
      var leixing = this.ctx.request.body.leixing;
      var date1 = this.ctx.request.body.date1;
      var date2 = this.ctx.request.body.date2;
      var querylist = await this.ctx.biModel.LogLogoutRole.findLog(leixing, role, date1, date2)
      this.ctx.body = { info: querylist, state: true , reason: 'success'}
    }catch(err){
      this.ctx.body = { state: false , reason: err.toString()};
      this.ctx.logger.error(err);
    }
  };

  async player_chat(){
    try{
      var role = this.ctx.request.body.role.trim();
      var leixing = this.ctx.request.body.leixing;
      var server_id = this.ctx.request.body.server_id;
      var date1 = this.ctx.request.body.date1;
      var date2 = this.ctx.request.body.date2;
      var querylist = await this.ctx.biModel.LogChat.querychatlog(leixing, role, server_id, date1, date2)
      this.ctx.body = { info: querylist, state: true , reason: 'success'}
    }catch(err){
      this.ctx.body = { state: false , reason: err.toString()};
      this.ctx.logger.error(err);
    }
  };

  async player_device(){
    try{
      var role = this.ctx.request.body.role.trim();
      var leixing = this.ctx.request.body.leixing;
      var server_id = this.ctx.request.body.server_id;
      var date1 = this.ctx.request.body.date1;
      var date2 = this.ctx.request.body.date2;
      var querylist = await this.ctx.biModel.LogLoginRole.querydevicelist(leixing, role, server_id, date1, date2)
      this.ctx.body = { info: querylist, state: true , reason: 'success'}
    }catch(err){
      this.ctx.body = { state: false , reason: err.toString()};
      this.ctx.logger.error(err);
    }
  };

  async operate_changename(){
    const ctx = this.ctx;
    try {
      let data = ctx.get_request_primary_data(ctx.request.body.data);
      var digit_id = data.digit_id;
      var server_id = data.server_id;
      var old_role_name = data.role_name;
      var new_role_name = data.new_role_name.trim();
      if( old_role_name == new_role_name || new_role_name == ''){
        this.ctx.response.rsp_table_error([{message: this.ctx.__('common_instructions_inputerr'), value: ' new_role_name CAN NOT NULL or SAME role_name'}]);
        return
      }
      var command = {
        cmd: "ServerModifyRoleName",
        realm_id: await this.service.realmselector.get_realm_by_server_id(server_id),
        server_id: server_id,
        digit_id: digit_id,
        new_role_name: new_role_name,
      }
      var result = await this.service.gmt.send_command(command,'success');

      if(result.state){
        this.ctx.body = result;
        await this.ctx.write_log(this.ctx.app.action.info, {
          customType:'modify_rolename',
          command: data,
        });
      }else {
        this.ctx.response.rsp_table_error([{message: this.ctx.__('common_instructions_modify_failed'), value: result.reason}]);
      }
    } catch(err) {
      this.ctx.logger.error(err)
      this.ctx.response.rsp_table_field_errors(err.errors);
    }
  }

  async operate_changeqianming(){
    const ctx = this.ctx;
    try {
      let data = ctx.get_request_primary_data(ctx.request.body.data);
      var digit_id = data.digit_id;
      var server_id = data.server_id;
      var new_describe = data.new_describe.trim();
      var command = {
        cmd: "ServerModifyRoleDescribe",
        realm_id: await this.service.realmselector.get_realm_by_server_id(server_id),
        server_id: server_id,
        digit_id: digit_id,
        new_role_describe: new_describe,
      }
      var result = await this.service.gmt.send_command(command,'success');
      if(result.state){
        this.ctx.body = result;
        await this.ctx.write_log(this.ctx.app.action.info, {
          customType:'modify_rolesign', 
          command: data,
        });
      }else {
        this.ctx.response.rsp_table_error([{message: this.ctx.__('common_instructions_modify_failed'), value: new_describe}]);
      }
    } catch(err) {
      this.ctx.logger.error(err)
      this.ctx.response.rsp_table_field_errors(err.errors);
    }
  }

  async operate_changescene(){
    try{
      let data = this.ctx.get_request_primary_data(this.ctx.request.body.data);
      this.ctx.validate({scene_id: 'id', uuid: 'string', server_id: 'id', change_reason: 'string'}, data);
      var scene_id = data.scene_id;
      var uuid = data.uuid;
      var server_id = data.server_id;
      var command = {
        cmd: "ServerRoleChangeScene",
        realm_id: await this.service.realmselector.get_realm_by_server_id(server_id),
        server_id: server_id,
        uuid: uuid,
        scene_id: scene_id,
      }
      var result = await this.service.gmt.send_command(command,'success');
        if(result.state){
          this.ctx.body = result;
          await this.ctx.write_log(this.ctx.app.action.info, {
            customType:'change_scene', 
            command: data, 
          });
        }else {
        this.ctx.response.rsp_table_error([{message: this.ctx.__('common_instructions_modify_failed'), value: scene_name}]);
        }

    }catch(err){
      this.ctx.logger.error(err)
      this.ctx.response.rsp_table_field_errors(err.errors);
    }
  }

  async operate_kick(){
    const ctx = this.ctx;
    try {
      let data = ctx.get_request_primary_data(ctx.request.body.data);
      var uuid = data.uuid;
      var server_id = data.server_id;
      var command = {
        cmd: "ServerKickRole",
        realm_id: await this.service.realmselector.get_realm_by_server_id(server_id),
        server_id: server_id,
        uuid: uuid,
        reason: data.change_reason,
      }
      var result = await this.service.gmt.send_command(command,'success');
      if(result.state){
        this.ctx.body = result;
        await this.ctx.write_log(this.ctx.app.action.info, {
          customType:'kick_role', 
          command: data,
        });
      }else {
        this.ctx.response.rsp_table_error([{message: this.ctx.__('common_instructions_op_failed'), value: result.reason}]);
      }
    } catch(err) {
      this.ctx.logger.error(err)
      this.ctx.response.rsp_table_field_errors(err.errors);
    }
  }

  async server_data_singleability(){
    const ctx = this.ctx;
    const rspp = [];
    for (var k = 0; k < ctx.request.body.server_id.length; k++) {
      try {
        var server_id = ctx.request.body.server_id[k];
        var command = {
          cmd: "ServerRankingList",
          realm_id: await this.service.realmselector.get_realm_by_server_id(server_id),
          server_id: server_id,
        }
        var result = await this.service.gmt.send_command(command,'success');
        let res = JSON.parse(result.ext).slice(0,50)
        //let rsp_result = res.slice(0,50);
        //rsp_result['server_id'] = server_id;
        rspp.push(res);
      } catch(err) {
        this.ctx.logger.error(err)
        this.ctx.response.rsp_body_errors(err.errors);
        return
      }
    }
    ctx.body = {state: true, reason: 'success', info: rspp}
  }

  async op_history(){
    var log_type_id = this.ctx.params[0];
    var log_data = await this.ctx.model.Log.findAll({
      order: [
            ['id', 'desc'],
        ],
      attributes: ['operation', 'updated_at'],
      where: { log_type_id: log_type_id },
      include: [ 
          { model: this.ctx.model.LogType, attributes: ['title_i18n']},
          { model: this.ctx.model.User, attributes: ['username']}
       ]
    });
    var log_arr = [];
    log_data.forEach(function(data){
      var a = JSON.parse(data.dataValues.operation);
        a.logs['updated_at'] = data.dataValues.updated_at;
        a.logs['username'] = data.dataValues.user == null ? 'remote' : data.dataValues.user.username;
        log_arr.push(a.logs);
    });

    this.ctx.body = log_arr
  }
}

module.exports = PlayerController;
