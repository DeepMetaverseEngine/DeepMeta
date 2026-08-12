const Service = require('egg').Service;

class ServerListService extends Service {
  async show_serverlist() {
    let servers = await this.ctx.model.Server.findAll({
    	attributes: ['id', 'name','realm_id','group','is_open','is_show','state','state_text','flag','view_rgba','view_index','view_realm_index','icon','open_at'],
	  	include: [ { model: this.ctx.model.Realm, as: 'realm', attributes: ['name']},
        { model: this.ctx.model.RecommendServer, as: 'recommend', attributes: ['server_id','period', [this.ctx.model.literal('1'), 'enable']]}

       ]
	});

	let realms = await this.ctx.model.Realm.findAll({
    	attributes: ['id', 'name'],
	});

    var options = []

    realms.forEach(function (realm) {
    	options.push({label:realm.name,value:realm.id})
    });
    return {data:servers,options:{'realm_id':options}};
  }


  async get_all_group(id){
    let servers = await this.ctx.model.Server.findAll({
      attributes: ['id','name','group'],
      where:{realm_id: id}
    });
    return servers
  }

  async show_realmlist() {
	let realms = await this.ctx.model.Realm.findAll();
	return realms;
  }

  async get_realms() {
    let realms = await this.ctx.model.Realm.findAll({
      attributes: ['id', 'name'],
    });
    return realms;
  }

  async get_all_servers() {
    var ctx = this.ctx
    let servers = await this.ctx.model.Server.findAll({
        attributes: ['id', 'name', 'group'],
        include: [ { model: this.ctx.model.Realm, as: 'realm', attributes: ['id','name']}]
    });
    
    var server_arr = []
    var temp_realm;
    servers.forEach(function(server) {
      var realm_flag = 0
      if(temp_realm != server.realm.name){
        temp_realm = server.realm.name
        realm_flag = 1
      }
      server_arr.push({id:server.id, group:server.group, name:server.name, flag:realm_flag, realm_id:server.realm.id, realm_name:server.realm.name})
    });
    return server_arr 
  }
}

module.exports = ServerListService;