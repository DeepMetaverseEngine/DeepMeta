'use strict';
const co = require('co');

module.exports = {
  up: co.wrap(function *(db, Sequelize) {
    const { STRING, INTEGER, DATE, BOOLEAN } = Sequelize;

    yield db.createTable('orders', {
      id: { type: INTEGER, primaryKey: true, autoIncrement: true },
      realm_id: { type: INTEGER, allowNull: false },
      server_id: { type: INTEGER, allowNull: false },
      role_id: { type: STRING(50), allowNull: false },
      cp_order_id: { type: STRING(50), allowNull: false },
      price: { type: INTEGER, allowNull: false },
      product_id: { type: INTEGER, allowNull: false },
      sdk_name: { type: STRING(50), allowNull: false },
      channel_id: { type: INTEGER, allowNull: false },
      status: { type: INTEGER, allowNull: false },
      created_at: { type: DATE, allowNull: false },
      updated_at: { type: DATE, allowNull: false },
      },
      {
          charset: 'utf8mb4'
    });
  }),

  down: co.wrap(function *(db, Sequelize) {
    yield db.dropTable('orders');
  }),
};
