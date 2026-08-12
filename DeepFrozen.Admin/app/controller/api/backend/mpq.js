'use strict';
const Controller = require('egg').Controller;
//创建规则
const createRule = {
    id:'id',
    cdn_url: 'string'
  };

//编辑规则
const editRule = {
        cdn_url: 'string'
      };

class MpqController extends Controller {
  async index(){
    this.ctx.body = await this.ctx.service.mpq.show()
  };

  async create(){
    const ctx = this.ctx;
    let action = ctx.request.body.action;
    let data = ctx.get_request_primary_data(ctx.request.body.data)
    
    if(action == 'edit') {
      try {
        await ctx.validate(editRule, data);
        var mpq = await ctx.model.Mpq.find(data.id);
        await ctx.model.Mpq.update({
            cdn_url: data.cdn_url.trim(),
            remark: data.remark
          },
          { 
            where:{id: data.id
            }
        });
        
        result = await this.ctx.model.Mpq.findOne({
          where:{id: data.id}
        });

        await ctx.write_log(ctx.app.action.update, {before: mpq, after: result, filter: ['gmt_key']})
       
        ctx.body = {data:[result]}
      } catch(err) {
        this.ctx.logger.error(err)
        ctx.response.rsp_table_field_errors(err.errors);
      }
      

    }else if(action == 'remove') {
      var task = await ctx.model.Mpq.findOne({
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
        var task = await ctx.model.Mpq.create({
          id:data.id, 
          cdn_url: data.cdn_url.trim(),
          remark: data.remark
        })
        await ctx.write_log(ctx.app.action.create, {after: task})
        var result = await this.ctx.model.Mpq.findOne({
            where:{id: data.id}
          });
        ctx.body = {data:[result]}
        }
      catch(err) {
        this.ctx.logger.error(err)
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

module.exports = MpqController;
