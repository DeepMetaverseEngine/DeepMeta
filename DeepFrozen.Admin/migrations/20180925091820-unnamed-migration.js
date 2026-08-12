'use strict';
const co = require('co');

module.exports = {
  up: co.wrap(function *(db, Sequelize) {
    const { STRING, INTEGER, DATE, BOOLEAN } = Sequelize;

    yield db.createTable('channels', {
      id: { type: INTEGER, primaryKey: true, autoIncrement: true },
      type: STRING(50),
      sdk_name: STRING(50),
      game_id: STRING(50),
      api_key: STRING(100),
      need_sign: BOOLEAN,
      query_order: BOOLEAN,
      verify_url: STRING(100),
      query_url: STRING(100),
      sign_url: STRING(100),
      created_at: DATE,
      updated_at: DATE,
      },
      {
          charset: 'utf8mb4'
    });
  }), 

  down: co.wrap(function *(db, Sequelize) {
    yield db.dropTable('channels');
  }),
};
