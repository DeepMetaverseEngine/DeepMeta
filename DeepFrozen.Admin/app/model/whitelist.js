const crypto = require('crypto');

module.exports = app => {
  const { STRING, INTEGER, DATE, BOOLEAN } = app.Sequelize;

  const Whitelist = app.model.define('whitelist', {
     id: { type: INTEGER, primaryKey: true, autoIncrement: true },
      username: STRING(50),
      pwd_encrypt: STRING(255),
      salt: STRING(255),
      privilege: INTEGER,
      is_enable: BOOLEAN,
      current_login_time: DATE,
      last_login_time: DATE,
      created_at: DATE,
      updated_at: DATE,
  }, {underscored: true});

  Whitelist.prototype.LogType = async function() {
    if(!Whitelist.logType) {
      var type = await app.model.LogType.findByName(this.constructor.name)
      Whitelist.logType = type.id
    }
    return Whitelist.logType
  }

  Whitelist.find = async function(id) {
        const user = await this.findOne({
            where: {
                id: id
            },
            raw: true
        })
        return user || {}
    }

  Whitelist.findByName = async function(name) {
        const user = await this.findOne({
            where: {
                username: name
            }
        })
        return user || {}
  }

  Whitelist.prototype.loginUpdate = async function () {
    if(!!this.current_login_time){
      this.last_login_time = this.current_login_time;
    }
    this.current_login_time = new Date();
    this.updated_at = new Date();
    this.save();
  }

  Whitelist.prototype.verifyPassword = function (pwd) {
    const hash = crypto.createHmac('sha256', pwd+this.salt).digest('hex');
    if(this.pwd_encrypt == hash) {
      return true
    }
    return false;
  }

  Whitelist.prototype.updatePassword = async function (pwd) {
    var salt = ((new Date())/1).toString();
    const hash = crypto.createHmac('sha256', pwd+salt).digest('hex');
    this.pwd_encrypt = hash;
    this.salt = salt;
    this.save();
  }

  return Whitelist;
};
