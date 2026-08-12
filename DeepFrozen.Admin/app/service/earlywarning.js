const Service = require('egg').Service;



class EarlyWarningService extends Service {
  async show() {
    let list = await this.ctx.model.EarlyWarningSettings.findAll();
    return list;
  }

  async record_show() {
    let list = await this.ctx.model.EarlyWarningRecord.findAll();
    return list;
  }

}

module.exports = EarlyWarningService;
