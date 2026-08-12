const Service = require('egg').Service;
const nodemailer = require('nodemailer');
const pluck = require('arr-pluck');


class WarnmailService extends Service {
  async send_message(content) {
    try{
      var ctx = this.ctx;
      var mailtolist = await this.ctx.model.Mailist.findAll({where: {enable: 1}})
      var maillist = pluck(mailtolist, 'address').join(',');
      if(maillist == '') return
      var mailTransport = nodemailer.createTransport(this.config.email_config)
      var options = {
            from: 'gmt@bianliangsh.com',
            to: maillist,
            subject: 'gmt warning',
            //text: 'gmt warning',
            html: content
          }
      mailTransport.sendMail(options, function(err, msg){
        if(err){
          ctx.logger.error(err);
        } else {
          ctx.logger.info(msg);
        }
      })

    }catch(e){
      this.ctx.logger.error(e);
    }
  }
  async send_test_message(mail_to, content) {
    try{
      var ctx = this.ctx;
      var mailTransport = nodemailer.createTransport(this.config.email_config)
      var options = {
            from: 'gmt@bianliangsh.com',
            to: mail_to,
            subject: 'add email address success',
            html: content
          }
      mailTransport.sendMail(options, function(err, msg){
        if(err){
          ctx.logger.error(err);
        } else {
          ctx.logger.info(msg);
        }
      })
    }catch(e){
      this.ctx.logger.error(e);
    }
  }
}

module.exports = WarnmailService;