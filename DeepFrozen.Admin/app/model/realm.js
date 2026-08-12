module.exports = app => {
  const { STRING, INTEGER, DATE, BOOLEAN } = app.Sequelize;

  const Realm = app.model.define('realm', {
    id: { type: INTEGER, primaryKey: true},
    name: STRING(30),
    address: STRING(128),
    is_open: BOOLEAN,
    state: INTEGER,
    state_text: STRING(512),
    view_rgba: STRING(30),
    view_realm_index: STRING(30),
    gmt_key: STRING(64),
    gmt_url: STRING(255),
    pay_url: STRING(255),
    created_at: DATE,
    updated_at: DATE,
  });

  Realm.prototype.LogType = async function() {
    if(!Realm.logType) {
      var type = await app.model.LogType.findByName(this.constructor.name)
      Realm.logType = type.id
    }
    return Realm.logType
  }

  Realm.find = async function(id) {
      const user = await this.findOne({
          where: {
              id: id
          },
          raw: true
      })
      return user || {}
  }

  Realm.findByName = function* (name) {
    return yield this.findOne({
      where: {
        name: name
      }
    });
  }
  Realm.associate = function() {
    app.model.Realm.hasMany(app.model.Server, { as: 'server' });
  }
  return Realm;
};
