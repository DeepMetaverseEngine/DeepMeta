const pluck = require('arr-pluck');
const moment = require('moment');


module.exports = app => {
  const { STRING, INTEGER, DATE, BOOLEAN } = app.Sequelize;

  const LogMintuesRecord = app.biModel.define('LogMintuesRecord', {
      msg_id: { type: INTEGER, primaryKey: true, autoIncrement: true },
      ymd: { type: DATE, primaryKey: true },
      time: {
        type: DATE,
        get: function() {
            return moment(this.getDataValue('time')).format('YYYY-MM-DD HH:mm:ss');
          }
      },
      server_id: INTEGER,
      online_openid: INTEGER,
      online_id: INTEGER,
      total_logged_openid: INTEGER,
      total_logged_id: INTEGER,
      total_recharge_amount: INTEGER,
      recharge_openid: INTEGER,
      recharge_id: INTEGER,
      total_reg_openid: INTEGER,
      total_reg_id: INTEGER,
      created_at: DATE,
      updated_at: DATE,
  }, {underscored: true,tableName: 'log_mintues_record'});

  LogMintuesRecord.prototype.LogType = async function() {
    if(!LogMintuesRecord.logType) {
      var type = await app.model.LogType.findByName(this.constructor.name)
      LogMintuesRecord.logType = type.id
    }
    return LogMintuesRecord.logType
  }

  LogMintuesRecord.findByType = async function(range, type, server_id) {

        var where = {
                ymd: range[0].split(' ')[0],
                time: {
                  [app.model.Op.in]: range
                },
        }

        var data = null

        if(server_id == 0){
          data = await this.findAll({
              where: where,
              attributes: [
                'time', [app.Sequelize.fn('sum', app.Sequelize.col(type)),type]
              ],
              group: ['time']
          })
        }else {
          where.server_id = server_id
          data = await this.findAll({
              where: where,
              attributes: ['time', type]
          })
        }

        return data || {}
  }

  LogMintuesRecord.countByType = async function(type, dt, server_id, channel) {

      

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

  LogMintuesRecord.countByTypeRange = async function(type, range, dt, server_id, channel) {

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

  LogMintuesRecord.findAndCountByType = async function(type, dt, server_id, channel) {

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


  LogMintuesRecord.findByTypeRange = async function(type, range, dt, server_id, channel) {


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

  LogMintuesRecord.findByName = function* (name) {
    return yield this.findOne({
      where: {
        name: name
      }
    });
  }


  return LogMintuesRecord;
};
