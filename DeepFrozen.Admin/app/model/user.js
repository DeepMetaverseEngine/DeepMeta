const crypto = require('crypto');

module.exports = app => {
  const { STRING, INTEGER, DATE } = app.Sequelize;

  const User = app.model.define('user', {
     id: { type: INTEGER, primaryKey: true, autoIncrement: true },
      username: STRING(50),
      pwd_encrypt: STRING(255),
      salt: STRING(255),
      privilege: INTEGER,
      privileges: STRING(255),
      current_login_time: DATE,
      last_login_time: DATE,
      created_at: DATE,
      updated_at: DATE,
  }, {underscored: true});

  User.prototype.LogType = async function() {
    if(!User.logType) {
      var type = await app.model.LogType.findByName(this.constructor.name)
      User.logType = type.id
    }
    return User.logType
  }

  User.find = async function(id) {
        const user = await this.findOne({
            where: {
                id: id
            },
            raw: true
        })
        return user || {}
    }

  User.findByName = function* (name) {
    return yield this.findOne({
      where: {
        username: name
      }
    });
  }

  User.prototype.loginUpdate = async function () {
    if(!!this.current_login_time){
      this.last_login_time = this.current_login_time;
    }
    this.current_login_time = new Date();
    this.updated_at = new Date();
    this.save();
  }

  User.prototype.verifyPassword = function (pwd) {
    const hash = crypto.createHmac('sha256', pwd+this.salt).digest('hex');
    if(this.pwd_encrypt == hash) {
      return true
    }
    return false;
  }

  User.prototype.updatePassword = async function (pwd) {
    var salt = ((new Date())/1).toString();
    const hash = crypto.createHmac('sha256', pwd+salt).digest('hex');
    this.pwd_encrypt = hash;
    this.salt = salt;
    this.save();
  }

  User.associate = function() {
    app.model.User.belongsTo(app.model.Usergroup, { foreignKey: 'privilege', targetKey: 'id', as: 'group' });
  }

  return User;
};
