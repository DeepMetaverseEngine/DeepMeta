'use strict';
const Controller = require('egg').Controller;
//创建规则
const createRule = {
    name: 'string',
    start_date: 'string',
    end_date: 'string',
    level: 'id',
    account_start_date: 'string',
    account_end_date: 'string',
    channels: 'string',
    rewards: 'string',
    prefix: 'string',
    mutex_ids: 'string'
  };

//编辑规则
const editRule = {
    id: 'id',
    name: 'string',
    start_date: 'string',
    end_date: 'string',
    level: 'id',
    account_start_date: 'string',
    account_end_date: 'string',
    channels: 'string',
    rewards: 'string',
    prefix: 'string',
    mutex_ids: 'string'
  };

class ActivityController extends Controller {
  async index(){
    this.ctx.body = await this.ctx.service.activity.show()
  };

  async create(){
    const ctx = this.ctx;
    let action = ctx.request.body.action;
    let data = ctx.get_request_primary_data(ctx.request.body.data)

    if(action == 'edit') {
      try {
        await ctx.validate(editRule, data);
        var update = await ctx.model.Activity.find(data.id);
        await ctx.model.Activity.update({
            level: data.level,
            rewards: data.rewards,
            channels: data.channels,
            name: data.name,
            account_start_date: data.account_start_date,
            account_end_date: data.account_end_date,
            start_date: data.start_date,
            end_date: data.end_date,
            prefix: data.prefix,
            mutex_ids: data.mutex_ids
          },
          {
            where:{id: data.id
            }
        });

        result = await this.ctx.model.Activity.findOne({
          where:{id: data.id}
        });

        await ctx.write_log(ctx.app.action.update, {before: update, after: result, filter: ['gmt_key']})

        ctx.body = {data:[result]}
      } catch(err) {
        ctx.response.rsp_table_field_errors(err.errors);
      }


    }else if(action == 'remove') {
      var task = await ctx.model.Activity.findOne({
         where: {id: data.id}
      });

      await task.destroy();
      await ctx.write_log(ctx.app.action.destroy, {after: task, filter: ['gmt_key']})
      ctx.body = {data:[]};
    }else if(action == 'create') {
       try {
        await ctx.validate(createRule, data);
      }catch(err){
        ctx.response.rsp_table_field_errors(err.errors);
        return;
      }
      try {
        var d = Date.now();
        var task = await ctx.model.Activity.create({
          level: data.level,
          rewards: data.rewards,
          channels: data.channels,
          name: data.name,
          account_start_date: data.account_start_date,
          account_end_date: data.account_end_date,
          start_date: data.start_date,
          end_date: data.end_date,
          prefix: data.prefix,
          mutex_ids: data.mutex_ids
        })
        await ctx.write_log(ctx.app.action.create, {after: task})
        var result = await this.ctx.model.Activity.findOne({
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


  async edit(){};

  async update(){};

  async destroy(){};

  async new() {
    this.ctx.body = 'new';
  }
}

module.exports = ActivityController;
