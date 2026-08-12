module.exports = app => {
  const { STRING, INTEGER, DATE, BOOLEAN } = app.Sequelize;

  const Mailist = app.model.define('mailist', {
     id: { type: INTEGER, primaryKey: true, autoIncrement: true},
      name: STRING,
      address: STRING,
      enable: INTEGER,
      created_at: DATE,
      updated_at: DATE
  }, {underscored: true, tableName: 'mailist'});

  Mailist.prototype.LogType = async function() {
    if(!Order.logType) {
      var type = await app.model.LogType.findByName(this.constructor.name)
      Order.logType = type.id
    }
    return Mailist.logType
  }

  return Mailist;
};
