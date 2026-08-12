'use strict';

module.exports = appInfo => {
  const config = exports = {};

  // use for cookie sign key, should change to your own and keep security
  config.keys = appInfo.name + '_1526270486490_2140';

  // 添加 view 配置
  exports.view = {
    defaultViewEngine: 'nunjucks',
    defaultExtension: '.nunjucks',
    mapping: {
      '.nunjucks': 'nunjucks',
    },
  };

  exports.i18n = {
    defaultLocale: 'zh-CN',
  };

  exports.logrotator = {
    filesRotateByHour: [],           // list of files that will be rotated by hour
    hourDelimiter: '-',              // rotate the file by hour use specified delimiter
    filesRotateBySize: [],           // list of files that will be rotated by size
    maxFileSize: 50 * 1024 * 1024,   // Max file size to judge if any file need rotate
    maxFiles: 10,                    // pieces rotate by size
    rotateDuration: 60000,           // time interval to judge if any file need rotate
    maxDays: 31,                     // keep max days log files, default is `31`. Set `0` to keep all logs
  };

  exports.session = {
    renew: true,
  }

  exports.security = { csrf: { enable: false, }};

  exports.multipart = {
    fileExtensions: [ '.xlsx' ]
  };

  config.security = {
    csrf: {
      enable: false,
      ignoreJSON: true
    },
    domainWhiteList: ['*']
  };
  config.cors = {
    origin:'*',
    allowMethods: 'GET,HEAD,PUT,POST,DELETE,PATCH'
  };

  // add your config here
  config.middleware = [];

  config.proxy = true;

  config.serverTimeout = 180000;

  //exports.privileges = [
  //  {privilege:1, action:'admin', title:'common_privilege_administrator',router:[]},
  //  {privilege:2, action:'operator', title:'common_privilege_operator',router:[]},
  //  {privilege:3, action:'product', title:'common_privilege_product',router:[]},
  //  {privilege:4, action:'tw_product', title:'common_privilege_tw_product',router:[]}
  //];

  return config;
};
