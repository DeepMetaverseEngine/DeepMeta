module.exports = app => {
  const { STRING, INTEGER, DATE, BOOLEAN } = app.Sequelize;

  const GmActivity = app.model.define('GmGmActivity', {
      activity_key: { type: STRING, primaryKey: true, autoIncrement: false },
      activity_status: INTEGER,
      show_type: INTEGER,
      activity_type: INTEGER,
      show_icon: STRING(100),
      activity_name: STRING(50),
      xlsx_name: STRING(50),
      sheet_name: STRING(50),
      activity_id: INTEGER,
      server_type: INTEGER,
      order: INTEGER,
      client_lua: STRING(50),
      client_xml: STRING(4100),
      goto_key: STRING(50),
      not_open_before: DATE,
      not_open_after: DATE,
      server_id: STRING(1000),
      limit_time: INTEGER,
      start_time: STRING(50),
      end_time: STRING(50),
      open_time: INTEGER,
      last_time: INTEGER,
      over_keep: INTEGER,
      need_Listener: INTEGER,
      requesttype: STRING(50),
      open_red_point: INTEGER,
      check_key: STRING(45),
      created_at: DATE,
      updated_at: DATE,
  }, {underscored: true, tableName: 'gm_activity'});

  GmActivity.prototype.LogType = async function() {
    if(!GmActivity.logType) {
      var type = await app.model.LogType.findByName(this.constructor.name)
      GmActivity.logType = type.id
    }
    return GmActivity.logType
  }

  GmActivity.find = async function(id) {
        const update = await this.findOne({
            where: {
                activity_key: id
            },
            raw: true
        })
        return update || {}
  }

  GmActivity.findChanged = async function(date) {
        const activities = await this.findAll({
            where: {
                updated_at: {
                  [app.model.Op.gte]: date
                }
            },
            raw: true
        })
        return activities || {}
  }

  GmActivity.findAvailable = async function() {
        const activities = await this.findAll({
            where: {
                activity_status: {
                  [app.model.Op.ne]: -1
                }
            },
            order: [['activity_key', 'ASC']],
            raw: true
        })
        return activities || {}
  }

  GmActivity.findByName = function* (name) {
    return yield this.findOne({
      where: {
        name: name
      }
    });
  }


  GmActivity.updateByKey = async function (obj, key) {
    await this.update(obj,
      {
        where:{activity_key: key}
    });
  }

  return GmActivity;
};
