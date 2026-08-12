'use strict';
const co = require('co');

module.exports = {
  up: co.wrap(function *(db, Sequelize) {
    const { STRING, INTEGER, DATE } = Sequelize;

    yield db.createTable('log_types', {
      id: { type: INTEGER, primaryKey: true, autoIncrement: true },
      name: STRING(50),
      title_i18n: STRING(100),
      created_at: DATE,
      updated_at: DATE,
      },
      {
          charset: 'utf8mb4'
    });

    yield db.bulkInsert('log_types', [
      {
        name: 'realm',
        title_i18n: '大区操作'
      },
      {
        name: 'server',
        title_i18n: '服务器操作'
      },
      {
        name: 'user',
        title_i18n: '用户操作'
      },
      {
        name: 'login',
        title_i18n: '登录日志'
      },
      {
        name: 'notice',
        title_i18n: '维护公告板'
      },
      {
        name: 'broadcast',
        title_i18n: '系统广播'
      },
      {
        name: 'email',
        title_i18n: '发送邮件'
      },
      {
        name: 'order',
        title_i18n: '充值系统'
      },
      {
        name: 'update',
        title_i18n: '渠道操作'
      },
      {
        name: 'blacklist',
        title_i18n: '禁言操作'
      },
      {
        name: 'ban',
        title_i18n: '角色封停'
      },
      {
        name: 'whitelist',
        title_i18n: '账号白名单'
      },
      {
        name: 'order',
        title_i18n: '充值系统'
      },
      {
        name: 'channel',
        title_i18n: '渠道修改'
      }
    ]);
  }),

  down: co.wrap(function *(db, Sequelize) {
    yield db.dropTable('log_types');
  }),
};
