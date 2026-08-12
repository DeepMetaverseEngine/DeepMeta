module.exports = app => {
  const { STRING, INTEGER, DATE, BOOLEAN } = app.Sequelize;

  const Mpq = app.model.define('mpq', {
      id: { type: INTEGER, primaryKey: true},
      cdn_url: STRING(255),
      remark: STRING(255),
      created_at: DATE,
      updated_at: DATE,
  }, {underscored: true,tableName: 'mpq'});

  Mpq.prototype.LogType = async function() {
    if(!Mpq.logType) {
      var type = await app.model.LogType.findByName(this.constructor.name)
      Mpq.logType = type.id
    }
    return Mpq.logType
  }

  Mpq.find = async function(id) {
        const data = await this.findOne({
            where: {
                id: id
            },
        })
        return data || {}
    }

  Mpq.findByName = function* (name) {
    return yield this.findOne({
      where: {
        name: name
      }
    });
  }


  return Mpq;
};
