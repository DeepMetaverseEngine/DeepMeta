'use strict';
const Controller = require('egg').Controller;

class GetSignatureController extends Controller {
  async index() {
    var params = this.ctx.request.body;
    var sdk_name = params.sdkName;
    var result = '';
    if(sdk_name == 'uc') {
      result = await this.ctx.service.pay.uc.getSignature(params);
    }else if(sdk_name == 'OneGame') {//测试渠道
      result = await this.ctx.service.pay.onegame.getSignature(params);
    }else {
      result = {code:0, message:'sdkname error.'}
    }
    this.ctx.body = result;
  }
}

module.exports = GetSignatureController;
