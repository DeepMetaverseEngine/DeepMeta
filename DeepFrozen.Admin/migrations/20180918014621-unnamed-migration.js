'use strict';
const co = require('co');

module.exports = {
  up: co.wrap(function *(db, Sequelize) {
    const { STRING, INTEGER, DATE, BOOLEAN } = Sequelize;

    yield db.createTable('orders', {
      id: { type: INTEGER, primaryKey: true, autoIncrement: true },
      realm_id: INTEGER,
      server_id: INTEGER,
      platform_account: STRING(45),
      role_id: STRING(50),
      platform_id: INTEGER,
      cp_order_id: STRING(50),
      currency_type: STRING(10),
      price: INTEGER,
      count: INTEGER,
      product_id: INTEGER,
      sdk_name: STRING(10),
      channel_id: INTEGER,
      order_id: STRING(100),
      status: INTEGER,
      created_at: DATE,
      updated_at: DATE,
      },
      {
          charset: 'utf8mb4'
    });
  }), 

  down: co.wrap(function *(db, Sequelize) {
    yield db.dropTable('orders');
  }),
};
