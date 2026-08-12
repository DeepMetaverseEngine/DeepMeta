const pluck = require('arr-pluck');

module.exports = app => {
  const { STRING, INTEGER, DATE } = app.Sequelize;

  const Urllist = app.model.define('url_list', {
     id: { type: INTEGER, primaryKey: true, autoIncrement: true },
      title: STRING(255),
      urls: STRING(2000),
      nevigate: STRING(255),
      enable: INTEGER,
  }, {timestamps: false, underscored: true, tableName: 'url_list'});

  Urllist.prototype.LogType = async function() {
    if(!Urllist.logType) {
      var type = await app.model.LogType.findByName(this.constructor.name)
      Urllist.logType = type.id
    }
    return Urllist.logType
  }

  return Urllist;
};
