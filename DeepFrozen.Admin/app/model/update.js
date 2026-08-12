module.exports = app => {
  const { STRING, INTEGER, DATE, BOOLEAN } = app.Sequelize;

  const Update = app.model.define('update', {
   id: { type: INTEGER, primaryKey: true },
      sdk_name: STRING(50),
      os_type: INTEGER,
      is_enable: INTEGER,
      least_build: INTEGER,
      current_build: INTEGER,
      update_url: STRING(255),
      res_type: INTEGER,
      created_at: DATE,
      updated_at: DATE,
  }, {underscored: true});

  Update.prototype.LogType = async function() {
    if(!Update.logType) {
      var type = await app.model.LogType.findByName(this.constructor.name)
      Update.logType = type.id
    }
    return Update.logType
  }

  Update.find = async function(id) {
        const update = await this.findOne({
            where: {
                id: id
            },
            raw: true
        })
        return update || {}
    }

  Update.findByName = function* (name) {
    return yield this.findOne({
      where: {
        name: name
      }
    });
  }

  return Update;
};
