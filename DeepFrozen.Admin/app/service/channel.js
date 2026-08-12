const Service = require('egg').Service;


class ChannelService extends Service {
  async show() {
    let channels = await this.ctx.model.Channel.findAll();
    return channels;
  }
}

module.exports = ChannelService;