'use strict';
const Controller = require('egg').Controller;
//创建规则
const createRule = {
    type: 'id',
    sub_type: 'id',
    condition: 'id',
    value: 'id',
  };

//编辑规则
const editRule = {
    id: 'id',
    type: 'id',
    sub_type: 'id',
    condition: 'id',
    value: 'id',
  };

const mailistRule = {
  id: {type: 'id', allowEmpty: true},
  name: 'string',
  address: 'email',
  enable: 'id'
}  
class EarlyWarningController extends Controller {
  async settings_show(){
    this.ctx.body = await this.ctx.service.earlywarning.show()
  };

  async record_show(){
    this.ctx.body = await this.ctx.service.earlywarning.record_show()
  };

  async settings_edit(){
    const ctx = this.ctx;
    let action = ctx.request.body.action;
    let data = ctx.get_request_primary_data(ctx.request.body.data)

    if(action == 'edit') {
      try {
        await ctx.validate(editRule, data);
        await ctx.model.EarlyWarningSettings.update({
            type: data.type,
            sub_type: data.sub_type,
            condition: data.condition,
            value: data.value,
            enable: data.enable
          },
          {
            where:{id: data.id
            }
        });

        result = await this.ctx.model.EarlyWarningSettings.findOne({
          where:{id: data.id}
        });

        ctx.body = {data:[result]}
      } catch(err) {
        this.ctx.logger.error(err)
        ctx.response.rsp_table_field_errors(err.errors);
      }


    }else if(action == 'remove') {
      var task = await ctx.model.EarlyWarningSettings.findOne({
         where: {id: data.id}
      });

      await task.destroy();
      // await ctx.write_log(ctx.app.action.destroy, {after: task, filter: ['gmt_key']})
      ctx.body = {data:[]};
    }else if(action == 'create') {
       try {
        await ctx.validate(createRule, data);
      }catch(err){
        this.ctx.logger.error(err)
        ctx.response.rsp_table_field_errors(err.errors);
        return;
      }
      try {
        var d = Date.now();
        var task = await ctx.model.EarlyWarningSettings.create({
          type: data.type,
          sub_type: data.sub_type,
          condition: data.condition,
          value: data.value,
          enable: data.enable
        })
        // await ctx.write_log(ctx.app.action.create, {after: task})
        var result = await this.ctx.model.EarlyWarningSettings.findOne({
            where:{id: task.id}
          });
        ctx.body = {data:[result]}
        }
      catch(err) {
        this.ctx.logger.error(err)
        ctx.response.rsp_table_error(err.errors);
      }
    }
  };

async mail_list(){
  //await this.ctx.service.talkRobot.send_message('https://oapi.dingtalk.com/robot/send?access_token=3785f489b5b8580822292b1e56e2f9c23e4c672c47113c18ceb87a0cc95dac8f','报警!\n1234567890');
  this.ctx.body = await this.ctx.model.Mailist.findAll()
}

async mail_list_eidt(){
  const ctx = this.ctx;
  let action = ctx.request.body.action;
  let data = ctx.get_request_primary_data(ctx.request.body.data)
  await ctx.validate(mailistRule, data);
  if(action == 'edit') {
    try {
      await ctx.model.Mailist.update({
          name: data.name,
          address: data.address,
          enable: data.enable
        }, {
          where:{id: data.id}
        });
      result = await this.ctx.model.Mailist.findOne({
        where:{id: data.id}
      });
      ctx.body = {data:[result]}
    } catch(err) {
      this.ctx.logger.error(err)
      ctx.response.rsp_table_field_errors(err.errors);
    }
  }else if(action == 'remove') {
    var task = await ctx.model.Mailist.findOne({
       where: {id: data.id}
    });
    await task.destroy();
    ctx.body = {data:[]};
  }else if(action == 'create') {
    try {
      var task = await ctx.model.Mailist.create({
          name: data.name,
          address: data.address,
          enable: data.enable
      })
      var result = await this.ctx.model.Mailist.findOne({
          where:{id: task.id}
      });
          ctx.body = {data:[result]}
          await this.ctx.service.warnmail.send_test_message(result.dataValues.address, '添加邮件成功')
    }catch(err) {
        this.ctx.logger.error(err)
        ctx.response.rsp_table_error(err.errors);
      }
  }
}
 
}

module.exports = EarlyWarningController;
