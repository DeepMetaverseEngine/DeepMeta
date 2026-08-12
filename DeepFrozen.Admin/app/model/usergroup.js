module.exports = app => {
  const { STRING, INTEGER } = app.Sequelize;

  const Usergroup = app.model.define('user_group', {
      id: { type: INTEGER, primaryKey: true, autoIncrement: true },
      user_group_name: STRING(45),
      group_privileges: STRING(500),
  }, {timestamps: false, underscored: true,tableName: 'user_group'});

  Usergroup.prototype.LogType = async function() {
    if(!Usergroup.logType) {
      var type = await app.model.LogType.findByName(this.constructor.name)
      Usergroup.logType = type.id
    }
    return Usergroup.logType
  }

  return Usergroup;
};
