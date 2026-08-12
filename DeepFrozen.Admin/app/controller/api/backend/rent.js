'use strict';
const Controller = require('egg').Controller;
const { BaseContextClass } = require('egg');
const moment = require('moment');


class RentController extends Controller {

  async show(){
    let ctx = this.ctx;
    var result = await ctx.model.RentMarket.findAll({where: {status: 0}});
    ctx.body = result
  }

  async detail(){
    let ctx = this.ctx;
    var params = ctx.request.body
    var rentData = await ctx.model.RentMarket.find(params.id)
    if(ctx.helper.is_empty(rentData)){
      this.ctx.logger.error('no record.')
      ctx.body = {success: false, msg:'no record.'}
      return
    }
    ctx.body =  {success: true, msg:rentData}
  }




  async add(){
    let ctx = this.ctx;
    var params = ctx.request.body
    var rsp = {success:true}
    try{
        ctx.logger.info(params)
        var task = await ctx.model.RentMarket.create({
        serverID:  params.serverID,
        heroUUID: params.heroUUID,
        maker: params.maker,
        account: params.account,
        price: params.price,
        status: 0,
        quality: params.quality,
        star: params.star,
        race: params.race,
        gender: params.gender,
        level: params.level,
        cost: params.cost,
        attack: params.attack,
        defense: params.defense,
        hp: params.hp,
        stamina: params.stamina,
        skills:JSON.stringify(params.skills)
      })
      ctx.logger.info(task.id)
    }catch(err){
      this.ctx.logger.error(err)
      rsp = {success:false}
    }
    ctx.body = rsp;
  };

  async take(){
    let ctx = this.ctx;
    var params = ctx.request.body
    try {
      var result = await this.send_take(params.id, params.lessee);
      if(result.state){
        await ctx.model.RentMarket.update({
          status: 2,
          lessee: params.lessee,
        },
        {
          where:{id: params.id
          }
      });
      }
      ctx.body = result
    } catch (err) {
      this.ctx.logger.error(err)
    }
  }


  async remove(){
    let ctx = this.ctx;
    var params = ctx.request.body
    try {
      var result = await this.send_cancelRent(params.id);
      if(result.state){
        await ctx.model.RentMarket.remove(params.id)
      }
      ctx.body = result
    } catch (err) {
      this.ctx.logger.error(err)
    }
  }

  async send_take(id, lessee){
    let ctx = this.ctx;
    var rentData = await ctx.model.RentMarket.find(id)
    if(ctx.helper.is_empty(rentData)){
      this.ctx.logger.error('no record.')
      return {success: false, msg:'no record.'}
    }

    if(rentData.status != 0){
      return {success: false, msg:'already taked.'}
    }
    var command = {
      cmd: "ServerRentTakeKCard",
      account_id: rentData.account,
      wallet_address: lessee,
      heroUUID: rentData.heroUUID,
      realm_id: await this.service.realmselector.get_realm_by_server_id(rentData.serverID)
    }
    return await this.service.gmt.send_command(command,this.ctx.__('page_email_success'));
  }

  async send_cancelRent(id){
    let ctx = this.ctx;
    var rentData = await ctx.model.RentMarket.find(id)
    if(ctx.helper.is_empty(rentData)){
      this.ctx.logger.error('no record.')
      return {success: false, msg:'no record.'}
    }
    if(rentData.status != 0){
      return {success: false, msg:'status error.'}
    }
    var command = {
      cmd: "ServerRentCancelKCard",
      wallet_address: rentData.maker,
      heroUUID: rentData.heroUUID,
      realm_id: await this.service.realmselector.get_realm_by_server_id(rentData.serverID)
    }
    return await this.service.gmt.send_command(command,this.ctx.__('page_email_success'));
  }
}

module.exports = RentController;
