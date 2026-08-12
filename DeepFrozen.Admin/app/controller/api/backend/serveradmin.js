'use strict';
const Controller = require('egg').Controller;

const editRule = {
  adm_command: ['start', 'sopen', 'stop', 'streload', 'restart'],
  id: 'id',
  type: ['realm_id', 'server_id']
};
const editRules = {
  adm_command: ['start', 'sopen', 'stop', 'streload', 'restart'],
  id: 'array',
  type: ['realm_ids']
};

class ServerAdminController extends Controller {
  async realmstat(){
    let service = this.service
    let realms = await this.ctx.model.Realm.findAll();
    let realm_r = [];
    try{
      realms.forEach(async function(v, i, r){
        let a = await service.mangs.realmstate(v.dataValues);
        realm_r.push({id: v.dataValues.id, name: v.dataValues.name, status: a ,state: 1})
      })
      while (realm_r.length < realms.length){
        await this.ctx.helper.sleepms(200)
      }
      this.ctx.body = realm_r
    }catch(err){
      this.ctx.logger.error(err)
    }
   
  }

  async serveradm(){
    const service = this.service;
    const ctx = this.ctx;
    const query_data = this.ctx.request.body;
    try {
      if(query_data.type == 'realm_ids'){
        let pl = [], result_s = 'success:', result_err = 'fail:'
        await this.ctx.validate(editRules, query_data);
        query_data.id.forEach(async function(v, i, r){
          let a = await service.mangs.managegs({realm_id: v, adm_command: query_data.adm_command})
          if(a.exitcode != 0){
            ctx.logger.error(`realm_id：${v}, 执行结果：${a}`)
          }
          pl.push({id: v, status: a.exitcode})
        })
        while (pl.length < query_data.id.length){
          await this.ctx.helper.sleepms(1000)
        }
        pl.forEach(function(v, i, r){
          if(v.status == 0) {
            result_s += ` ${v.id},`
          }else {
            result_err += ` ${v.id},`
          }
        })
        let result = {}
        result.state = result_err == 'fail:'
        result.reason = result_s.slice(0,-1) + '；' + result_err.slice(0,-1)
        this.ctx.body = result
        await this.ctx.write_log(this.ctx.app.action.info, {
          customType:'managegs',
          command: query_data,
          result: result
        });
      }else{
        let realm_id;
        await this.ctx.validate(editRule, query_data);
        if(query_data.type == 'server_id'){
          realm_id = await this.service.realmselector.get_realm_by_server_id(query_data.id)
          if(realm_id == 0){
            this.ctx.body = {state: false, reason: 'server_id error'}
            return
          }
        }else{
          realm_id = query_data.id
        }
        let resul = await this.service.mangs.managegs({realm_id: realm_id, adm_command: query_data.adm_command})

        let result = {}
        result.state = resul.exitcode == 0
        result.reason = resul.reason.toString()
        this.ctx.body = result
        await this.ctx.write_log(this.ctx.app.action.info, {
          customType:'managegs',
          command: query_data,
          result: result
        });
      }
    } catch(err) {
      this.ctx.body = {state: false, reason: err.toString()}
      this.ctx.logger.error(err)
    }

  };
}

module.exports = ServerAdminController;
