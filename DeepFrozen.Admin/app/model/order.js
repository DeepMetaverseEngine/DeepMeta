const crypto = require('crypto');
const moment = require('moment');

module.exports = app => {
  const { STRING, INTEGER, DATE, BOOLEAN } = app.Sequelize;

  const Order = app.model.define('order', {
     id: { type: INTEGER, primaryKey: true, autoIncrement: true },
      realm_id: INTEGER,
      server_id: INTEGER,
      platform_account: STRING(45),
      digit_id: STRING(45),
      role_id: STRING(50),
      platform_id: INTEGER,
      cp_order_id: STRING(50),
      currency_type: STRING(10),
      price: INTEGER,
      count: INTEGER,
      product_id: INTEGER,
      sell_id: STRING(100),
      sdk_name: STRING(10),
      channel_id: INTEGER,
      order_id: STRING(100),
      status: INTEGER,
      sandbox: INTEGER,
      created_at: DATE,
      updated_at: DATE,
  }, {underscored: true});

  Order.prototype.LogType = async function() {
    if(!Order.logType) {
      var type = await app.model.LogType.findByName(this.constructor.name)
      Order.logType = type.id
    }
    return Order.logType
  }

  Order.Chachzhjl = async function(account_to){
    var where = { 
      platform_account: {[app.model.Op.like]: '%' + account_to},
//      status: {[app.model.Op.between]: [1,4]},
    }

    var attributes = [ 'role_id','cp_order_id','price','channel_id','order_id','status','updated_at' ];

    const data = await this.findAll({
      where: where,
      attributes: attributes,
      order: [['updated_at', 'DESC']],
    })
    return data || {}

  }

  Order.AllMoneyByDate = async function(dt, server_id, channel) {

    var where = {
            status: app.orderStatus.OrderFinish
    }

    if(dt != 'all'){
      where.created_at = {[app.model.Op.between]:[moment(dt).format('YYYY-MM-DD HH:mm:ss'),moment(dt).add(86399,'second').format('YYYY-MM-DD HH:mm:ss')]}
    }

    if(server_id != -1){
      where.server_id = server_id
    }

    if(channel != -1){
      where.channel_id = channel
    }

    const sum = await this.sum('price', 
          { 
            where: where
          })
    return sum || 0
  }

  Order.countByDate = async function(dt, server_id, channel) {

      var where = {
                status: app.orderStatus.OrderFinish,
                created_at: {[app.model.Op.between]:[moment(dt).format('YYYY-MM-DD HH:mm:ss'),moment(dt).add(86399,'second').format('YYYY-MM-DD HH:mm:ss')]}
      }

      if(server_id != -1){
        where.server_id = server_id
      }

      if(channel != -1){
        where.channel_id = channel
      }

      const sum = await this.count({
          where: where,
           distinct:true, 
            col: 'platform_account',
            raw : true

      })
      return sum || 0
  }

  Order.countByRange = async function(range, dt, server_id, channel) {

        if(range.length == 0) {
          range = ['0']
        }

        var where = {
                status: app.orderStatus.OrderFinish,
                created_at: {[app.model.Op.between]:[moment(dt).format('YYYY-MM-DD HH:mm:ss'),moment(dt).add(86399,'second').format('YYYY-MM-DD HH:mm:ss')]},
                platform_account: {
                  [app.model.Op.in]: [range]
                },
        }

        if(server_id != -1){
          where.server_id = server_id
        }

        if(channel != -1){
          where.channel_id = channel
        }

        const count = await this.count({
            where: where,
            distinct:true, 
            col: 'platform_account',
            raw : true
        })
        return count || 0
  }

  Order.findByRangeSum = async function(range, dt, server_id, channel) {


      if(range.length == 0) {
          range = ['0']
        }

        var where = {
                status: app.orderStatus.OrderFinish,
                created_at: {[app.model.Op.between]:[moment(dt).format('YYYY-MM-DD HH:mm:ss'),moment(dt).add(86399,'second').format('YYYY-MM-DD HH:mm:ss')]},
                platform_account: {
                  [app.model.Op.in]: [range]
                }
        }

        if(server_id != -1){
          where.server_id = server_id
        }

        if(channel != -1){
          where.channel_id = channel
        }

        const sum = await this.sum('price', 
        { 
          where: where
        })

        // const data = await this.findAll({
        //     where: where,
        //     raw : true
        // })
        return sum || 0
  }

  Order.find = async function(id) {
        const order = await this.findOne({
            where: {
                cp_order_id: id
            },
            raw: true
        })
        return order || {}
    }

  Order.findByOrderId = async function(id) {
        const order = await this.findOne({
            where: {
                cp_order_id: id
            },
            include: [ { model: app.model.Channel, as: 'channel'} ]
        })
        return order || {}
  }

  Order.findByRoleId = async function(id, date) {
        const orders = await this.findAll({
            where: {
                role_id: id,
                status: {
                  [app.model.Op.ne]: app.orderStatus.OrderInvalid
                }
            },
            order: [
              ['created_at', 'DESC']
            ],
            include: [ { model: app.model.Channel, as: 'channel'} ]
        })
        return orders || {}
  }

  Order.findMany = async function(order_list) {
        const orders = await this.findAll({
            where: {
                cp_order_id: order_list
            },
            include: [ { model: app.model.Channel, as: 'channel'} ]
        })
        return orders || {}
  }

  Order.findUnverifiedOrder = async function() {
        const orders = await this.findAll({
            where: {
              sdk_name:'OneGame',
              [app.model.Op.or]: [{status: app.orderStatus.Create}, {status: app.orderStatus.PaySuccess}]
            },
            include: [ { model: app.model.Channel, as: 'channel'} ]
        })
        return orders || {}
  }


  Order.prototype.updateOrder = async function (order_id, status, price, currency_type) {
    this.order_id = order_id;
    this.status = status;
    
    if(price){
      this.price = price;
    }
    if(currency_type){
      this.currency_type = currency_type;
    }
    await this.save();
  }

  Order.associate = function() {
    app.model.Order.belongsTo(app.model.Channel, { foreignKey: 'sdk_name', targetKey: 'sdk_name', as: 'channel' });
  }

  return Order;
};
