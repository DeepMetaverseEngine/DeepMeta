'use strict';
const Controller = require('egg').Controller;

//编辑验证规则
const editRule = {
  //privilege: 'id',
  password: {type: 'password', allowEmpty: true}
};

//创建验证规则
const createRule = {
  username: 'email',
//  privilege: 'id',
  password: 'password'
};

class UsersController extends Controller {
  async index(){
    let ctx = this.ctx;
    ctx.body = await ctx.service.users.show()
    //this.ctx.logger.info("isAuthenticated ", this.ctx.isAuthenticated())
    //this.ctx.logger.info("userinfo ", this.ctx.user)
   // this.ctx.logger.info("session ", this.ctx.session)

  };

  async create(){
    const ctx = this.ctx;
    let data, action;
    if(ctx.request.body.src == 'post'){
      data = ctx.request.body;
      action = data.action;
      delete data.src
      delete data.action
      data.privileges = data.privileges.sort((a,b)=>{return a-b}).join(',')
    }else{
      data = ctx.get_request_primary_data(ctx.request.body.data)
      action = ctx.request.body.action
    }
    delete data.group

    if(action == 'edit') {
      try {
        ctx.validate(editRule, data);
        var user = await ctx.model.User.find(data.id);
        await ctx.model.User.update(data,
          {
            where:{id: data.id}
        });
        result = await ctx.service.users.find_one(data.id)

        if(!!data.password){
          result.updatePassword(data.password);
        }
        await ctx.write_log(ctx.app.action.update, {filter:['pwd_encrypt','salt'], before:user,after:result})
        ctx.body = {reason: this.ctx.__('common_instructions_success'), data:[result]}
      } catch(err) {
        this.ctx.logger.error(err)
        ctx.response.rsp_table_field_errors(err.errors);
      }
    }else if(action == 'remove') {
       var task = await ctx.model.User.findOne({
         where: {id: data.id}
      });
      await task.destroy();
      await ctx.write_log(ctx.app.action.destroy, {filter:['pwd_encrypt','salt'], after:task})
      ctx.body = {reason: this.ctx.__('common_instructions_success'), data:[]}
    }else if(action == 'create') {
      try {
          ctx.validate(createRule, data);
          var task = await ctx.model.User.create(data)
          task.updatePassword(data.password);
          var result = await ctx.service.users.find_one(task.id)
          await result.updatePassword(data.password);
          await ctx.write_log(ctx.app.action.create, {filter:['pwd_encrypt','salt'], after:result})
          ctx.body = {reason: this.ctx.__('common_instructions_success'), data:[result]}
        } 
      catch(err) {
        console.log(err)
        ctx.response.rsp_table_field_errors(err.errors);
      }
    }
  };

  async group(){
    const ctx = this.ctx;
    let data = ctx.request.body
    data.group_privileges = data.group_privileges.sort((a,b)=>{return a-b}).join(',')
    if(data.action == 'create'){
      delete data.action
      try {
        await ctx.model.Usergroup.create(data)
        let group_p = {}, group_l = await ctx.model.Usergroup.findAll()
        group_l.forEach(function(v, i, a){
          group_p[v.dataValues.id] = v.dataValues.group_privileges.split(',')
        })
        ctx.app.messenger.sendToApp('update_var', { varname: 'group_p', value: group_p})

        ctx.body = {reason: this.ctx.__('common_instructions_success')}
      } 
    catch(err) {
      console.log(err)
      ctx.response.rsp_table_field_errors(err.errors);
    }
    }else if(data.action == 'edit'){
      try {
        delete data.action
        await ctx.model.Usergroup.update(data,
          {
            where:{id: data.id}
        });
        let group_p = {}, group_l = await ctx.model.Usergroup.findAll()
        group_l.forEach(function(v, i, a){
          group_p[v.dataValues.id] = v.dataValues.group_privileges.split(',')
        })
        ctx.app.messenger.sendToApp('update_var', { varname: 'group_p', value: group_p})

        ctx.body = {reason: this.ctx.__('common_instructions_success')}
      } catch(err) {
        this.ctx.logger.error(err)
        ctx.response.rsp_table_field_errors(err.errors);
      }
    }else if(data.action == 'remove'){
      var task = await ctx.model.Usergroup.findOne({
        where: {id: data.id}
      });
      await task.destroy();
      let group_p = {}, group_l = await ctx.model.Usergroup.findAll()
      group_l.forEach(function(v, i, a){
        group_p[v.dataValues.id] = v.dataValues.group_privileges.split(',')
      })
      ctx.app.messenger.sendToApp('update_var', { varname: 'group_p', value: group_p})
      ctx.body = {reason: this.ctx.__('common_instructions_success')}
    }else{
      ctx.body = { reason: 'error' }
    }
  }


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

module.exports = UsersController;
