const moment = require('moment');

module.exports = app => {
  const { STRING, INTEGER, DATE } = app.Sequelize;

  const Itemuse = app.biModel.define('itemuse', {
      msg_id: { type: STRING(36), primaryKey: true },
      ymd: { type: DATE, primaryKey: true },
      time: DATE,
      openid: STRING(36),
      server_id: STRING(8),
      role_id: STRING(36),
      role_name: STRING(14),
      create_time: DATE,
      job: INTEGER,
      sex: INTEGER,
      channel: INTEGER,
      server_time: DATE,
      ability: INTEGER,
      level: INTEGER,
      vip_level: INTEGER,
      CostItemId: INTEGER,
      CostValue: INTEGER,
      AddCosts: STRING(60),
      reason: STRING(20),
      created_at: DATE,
      updated_at: DATE,
  }, {underscored: true,tableName: 'log_item_use'});

  Itemuse.prototype.LogType = async function() {
    if(!Itemuse.logType) {
      var type = await app.model.LogType.findByName(this.constructor.name)
      Itemuse.logType = type.id
    }
    return Itemuse.logType
  }

  Itemuse.getroleitem = async function(leixing, role, server_id, item_id, date1, date2) {
      var where = {
            ymd: {[app.model.Op.between]: [date1, date2]}
      }
      where[leixing] = role;
      if(item_id != -1){
        where.CostItemId = item_id
      }

      const itemlist = await this.findAll({
          where: where,
      })
      return  itemlist || {}
  }

  return Itemuse;
};
