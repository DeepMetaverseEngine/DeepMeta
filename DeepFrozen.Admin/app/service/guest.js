const Service = require('egg').Service;
const crypto = require('crypto');


const verifyRule = {
  account: 'string',
  password: 'password',
};


class GuestService extends Service {
  async create_guest() {
    let ctx = this.ctx;
      try {
        var guest_account = this.ctx.service.rechargelist.generate_order()
        var pwd = Math.random().toString(36).slice(-10);
        ctx.logger.error(guest_account,pwd);
        var salt = ((new Date())/1).toString();
        var hash = crypto.createHmac('sha256', pwd+salt).digest('hex');
        var task = await ctx.model.Guest.create({
          username: guest_account,
          pwd_encrypt: hash,
          salt: salt
        })
        return {state:1, account:guest_account, pwd:Buffer.from(pwd).toString('base64')}
      }
      catch(err) {
        ctx.logger.error(err)
        return {state:0, message:'error.'}
      }
  }

  async find_one(id) {
  	var account = await this.ctx.model.Whitelist.findOne({
          where:{id: id},
        });
  	account.dataValues.title = this.ctx.__(this.get_privilege_by_group(account.privilege).title)
  	return account;
  }

  async find_by_name(name) {
    
    return account;
  }

  get_privilege_by_group(privilege) {
  	var idx = privilege
  	let privileges = this.ctx.app.rolePrivileges;
  	return privileges[idx];
  }
}

module.exports = GuestService;