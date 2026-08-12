'use strict';
const co = require('co');

module.exports = {
  up: co.wrap(function *(db, Sequelize) {
    const { STRING, INTEGER, DATE, BOOLEAN } = Sequelize;

    yield db.createTable('realms', {
      id: { type: INTEGER, primaryKey: true},
      name: STRING(30),
      address: STRING(128),
      is_open: BOOLEAN,
      state: INTEGER,
      state_text: STRING(512),
      view_rgba: STRING(30),
      view_realm_index: STRING(30),
      gmt_key: STRING(64),
      gmt_url: STRING(255),
      pay_url: STRING(255),
      created_at: DATE,
      updated_at: DATE,
      },
      {
          charset: 'utf8mb4'
    });
  }),

  down: co.wrap(function *(db, Sequelize) {
    yield db.dropTable('realms');
  }),
};
