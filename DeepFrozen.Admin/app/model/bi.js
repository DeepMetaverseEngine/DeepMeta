const pluck = require('arr-pluck');

module.exports = app => {
  const { STRING, INTEGER, DATE, BOOLEAN } = app.Sequelize;

  const Bi = app.model.define('bi', {
      msg_id: { type: STRING(36), primaryKey: true },
      msgtype: STRING(30),
      time: DATE,
      app_id: STRING(6),
      channel_id: STRING(6),
      server_id: STRING(8),
      device_model: STRING(10),
      device_os: STRING(2),
      sdk_version_id: STRING(6),
      ip: STRING(15),
      network: STRING(18),
      device_id: STRING(36),
      account_id: STRING(36),
      role_id: STRING(36),
      role_name: STRING(14),
      role_level: STRING(4),
      role_type: STRING(16),
      gender: STRING(2),
      app_version: STRING(2),
      msg_version: STRING(6),
      sdk_version: STRING(2),
      party_id: STRING(36),
      party_name: STRING(2),
      dt: DATE,
      ext: STRING(2000),
      created_at: DATE,
      updated_at: DATE,
  }, {underscored: true,tableName: 'log_python_test'});

  Bi.prototype.LogType = async function() {
    if(!Bi.logType) {
      var type = await app.model.LogType.findByName(this.constructor.name)
      Bi.logType = type.id
    }
    return Bi.logType
  }

  Bi.countByType = async function(type, dt, server_id, channel) {

      var where = {
                msgtype: type,
                dt: dt
      }

      if(server_id != -1){
        where.server_id = server_id
      }

      if(channel != -1){
        where.channel_id = channel
      }

      const count = await this.count({
          where: where,
           distinct:true, 
            col: 'account_id',
            raw : true

      })
      return count || 0
  }

  Bi.countByTypeRange = async function(type, range, dt, server_id, channel) {

        if(range.length == 0) {
          range = ['0']
        }

        var where = {
                msgtype: type,
                account_id: {
                  [app.model.Op.in]: [range]
                },
                dt: dt
        }

        if(server_id != -1){
          where.server_id = server_id
        }

        if(channel != -1){
          where.channel_id = channel
        }

        const count = await this.count({
            where: where,
            distinct:true, 
            col: 'account_id',
            raw : true
        })
        return count || 0
  }

  Bi.findAndCountByType = async function(type, dt, server_id, channel) {

      var where = {
                msgtype: type,
                dt: dt
      }

      if(server_id != -1){
          where.server_id = server_id
      }

      if(channel != -1){
          where.channel_id = channel
        }

      const data = await this.findAndCountAll({
          where: where,
          attributes: [app.Sequelize.fn('DISTINCT', app.Sequelize.col('account_id'))],
          raw : true
      })

      data.rows = pluck(data.rows, 'account_id');

      return data.rows || []
  }


  Bi.findByType = async function(type, dt, server_id, channel) {

        var where = {
                msgtype: type
        }

        if(dt != 'all'){
          where.dt = dt
        }

        if(server_id != -1){
          where.server_id = server_id
        }

        if(channel != -1){
          where.channel_id = channel
        }
        const data = await this.findAll({
            where: where,
            raw : true
        })
        return data || {}
  }

  Bi.findByTypeRange = async function(type, range, dt, server_id, channel) {


      if(range.length == 0) {
          range = ['0']
        }

        var where = {
                msgtype: type,
                account_id: {
                  [app.model.Op.in]: [range]
                },
                dt: dt
        }

        if(server_id != -1){
          where.server_id = server_id
        }

        if(channel != -1){
          where.channel_id = channel
        }
        const data = await this.findAll({
            where: where,
            raw : true
        })
        return data || {}
  }

  Bi.findByName = function* (name) {
    return yield this.findOne({
      where: {
        name: name
      }
    });
  }


  return Bi;
};
