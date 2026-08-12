'use strict';

const Controller = require('egg').Controller;

class GuildController extends Controller {
  async show_guild(){
    const ctx = this.ctx;
    try {
      var server_id = this.ctx.request.body.server_id;
      var result;
      var command = {
        cmd: "ServerQueryGuildList",
        realm_id: await this.service.realmselector.get_realm_by_server_id(server_id),
        server_id: server_id,
      }

      var result = await this.service.gmt.send_command(command,'success');
      if(result.state){
        var res = JSON.parse(result.ext);
        this.ctx.body = {state: true, data: res, reason: result.reason};
      }else {
        this.ctx.body = result;
      }
    } catch(err) {
      this.ctx.logger.error(err)
      this.ctx.response.rsp_table_field_errors(err.errors);
    }
}

  async operate_changeguild(){
    const ctx = this.ctx;
    var server_id = ctx.request.body.server_id;
    var data = ctx.get_request_primary_data(ctx.request.body.data);
    data['server_id'] = server_id;
    try {
      var result;
      var command = {
        cmd: "ServerModifyGuildName",
        realm_id: await this.service.realmselector.get_realm_by_server_id(server_id),
        server_id: server_id,
        guild_id: data.id,
        new_name: data.new_guild_name,
      }

      result = await this.service.gmt.send_command(command,'success');
      if(result.state){
        this.ctx.body = result;
        await this.ctx.write_log(this.ctx.app.action.info, {
          customType:'modify_guildname', 
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

  async operate_changeguildnotice(){
    const ctx = this.ctx;
    var server_id = ctx.request.body.server_id;
    var data = ctx.get_request_primary_data(ctx.request.body.data);
    data['server_id'] = server_id;
    try {
      var result;
      var command = {
        cmd: "ServerModifyGuildNotice",
        realm_id: await this.service.realmselector.get_realm_by_server_id(server_id),
        server_id: server_id,
        guild_id: data.id,
        new_notice: data.new_guild_notice,
      }

      result = await this.service.gmt.send_command(command,'success');
      if(result.state){
        this.ctx.body = result;
        await this.ctx.write_log(this.ctx.app.action.info, {
          customType:'modify_guildnotice', 
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

}

module.exports = GuildController;
