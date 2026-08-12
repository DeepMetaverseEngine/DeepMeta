'use strict';

const moment = require('moment');
const Controller = require('egg').Controller;

class TestController extends Controller {
  // async index() {
  //   // const client1 = this.ctx.app.mysql.get('gmt');
  //   const ctx = this.ctx;
  //   const realm_id = ctx.params.realm_id;
  //   let servers;
  //   if(realm_id)
  //   {
  //     servers = await ctx.model.Server.findAll({
  //       where: {
  //         realm_id: ctx.params.realm_id
  //       },
  //        include : [ { model: ctx.model.Realm, as: 'realm' } ],
  //     });
  //   }
  //   else {
  //     servers = await ctx.model.Server.findAll({ include : [ { model: ctx.model.Realm, as: 'realm' } ],});
  //   }
  //   //items.forEach(function(i) {console.log(i)});
  //   this.ctx.set('content-type', 'application/xml');
  //   await this.ctx.render('api/server/server_list.nunjucks', {servers:servers});
  // }
  // async call() {
  //   this.ctx.body = 'ok\n' + (await this.ctx.service.backend.call("111", this.ctx.request.body.cmd));
  // }
  //
  // async serverlist(){
  //   let srvlist = await this.ctx.service.serverlist.show_serverlist()
  //   this.ctx.body = srvlist
  // }
  //
  // async realmlist(){
  //   let realmlist = await this.ctx.service.realmlist.show_realmlist()
  //   this.ctx.body = realmlist
  // }
  //
  // async verify_account(){
  //   let result = await this.ctx.service.whitelist.verify_account()
  //   this.ctx.body = result
  // }

  async test_early() {
    await this.ctx.service.corn.send_notification()
  }

  async schedule(){
    var min=new Date().getMinutes()
    // if(min % 5 != 0){
    //   return
    // }

    var datetime =  moment().format('YYYY-MM-DD HH:mm:00');
    var ymd = datetime.split(' ')[0]
    var ctx = this.ctx
    var insert_list = []
    let server_list = await this.ctx.service.serverlist.get_all_servers()
    for (var i = 0; i < server_list.length; i++) {
      var server_id = server_list[i].id
      //在线账号数量/在线角色数量
      var online_id = this.randomNum(1,1000)
      //今日累计登陆账号
      var total_logged_openid = await ctx.biModel.LogLoginRole.countByType(ymd, server_id, 'openid')
       //今日累计登陆角色
      var total_logged_id = await ctx.biModel.LogLoginRole.countByType(ymd, server_id, 'role_id')
      //今日充值额度
      var total_recharge_amount = this.randomNum(1,1000)
      //今日充值账号
      var recharge_openid = this.randomNum(1,1000)
      //今日充值角色
      var recharge_id = this.randomNum(1,1000)
      //今日注册账号
      var total_reg_openid = await ctx.biModel.Createrole.countByType(ymd, server_id, 'openid')
      //今日注册角色
      var total_reg_id = await ctx.biModel.Createrole.countByType(ymd, server_id, 'role_id')


      insert_list.push({
        ymd: ymd,
        time: datetime,
        server_id: server_id,
        online_openid: online_id,
        online_id: online_id,
        total_logged_openid: total_logged_openid,
        total_logged_id: total_logged_id,
        total_recharge_amount: total_recharge_amount,
        recharge_openid : recharge_openid,
        recharge_id : recharge_id,
        total_reg_openid : total_reg_openid,
        total_reg_id : total_reg_id
      })
    }

    try {
      await this.ctx.biModel.LogMintuesRecord.bulkCreate(insert_list)
    }catch(err){
      ctx.logger.error('bulk create error.')
      ctx.logger.error(err)
    }
  }


  async multipledb(){

    var test = await this.ctx.biModel.Createrole.count()

    this.ctx.logger.info(test)

    var test = await this.ctx.model.Activity.count()

    this.ctx.logger.info(test)

    this.ctx.body = '123'

  }

  async cdkey(){
    var d = Date.now();
    const req = this.ctx.query;
    console.log("param: " + JSON.stringify(req));
    var task = await this.ctx.model.Activity.create({
      level: req.level,
      rewards: req.rewards,
      channels: req.channels,
      name: req.name,
      account_start_date: req.account_start_date,
      account_end_date: req.account_end_date,
      start_date: req.start_date,
      end_date: req.end_date,
      prefix : req.prefix,
      created_at: d,
      updated_at: d
    });
    let cdkeys = await this.ctx.service.activity.generate_cdkey(task.id, req.prefix, req.qty);
    var rtn = "";
    cdkeys.forEach(function(value, key) {
      rtn = rtn + key + "\n";
    });
    this.ctx.body = rtn;
  }
}

module.exports = TestController;
