'use strict';
const co = require('co');
const crypto = require('crypto');

var encrypt= function(pwd, salt)
{

const hash = crypto.createHmac('sha256', pwd+salt)
                   .digest('hex');
                   return hash;
}

module.exports = {
  up: co.wrap(function *(db, Sequelize) {
    const { STRING, INTEGER, DATE } = Sequelize;

    yield db.createTable('users', {
      id: { type: INTEGER, primaryKey: true, autoIncrement: true },
      username: STRING(50),
      pwd_encrypt: STRING(255),
      salt: STRING(255),
      privilege: INTEGER,
      current_login_time: DATE,
      last_login_time: DATE,
      created_at: DATE,
      updated_at: DATE,
      },
      {
          charset: 'utf8mb4'
    });

    var salt = ((new Date())/1).toString();
    yield db.bulkInsert('users', [
      {
        username: 'admin@bianliangsh.com',
        pwd_encrypt: encrypt('123456',salt),
        salt: salt,
        privilege: '1',
        created_at: new Date(),
        updated_at: new Date(),
      },
      {
        username: 'test@qq.com',
        pwd_encrypt: encrypt('123456',salt),
        salt: salt,
        privilege: '2',
        created_at: new Date(),
        updated_at: new Date(),
      },
      {
        username: 'test2@qq.com',
        pwd_encrypt: encrypt('123456',salt),
        salt: salt,
        privilege: '3',
        created_at: new Date(),
        updated_at: new Date(),
      },
    ]);
  }),

  down: co.wrap(function *(db, Sequelize) {
    yield db.dropTable('users');
  }),
};
