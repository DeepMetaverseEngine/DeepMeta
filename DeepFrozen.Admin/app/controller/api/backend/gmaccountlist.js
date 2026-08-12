'use strict';

const Controller = require('egg').Controller;
const pluck = require('arr-pluck');

const rule = {
    department: 'string',
    owner: 'string',
    realm_id: 'id',
    role_id: 'id',
  };

class Gmaccountlist extends Controller{
  async index(){
    const ctx = this.ctx;
    const acc_list = await this.ctx.model.Gmaccountlist.findAll({ include: [ { model: this.ctx.model.User, as: 'op', attributes: ['username']}]});
    let data, command, result, res, resd, resdata = []
    for(let i = 0; i < acc_list.length; i++){
      data = acc_list[i];
      resd = data.dataValues
      command = {
        cmd: "ServerQueryRoleList",
        realm_id: await ctx.service.realmselector.get_realm_by_server_id(resd.server_id),
        type: 1,
        role_id: resd.role_id,
      }
      result = await ctx.service.gmt.send_command(command,'success');
      if(result.state == true){
        res = JSON.parse(result.ext)[0]
        resd.pro = res.pro;
        resd.role_name = res.role_name;
        resd.role_level = res.role_level;
        resd.diamond_num = res.diamond_num;
        resd.gold_num = res.gold_num;
      }
    resdata.push(resd);
    }
    ctx.body = resdata;
  }

  async create(){
    const action = this.ctx.request.body['action'];
    const data_to = this.ctx.request.body['data'][0];
    switch(action){
      case 'create':
        try{
          await this.ctx.validate(rule, data_to);
        }catch(err){
          this.ctx.response.rsp_table_field_errors(err.errors);
          return
        }
        try{
          //var realm_id = await this.service.realmselector.get_realm_by_server_id(data_to.server_id)
          //if(realm_id == 0){
          //  this.ctx.response.rsp_table_error([{message: 'Error', value: this.ctx.__('common_nevigate_field_servers') + this.ctx.__('common_instructions_inputerr') }]);
          //  return
          //}
          var command = {
            cmd: "ServerQueryRoleList",
            realm_id: data_to.realm_id,
            role_id: data_to.role_id,
            type: 1,
          }
          var result = await this.service.gmt.send_command(command,'success');
          if(! result.state){
            this.ctx.response.rsp_table_error([{message: 'Error' , value: result.reason }]);
            return
          }
            await this.ctx.model.Gmaccountlist.create({
            department: data_to.department,
            owner: data_to.owner,
            server_id: JSON.parse(result.ext)[0].server_id,
            role_id: data_to.role_id,
            operator: this.ctx.user.id,
          })
          var resulttt = await this.ctx.model.Gmaccountlist.findAll({
            include: [ { model: this.ctx.model.User, as: 'op', attributes: ['username']}]
          });
          this.app.messenger.sendToApp('update_var', { varname: 'gm_acc_list', value: pluck(resulttt, 'role_id')})
          for(let i of resulttt){
            result = {};
            if(i.dataValues.role_id == data_to.role_id){
              result = i;
              break;
            }
          }
          var resultt = await this.ctx.service.gmt.send_command(command,'success');
          if(resultt.state == true){
            var res = JSON.parse(resultt.ext)[0]
            result.dataValues.pro = res.pro;
            result.dataValues.role_name = res.role_name;
            result.dataValues.role_level = res.role_level;
            result.dataValues.diamond_num = res.diamond_num;
            result.dataValues.gold_num = res.gold_num;
          }
          this.ctx.body = {data: [result]};
        }catch(err){
          this.ctx.logger.info(err);
          this.ctx.response.rsp_table_error(err.errors);
        }
        break;
      //case 'edit':
      //  try{
      //    var task = await this.ctx.model.Gmaccountlist.update({
      //        server_id: data_to.server_id,
      //        role_id: data_to.role_id,
      //        operator: this.ctx.user.id,
      //      },{
      //        where: {id: data_to.id}
      //      });

      //    var result = await this.ctx.model.Gmaccountlist.findOne({
      //      where: { role_id: data_to.role_id },
      //      include: [ { model: this.ctx.model.User, as: 'op', attributes: ['username']}]
      //    });
      //    var command = {
      //      cmd: "ServerQueryRoleList",
      //      realm_id: await this.ctx.service.realmselector.get_realm_by_server_id(data_to.server_id),
      //      type: 1,
      //      role_id: data_to.role_id,
      //    }
      //    var resultt = await this.ctx.service.gmt.send_command(command,'success');
      //    if(resultt.state == true){
      //      var res = JSON.parse(resultt.ext)[0]
      //      result.dataValues.pro = res.pro;
      //      result.dataValues.role_name = res.role_name;
      //      result.dataValues.role_level = res.role_level;
      //      result.dataValues.diamond_num = res.diamond_num;
      //      result.dataValues.gold_num = res.gold_num;
      //    }
      //    this.ctx.body = {data: [result]};
      //  }catch(err){
      //    this.ctx.logger.info(err);
      //    this.ctx.response.rsp_table_error(err.errors);
      //  }
      //  break;
      case 'remove':
        try{
          var remove_to = await this.ctx.model.Gmaccountlist.findOne({ where: { id: data_to.id }});
          await remove_to.destroy();
          this.ctx.body = {data: []}
          var resulttt = await this.ctx.model.Gmaccountlist.findAll({
            include: [ { model: this.ctx.model.User, as: 'op', attributes: ['username']}]
          });
          this.app.messenger.sendToApp('update_var', { varname: 'gm_acc_list', value: pluck(resulttt, 'role_id')})
        }catch(err){
          this.ctx.logger.info(err);
          this.ctx.response.rsp_table_error(err.errors);
        }
    }
  }
}

module.exports = Gmaccountlist;
