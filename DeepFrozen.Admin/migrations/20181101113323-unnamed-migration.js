'use strict';
const co = require('co');

module.exports = {
  up: co.wrap(function *(db, Sequelize) {
    const { STRING, INTEGER, DATE, BOOLEAN } = Sequelize;

    yield db.createTable('ip_list', {
      id: { type: INTEGER, primaryKey: true, autoIncrement: true },
      type: STRING(20),
      address: STRING(45),
      is_enable: BOOLEAN,
      remark: STRING(100),
      created_at: DATE,
      updated_at: DATE,
      },
      {
          charset: 'utf8mb4'
    });
  }), 

  down: co.wrap(function *(db, Sequelize) {
    yield db.dropTable('ip_list');
  }),
};
