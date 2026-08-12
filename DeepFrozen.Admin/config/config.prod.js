'use strict';

exports.sequelize = {
  dialect: 'mysql', // support: mysql, mariadb, postgres, mssql
  database: 'gmt2',
  host: '192.168.1.201',
  port: '3306',
  username: 'gmt2',
  password: 'K8oN0lkudUH7pgwj',
  timezone: "+08:00" // 设置北京时区
};

exports.logger = {
  dir: 'logs/DeepFrozen.Admin/',
  level: 'WARN',
};

