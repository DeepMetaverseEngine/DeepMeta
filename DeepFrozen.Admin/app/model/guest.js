const crypto = require('crypto');

module.exports = app => {
  const { STRING, INTEGER, DATE, BOOLEAN } = app.Sequelize;

  const Guest = app.model.define('guest', {
     id: { type: INTEGER, primaryKey: true, autoIncrement: true },
      username: STRING(50),
      pwd_encrypt: STRING(64),
      salt: STRING(20),
      created_at: DATE,
      updated_at: DATE,
  }, {underscored: true, tableName: 'guest'});

  Guest.prototype.LogType = async function() {
    if(!Guest.logType) {
      var type = await app.model.LogType.findByName(this.constructor.name)
      Guest.logType = type.id
    }
    return Guest.logType
  }

  Guest.find = async function(id) {
        const user = await this.findOne({
            where: {
                id: id
            },
            raw: true
        })
        return user || {}
    }

  Guest.findByName = async function(name) {
        const user = await this.findOne({
            where: {
                username: name
            }
        })
        return user || {}
  }


  Guest.prototype.verifyPassword = function (pwd) {
    const hash = crypto.createHmac('sha256', pwd+this.salt).digest('hex');
    if(this.pwd_encrypt == hash) {
      return true
    }
    return false;
  }

  Guest.prototype.updatePassword = async function (pwd) {
    var salt = ((new Date())/1).toString();
    const hash = crypto.createHmac('sha256', pwd+salt).digest('hex');
    this.pwd_encrypt = hash;
    this.salt = salt;
    this.save();
  }

  return Guest;
};
