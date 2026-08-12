const moment = require('moment');

module.exports = app => {
  const { STRING, INTEGER, DATE, BOOLEAN } = app.Sequelize;

  const MailApply = app.model.define('mail_apply', {
      id: { type: INTEGER, primaryKey: true },
      title: STRING(45),
      content: STRING(500),
      attach: STRING(500),
      realm_id: INTEGER,
      server_id: INTEGER,
      level: INTEGER,
      vip: INTEGER,
      role_id: INTEGER,
      reason: STRING(100),
      status: INTEGER,
      desc: STRING(1000),
      created_at: DATE,
      updated_at: DATE,
  }, {underscored: true, tableName: 'mail_apply'});

  MailApply.prototype.LogType = async function() {
    if(!MailApply.logType) {
      var type = await app.model.LogType.findByName(this.constructor.name)
      MailApply.logType = type.id
    }
    return MailApply.logType
  }

  MailApply.findBy = async function(dt1, dt2, server_id, role_id) {

        var where = {
          status: {
            [app.model.Op.ne]: 0
          }
        }

        where.created_at = {[app.model.Op.between]:[moment(dt1).format('YYYY-MM-DD HH:mm:ss'),moment(dt2).add(86399,'second').format('YYYY-MM-DD HH:mm:ss')]}

        if(server_id != -1){
          where.server_id = { [app.model.Op.like]: '%' + server_id + '%' }
        }

        if(role_id != ''){
          where.role_id = { [app.model.Op.like]: '%' + role_id + '%' }
        }
        const data = await this.findAll({
            where: where,
            raw : true
        })
        return data || {}
  }


  MailApply.findByName = function* (name) {
    return yield this.findOne({
      where: {
        name: name
      }
    });
  }

  return MailApply;
};
