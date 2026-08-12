module.exports = app => {
  const { STRING, INTEGER, DATE } = app.Sequelize;

  const Gmaccountlist = app.model.define('gmaccountlist', {
     id: { type: INTEGER, primaryKey: true, autoIncrement: true },
      role_id: STRING(36),
      owner: STRING(50),
      department: STRING(255),
      operator: INTEGER,
      server_id: INTEGER,
      created_at: DATE,
      updated_at: DATE,
  }, {underscored: true,  tableName: 'gmaccountlist'});

  Gmaccountlist.prototype.LogType = async function() {
    if(!Gmaccountlist.logType) {
      var type = await app.model.LogType.findByName(this.constructor.name)
      Gmaccountlist.logType = type.id
    }
    return Gmaccountlist.logType
  }

  Gmaccountlist.find = async function(id) {
        const user = await this.findOne({
            where: {
                id: id
            },
            raw: true
        })
        return user || {}
    }

  Gmaccountlist.findByName = async function(name) {
        const user = await this.findOne({
            where: {
                username: name
            }
        })
        return user || {}
  }

  Gmaccountlist.associate = function() {
    app.model.Gmaccountlist.belongsTo(app.model.User, { foreignKey: 'operator', as: 'op' });
  }

  return Gmaccountlist;
};
