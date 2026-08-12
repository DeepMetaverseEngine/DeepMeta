'use strict';
const Controller = require('egg').Controller;
//创建规则
const createRule = {
    title: 'string',
    content: 'string',
    started_at: 'string',
    ended_at: 'string',
  };

//编辑规则
const editRule = {
        id: 'id',
        title: 'string',
        content: 'string',
        started_at: 'string',
        ended_at: 'string',
      };

class NoticesController extends Controller {
  async index(){
    this.ctx.body = await this.ctx.service.notice.show_all()
  };

  async create(){
    const ctx = this.ctx;
    let action = ctx.request.body.action;
    let data = ctx.get_request_primary_data(ctx.request.body.data)
    
    if(action == 'edit') {
      try {
        this.ctx.logger.info(data);
        await ctx.validate(editRule, data);
        var update = await ctx.model.Notice.find(data.id);
        await ctx.model.Notice.update({
            is_open: data.is_open || 0,
            is_top: data.is_top || 0,
            title: data.title,
            content: data.content,
            started_at: data.started_at,
            ended_at: data.ended_at
          },
          { 
            where:{id: data.id
            }
        });
        
        result = await this.ctx.model.Notice.findOne({
          where:{id: data.id}
        });

        await ctx.write_log(ctx.app.action.update, {before: update, after: result})
       
        ctx.body = {data:[result]}
      } catch(err) {
        ctx.response.rsp_table_field_errors(err.errors);
      }
      

    }else if(action == 'remove') {
      var task = await ctx.model.Notice.findOne({
         where: {id: data.id}
      });

      await task.destroy();
      await ctx.write_log(ctx.app.action.destroy, {after: task})
      ctx.body = {data:[]};
    }else if(action == 'create') {
       try {
        await ctx.validate(createRule, data);
      }catch(err){
        ctx.response.rsp_table_field_errors(err.errors);
        return;
      }
      try {
            var task = await ctx.model.Notice.create({
              is_open: data.is_open || 0,
              is_top: data.is_top || 0,
              title: data.title,
              content: data.content,
              started_at: data.started_at,
              ended_at: data.ended_at
            })

            var result = await ctx.model.Notice.findOne({
              where:{id: task.id}
            });
            ctx.body = {data:[result]}
            await ctx.write_log(ctx.app.action.create, {after: result})
        }
      catch(err) {
        this.ctx.logger.info(err)
        ctx.response.rsp_table_error(err.errors);
      }
    }
  };

  async show(){
    this.ctx.body = 'show';
  };

  async edit(){};

  async update(){};

  async destroy(){};

  async new() {
    this.ctx.body = 'new';
  }
}

module.exports = NoticesController;
