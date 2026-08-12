'use strict';
const Controller = require('egg').Controller;

//验证规则
const editRule = {
    op_reason: 'string',
    type: ['1', '2', '3', '4', '5', '6', '-1'],
  };

class BlacklistController extends Controller {
  async create(){
    const request = this.ctx.get_request_primary_data(this.ctx.request.body.data);
    var server_id = request.server_id;
    try {
      try{
        await this.ctx.validate(editRule, request);
      }catch(err){
        this.ctx.response.rsp_table_field_errors(err.errors);
        return
      }
      var command = {
        cmd: "blacklist",
        realm_id: await this.service.realmselector.get_realm_by_server_id(server_id),
        channel: [0, 1, 2, 3, 4, 5],
        type: request.type,
        role: request.type == 6 ? request.digit_id : request.role_name,
        minutes: 60*24*365*99
      }
      var result = await this.service.gmt.send_command(command,this.ctx.__('common_instructions_success'));
      this.ctx.body = result;
      await this.ctx.write_log(this.ctx.app.action.info, {
        customType:'blacklist', 
        command: request, 
        result: result
      });

    } catch(err) {
      this.ctx.response.rsp_table_error(err.errors);
      this.ctx.logger.error(err)
    }
  };
}

module.exports = BlacklistController;
