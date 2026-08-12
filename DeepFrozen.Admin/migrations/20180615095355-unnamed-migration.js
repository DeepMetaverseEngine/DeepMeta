'use strict';
const co = require('co');

module.exports = {
  up: co.wrap(function *(db, Sequelize) {
    const { STRING, INTEGER, DATE, BOOLEAN } = Sequelize;

    yield db.createTable('notices', {
      id: { type: INTEGER, primaryKey: true, autoIncrement: true },
      is_open: BOOLEAN,
      is_top: BOOLEAN,
      title: STRING(50),
      content: STRING(1000),
      started_at: DATE,
      ended_at: DATE,
      created_at: DATE,
      updated_at: DATE,
      },
      {
          charset: 'utf8mb4'
    });
  }),

  down: co.wrap(function *(db, Sequelize) {
    yield db.dropTable('notices');
  }),
};
