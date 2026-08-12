const moment = require('moment');


module.exports = app => {
  const { STRING, INTEGER, DATE, BOOLEAN } = app.Sequelize;

  const Notice = app.model.define('notice', {
   id: { type: INTEGER, primaryKey: true, autoIncrement: true },
      is_open: BOOLEAN,
      is_top: BOOLEAN,
      title: STRING(50),
      content: STRING(1000),
      started_at: {
        type: DATE,
        get: function() {
            return moment(this.getDataValue('started_at')).format('YYYY-MM-DD HH:mm:ss');
          }
      },
      ended_at: {
        type: DATE,
        get: function() {
            return moment(this.getDataValue('ended_at')).format('YYYY-MM-DD HH:mm:ss');
          }
      },
      created_at: DATE,
      updated_at: DATE,
  }, {underscored: true});

  Notice.prototype.LogType = async function() {
    if(!Notice.logType) {
      var type = await app.model.LogType.findByName(this.constructor.name)
      Notice.logType = type.id
    }
    return Notice.logType
  }

  Notice.find = async function(id) {
        const user = await this.findOne({
            where: {
                id: id
            },
            raw: true
        })
        return user || {}
    }

  Notice.findByName = function* (name) {
    return yield this.findOne({
      where: {
        name: name
      }
    });
  }

  return Notice;
};
