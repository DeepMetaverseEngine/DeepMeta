module.exports = app => {
  const { STRING, INTEGER, DATE, BOOLEAN } = app.Sequelize;

  const Activity = app.model.define('activity', {
      id: { type: INTEGER, primaryKey: true, autoIncrement: true },
      level: INTEGER,
      rewards: STRING(255),
      channels: STRING(255),
      name: STRING(255),
      account_start_date: DATE,
      account_end_date: DATE,
      start_date: DATE,
      end_date: DATE,
      prefix: STRING(255),
      mutex_ids: STRING(255),
      last_batch: INTEGER,
      is_enable: INTEGER,
      created_at: DATE,
      updated_at: DATE,
  }, {underscored: true, tableName: 'activity'});

  Activity.prototype.LogType = async function() {
    if(!Activity.logType) {
      var type = await app.model.LogType.findByName(this.constructor.name)
      Activity.logType = type.id
    }
    return Activity.logType
  }

  Activity.find = async function(id) {
        const update = await this.findOne({
            where: {
                id: id
            },
            raw: true
        })
        return update || {}
    }

  Activity.findByName = function* (name) {
    return yield this.findOne({
      where: {
        name: name
      }
    });
  }

  Activity.prototype.updateActivity = async function (level, rewards, channels,
    name, account_start_date, account_end_date, start_date, end_date) {
    this.level = level;
    this.rewards = rewards;
    this.channels = channels;
    this.name = name;
    this.account_start_date = account_start_date;
    this.account_end_date = account_end_date;
    this.start_date = start_date;
    this.end_date = end_date;
    this.last_batch = last_batch;
    await this.save();
  }

  Activity.updateBatch = async function (id, batch) {
    await this.update({
        last_batch: batch
      },
      {
        where:{id: id}
    });
  }

  Activity.associate = function() {
    app.model.Activity.hasMany(app.model.Cdkey, { as: 'cdkey' });
  }

  return Activity;
};
