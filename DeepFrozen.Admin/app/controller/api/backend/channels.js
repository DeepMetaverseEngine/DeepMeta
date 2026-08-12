'use strict';
const Controller = require('egg').Controller;
//创建规则
const createRule = {
  type: 'string',
  sdk_name: 'string',
  game_id: 'id',
  api_key: 'string',
  need_sign: 'id',
  query_order: 'id',
  verify_url: 'string',
  query_url: 'string',
  sign_url: 'string',
  };

//编辑规则
const editRule = {
  id: 'id',
  type: 'string',
  sdk_name: 'string',
  game_id: 'id',
  api_key: 'string',
  need_sign: 'id',
  query_order: 'id',
  verify_url: 'string',
  query_url: 'string',
  sign_url: 'string',
      };

class ChannelsController extends Controller {
  async index(){
    this.ctx.body = await this.ctx.service.channel.show()
  };

  async create(){
    const ctx = this.ctx;
    let action = ctx.request.body.action;
    let data = ctx.get_request_primary_data(ctx.request.body.data)
    if(action == 'edit') {
      try {
        await ctx.validate(editRule, data);
      }catch(err){
        ctx.response.rsp_table_field_errors(err.errors);
        return;
      }
      try {
        var update = await ctx.model.Channel.find(data.id);
        await ctx.model.Channel.update({
                type: data.type,
                sdk_name: data.sdk_name,
                game_id: data.game_id,
                api_key: data.api_key,
                need_sign: data.need_sign,
                query_order: data.query_order,
                verify_url: data.verify_url,
                query_url: data.query_url,
                sign_url: data.sign_url,
          },
          { 
            where:{id: data.id
            }
          });
        
        result = await this.ctx.model.Channel.findOne({
          where:{id: data.id}
        });

        await ctx.write_log(ctx.app.action.update, {before: update, after: result, filter: ['gmt_key']})
       
        ctx.body = {data:[result]}
      } catch(err) {
        ctx.response.rsp_table_field_errors(err.errors);
      }
      

    }else if(action == 'remove') {
      var task = await ctx.model.Channel.findOne({
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
        var task = await ctx.model.Channel.create({
          type: data.type,
          sdk_name: data.sdk_name,
          game_id: data.game_id,
          api_key: data.api_key,
          need_sign: data.need_sign,
          query_order: data.query_order,
          verify_url: data.verify_url,
          query_url: data.query_url,
          sign_url: data.sign_url,
        })
        await ctx.write_log(ctx.app.action.create, {after: task})
        var result = await this.ctx.model.Channel.findOne({
            where:{id: task.id}
          });
        ctx.body = {data:[result]}
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

module.exports = ChannelsController;
