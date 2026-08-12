const moment = require('moment');

module.exports = app => {
  const { STRING, INTEGER, DATE } = app.Sequelize;

  const Itemgain = app.biModel.define('item_gain', {
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
      AddItemId: INTEGER,
      AddValue: INTEGER,
      AddCosts: STRING(60),
      reason: STRING(20),
      created_at: DATE,
      updated_at: DATE,
  }, {underscored: true,tableName: 'log_item_gain'});

  Itemgain.prototype.LogType = async function() {
    if(!Itemgain.logType) {
      var type = await app.model.LogType.findByName(this.constructor.name)
      Itemgain.logType = type.id
    }
    return Itemgain.logType
  }

  Itemgain.getroleitem = async function(leixing, role, server_id, item_id, date1, date2) {

      var where = {
            ymd: {[app.model.Op.between]: [date1, date2]}
      }
      where[leixing] = role;
      if(item_id != -1){
        where.AddItemId = item_id
      }
      if(server_id != 0){
        where.server_id = server_id
      }

      const itemlist = await this.findAll({
          where: where,
      })
      return  itemlist || {}
  }

  Itemgain.get_once_overflow = async function(date1, date2, item, num) {
      const data = await this.findAll({
          where: {
            ymd: date1.split(' ')[0],
            time: {[app.model.Op.between]: [date1, date2]},
            AddItemId: item,
            AddValue: {[app.model.Op.gte]: num},
          },
          attributes: ['time', 'server_id', 'role_id', 'role_name', 'AddItemId', 'AddValue', 'reason'],
          raw: true
      })
      return  data || {}
  }

  return Itemgain;
};
