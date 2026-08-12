module.exports = app => {
  const { STRING, INTEGER, DATE } = app.Sequelize;

  const AbilityChange = app.biModel.define('AbilityChange', {
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
      old_ability: INTEGER,
      new_ability: INTEGER,
      reason: STRING(20),
      created_at: DATE,
      updated_at: DATE,
  }, {underscored: true,tableName: 'log_ability_change'});

  AbilityChange.prototype.LogType = async function() {
    if(!AbilityChange.logType) {
      var type = await app.model.LogType.findByName(this.constructor.name)
      AbilityChange.logType = type.id
    }
    return AbilityChange.logType
  }

  AbilityChange.getabchangelist = async function(leixing, role, server_id, date1, date2) {

      var where = {
            ymd: {[app.model.Op.between]: [date1, date2]}
      }
      where[leixing] = role;
      if(server_id != 0){
        where.server_id = server_id
      }
      const abchangelist = await this.findAll({
          where: where,
      })
      return  abchangelist || {}
  }
  return AbilityChange;
};
