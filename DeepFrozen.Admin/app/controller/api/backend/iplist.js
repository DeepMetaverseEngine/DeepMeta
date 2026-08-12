'use strict';
const Controller = require('egg').Controller;
//规则
const rule = {
    type: 'string',
    address: 'string',
    is_enable: 'id',
    remark: {type: 'string',allowEmpty: true}
  };

class IplistController extends Controller {
  async index(){
    this.ctx.body = await this.ctx.service.iplist.show()
  };

  async create(){
    const ctx = this.ctx;
    let action = ctx.request.body.action;
    let data = ctx.get_request_primary_data(ctx.request.body.data)
    
    if(action == 'edit') {
      try {
        await ctx.validate(rule, data);
        var update = await ctx.model.Iplist.find(data.id);
        await ctx.model.Iplist.update({
            type: data.type,
            address: data.address,
            is_enable: data.is_enable,
            remark: data.remark
          },
          { 
            where:{id: data.id
            }
        });
        
        result = await this.ctx.model.Iplist.findOne({
          where:{id: data.id}
        });

        //await ctx.write_log(ctx.app.action.update, {before: update, after: result, filter: ['gmt_key']})
       
        ctx.body = {data:[result]}
      } catch(err) {
        ctx.response.rsp_table_field_errors(err.errors);
      }
      

    }else if(action == 'remove') {
      var task = await ctx.model.Iplist.findOne({
         where: {id: data.id}
      });

      await task.destroy();
      //await ctx.write_log(ctx.app.action.destroy, {after: task, filter: ['gmt_key']})
      ctx.body = {data:[]};
    }else if(action == 'create') {
       try {
        await ctx.validate(rule, data);
      }catch(err){
        ctx.response.rsp_table_field_errors(err.errors);
        return;
      }
      try {
        var task = await ctx.model.Iplist.create({
            type: data.type, 
            address: data.address,
            is_enable: data.is_enable,
            remark: data.remark
          })
          var result = await ctx.model.Iplist.findOne({
                where:{id: task.id}
              });
          //await ctx.write_log(ctx.app.action.create, {filter:['pwd_encrypt','salt'], after:result})
          ctx.body = {data:[result]}
        }
      catch(err) {
        this.ctx.logger.info(err)
        ctx.response.rsp_table_error(err.errors);
      }
    }
  };

  async show(){
    this.ctx.body = 'show';
  };

  async edit(){};

  async update(){};

  async destroy(){};

  async new() {
    this.ctx.body = 'new';
  }
}

module.exports = IplistController;
