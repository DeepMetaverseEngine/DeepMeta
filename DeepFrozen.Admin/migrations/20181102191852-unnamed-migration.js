'use strict';
const co = require('co');

module.exports = {
  up: co.wrap(function *(db, Sequelize) {
    const { STRING, INTEGER, DATE, BOOLEAN } = Sequelize;

    yield db.createTable('recharge', {
      id: { type: INTEGER, primaryKey: true, autoIncrement: true },
      type: INTEGER,
      name: STRING(100),
      price: INTEGER,
      created_at: DATE,
      updated_at: DATE,
      },
      {
          charset: 'utf8mb4'
    });
  }), 

  down: co.wrap(function *(db, Sequelize) {
    yield db.dropTable('recharge');
  }),
};
