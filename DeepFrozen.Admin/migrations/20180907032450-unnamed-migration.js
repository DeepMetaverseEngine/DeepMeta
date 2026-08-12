'use strict';
const co = require('co');

module.exports = {
  up: co.wrap(function *(db, Sequelize) {
    const { STRING, INTEGER, DATE, BOOLEAN } = Sequelize;

    yield db.createTable('whitelists', {
      id: { type: INTEGER, primaryKey: true, autoIncrement: true },
      username: STRING(50),
      pwd_encrypt: STRING(255),
      salt: STRING(255),
      privilege: INTEGER,
      is_enable: BOOLEAN,
      current_login_time: DATE,
      last_login_time: DATE,
      created_at: DATE,
      updated_at: DATE,
      },
      {
          charset: 'utf8mb4'
    });
  }),

  down: co.wrap(function *(db, Sequelize) {
    yield db.dropTable('whitelists');
  }),
};
