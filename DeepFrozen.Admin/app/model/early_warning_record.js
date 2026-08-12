module.exports = app => {
  const { STRING, INTEGER, DATE, BOOLEAN } = app.Sequelize;

  const EarlyWarningRecord = app.model.define('EarlyWarningRecordRecord', {
      id: { type: INTEGER, primaryKey: true, autoIncrement: true },
      time: DATE,
      type: INTEGER,
      sub_type: INTEGER,
      condition: INTEGER,
      server_id: INTEGER,
      role_id: STRING,
      role_name: STRING,
      value: INTEGER,
      actual: INTEGER,
      total: INTEGER,
      reason:STRING,
      status: INTEGER,
      created_at: DATE,
      updated_at: DATE,
  }, {underscored: true, tableName: 'early_warning_record'});

  EarlyWarningRecord.prototype.LogType = async function() {
    if(!EarlyWarningRecord.logType) {
      var type = await app.model.LogType.findByName(this.constructor.name)
      EarlyWarningRecord.logType = type.id
    }
    return EarlyWarningRecord.logType
  }

  EarlyWarningRecord.find = async function() {
        const data = await this.findAll({
            where: {
                status: 0
            },
            raw: true
        })
        return data || {}
  }

  EarlyWarningRecord.finish = async function(arr) {
        this.update({
          status: 1,
        }, {
          where: {
              id: {
                [app.model.Op.in]: arr
              }
            }
          });
  }

  EarlyWarningRecord.findByName = function* (name) {
    return yield this.findOne({
      where: {
        name: name
      }
    });
  }

  return EarlyWarningRecord;
};
