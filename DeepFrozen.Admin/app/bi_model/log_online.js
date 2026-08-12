const pluck = require('arr-pluck');
const moment = require('moment');


module.exports = app => {
  const { STRING, INTEGER, DATE, BOOLEAN } = app.Sequelize;

  const LogOnline = app.biModel.define('LogOnline', {
      msg_id: { type: INTEGER, primaryKey: true, autoIncrement: true },
      ymd: { type: DATE, primaryKey: true },
      time: {
        type: DATE,
        get: function() {
            return moment(this.getDataValue('time')).format('YYYY-MM-DD HH:mm:ss');
          }
      },
      realm_id: INTEGER,
      s_group: INTEGER,
      count: INTEGER,
      created_at: DATE,
      updated_at: DATE,
  }, {underscored: true,tableName: 'log_online'});

  LogOnline.prototype.LogType = async function() {
    if(!LogOnline.logType) {
      var type = await app.model.LogType.findByName(this.constructor.name)
      LogOnline.logType = type.id
    }
    return LogOnline.logType
  }

  LogOnline.getOnline = async function(ymd, realm_id, group) {

        var data = await this.findOne({
          where: {
            ymd: ymd,
            realm_id: realm_id,
            s_group: group
          },
          order: [
            ['time', 'DESC']
          ]
        });

        if(data)
          return data.count
        else
          return 0
  }

  LogOnline.countByType = async function(type, dt, server_id, channel) {

      

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

  LogOnline.countByTypeRange = async function(type, range, dt, server_id, channel) {

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

  LogOnline.findAndCountByType = async function(type, dt, server_id, channel) {

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


  LogOnline.findByTypeRange = async function(type, range, dt, server_id, channel) {


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

  LogOnline.getonlinenow = async function(server_id) {
        var where = {
                realm_id: server_id.realm_id,
                s_group: server_id.group
        }
        const data = await this.find({
            where: where,
            order: [
              ['time', 'DESC']
            ],
            limit: 1,
        })
        return data || {dataValues: {count: 0}}
  }

  LogOnline.findByName = function* (name) {
    return yield this.findOne({
      where: {
        name: name
      }
    });
  }


  return LogOnline;
};
