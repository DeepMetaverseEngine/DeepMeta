const pluck = require('arr-pluck');
const _ = require('underscore')._;


module.exports = app => {
  const { STRING, INTEGER, DATE } = app.Sequelize;

  const LogCreateRole = app.biModel.define('LogCreateRole', {
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
      created_at: DATE,
      updated_at: DATE,
  }, {underscored: true,tableName: 'log_createrole'});

  LogCreateRole.prototype.LogType = async function() {
    if(!LogCreateRole.logType) {
      var type = await app.model.LogType.findByName(this.constructor.name)
      LogCreateRole.logType = type.id
    }
    return LogCreateRole.logType
  }

  LogCreateRole.Todayreg = async function(ymd, server_id, channel) {
    var where = {ymd: ymd,
        role_id: {
        [app.model.Op.notIn]: app.gm_acc_list,
      },
        os_name: {
            [app.model.Op.ne]: 0,},
    }
    if(! server_id.includes('-1')){
      where['server_id'] = {[app.model.Op.in]: server_id}
    }

    if(! channel.includes('-1')){
      where['channel'] = {[app.model.Op.in]: channel}
    }

    const reglist = await this.findAll({ where: where, attributes: ['role_id', 'server_id', 'channel'] })
    return reglist
  }

  LogCreateRole.sum_reg = async function(server_id) {
    const sum_reg_list = await this.findAll({
      where: { server_id: server_id,
        role_id: {
          [app.model.Op.notIn]: app.gm_acc_list,
        },
      },
      group: 'os_name',
      attributes: [ 'os_name', [app.Sequelize.fn('COUNT', app.Sequelize.fn('DISTINCT', app.Sequelize.col('role_id'))), 'reg_num'] ]
    })
    return sum_reg_list
  }

  LogCreateRole.findAndCount = async function(ymd, server_id, channel) {
      var where = {ymd: ymd}

      if(Array.isArray(server_id)){
          where.server_id = {
            [app.model.Op.in]: server_id
          }
      }
      else if(server_id != -1){
          where.server_id = server_id
      }

      if(Array.isArray(channel)){
          where.channel = {
            [app.model.Op.in]: channel
          }
      }else if(channel != -1){
          where.channel = channel
      }

      const data = await this.findAll({
          where: where,
          attributes: [app.Sequelize.fn('DISTINCT', app.Sequelize.col('role_id'))],
          raw : true
      })

      var rows = pluck(data, 'role_id');

      return _.difference(rows, app.gm_acc_list)
  }

  LogCreateRole.countByType = async function(ymd, server_id, type) {
    var where = {
              ymd: ymd,
              server_id: server_id
    }
    const count = await this.count({
      where: where,
       distinct:true, 
        col: type,
        raw : true
    })
    return count || 0
  }
  return LogCreateRole;
};
