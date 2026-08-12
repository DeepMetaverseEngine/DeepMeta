const Service = require('egg').Service;

class SysboardcastService extends Service {

  async show_all() {
    let sysbroadcast = await this.ctx.model.Sysbroadcast.findAll();
    return sysbroadcast || {};
  }

  async get_server_notice() {
    var date = new Date()
    let notices = await this.ctx.model.Notice.findAll({
      where: {
        is_open: 1
        ,started_at: {
          [this.ctx.model.Op.lte]: date,
        },ended_at: {
          [this.ctx.model.Op.gte]: date,
        }
      },order: [
        ['is_top', 'DESC'],
        ['id']
      ],attributes: ['is_top','title','content'],
    });
    if(notices.length != 0){
      return JSON.parse(JSON.stringify(notices))
    }else{
      return null;
    }
  }
}

module.exports = SysboardcastService;