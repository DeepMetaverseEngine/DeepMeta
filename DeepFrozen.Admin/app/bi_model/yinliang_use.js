module.exports = app => {
  const { STRING, INTEGER, DATE } = app.Sequelize;

  const YinliangUse = app.biModel.define('YinliangUse', {
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
      CostSilver: INTEGER,
      Silver: INTEGER,
      reason: STRING(20),
  }, {underscored: true,tableName: 'log_yinliang_use'});

  YinliangUse.prototype.LogType = async function() {
    if(!YinliangUse.logType) {
      var type = await app.model.LogType.findByName(this.constructor.name)
      YinliangUse.logType = type.id
    }
    return YinliangUse.logType
  }

  YinliangUse.queryrolelist = async function(leixing, role, server_id, options, date1, date2) {

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

  return YinliangUse;
};
