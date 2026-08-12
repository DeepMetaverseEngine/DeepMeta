const crypto = require('crypto');

module.exports = app => {
  const { STRING, INTEGER, DATE } = app.Sequelize;

  const Log = app.model.define('log', {
     id: { type: INTEGER, primaryKey: true, autoIncrement: true },
      user_id: INTEGER,
      operator_ip: STRING(25),
      log_type_id: INTEGER,
      operation: STRING(1000),
      remark: STRING(500),
      created_at: DATE,
      updated_at: DATE,
  }, {underscored: true});

  Log.findLogByType = async function(type) {
    const logs = await this.findAll({
      where: {
                log_type_id: type
            },
      order: [
            ['id', 'desc'],
        ],
      attributes: ['id', 'operator_ip', 'operation', 'remark', 'created_at']
    });

    return logs || {}
  }

  Log.associate = function() {
    app.model.Log.belongsTo(app.model.LogType);
    app.model.Log.belongsTo(app.model.User);
  }

  return Log;
};
