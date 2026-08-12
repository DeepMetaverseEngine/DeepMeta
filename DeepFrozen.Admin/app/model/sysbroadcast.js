const moment = require('moment');


module.exports = app => {
  const { STRING, INTEGER, DATE, BOOLEAN } = app.Sequelize;

  const Sysbroadcast = app.model.define('system_broadcast', {
   id: { type: INTEGER, primaryKey: true, autoIncrement: true },
      realm_id: INTEGER,
      servers: STRING(100),
      channel: INTEGER,
      type: INTEGER,
      start_time: {
        type: DATE,
        get: function() {
            return moment(this.getDataValue('start_time')).format('YYYY-MM-DD HH:mm:ss');
          }
      },
      end_time: {
        type: DATE,
        get: function() {
            return moment(this.getDataValue('end_time')).format('YYYY-MM-DD HH:mm:ss');
          }
      },
      interval: INTEGER,
      content: STRING(500),
      enable: BOOLEAN,
      created_at: DATE,
      updated_at: DATE,
  }, {underscored: true,tableName: 'system_broadcast'});

  Sysbroadcast.prototype.LogType = async function() {
    if(!Sysbroadcast.logType) {
      var type = await app.model.LogType.findByName(this.constructor.name)
      Sysbroadcast.logType = type.id
    }
    return Sysbroadcast.logType
  }

  Sysbroadcast.find = async function(id) {
        const user = await this.findOne({
            where: {
                id: id
            },
            raw: true
        })
        return user || {}
    }

  Sysbroadcast.findByName = function* (name) {
    return yield this.findOne({
      where: {
        name: name
      }
    });
  }

  return Sysbroadcast;
};
