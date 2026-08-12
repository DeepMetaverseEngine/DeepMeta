const Service = require('egg').Service;

class ServerListService extends Service {
  async show() {
    let ctx = this.ctx;
    let users = await ctx.model.User.findAll(
      {include: [ { model: ctx.model.Usergroup, as: 'group', attributes: ['user_group_name']}]}
    );
    var options = []
    let prilist = await ctx.model.Usergroup.findAll({where: {id: {[ctx.model.Op.gt]: 1 }}})
    prilist.forEach(function(v, i, a){
    	options.push({label: v.dataValues.user_group_name, value: v.dataValues.id})
    });
    return {data:users,options:{'privilege':options}};
  }

  async find_one(id) {
  	var user = await this.ctx.model.User.findOne({
          where:{id: id},
          include: [ { model: this.ctx.model.Usergroup, as: 'group', attributes: ['user_group_name']}]
        });
  	//user.dataValues.title = this.ctx.__(this.get_privilege_by_group(user.privilege).title)
  	return user;
  }

  //get_privilege_by_group(privilege) {
  //	var idx = privilege - 1
  //	let privileges = this.ctx.app.config.privileges;
  //	return privileges[idx];
  //}
}

module.exports = ServerListService;