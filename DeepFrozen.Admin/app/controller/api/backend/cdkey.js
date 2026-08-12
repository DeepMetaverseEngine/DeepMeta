'use strict';
const Controller = require('egg').Controller;
//创建规则
const rule = {
    qty: 'id',
    id: 'id'
  };


class CdkeyController extends Controller {

  async create(){
    const ctx = this.ctx;
     try {
        await ctx.validate(rule, ctx.query);
        const req = this.ctx.query;

        var activity = await this.ctx.model.Activity.find(req.id);

        if(ctx.helper.is_empty(activity)){
          this.ctx.body = 'activity not exist.';
          return
        }
        let cdkeys = await this.ctx.service.activity.generate_cdkey(req.id, activity.prefix, req.qty);
        var rtn = "";
        cdkeys.forEach(function(value, key) {
          rtn = rtn + key + "\n";
        });
        this.ctx.body = rtn;
      } catch(err) {
        this.ctx.body = err.errors;
      }

  };


  async edit(){};

  async update(){};

  async destroy(){};

  async new() {
    this.ctx.body = 'new';
  }
}

module.exports = CdkeyController;
