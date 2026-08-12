
module.exports = app => {
  const { STRING, INTEGER, DATE, BOOLEAN } = app.Sequelize;

  const LogPrepaid = app.biModel.define('LogPrepaid', {
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
      order_id: STRING(36),
      totalPay: INTEGER,
      recharge_diamond: INTEGER,
      Diamond: INTEGER,
      item_type: INTEGER,
      created_at: DATE,
      updated_at: DATE,
  }, {underscored: true,tableName: 'log_prepaid'});

  LogPrepaid.prototype.LogType = async function() {
    if(!LogPrepaid.logType) {
      var type = await app.model.LogType.findByName(this.constructor.name)
      LogPrepaid.logType = type.id
    }
    return LogPrepaid.logType
  }

  LogPrepaid.countBy = async function(ymd, server_id, channel) {

      var where = {
                ymd: ymd,
          role_id: {[app.model.Op.notIn]: app.gm_acc_list}
      }

      if(server_id != -1){
          where.server_id = server_id
      }

      if(channel != -1){
          where.channel = channel
        }else{
          where.channel = {[app.model.Op.ne]: 0}
        }

      const count = await this.count({
          where: where,
           distinct:true, 
            col: 'role_id',
            raw : true

      })
      return count || 0
  }

  LogPrepaid.countByRange = async function(range, ymd, server_id, channel) {

      var where = {
                ymd: ymd,
                role_id: {
                  [app.model.Op.in]: range
                },
      }

      if(server_id != -1){
          where.server_id = server_id
      }

      if(channel != -1){
          where.channel = channel
        }else{
          where.channel = {[app.model.Op.ne]: 0}
        }

      const count = await this.count({
          where: where,
           distinct:true, 
            col: 'role_id',
            raw : true

      })
      return count || 0
  }

  LogPrepaid.sumBy = async function(ymd, server_id, channel) {

        var where = {
          ymd: ymd,
            role_id: {[app.model.Op.notIn]: app.gm_acc_list}
        }

        if(server_id != -1){
          where.server_id = server_id
        }

        if(channel != -1){
          where.channel = channel
        }else{
          where.channel = {[app.model.Op.ne]: 0}
        }

        const sum = await this.sum('totalPay', 
        { 
          where: where
        })

        return sum || 0
  }

  LogPrepaid.sumByServer = async function(server_id) {

        var where = {
          server_id: server_id,
          channel: {[app.model.Op.ne]: 0},
            role_id: {[app.model.Op.notIn]: app.gm_acc_list}
        }

        const sum = await this.sum('totalPay', 
        { 
          where: where
        })

        return sum || 0
  }

  LogPrepaid.rangeSum = async function(range, ymd, server_id, channel) {

        var where = {
                role_id: {
                  [app.model.Op.in]: range
                },
                ymd: ymd
        }
        if(server_id != -1){
          where.server_id = server_id
        }

        if(channel != -1){
          where.channel = channel
        }else{
          where.channel = {[app.model.Op.ne]: 0}
        }
        const sum = await this.sum('totalPay', 
        { 
          where: where
        })

        return sum || 0
  }


  LogPrepaid.chargecount = async function(ymd, server_id, field) {
      var where = {
        ymd: ymd,
        server_id: server_id,
        channel: {[app.model.Op.ne]: 0},
          role_id: {[app.model.Op.notIn]: app.gm_acc_list}
      }

      const count = await this.count({
      where: where,
        distinct:true, 
        col: field,
        raw : true
      })
      return count || 0
  }

  LogPrepaid.findByRangeSum = async function(role_range, ymd_range1, ymd_range2) {

        var where = {
                role_id: {
                  [app.model.Op.in]: [role_range]
                },
                ymd: {
                  [app.model.Op.between]: [ymd_range1, ymd_range2]
                }
        }

        const sum = await this.sum('totalPay', 
        { 
          where: where
        })

        return sum || 0
  }

  LogPrepaid.findChargelist = async function(date, server_id, channel, role_list) {
    var where = {}
    if(date == null){
      where = {}
    }else if(typeof(date) == 'string'){
      where['ymd'] = date
    }else if(date.constructor == Array){
      where['ymd'] = {[app.model.Op.between]: date}
    }

    if(role_list.length > 0 ){
      where['role_id'] = {[app.model.Op.in]: role_list}
    }else{
      where['role_id'] = {[app.model.Op.notIn]: app.gm_acc_list}
     }
    var group = 'role_id'
    if(!server_id.includes('-1')){
      where['server_id'] = {[app.model.Op.in]: server_id} 
    }
    if(channel.includes('-1')){
      where['channel'] = {[app.model.Op.ne]: 0}
    }else{
      where['channel'] = {[app.model.Op.in]: channel}
    }
    const chargelist = await this.findAll({
      where: where,
      group: group,
      attributes: ['role_id', 'role_name', 'ability', 'server_id', 'channel', [app.Sequelize.fn('MAX', app.Sequelize.col('level')), 'level'], [app.Sequelize.fn('MAX', app.Sequelize.col('vip_level')), 'vip_level'], [app.Sequelize.fn('SUM', app.Sequelize.col('totalPay')), 'topay']],
      order: [ [app.Sequelize.fn('SUM', app.Sequelize.col('totalPay')), 'DESC'] ],
      limit: 50,
    })

    return chargelist
  }

  return LogPrepaid;
};
