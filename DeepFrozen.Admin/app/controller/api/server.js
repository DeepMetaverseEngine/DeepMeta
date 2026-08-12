'use strict';
const Controller = require('egg').Controller;

class ServerController extends Controller {
  async index() {
    // const client1 = this.ctx.app.mysql.get('gmt');
    const ctx = this.ctx;
    const realm_id = ctx.params.realm_id;
    let servers;
    if(realm_id)
    {
      servers = await ctx.model.Server.findAll({
        where: {
          realm_id: ctx.params.realm_id
        },
         include : [ { model: ctx.model.Realm, as: 'realm' } ],
      });
    }
    else {
      servers = await ctx.model.Server.findAll({ include : [ { model: ctx.model.Realm, as: 'realm' } ],});
    }
    //items.forEach(function(i) {console.log(i)});
    this.ctx.set('content-type', 'application/xml');
    await this.ctx.render('api/server/server_list.nunjucks', {servers:servers});
  }
  async call() {
    this.ctx.body = 'ok\n' + (await this.ctx.service.backend.call("111", this.ctx.request.body.cmd));
  }

  async serverlist(){
    let srvlist = await this.ctx.service.serverlist.show_serverlist()
    this.ctx.body = srvlist
  }

  async realmlist(){
    let realmlist = await this.ctx.service.realmlist.show_realmlist()
    this.ctx.body = realmlist
  }

  async verify_account(){
    let result = await this.ctx.service.whitelist.verify_account()
    this.ctx.body = result
  }

  async get_activities() {
    var activitys = await this.ctx.service.gmactivity.show()
    activitys.forEach(function(data) {
      if(typeof(data.server_id) == 'string'){
        data.server_id = data.server_id.split(',')
      }else if(typeof(data.server_id) == 'number'){
        data.server_id = [data.server_id]
      }

      if(data.check_key != null){
        data.check_key = data.check_key.split(',')
        if(!Array.isArray(data.check_key)){
            data.check_key = [data.check_key]
          }
      }
    });

    this.ctx.body = activitys
  }
}

module.exports = ServerController;
