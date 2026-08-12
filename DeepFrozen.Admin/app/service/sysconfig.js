const Service = require('egg').Service;

class SysconfigService extends Service {

  async get(key) {
    return await this.ctx.model.Sysconfig.find(key)
  }

  async set(key, value) {
  	await this.ctx.model.Sysconfig.set(key, value)
  }

  //get_privilege_by_group(privilege) {
  //	var idx = privilege - 1
  //	let privileges = this.ctx.app.config.privileges;
  //	return privileges[idx];
  //}
}

module.exports = SysconfigService;