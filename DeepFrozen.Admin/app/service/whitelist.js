const Service = require('egg').Service;


const verifyRule = {
  account: 'string',
  password: 'password',
};


class WhitelistService extends Service {
  async show() {
    let ctx = this.ctx;
    let accounts = await ctx.model.Whitelist.findAll();
    for(var i=0;i<accounts.length;i++){
    	accounts[i].dataValues.title = ctx.__(this.get_privilege_by_group(accounts[i].privilege).title);
    }
    var options = []

    ctx.app.rolePrivileges.forEach(function (privilege) {
    	options.push({label:ctx.__(privilege.title),value:privilege.privilege})
    });
    return {data:accounts,options:{'privilege':options}};
  }

  async verify_account(){
    let ctx = this.ctx;
    var params = ctx.request.body;
    try {
      ctx.logger.info(params)
      await ctx.validate(verifyRule, ctx.request.body);
    }catch(err) {
      ctx.logger.info(err)
      return {status:-1,message:"Verification failed."};
    }
    // if(ctx.helper.is_local_ip(ctx.ip)){
    //   return {status:1, privilege:3, message:"ip whitelist verified. "};
    // }
    var data = await this.ctx.model.Whitelist.findByName(params.account)
    if(!ctx.helper.is_empty(data)){
      var password = new Buffer(params.password, 'base64').toString()
      if(!data.is_enable){
        return {status:-1,message:"Account is disabled."};
      }
      if(data.verifyPassword(password)){
        data.loginUpdate()
        return {status:1, privilege:data.privilege, message:"Successful."};
      }else {
        return {status:-1,message:"Incorrect password."};
      }
    }else{
      return {status:-1,message:"Account not exist."};
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

module.exports = WhitelistService;