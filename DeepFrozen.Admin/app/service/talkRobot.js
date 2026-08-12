const Service = require('egg').Service;


class TalkRobotService extends Service {
  async send_message(token, content) {
    try{
      const result = await this.ctx.curl(token, {
        method: 'POST',
        dataType: 'json',
        timeout: 30000,
        contentType: 'json',
        data: {
          "msgtype": "text", 
          "text": {
             "content": content
           }, 
        },
      });
    }catch(e){
      this.ctx.logger.error(e);
    }
  }
}

module.exports = TalkRobotService;