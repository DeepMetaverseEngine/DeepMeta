'use strict';
const Controller = require('egg').Controller;

//附件邮件编辑规则
const editRule = {
    type:'id',
    title: 'string',
    content: 'string',
    level: 'id',
    vip: 'id',
    reason: 'string',
    role_list: {type: 'string', allowEmpty: true}
  };


//普通邮件编辑规则
const editCommonRule = {
    type:'id',
    title: 'string',
    content: 'string',
    role_list: {type: 'string', allowEmpty: true}
 };

class EmailController extends Controller {
  
  async create(){
    const ctx = this.ctx
    const request = this.ctx.request.body;
    try {
     this.ctx.logger.info(request)
      await this.ctx.validate(editRule, request);

      var item = request.item_list == undefined ? [] : request.item_list

      try {
        var task = await ctx.model.MailApply.create({
          title: request.title,
          content: request.content,
          attach: JSON.stringify(item),
          realm_id: this.service.realmselector.get_session_realmid(),
          server_id: request.group,
          level: request.level,
          vip: request.vip,
          role_id: request.type == 2 ? '' : request.role_list,
          reason: request.reason,
          status: 0
        })

         this.ctx.body = {state:true,reason:this.ctx.__('common_instructions_send_success')}
        }
      catch(err) {
        this.ctx.logger.error(err)
        this.ctx.body = {state: false, reason: this.ctx.__('common_instructions_send_failed')}
      }
    } catch(err) {
      this.ctx.logger.error(err)
      this.ctx.body = {state: false, reason: this.ctx.__('common_instructions_validation_failed')}
    }
  };


  async review(){
    var review_list = await this.ctx.model.MailApply.findAll({where: {status: 0}});
    this.ctx.body = review_list
  }

  async review_act(){
    const ctx = this.ctx
    const request = this.ctx.request.body;
    this.ctx.logger.info(request)

    for (var i = 0; i < request.idx.length; i++) {
      var task = await ctx.model.MailApply.findOne({
         where: {id: request.idx[i]}
      });
      if(!ctx.helper.is_empty(task)){
        if(request.type == 1) {
          var result = await this.send_mail(task)
          if(result.state){
            task.status = 1
          }else{
            task.status = -1
          }
          task.desc = JSON.stringify(result)
        }else {
          task.status = 3
        }
        await task.save()
      }
      
    }

    this.ctx.body = {state:true,reason:this.ctx.__('common_instructions_success')}

  }


  async record(){
    const ctx = this.ctx
    const request = ctx.request.body;
    var review_list = await ctx.model.MailApply.findBy(request.date1, request.date2, request.server_id, request.role_id);
    ctx.body = {state: true, info:review_list, reason: ctx.__('common_instructions_success')}
  }

  async send_mail(task){
    var command = {
          realm_id: task.realm_id,
          cmd: "ServerMail",
          group: [task.server_id],
          vip: task.vip,
          level: task.level,
          type: task.role_id == '' ? 2 : 3,
          role_list: task.role_id == '' ? [] : task.role_id.split(','),
          mail: {
            title: task.title,
            content: task.content,
            item: task.attach
          }
    }
    return await this.service.gmt.send_command(command,this.ctx.__('page_email_success'));
  }
}

module.exports = EmailController;
