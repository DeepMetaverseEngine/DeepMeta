const Service = require('egg').Service;
const utf8js = require('utf8js');
const Ice = require("ice").Ice;
const DeepFrozenIceImpl = require("../extend/ice/generated/DeepFrozen.AdminNode").DeepFrozenIceImpl;

class GameServer {

  //static servers = new Map();

  constructor(server_id, end_point) {
    this.server_id = server_id;
    this.end_point = end_point;

    try{
    }
    catch(e)
    {
        console.log(e);
    }
  }

  static find(server_id){
    //TODO 从数据库取endpoint
    let end_point = "AdminServer:default -p 17000";
    // if(!servers.has(server_id))
    // {
    //   servers.set(server_id) = new GameServer(server_id, end_point);
    // }
    // return servers[server_id];
    return new GameServer(server_id, end_point);
  }

  static async destroy() {
    // servers.forEach(
    //   ([key, value]) => {
    //       await value.communicator.destroy();
    //   }
    // );
    // servers.clear();
  }

  async call(json){
    var communicator = Ice.initialize();
    //const proxy = communicator.stringToProxy("AdminServer:default -p 17000").ice_twoway().ice_secure(false);
    const proxy = communicator.stringToProxy(this.end_point).ice_twoway().ice_secure(false);
    this.ice_proxy = await DeepFrozenIceImpl.IAdminServiceAdapterPrx.checkedCast(proxy);
    this.communicator = communicator;

    var uint8array = new Uint8Array(await utf8js.encode(json));
    let rtn = await this.ice_proxy.externalRequest("admin backend: " + this.server_id, uint8array);
    return await utf8js.decode(rtn);
  }
}

class BackendService extends Service {
  // async find(uid) {
  //   const user = await this.ctx.db.query('select * from user where uid = ?', uid);
  //   return user;
  // }
  async call(server_id, json) {

    return await GameServer.find(server_id).call(json);
  }

  async destroy() {
    await GameServer.destroy();
  }
}

module.exports = BackendService;
