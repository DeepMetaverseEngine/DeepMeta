'use strict';
const co = require('co');

module.exports = {
  up: co.wrap(function *(db, Sequelize) {
    const { STRING, INTEGER, DATE, BOOLEAN } = Sequelize;

    yield db.createTable('system_broadcast', {
      id: { type: INTEGER, primaryKey: true, autoIncrement: true },
      realm_id: INTEGER,
      servers: STRING(100),
      channel: INTEGER,
      type: INTEGER,
      start_time: DATE,
      end_time: DATE,
      interval: INTEGER,
      content: STRING(500),
      enable: BOOLEAN,
      created_at: DATE,
      updated_at: DATE,
      },
      {
          charset: 'utf8mb4'
    });
  }), 

  down: co.wrap(function *(db, Sequelize) {
    yield db.dropTable('system_broadcast');
  }),
};
