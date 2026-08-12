'use strict';

const Controller = require('egg').Controller;

const rule = {
    address: 'chkmac',
    remark: {type: 'string'},
    //end_dt: {type: 'string', format: /^\d{4}\-\d{2}\-\d{2} \d{2}:\d{2}$/}
  };

class ForbidmacController extends Controller{
  async index(){
    const mac_list = await this.ctx.model.Blocklist.findAll({where: {type: 'MAC'}});
    this.ctx.body = mac_list;
  }

  async ban_mac_list(){
    const ban_list = await this.ctx.model.Blocklist.findAll({attributes: ['address',], where: {type: 'MAC', end_dt: { [this.app.model.Op.gte]: new Date() }}});
    let macbanlist = [];
    for(let j = 0,len = ban_list.length; j < len; j++){
      macbanlist.push(ban_list[j].dataValues.address)
    } 
    this.ctx.body = macbanlist;
  }

  async create(){
    const action = this.ctx.request.body['action'];
    const data_to = this.ctx.request.body['data'][0];
    try{
      await this.ctx.validate(rule, data_to);
    }catch(err){
      this.ctx.logger.error(err);
      this.ctx.response.rsp_table_field_errors(err.errors);
      return
    }
    switch(action){
      case 'create':
        try{
          var task = await this.ctx.model.Blocklist.create({
            address: data_to.address.toUpperCase().split(':').join(''),
            type: 'MAC',
            remark: data_to.remark,
            end_dt: data_to.end_dt,
            created_dt: new Date(),
          })
          var result = await this.ctx.model.Blocklist.findOne({
            where: { address: data_to.address }
          });
          this.ctx.body = {data: [result]}
        }catch(err){
          this.ctx.logger.error(err);
          this.ctx.response.rsp_table_error(err.errors);
        }
        break;
      case 'edit':
        try{
          var task = await this.ctx.model.Blocklist.update({
            address: data_to.address.toUpperCase().split(':').join(''),
            type: 'MAC',
            remark: data_to.remark,
            end_dt: data_to.end_dt,
            created_dt: new Date(),
          },{
            where: {id: data_to.id}
          })
          var result = await this.ctx.model.Blocklist.findOne({
            where: { address: data_to.address }
          });
          this.ctx.body = {data: [result]}
        }catch(err){
          this.ctx.logger.error(err);
          this.ctx.response.rsp_table_error(err.errors);
        }
        break;
      case 'remove':
        try{
          var remove_to = await this.ctx.model.Blocklist.findOne({ where: { id: data_to.id }})
          await remove_to.destroy();
          this.ctx.body = {data: []}
        }catch(err){
          this.ctx.logger.error(err);
          this.ctx.response.rsp_table_error(err.errors);
        }
    }
  }
}

module.exports = ForbidmacController;
