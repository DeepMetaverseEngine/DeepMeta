'use strict';
const Controller = require('egg').Controller;

//编辑验证规则
const editRule = {
  privilege: 'id',
  password: {type: 'password', allowEmpty: true},
  is_enable: 'id',
};

//创建验证规则
const createRule = {
  username: 'string',
  privilege: 'id',
  password: 'password',
  is_enable: 'id',
};

class WhitelistController extends Controller {
  async index(){
    let ctx = this.ctx;
    ctx.body = await ctx.service.whitelist.show()
  };

  async create(){
    const ctx = this.ctx;
    let action = ctx.request.body.action;
    let data = ctx.get_request_primary_data(ctx.request.body.data)
    if(action == 'edit') {
      try {
        ctx.validate(editRule, data);
        var account = await ctx.model.Whitelist.find(data.id);
        await ctx.model.Whitelist.update({
            privilege: data.privilege,
            is_enable: data.is_enable
          },
          { 
            where:{id: data.id}
        });

        result = await ctx.service.whitelist.find_one(data.id)

        if(!!data.password){
          result.updatePassword(data.password);
        }
        await ctx.write_log(ctx.app.action.update, {filter:['pwd_encrypt','salt'], before:account,after:result})
        ctx.body = {data:[result]}
      } catch(err) {
        ctx.response.rsp_table_field_errors(err.errors);
        ctx.logger.info(err)
      }
    }else if(action == 'remove') {
       var task = await ctx.model.Whitelist.findOne({
         where: {id: data.id}
      });
      await task.destroy();
      await ctx.write_log(ctx.app.action.destroy, {filter:['pwd_encrypt','salt'], after:task})

      ctx.body = {data:[]}
    }else if(action == 'create') {
      try {
        ctx.validate(createRule, data);
      }
      catch(err){
        ctx.response.rsp_table_field_errors(err.errors);
        return;
      }
      try {
          var task = await ctx.model.Whitelist.create({
            username: data.username, 
            privilege: data.privilege,
            is_enable: data.is_enable
          })
          var result = await ctx.service.whitelist.find_one(task.id)
          await result.updatePassword(data.password);
          await ctx.write_log(ctx.app.action.create, {filter:['pwd_encrypt','salt'], after:result})
          ctx.body = {data:[result]}
        } 
      catch(err) {
        ctx.logger.info(err)
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

module.exports = WhitelistController;
