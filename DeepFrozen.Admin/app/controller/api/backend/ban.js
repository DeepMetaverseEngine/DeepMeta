'use strict';
const Controller = require('egg').Controller;
const moment = require('moment');

//验证规则
const editRule = {
    end_dt: 'datetime',
    digit_id: 'id',
    online_state: 'string',
    server_id: 'id',
  };

class RoleBanController extends Controller {
  async create(){
    const action = this.ctx.request.body['action'];
    const data_to = this.ctx.get_request_primary_data(this.ctx.request.body.data)
    //if(action == 'edit'){
    //  data_to['end_dt'] = moment(data_to.end_dt).format('YYYY-MM-DD HH:mm:ss')
    //}else{
      data_to['end_dt'] = moment(data_to.end_dt).format('YYYY-MM-DD HH:mm:ss')
    const types = { 'edit': 1, 'remove': -1 }
      try {
        await this.ctx.validate(editRule, data_to);
        var command = {
          cmd: "ban",
          realm_id: await this.service.realmselector.get_realm_by_server_id(data_to.server_id),
          date: data_to.end_dt,
          role: data_to.role_name,
          type: types[action],
          reason: data_to.online_state,
        }
        var resule = await this.service.gmt.send_command(command,this.ctx.__('common_instructions_success'));
        if(resule.state){
          this.ctx.body = {data: []};
        }else{
          this.ctx.response.rsp_table_error([{message: this.ctx.__('common_instructions_op_failed'), value: resule.reason }]);
        }
        data_to['action'] = action
        await this.ctx.write_log(this.ctx.app.action.info, {
          customType:'ban', 
          command: data_to, 
          result: resule
        });
      } catch(err) {
        this.ctx.response.rsp_table_error([{message: err.message, value: err.errors[0]['field']}]);
        console.log(err);
      }

  };
}

module.exports = RoleBanController;
