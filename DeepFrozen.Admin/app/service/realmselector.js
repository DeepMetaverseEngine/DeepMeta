const Service = require('egg').Service;

class RealmSelectorService extends Service {

  async set_realm() {
    let ctx = this.ctx;
    var data = {};
    try {
      ctx.validate({id: 'id'},ctx.request.query);
      var realm_id = ctx.request.query.id;
      if(ctx.session.realm_id != realm_id){
        var realm = await ctx.model.Realm.find(realm_id);
        if(!ctx.helper.is_empty(realm)) {
          ctx.session.realm_id = realm.id;
          ctx.session.realm_name = realm.name;
          data = {state: true, name: realm.name, id: realm.id};
        }else {
          this.clean_realm_session()
          data = {state: false};
        }
      }
    }catch(err) {
      ctx.logger.info(err)
      data = {err};
    }

    return data;
  };

  async get_realm_selector() {
    let realms = await this.ctx.model.Realm.findAll({
      attributes: ['id', 'name']
    });
    var session_realm = this.get_session_realmid();
    var options = []
    let ctx = this.ctx;
    realms.forEach(function (realm) {
      options.push({id:realm.id,name:realm.name,selected:session_realm == realm.id})
    });

    return options;
  }

  get_session_realmid(){
    var session = this.ctx.session;
    if(session != null && session.hasOwnProperty('realm_id'))return session.realm_id;
    return 0;
  }

  async get_realm(){
    var realm_id = this.get_session_realmid()
    if(realm_id){
      let realm = await this.get_realm_by_id(realm_id);
      this.ctx.session.realm = {};
      this.ctx.session.realm.realm_id = realm.id;
      this.ctx.session.realm.realm_name = realm.name;
      //this.ctx.session.realm.gmt_url = realm.gmt_url;
      return realm;
    }
    return null;
  }

  async get_realm_by_id(realm_id){
    let realm = await this.ctx.model.Realm.find(realm_id);
    return realm;
  }

  async get_realm_by_server_id(srv_id){
    var realm_id = 0
    let srv_list = await this.ctx.service.serverlist.get_all_servers()
    srv_list.forEach(function(srv) {
          if(srv.id == srv_id) {
            realm_id = srv.realm_id
          }
    });
    return realm_id;
  }

  clean_realm_session(){
    this.ctx.session.realm_id = 0;
    this.ctx.session.realm_name = "";
    this.ctx.session.realm = null;
  }
}

module.exports = RealmSelectorService;