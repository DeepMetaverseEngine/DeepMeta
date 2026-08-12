module.exports = app => {
  const { STRING, INTEGER, DATE } = app.Sequelize;

  const JinbiUse = app.biModel.define('JinbiUse', {
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
	  client: INTEGER,
	  device_id: STRING(36),
      CostCopper: INTEGER,
      Copper: INTEGER,
      reason: STRING(20),
  }, {underscored: true,tableName: 'log_jinbi_use'});

  JinbiUse.prototype.LogType = async function() {
    if(!JinbiUse.logType) {
      var type = await app.model.LogType.findByName(this.constructor.name)
      JinbiUse.logType = type.id
    }
    return JinbiUse.logType
  }

  JinbiUse.queryrolelist = async function(leixing, role, server_id, options, date1, date2) {

      var where = {
            ymd: {[app.model.Op.between]: [date1, date2]},
            reason: {[app.model.Op.in]: options},
          }
      where[leixing] = role;
      if(server_id != 0){
        where.server_id = server_id
      }
      const querylist = await this.findAll({
          where: where,
      })
      return  querylist || {}
  }

  return JinbiUse;
};
