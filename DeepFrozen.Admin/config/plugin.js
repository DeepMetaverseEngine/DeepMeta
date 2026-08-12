'use strict';

// had enabled by egg
// exports.static = true;
exports.nunjucks = {
  enable: true,
  package: 'egg-view-nunjucks'
};

exports.sequelize = {
  enable: true,
  package: 'egg-sequelize'
};

exports.logrotator = {
  enable: true,
  package: 'egg-logrotator',
};

exports.validate = {
  enable: true,
  package: 'egg-validate',
};

exports.userrole = {
  package: 'egg-userrole',
};

exports.passport = {
  enable: true,
  package: 'egg-passport',
};

exports.cors = {
  enable: true,
  package: 'egg-cors',
};