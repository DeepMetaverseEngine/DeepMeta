module.exports = app => {
  const { STRING, INTEGER, DATE, BOOLEAN } = app.Sequelize;

  const Recharge = app.model.define('recharge', {
      id: { type: INTEGER, primaryKey: true, autoIncrement: true },
      type: INTEGER,
      name: STRING(100),
      price: INTEGER,
      platform_id: INTEGER,
      created_at: DATE,
      updated_at: DATE,
  }, {underscored: true,tableName: 'recharge'});

  Recharge.prototype.LogType = async function() {
    if(!Recharge.logType) {
      var type = await app.model.LogType.findByName(this.constructor.name)
      Recharge.logType = type.id
    }
    return Recharge.logType
  }

  Recharge.find = async function(id) {
        const data = await this.findOne({
            where: {
                id: id
            },
        })
        return data || {}
    }

  Recharge.findByName = function* (name) {
    return yield this.findOne({
      where: {
        name: name
      }
    });
  }


  return Recharge;
};
