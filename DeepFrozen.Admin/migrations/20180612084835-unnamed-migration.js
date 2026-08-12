'use strict';
const co = require('co');

module.exports = {
  up: co.wrap(function *(db, Sequelize) {
    const { STRING, INTEGER, DATE } = Sequelize;

    yield db.createTable('logs', {
      id: { type: INTEGER, primaryKey: true, autoIncrement: true },
      user_id: INTEGER,
      operator_ip: STRING(25),
      log_type_id: INTEGER,
      operation: STRING(1000),
      remark: STRING(500),
      created_at: DATE,
      updated_at: DATE,
      },
      {
          charset: 'utf8mb4'
    });
  }),

  down: co.wrap(function *(db, Sequelize) {
    yield db.dropTable('logs');
  }),
};
