'use strict';
const moment = require('moment');
const Controller = require('egg').Controller;

//广播验证规则
const editRule = {
    channel: 'array',
    style: 'id',
    content: 'string',
    group: 'array',
  };

class BroadcastController extends Controller {
  async index(){
    const notice_list = await this.ctx.model.Horseracelamp.findAll();
    this.ctx.body = notice_list;
  }
  async create(){
    const request = this.ctx.request.body.data[0];
    if(this.ctx.request.body.action == '1'){
      try {
        //this.ctx.logger.info(request)
        var task = await this.ctx.model.Horseracelamp.create({
          realm_id: this.ctx.session.realm_id,
          group_id: request.server_id.toString(),
          channel_arr: request.channel.toString(),
          content: request.content,
          func_type: request.style,
          interval: request.repeat,
          start_time: request.start_time,
          end_time: request.end_time
        })
        var command = {
          cmd: "ServerHorseRaceLamp",
          id: task.dataValues.id,
          channel: request.channel,
          func_type:  request.style,
          content: request.content,
          server_id: request.server_id,
          start_time: request.start_time,
          end_time: request.end_time,
          type: 1,
          repeat: request.repeat * 60,
        }
        var result = await this.service.gmt.send_command(command,this.ctx.__('page_broadcast_success'));
        this.ctx.body = result;
      } catch(err) {
        this.ctx.logger.error(err)
        this.ctx.body = {state: false, reason: err.errors}
      }
    }else{
      try{
        var command = {
          cmd: "ServerHorseRaceLamp",
          id: request.id,
          realm_id: request.realm_id,
          channel: request.channel_arr.split(','),
          server_id: request.group_id.split(','),
          type: 0
        }
        var result = await this.service.gmt.send_command(command,this.ctx.__('page_broadcast_success'));
        if(result.state){
          var remove_to = await this.ctx.model.Horseracelamp.findOne({ where: { id: request.id }});
          await remove_to.destroy();
          this.ctx.body = {data: []};
        }else{
          this.ctx.body = {error: result.reason};
        }
      }catch(err){
        this.ctx.logger.error(err)
        this.ctx.response.rsp_table_error(err.errors);
      }
    }
  };
}

module.exports = BroadcastController;
