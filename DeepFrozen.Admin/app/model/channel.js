module.exports = app => {
  const { STRING, INTEGER, DATE, BOOLEAN } = app.Sequelize;

  const Channel = app.model.define('channel', {
      id: { type: INTEGER, primaryKey: true, autoIncrement: true },
      type: STRING(50),
      sdk_name: STRING(50),
      game_id: STRING(50),
      api_key: STRING(100),
      need_sign: BOOLEAN,
      query_order: BOOLEAN,
      verify_url: STRING(100),
      query_url: STRING(100),
      sign_url: STRING(100),
      created_at: DATE,
      updated_at: DATE,
  }, {underscored: true});

  Channel.prototype.LogType = async function() {
    if(!Channel.logType) {
      var type = await app.model.LogType.findByName(this.constructor.name)
      Channel.logType = type.id
    }
    return Channel.logType
  }

  Channel.find = async function(id) {
        const update = await this.findOne({
            where: {
                id: id
            },
            raw: true
        })
        return update || {}
    }

  Channel.findBySDKName = async function(sdk_name) {
        const update = await this.findOne({
            where: {
                sdk_name: sdk_name
            },
            raw: true
        })
        return update || {}
    }

  Channel.findByName = function* (name) {
    return yield this.findOne({
      where: {
        name: name
      }
    });
  }

  Channel.associate = function() {
    app.model.Channel.hasMany(app.model.Order, { as: 'order' });
  }

  return Channel;
};
