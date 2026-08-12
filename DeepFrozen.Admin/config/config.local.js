'use strict';

exports.sequelize = {
	datasources: [
    {
      delegate: 'model', // load all models to app.model and ctx.model
      baseDir: 'model', // load models from `app/model/*.js
      dialect: 'mysql', // support: mysql, mariadb, postgres, mssql`
      database: 'gmt',
      host: '127.0.0.1',
      port: '3307',
	    username: 'root',
      password: '123456',
      timezone: "+08:00", // 设置北京时区
      dialectOptions: {  // 让读取date类型数据时返回字符串而不是UTC时间
        dateStrings: true,
        typeCast(field, next) {
          if (field.type === "DATETIME") {
            return field.string();
          }
          return next();
        }
      },
    },
    // {
    //   delegate: 'biModel', // load all models to app.biModel and ctx.biModel
    //   baseDir: 'bi_model', // load models from `app/admin_model/*.js`
    //   dialect: 'mysql', // support: mysql, mariadb, postgres, mssql`
    //   database: 'bi_log',
    //   host: '192.168.1.232',
    //   port: '3306',
  	 //  username: 'gmt',
    //   password: 'K8oN0lkudUH7pgwj',
    //   timezone: "+08:00", // 设置北京时区
    //   dialectOptions: {  // 让读取date类型数据时返回字符串而不是UTC时间
    //     dateStrings: true,
    //     typeCast(field, next) {
    //       if (field.type === "DATETIME") {
    //         return field.string();
    //       }
    //       return next();
    //     }
    //   },
    // },
  ],
};

exports.email_config = {
  host: 'smtp.mxhichina.com',
  secure: false,
  auth: {
        user: 'gmt@bianliangsh.com',
        pass: 'hO6SEPkwgy3srTqh'
  }
};

exports.ssh_config = {
  username: 'root',
  port: 22
}