'use strict';

var _ = require("underscore")._;

const Controller = require('egg').Controller;


const rule = {
        channel: 'id',
        sdkName: 'string',
        ostype: 'id',
        version: 'id',
      };


class ClientController extends Controller {
  async index() {
    const ctx = this.ctx;
    var ip = this.ctx.ip;
    
    var area_id = this.ctx.query.area_id
    var validate = await ctx.service.iplist.validate_ip(ip)
    var client_version = 0;
    var os_type = 5;
    if(this.ctx.query.clientVersion){
      client_version = this.ctx.query.clientVersion
      os_type = this.ctx.query.deviceType
    }



    let items;
    let recommend_servers;

    //hack 代码
    if(validate){
      items = await ctx.model.Server.findAll({
        include : [ { model: ctx.model.Realm, as: 'realm' } ],
      });
      recommend_servers = await ctx.model.RecommendServer.findAll({order: [['period', 'DESC']]});
      var srvCfg = this.ctx.helper.get_srv_state(1)
        items.forEach(function(server) {
          server.state = 1;
          server.is_open = true;
          server.state_text = srvCfg.state_text;
          server.view_rgba =  srvCfg.view_rgba;
          server.icon = srvCfg.icon;
        });
    }else {
      var server_id = 0
      // if(client_version == 20076){   //IOS平台评审服
      //    server_id = 2001
      //  }

      if(server_id == 0){
        var date = new Date();
          items = await ctx.model.Server.findAll({
            where: {
              is_show: true,
              open_at: {
                [ctx.model.Op.lte]: date,
              }
            },
            include: [ { model: ctx.model.Realm, as: 'realm', attributes: ['id','address','name','view_realm_index']}],
            // raw: true
          });
          recommend_servers = await ctx.model.RecommendServer.findAll({order: [['period', 'DESC']]});
        }else{
          items = await ctx.model.Server.findAll({
            where: {
              //is_show: true,
              id: server_id
            },
            include : [ { model: ctx.model.Realm, as: 'realm' } ],
            // raw: true
          });
          recommend_servers = [{server_id:server_id}]
        }
    }

    //排序
    items = _.sortBy(items, function(o) { return o.view_index; })

    items.forEach(function(server) {
      if(server.view_realm_index == -1){
          server.view_realm_index = server.realm.view_realm_index
          server.view_realm_name = server.realm.name
        }

      // if(server.is_open && server.flag != 0){
      //   if(server.flag == 1){
      //     server.icon = ctx.app.srvIconAlias.new
      //   }else if(server.flag == 2){
      //     server.icon = ctx.app.srvIconAlias.recommend
      //   }
      // }
    });

    if(area_id !== undefined && area_id != 0 && area_id != '0_0'){
      try {
        var group = _.groupBy(items,function(o){ return o.view_realm_index; })
        var area_server_list = []
        var area_list = area_id.split(',')
        area_list.forEach(function(area_ids) {
          var merge_id = 0
          var index = area_ids.indexOf('$')
          if(index > 0){
            merge_id = area_ids.substring(index + 1)
            area_ids = area_ids.substring(0,index)
          }
          var area = area_ids.split('_')
          var group_list = group[area[0]]
          for (var i = area[1] - 1; i < group_list.length; i++) {
            area_server_list.push(group_list[i])
          }

          if(merge_id != 0){
            area_server_list.forEach(function(server) {
              server.view_realm_index = group[merge_id][0].view_realm_index
              server.view_realm_name = group[merge_id][0].view_realm_name
            });
          }
        });
        
      }catch(e){
        ctx.logger.error(e)
      }
      items = area_server_list
    }
    //低于此版本一律显示维护
    // if(client_version < 20076){
    //   var srvCfg = this.ctx.helper.get_srv_state(4)
    //     items.forEach(function(server) {
    //       server.is_open = false;
    //       server.state = 4;
    //       server.state_text = srvCfg.state_text;
    //       server.view_rgba =  srvCfg.view_rgba;
    //       server.icon = srvCfg.icon;
    //     });
    // }

    this.ctx.set('content-type', 'application/xml');
    await this.ctx.render('api/client/server_list.nunjucks', {items:items, recommend_servers:recommend_servers});
  }
  async call() {
    this.ctx.body = 'ok\n' + (await this.ctx.service.backend.call("111", this.ctx.request.body.cmd));
  }

  async get_server_notice() {
    this.ctx.response.rsp_xml_object(await this.ctx.service.notice.get_server_notice(this.ctx.request));
  }

  async ipaddr() {
    this.ctx.body = "Your IP address is:" + this.ctx.ip
  }

  async check_update() {
    const ctx = this.ctx;
    try {
      await ctx.validate(rule, ctx.request.body.data);
      await ctx.service.update.check_update()
    }catch(err) {
        ctx.logger.info(err)
        ctx.response.rsp_xml_object({status:-1,message:"parse error."});
      }
  }

  async create_guest() {
    this.ctx.body = await this.ctx.service.guest.create_guest();
  }
}

module.exports = ClientController;
