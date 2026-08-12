const moment = require('moment');

module.exports = app => {
  const { STRING, INTEGER, DATE, BOOLEAN } = app.Sequelize;

  const Server = app.model.define('server', {
    id: { type: INTEGER, primaryKey: true},
    name: STRING(30),
    realm_id: INTEGER,
    group: INTEGER,
    is_open: BOOLEAN,
    is_show: BOOLEAN,
    state: INTEGER,
    state_text: STRING(512),
    flag: STRING(200),
    view_rgba: STRING(30),
    view_index: INTEGER,
    view_realm_index: INTEGER,
    view_realm_name: STRING(30),
    icon: STRING(512),
    open_at: {
        type: DATE,
        get: function() {
            return moment(this.getDataValue('open_at')).format('YYYYMMDD_HHmmss_SSS');
          }
      },
    created_at: DATE,
    updated_at: DATE,
  }, {underscored: true});

  Server.prototype.LogType = async function() {
    if(!Server.logType) {
      var type = await app.model.LogType.findByName(this.constructor.name)
      Server.logType = type.id
    }
    return Server.logType
  }

  Server.find = async function(id) {
        const user = await this.findOne({
            where: {
                id: id
            },
            raw: true
        })
        return user || {}
    }

  Server.findByName = function* (name) {
    return yield this.findOne({
      where: {
        name: name
      }
    });
  }

  Server.associate = function() {
    app.model.Server.belongsTo(app.model.Realm, { as: 'realm' });
    app.model.Server.hasOne(app.model.RecommendServer, { foreignKey: 'server_id', as: 'recommend' })
  }

  return Server;
};
