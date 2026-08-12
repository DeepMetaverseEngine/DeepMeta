'use strict';
const co = require('co');

module.exports = {
  up: co.wrap(function *(db, Sequelize) {
    const { STRING, INTEGER, DATE, BOOLEAN } = Sequelize;

    yield db.createTable('mpq', {
      id: { type: INTEGER, primaryKey: true},
      remark: STRING(255),
      cdn_url: STRING(255),
      created_at: DATE,
      updated_at: DATE,
      },
      {
          charset: 'utf8mb4'
    });
  }), 

  down: co.wrap(function *(db, Sequelize) {
    yield db.dropTable('mpq');
  }),
};
