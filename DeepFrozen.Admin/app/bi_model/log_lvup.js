module.exports = app => {
  const { STRING, INTEGER, DATE } = app.Sequelize;

  const LogLvup = app.biModel.define('LogLvup', {
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
    os_version: STRING(10),
    ip: STRING,
    models: STRING(15),
    network: STRING(6),
    client: INTEGER,
    device_id: STRING(36),
    new_level: INTEGER,
    add_ability: INTEGER,
    TotalSeconds: INTEGER,
  }, {underscored: true,tableName: 'log_lvup'});

  LogLvup.prototype.LogType = async function() {
    if(!LogLvup.logType) {
      var type = await app.model.LogType.findByName(this.constructor.name)
      LogLvup.logType = type.id
    }
    return LogLvup.logType
  }

  LogLvup.lvuplog = async function(leixing, role, server_id, date1, date2) {
      var where = {
            ymd: {[app.model.Op.between]: [date1, date2]}
      }
      where[leixing] = role;
      if(server_id != 0){
        where.server_id = server_id
      }
    
      const lvuplist = await this.findAll({
          where: where,
      })
      return  lvuplist
  }
  return LogLvup;
};
