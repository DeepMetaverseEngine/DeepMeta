'use strict';
const Controller = require('egg').Controller;

//创建规则
const createRule = {
    id:'id',
    name: 'string',
    address: 'string',
    is_open: 'id',
    state: 'id',
    state_text: 'string',
    view_rgba: 'string',
    view_realm_index: 'id',
    gmt_key: 'string',
    gmt_url: 'string',
    pay_url: 'string',
  };

//编辑规则
const editRule = {
        name: 'string',
        address: 'string',
        is_open: 'id',
        state: 'id',
        state_text: 'string',
        view_rgba: 'string',
        view_realm_index: 'id',
        gmt_key: 'string',
        gmt_url: 'string',
        pay_url: 'string',
      };

class RealmsController extends Controller {
  async index(){
    this.ctx.body = await this.ctx.service.serverlist.show_realmlist()
  };

  async create(){
    const ctx = this.ctx;
    let action = ctx.request.body.action;
    let data = ctx.get_request_primary_data(ctx.request.body.data)
    
    if(action == 'edit') {
      try {
        await ctx.validate(editRule, data);
        var realm = await ctx.model.Realm.find(data.id);
        await ctx.model.Realm.update({
            name: data.name,
            address: data.address,
            is_open: data.is_open,
            state: data.state,
            state_text: data.state_text,
            view_rgba: data.view_rgba,
            view_realm_index: data.view_realm_index,
            gmt_key: data.gmt_key.trim(),
            gmt_url: data.gmt_url.trim(),
            pay_url: data.pay_url.trim()
          },
          { 
            where:{id: data.id
            }
        });
        
        result = await this.ctx.model.Realm.findOne({
          where:{id: data.id}
        });

        await ctx.write_log(ctx.app.action.update, {before: realm, after: result, filter: ['gmt_key']})
       
        ctx.body = {data:[result]}
      } catch(err) {
        ctx.response.rsp_table_field_errors(err.errors);
      }
      

    }else if(action == 'remove') {
      var task = await ctx.model.Realm.findOne({
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
        var now = new Date();
        var task = await ctx.model.Realm.create({
          id:data.id, 
          name: data.name, 
          address: data.address,
          is_open: data.is_open, 
          state: data.state, 
          state_text: data.state_text,
          view_rgba: data.view_rgba,
          view_realm_index: data.view_realm_index,
          gmt_key: data.gmt_key.trim(),
          gmt_url: data.gmt_url.trim(),
          pay_url: data.pay_url.trim()
        })
        await ctx.write_log(ctx.app.action.create, {after: task, filter: ['gmt_key']})
        var result = await this.ctx.model.Realm.findOne({
            where:{id: data.id}
          });
        ctx.body = {data:[result]}
        }
      catch(err) {
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

module.exports = RealmsController;
