'use strict';
const Controller = require('egg').Controller;
//创建规则
const createRule = {
    id:'id',
    sdk_name: 'string',
    os_type: 'id',
    is_enable: 'id',
    least_build: 'id',
    current_build: 'id',
    update_url: 'string',
  };

//编辑规则
const editRule = {
        sdk_name: 'string',
        os_type: 'id',
        is_enable: 'id',
        least_build: 'id',
        current_build: 'id',
        update_url: 'string',
      };

class UpdatesController extends Controller {
  async index(){
    this.ctx.body = await this.ctx.service.update.show()
  };

  async create(){
    const ctx = this.ctx;
    let action = ctx.request.body.action;
    let data = ctx.get_request_primary_data(ctx.request.body.data)
    
    if(action == 'edit') {
      try {
        await ctx.validate(editRule, data);
        var update = await ctx.model.Update.find(data.id);
        await ctx.model.Update.update({
            sdk_name: data.sdk_name,
            os_type: data.os_type,
            is_enable: data.is_enable,
            least_build: data.least_build,
            current_build: data.current_build,
            update_url: data.update_url,
          },
          { 
            where:{id: data.id
            }
        });
        
        result = await this.ctx.model.Update.findOne({
          where:{id: data.id}
        });

        await ctx.write_log(ctx.app.action.update, {before: update, after: result, filter: ['gmt_key']})
       
        ctx.body = {data:[result]}
      } catch(err) {
        ctx.response.rsp_table_field_errors(err.errors);
      }
      

    }else if(action == 'remove') {
      var task = await ctx.model.Update.findOne({
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
        var task = await ctx.model.Update.create({
          id:data.id, 
          sdk_name: data.sdk_name, 
          os_type: data.os_type,
          is_enable: data.is_enable, 
          least_build: data.least_build, 
          current_build: data.current_build,
          update_url: data.update_url,
        })
        await ctx.write_log(ctx.app.action.create, {after: task})
        var result = await this.ctx.model.Update.findOne({
            where:{id: data.id}
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

module.exports = UpdatesController;
