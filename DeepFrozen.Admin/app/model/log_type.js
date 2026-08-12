module.exports = app => {
  const { STRING, INTEGER, DATE, BOOLEAN } = app.Sequelize;

  const LogType = app.model.define('log_type', {
    id: { type: INTEGER, primaryKey: true},
    name: STRING(50),
    title_i18n: STRING(100),
  });

  LogType.find = async function(id) {
      const user = await this.findOne({
          where: {
              id: id
          }
      })
      return user || {}
  }

  LogType.findByName =async function (name) {
    return await this.findOne({
      where: {
        name: name
      }
    });
  }
  LogType.associate = function() {
    app.model.LogType.hasMany(app.model.Log, { as: 'log' });
  }
  return LogType;
};
