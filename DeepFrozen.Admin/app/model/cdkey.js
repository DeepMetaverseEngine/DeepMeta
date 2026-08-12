module.exports = app => {
  const { STRING, INTEGER, DATE, BOOLEAN } = app.Sequelize;

  const Cdkey = app.model.define('cdkey', {
     id: { type: STRING(255), primaryKey: true },
      activity_id: INTEGER,
      status: INTEGER,
      created_at: DATE,
      updated_at: DATE,
      batch: INTEGER
  }, {underscored: true, tableName: 'cdkey'});

  Cdkey.prototype.LogType = async function() {
    if(!Order.logType) {
      var type = await app.model.LogType.findByName(this.constructor.name)
      Order.logType = type.id
    }
    return Cdkey.logType
  }

  Cdkey.find = async function(id) {
        const cdkey = await this.findOne({
            where: {
                id: id
            },
            raw: true
        })
        return cdkey || {}
    }

  Cdkey.findMany = async function(id_list) {
        const cdkey = await this.findAll({
            where: {
                id: id_list
            }
        })
        return cdkey || {}
  }

  Cdkey.associate = function() {
    app.model.Cdkey.belongsTo(app.model.Activity, { foreignKey: 'activity_id', targetKey: 'id', as: 'activity' });
  }

  return Cdkey;
};
