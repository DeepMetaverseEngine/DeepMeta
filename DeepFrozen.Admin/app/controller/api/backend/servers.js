'use strict';
const Controller = require('egg').Controller;

//创建规则
const createRule = {
        id: 'id',
        name: 'string',
        realm_id: 'id',
        group: 'id',
        is_show: 'id',
        state: 'id',
        view_index: 'id',
      };

//编辑规则
const editRule = {
        name: 'string',
        realm_id: 'id',
        group: 'id',
        is_show: 'id',
        state: 'id',
        view_index: 'id',
      };

class ServerController extends Controller {
  async index(){
    this.ctx.body = await this.ctx.service.serverlist.show_serverlist()
  };

  async create(){
    const ctx = this.ctx;
    let action = ctx.request.body.action;
    let data = ctx.get_request_primary_data(ctx.request.body.data)
    
    if(action == 'edit') {

      try {
        await ctx.validate(editRule, data);
        var server = await ctx.model.Server.find(data.id);

        var srvCfg = ctx.helper.get_srv_state(data.state)
        await ctx.model.Server.update({
            name: data.name,
            realm_id: data.realm_id,
            is_open: data.state == '4' ? 0 : 1,
            is_show: data.is_show,
            state: data.state,
            state_text: srvCfg.state_text,
            flag: data.flag,
            view_rgba: srvCfg.view_rgba,
            view_index: data.view_index,
            icon: srvCfg.icon,
            open_at: data.open_at,
            updated_at: new Date()
          },
          { 
            where:{id: data.id}
        });
        if(data.recommend){
          var recommend_info = await ctx.model.RecommendServer.find(data.id);
          if(this.ctx.helper.is_empty(recommend_info)){
             var recommend = await ctx.model.RecommendServer.create({
                server_id:data.id,
                period: 10
              })
          }
        }else {
         await ctx.model.RecommendServer.destroy({
            where: {
              server_id:data.id
            }
          });
        }

        var result = await this.ctx.model.Server.findOne({
          where:{id: data.id},
          attributes: ['id', 'name','realm_id','group','is_open','is_show','state','state_text','flag','view_rgba','view_index','view_realm_index', 'icon','open_at'],
          include: [ { model: this.ctx.model.Realm, as: 'realm', attributes: ['name']},
                    { model: this.ctx.model.RecommendServer, as: 'recommend', attributes: ['server_id','period',[this.ctx.model.literal('1'), 'enable']]}
            ]
        });
        ctx.body = {data:[result]}
        if(data.open_at != server.open_at){
          await this.service.mangs.managegs({realm_id: data.realm_id, adm_command: 'streload'})
        }
        await ctx.write_log(ctx.app.action.update, {before:server,after:result})

      } catch(err) {
        ctx.logger.info(err)
        ctx.response.rsp_table_field_errors(err.errors);
      }


    }else if(action == 'remove') {
      var task = await ctx.model.Server.findOne({
         where: {id: data.id}
      });

      await task.destroy();
      await ctx.write_log(ctx.app.action.destroy, {after:task})

      ctx.body = {data:[]};

    }else if(action == 'create') {

      try {
        await ctx.validate(createRule, data);
      }catch(err){
        ctx.response.rsp_table_field_errors(err.errors);
        return;
      }
      try {
        var srvCfg = ctx.helper.get_srv_state(data.state)
        var now = new Date();
        var task = await ctx.model.Server.create({
          id:data.id,
          name: data.name,
          realm_id: data.realm_id,
          group: data.group,
          is_open: data.state == '4' ? 0 : 1,
          is_show: data.is_show,
          state: data.state,
          state_text: srvCfg.state_text,
          flag: data.flag,
          view_rgba: srvCfg.view_rgba,
          view_index: data.view_index,
          icon: srvCfg.icon,
          open_at: data.open_at
        })

        if(data.recommend){
          var recommend_info = await ctx.model.RecommendServer.find(data.id);
          if(this.ctx.helper.is_empty(recommend_info)){
             var recommend = await ctx.model.RecommendServer.create({
                server_id:data.id,
                period: 10
              })
          }
        }else {
         await ctx.model.RecommendServer.destroy({
            where: {
              server_id:data.id
            }
          });
        }

        await ctx.write_log(ctx.app.action.create, {after:task})
        result = await this.ctx.model.Server.findOne({
            where:{id: data.id},
            attributes: ['id', 'name','realm_id','group','is_open','is_show','state','state_text','flag','view_rgba','view_index','view_realm_index', 'icon'],
            include: [ { model: this.ctx.model.Realm, as: 'realm', attributes: ['name']},
             { model: this.ctx.model.RecommendServer, as: 'recommend', attributes: ['server_id','period',[this.ctx.model.literal('1'), 'enable']]} ]
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

module.exports = ServerController;
