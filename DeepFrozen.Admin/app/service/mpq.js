const Service = require('egg').Service;


class MpqService extends Service {
  async show() {
    let mpq_list = await this.ctx.model.Mpq.findAll();
    return mpq_list;
  }

  async find_mpq(version) {
    let mpq = await this.ctx.model.Mpq.find(version);
    return mpq;
  }
    
}

module.exports = MpqService;