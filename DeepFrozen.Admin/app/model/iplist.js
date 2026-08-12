module.exports = app => {
  const { STRING, INTEGER, DATE, BOOLEAN } = app.Sequelize;

  const Iplist = app.model.define('ip_list', {
      id: { type: INTEGER, primaryKey: true, autoIncrement: true },
      type: STRING(20),
      address: STRING(45),
      is_enable: BOOLEAN,
      remark: STRING(100),
      created_at: DATE,
      updated_at: DATE,
  }, {underscored: true,tableName: 'ip_list'});

  Iplist.prototype.LogType = async function() {
    if(!Iplist.logType) {
      var type = await app.model.LogType.findByName(this.constructor.name)
      Iplist.logType = type.id
    }
    return Iplist.logType
  }

  Iplist.find = async function(id) {
        const update = await this.findOne({
            where: {
                id: id
            },
            raw: true
        })
        return update || {}
    }

  Iplist.findByName = function* (name) {
    return yield this.findOne({
      where: {
        name: name
      }
    });
  }


  return Iplist;
};
