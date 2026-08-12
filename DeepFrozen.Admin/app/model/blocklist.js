module.exports = app => {
  const { STRING, INTEGER, DATE } = app.Sequelize;

  const Blocklist = app.model.define('blocklist', {
    id: { type: INTEGER, primaryKey: true, autoIncrement: true },
      address: STRING(18),
      type: STRING(3),
      remark: STRING(255),
      created_dt: DATE,
      end_dt: DATE,
  }, {timestamps: false, underscored: true, tableName: 'blocklist'});

  Blocklist.prototype.LogType = async function() {
    if(!Blocklist.logType) {
      var type = await app.model.LogType.findByName(this.constructor.name)
      Blocklist.logType = type.id
    }
    return Blocklist.logType
  }

  return Blocklist;
};
