module.exports = app => {
  const { STRING, INTEGER, DATE } = app.Sequelize;

  const JinbiGain = app.biModel.define('JinbiGain', {
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
      AddCopper: INTEGER,
      Copper: INTEGER,
      reason: STRING(20),
  }, {underscored: true,tableName: 'log_jinbi_gain'});

  JinbiGain.prototype.LogType = async function() {
    if(!JinbiGain.logType) {
      var type = await app.model.LogType.findByName(this.constructor.name)
      JinbiGain.logType = type.id
    }
    return JinbiGain.logType
  }

  JinbiGain.queryrolelist = async function(leixing, role, server_id, options, date1, date2) {

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


  JinbiGain.get_once_overflow = async function(num, date1, date2) {
      const data = await this.findAll({
          where: {
            ymd: date1.split(' ')[0],
            time: {[app.model.Op.between]: [date1, date2]},
            AddCopper: {[app.model.Op.gte]: num},
          },
          attributes: ['time', 'server_id', 'role_id', 'role_name', 'AddCopper', 'Copper', 'reason'],
          raw: true
      })
      return  data || {}
  }



  return JinbiGain;
};
