const Service = require('egg').Service;

class LogService extends Service {
  async show() {
    let logs = await this.ctx.model.Log.findAll({
      order: [
            ['id', 'desc'],
        ],
      attributes: ['id', 'operator_ip', 'operation', 'remark', 'created_at'],
      include: [ 
          { model: this.ctx.model.LogType, attributes: ['title_i18n']},
          { model: this.ctx.model.User, attributes: ['username']}
       ]
    });

    return logs;
  } 
}

module.exports = LogService;