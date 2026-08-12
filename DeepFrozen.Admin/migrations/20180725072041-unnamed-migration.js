'use strict';
const co = require('co');

module.exports = {
  up: co.wrap(function *(db, Sequelize) {
    const { STRING, INTEGER, DATE, BOOLEAN } = Sequelize;

    yield db.createTable('updates', {
      id: { type: INTEGER, primaryKey: true},
      sdk_name: { type: STRING(50), allowNull: false },
      os_type: { type: INTEGER, allowNull: false },
      is_enable: { type: INTEGER, allowNull: false },
      least_build: { type: INTEGER, allowNull: false },
      current_build: { type: INTEGER, allowNull: false },
      update_url: { type: STRING(255), allowNull: false },
      res_type: { type: INTEGER, allowNull: false },
      created_at: { type: DATE, allowNull: false },
      updated_at: { type: DATE, allowNull: false },
      },
      {
          charset: 'utf8mb4'
    });
  }),

  down: co.wrap(function *(db, Sequelize) {
    yield db.dropTable('updates');
  }),
};
