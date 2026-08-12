module.exports = app => {
  const { STRING, INTEGER, DATE, BOOLEAN } = app.Sequelize;

  const EarlyWarningSettings = app.model.define('EarlyWarningSettings', {
      id: { type: INTEGER, primaryKey: true, autoIncrement: true },
      type: INTEGER,
      sub_type: INTEGER,
      condition: INTEGER,
      value: INTEGER,
      enable: INTEGER,
      created_at: DATE,
      updated_at: DATE,
  }, {underscored: true, tableName: 'early_warning_settings'});

  EarlyWarningSettings.prototype.LogType = async function() {
    if(!EarlyWarningSettings.logType) {
      var type = await app.model.LogType.findByName(this.constructor.name)
      EarlyWarningSettings.logType = type.id
    }
    return EarlyWarningSettings.logType
  }

  EarlyWarningSettings.findByType = async function(type, sub_type) {
        const data = await this.findAll({
            where: {
                type: type,
                sub_type: sub_type,
                enable: 1
            },
            raw: true
        })
        return data || {}
  }

  EarlyWarningSettings.find = async function(id) {
        const data = await this.findOne({
            where: {
                id: id
            }
        })
        return data || {}
  }

  EarlyWarningSettings.findByName = function* (name) {
    return yield this.findOne({
      where: {
        name: name
      }
    });
  }

  return EarlyWarningSettings;
};
