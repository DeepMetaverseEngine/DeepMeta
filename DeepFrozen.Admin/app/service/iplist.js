const Service = require('egg').Service;


class IplistService extends Service {
  async show() {
    let list = await this.ctx.model.Iplist.findAll();
    return list;
  }

  async validate_ip(ip) {
    var result = await this.ctx.model.Iplist.count({
                where:{
                    address: ip,
                    is_enable:true}
              });
    return result > 0; 
  }

}

module.exports = IplistService;