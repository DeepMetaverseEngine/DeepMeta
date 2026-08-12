'use strict';
const co = require('co');

module.exports = {
  up: co.wrap(function *(db, Sequelize) {
    /*
      Add altering commands here.
      Return a promise to correctly handle asynchronicity.

      Example:
      return queryInterface.createTable('users', { id: Sequelize.INTEGER });
    */
    const { STRING, INTEGER, DATE, BOOLEAN } = Sequelize;

    yield db.createTable('servers', {
      id: { type: INTEGER, primaryKey: true},
      name: STRING(30),
      realm_id: INTEGER,
      group: INTEGER,
      is_open: BOOLEAN,
      is_show: BOOLEAN,
      state: INTEGER,
      state_text: STRING(512),
      view_rgba: STRING(30),
      view_index: STRING(30),
      icon: STRING(512),
      open_at: DATE,
      created_at: DATE,
      updated_at: DATE,
      },
      {
          charset: 'utf8mb4'
    });

    //yield db.addIndex('users', ['email'], { indicesType: 'UNIQUE' });
  }),

  down: co.wrap(function *(db, Sequelize) {
    /*
      Add reverting commands here.
      Return a promise to correctly handle asynchronicity.

      Example:
      return queryInterface.dropTable('users');
    */
    yield db.dropTable('servers');
  }),
};
