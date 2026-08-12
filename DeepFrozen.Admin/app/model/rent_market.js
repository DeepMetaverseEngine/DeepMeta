module.exports = app => {
  const { STRING, INTEGER, DATE, BOOLEAN } = app.Sequelize;

  const RentMarket = app.model.define('rent_market', {
      id: { type: INTEGER, primaryKey: true, autoIncrement: true },
      serverID: STRING(45),
      maker: STRING(45),
      account: STRING(45),
      price: INTEGER,
      status: INTEGER,
      lessee: STRING(45),
      heroUUID: STRING(45),
      quality: INTEGER,
      star: INTEGER,
      race: INTEGER,
      gender: INTEGER,
      level: INTEGER,
      cost: INTEGER,
      attack: INTEGER,
      defense: INTEGER,
      hp: INTEGER,
      stamina: INTEGER,
      skills: STRING(2000),
      expire: DATE,
      created_at: DATE,
      updated_at: DATE,
  }, {underscored: true,tableName: 'rent_market'});

  RentMarket.prototype.LogType = async function() {
    if(!RentMarket.logType) {
      var type = await app.model.LogType.findByName(this.constructor.name)
      RentMarket.logType = type.id
    }
    return RentMarket.logType
  }

  RentMarket.remove = async function(id) {
      const update = await this.destroy({
          where: {
              id: id
          }
      })
      return update || {}
  }

  RentMarket.find = async function(id) {
        const update = await this.findOne({
            where: {
                id: id
            }
        })
        return update || {}
    }

    RentMarket.findByName = function* (name) {
    return yield this.findOne({
      where: {
        name: name
      }
    });
  }


  return RentMarket;
};
