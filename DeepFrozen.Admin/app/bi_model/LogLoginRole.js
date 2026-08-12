const _ = require('underscore')._;

module.exports = app => {
  const { STRING, INTEGER, DATE, BOOLEAN } = app.Sequelize;

  const LogLoginRole = app.biModel.define('LogLoginRole', {
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
      last_logout: DATE,
      created_at: DATE,
      updated_at: DATE,
  }, {underscored: true,tableName: 'log_loginrole'});

  LogLoginRole.prototype.LogType = async function() {
    if(!LogLoginRole.logType) {
      var type = await app.model.LogType.findByName(this.constructor.name)
      LogLoginRole.logType = type.id
    }
    return LogLoginRole.logType
  }

  LogLoginRole.countByType = async function(ymd, server_id, type) {

      var where = {
                ymd: ymd,
                server_id: server_id,
                role_id: {
                  [app.model.Op.notIn]: app.gm_acc_list,
                },
      }

      const count = await this.count({
          where: where,
           distinct:true, 
            col: type,
            raw : true

      })
      return count || 0
  }

  LogLoginRole.countBy = async function(ymd, server_id, channel) {

      var where = {
                ymd: ymd,
                role_id: {
                  [app.model.Op.notIn]: app.gm_acc_list,
                },
      }

      if(server_id != -1){
          where.server_id = server_id
      }

      if(channel != -1){
          where.channel = channel
        }

      const count = await this.count({
          where: where,
           distinct:true, 
            col: 'role_id',
            raw : true
      })
      return count || 0
  }

  LogLoginRole.countdetail = async function(ymds, role_ids, server_type, channel_type) {
    var attributes = [[app.Sequelize.fn('COUNT', app.Sequelize.fn('DISTINCT', app.Sequelize.col('role_id'))), 'countlogin'], 'ymd' ];
    var group = ['ymd'];
    var where = {
              ymd: { [app.model.Op.in]: ymds },
              role_id: { [app.model.Op.in]: _.difference(role_ids, app.gm_acc_list) },
    }

    if(server_type){
        attributes.push('server_id');
        group.push('server_id')
    }

    if(channel_type){
      attributes.push('channel');
      group.push('channel')
    }

    const countlogin = await this.findAll({
        where: where,
        attributes: attributes,
        group: group,
    })
    return countlogin
}

  LogLoginRole.querydevicelist = async function(leixing, role, server_id, date1, date2) {
    var where = {
          ymd: {[app.model.Op.between]: [date1, date2]},
        }
    where[leixing] = role;
    if(server_id != 0){
      where.server_id = server_id
    }
    const querylist = await this.findAll({
        where: where,
    })
    return  querylist
}

  return LogLoginRole;
};
