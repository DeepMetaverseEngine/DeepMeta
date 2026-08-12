const pluck = require('arr-pluck');

module.exports = app => {
  const { STRING, INTEGER, DATE, BOOLEAN } = app.Sequelize;

  const LogLogoutRole = app.biModel.define('LogLogoutRole', {
      msg_id: { type: STRING(36), primaryKey: true },
      ymd: { type: DATE, primaryKey: true },
      time: DATE,
      openid: STRING(36),
      server_id: STRING(8),
      role_id: STRING(36),
      role_name: STRING(14),
      create_time: DATE,
      job: INTEGER,
      sex: INTEGER,
      channel: INTEGER,
      server_time: DATE,
      ability: INTEGER,
      level: INTEGER,
      vip_level: INTEGER,
      os_name: INTEGER,
      os_version: STRING(8),
      ip: STRING(15),
      models: STRING(15),
      network: STRING(6),
      client: INTEGER,
      device_id: STRING(36),
      onlineTime: INTEGER,
      tianshu: INTEGER,
      scene: STRING(8),
      place: STRING(24),
      Diamond: INTEGER,
      Copper: INTEGER,
      Silver: INTEGER,
      ActivityPoint: INTEGER,
      theGetExp: INTEGER,
      theGetDiamond: INTEGER,
      theGetSilver: INTEGER,
      tasks: STRING(60),
      created_at: DATE,
      updated_at: DATE,
  }, {underscored: true,tableName: 'log_logoutrole'});

  LogLogoutRole.prototype.LogType = async function() {
    if(!LogLogoutRole.logType) {
      var type = await app.model.LogType.findByName(this.constructor.name)
      LogLogoutRole.logType = type.id
    }
    return LogLogoutRole.logType
  }

  LogLogoutRole.findLog = async function(leixing, role, date1, date2) {
      var where = {
            ymd: {[app.model.Op.between]: [date1, date2]}
      }
      where[leixing] = role;

      const list = await this.findAll({
          where: where,
      })
      return  list
  }

  return LogLogoutRole;
};
