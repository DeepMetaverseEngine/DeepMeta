'use strict';
const Controller = require('egg').Controller;

class RealmSelectorController extends Controller {

  async set_realm() {
    this.ctx.body = await this.ctx.service.realmselector.set_realm();
  };

  async get_realm(){
    this.ctx.body = await this.ctx.service.realmselector.get_realm_selector();
  };

  async get_all_group(){
  	let realm = this.ctx.service.realmselector.get_session_realmid();
  	if(realm != 0){
  		var group = await this.ctx.service.serverlist.get_all_group(realm);
  		this.ctx.body = group;
  	}else{
  		this.ctx.body = {}
  	}
  };

  async get_all_realms(){
    let realms = await this.ctx.service.serverlist.show_realmlist();
    var options = []
    realms.forEach(function (realm) {
      options.push({id:realm.id,text:realm.name + '(' + realm.address +')'})
    });

    this.ctx.body = options;

  };
}

module.exports = RealmSelectorController;
