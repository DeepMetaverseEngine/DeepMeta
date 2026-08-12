'use strict';
const Controller = require('egg').Controller;

//验证规则
const editRule = {
    privilege: 'id',
    role: 'string',
  };

class PrivilegeController extends Controller {
  async create(){
    const request = this.ctx.request.body;
    try {
      this.ctx.logger.info(request)
      await this.ctx.validate(editRule, request);
      var command = {
        cmd: "SetRolePrivilege",
        privilege: request.privilege,
        role: request.role,
        operator:'GMT'
      }
      var result = await this.service.gmt.send_command(command,this.ctx.__('common_instructions_success'));
      this.ctx.body = result;
      await this.ctx.write_log(this.ctx.app.action.info, {
        customType:'privilege', 
        command: command, 
        result: result
      });

    } catch(err) {
      this.ctx.logger.error(err)
    }
  };
}

module.exports = PrivilegeController;
