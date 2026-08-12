module.exports = app => {
  const { STRING, INTEGER, DATE } = app.Sequelize;

  const Horseracelamp = app.model.define('horseracelamp', {
      id: { type: INTEGER, primaryKey: true, autoIncrement: true },
      start_time: DATE,
      end_time: DATE,
      group_id: STRING,
      content: STRING,
      channel_arr: STRING,
      func_type: INTEGER,
      interval: INTEGER,
      realm_id: INTEGER,
  }, {timestamps: false, underscored: true,  tableName: 'horseracelamp'});

  Horseracelamp.prototype.LogType = async function() {
    if(!Horseracelamp.logType) {
      var type = await app.model.LogType.findByName(this.constructor.name)
      Horseracelamp.logType = type.id
    }
    return Horseracelamp.logType
  }

  return Horseracelamp;
};
