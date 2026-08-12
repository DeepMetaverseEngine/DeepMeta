const moment = require('moment');

module.exports = app => {
  const { STRING, INTEGER, DATE, BOOLEAN } = app.Sequelize;

  const GmRechargeApply = app.model.define('GmRechargeApply', {
      id: { type: INTEGER, primaryKey: true },
      realm_id: INTEGER,
      server_id: STRING(200),
      role_name: STRING(100),
      role_id: STRING(100),
      department: STRING(45),
      owner: STRING(50),
      product_id: INTEGER,
      product_name: STRING(100),
      price:INTEGER,
      platform_id: INTEGER,
      order_id:STRING(45),
      signger: INTEGER,
      operator: INTEGER,
      status: INTEGER,
      reason: STRING(100),
      created_at: DATE,
      updated_at: DATE,
  }, {underscored: true, tableName: 'gm_recharge_apply'});

  GmRechargeApply.prototype.LogType = async function() {
    if(!GmRechargeApply.logType) {
      var type = await app.model.LogType.findByName(this.constructor.name)
      GmRechargeApply.logType = type.id
    }
    return GmRechargeApply.logType
  }

  GmRechargeApply.findBy = async function(dt1, dt2, server_id, role_id, department, status) {

        var where = {
          status: {
            [app.model.Op.ne]: 0
          }
        }

        where.created_at = {[app.model.Op.between]:[moment(dt1).format('YYYY-MM-DD HH:mm:ss'),moment(dt2).add(86399,'second').format('YYYY-MM-DD HH:mm:ss')]}

        if(server_id != -1){
          where.server_id = server_id
        }

        if(role_id != ''){
          where.role_id = role_id
        }

        if(department != ''){
          where.department = department
        }

        if(status != 0){
          where.status = status
        }


        const data = await this.findAll({
            where: where,
            include: [ { model: app.model.User, as: 'opt', attributes: ['username']}],
            raw : true
        })
        return data || {}
  }


  GmRechargeApply.findByName = function* (name) {
    return yield this.findOne({
      where: {
        name: name
      }
    });
  }

  GmRechargeApply.associate = function() {
    app.model.GmRechargeApply.belongsTo(app.model.User, { foreignKey: 'signger', as: 'sign' });
    app.model.GmRechargeApply.belongsTo(app.model.User, { foreignKey: 'operator', as: 'opt' });
  }

  return GmRechargeApply;
};
