'use strict';
const co = require('co');

module.exports = {
  up: co.wrap(function *(db, Sequelize) {
    const { STRING, INTEGER, DATE, BOOLEAN } = Sequelize;

    yield db.createTable('recommend_servers', {
      server_id: { type: INTEGER, primaryKey: true},
      period: INTEGER,
      created_at: DATE,
      updated_at: DATE,
      },
      {
          charset: 'utf8mb4'
    });

    yield db.bulkInsert('recommend_servers', [
      {
        server_id : 2,
        period : 10,
        created_at: new Date(),
        updated_at: new Date(),
      },
      {
        server_id : 3,
        period : 20,
        created_at: new Date(),
        updated_at: new Date(),
      },
    ]);

    //yield db.addIndex('server_config', ['email'], { indicesType: 'UNIQUE' });
  }),

  down: co.wrap(function *(db, Sequelize) {
    yield db.dropTable('recommend_servers');
  }),
};
