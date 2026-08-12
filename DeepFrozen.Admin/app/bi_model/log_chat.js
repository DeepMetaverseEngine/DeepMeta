module.exports = app => {
  const { STRING, INTEGER, DATE } = app.Sequelize;

  const LogChat = app.biModel.define('LogChat', {
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
    os_name: INTEGER,
    os_version: STRING(10),
	  ip: STRING,
	  models: STRING(15),
	  network: STRING(6),
	  client: INTEGER,
	  device_id: STRING(36),
	  chat_channel: INTEGER,
    uuid: STRING(36),
    to_name: STRING(14),
	  content: STRING(255),
	  is_out: INTEGER,
  }, {underscored: true,tableName: 'log_chat'});

  LogChat.prototype.LogType = async function() {
    if(!LogChat.logType) {
      var type = await app.model.LogType.findByName(this.constructor.name)
      LogChat.logType = type.id
    }
    return LogChat.logType
  }

  LogChat.querychatlog = async function(leixing, role, server_id, date1, date2) {
      var where
      if(leixing == 'role_id'){
        where = {
          ymd: {[app.model.Op.between]: [date1, date2]},
          [app.model.Op.or]: [{role_id: role}, {uuid: role}]
        }
      }else{
        where = {
          ymd: {[app.model.Op.between]: [date1, date2]},
          [app.model.Op.or]: [{role_name: role}, {to_name: role}]
        }
      }
      if(server_id != 0){
        where.server_id = server_id
      }
      const chatlist = await this.findAll({
          where: where,
      })
      return  chatlist
  }
  return LogChat;
};
