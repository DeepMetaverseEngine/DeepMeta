const Service = require('egg').Service;

class NoticeService extends Service {

  async show_all() {
    let notices = await this.ctx.model.Notice.findAll({
      order: [
        ['is_top', 'DESC'],
        ['id']
      ]
    });
    return notices || {};
  }

  async get_server_notice(req) {
    var ip = this.ctx.ip;
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
    if(notices.length != 0 && !await this.ctx.service.iplist.validate_ip(ip)){
      return JSON.parse(JSON.stringify(notices))
    }else{
      return null;
    }
  }
}

module.exports = NoticeService;