'use strict';
const Controller = require('egg').Controller;

class ItemController extends Controller {
  async get_all_item(){
    var command = {
      cmd: "ServerItemBase",
      lang: "zh_CN"
    }
    var result = await this.service.gmt.send_command(command);
    this.ctx.body = result.ext
  };

  async get_realm_item(){
    var command = {
      realm_id: this.ctx.query.realm_id,
      cmd: "ServerItemBase",
      lang: "zh_CN"
    }
    var result = await this.service.gmt.send_command(command);
    this.ctx.body = result.ext
  };
}

module.exports = ItemController;
