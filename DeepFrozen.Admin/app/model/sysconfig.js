module.exports = app => {
  const { STRING, INTEGER, DATE, BOOLEAN } = app.Sequelize;

  const Sysconfig = app.model.define('sysconfig', {
      key: { type: STRING, primaryKey: true, autoIncrement: false },
      value: STRING(255),
      created_at: DATE,
      updated_at: DATE,
  }, {underscored: true, tableName: 'sysconfig'});

  Sysconfig.prototype.LogType = async function() {
    if(!Sysconfig.logType) {
      var type = await app.model.LogType.findByName(this.constructor.name)
      Sysconfig.logType = type.id
    }
    return Sysconfig.logType
  }

  Sysconfig.find = async function(key) {
      const val = await this.findOne({
          where: {
              key: key
          },
          raw: true
      })
      return val || ''
    }

  Sysconfig.set = async function(key, value) {
    var val = await Sysconfig.find(key)
    if(val == ''){
      await Sysconfig.create({ key: key, value: value });
    }
    else {
      await Sysconfig.update({ value: value }, {
          where: {
            key: key
          }
      });
    }
  }


  return Sysconfig;
};
