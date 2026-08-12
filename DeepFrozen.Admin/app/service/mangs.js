const Service = require('egg').Service
const Client = require('ssh2').Client
const socket = require('net').Socket

class MangsService extends Service {
  async managegs(data) {
    let realm, jieguo, ctx = this.ctx, ssh_config = this.config.ssh_config, conn = new Client();
    try{
      if(data.hasOwnProperty('realm_id')){
        realm = await this.ctx.model.Realm.find(data.realm_id);
      }else{
        let realm_id = await this.service.realmselector.get_realm_by_server_id(data.server_id);
        realm = await this.ctx.model.Realm.find(realm_id);
      }
      let realm_host = realm.gmt_url.match(/(192|172|10)(\.\d{1,3}){3}/)[0]
      let asyncsl = function(){
        return new Promise(function(resolve, reject){
          conn.on('ready', function(){ 
            conn.exec(`/bin/bash /data/${realm.address.split(':')[0]}/Publish.Server/kaiguanfu.sh ${data.adm_command}`, function(err, stream){
              let stderr_reason = '';
              if(err){
                reject({exitcode: 254, reason: err});
              }
              stream.on('exit', function(code){
                conn.end();
                resolve({exitcode: code, reason: stderr_reason});
              //}).on('data', function(data){
              //  console.log('Stdout: ', data.toString())
              }).stderr.on('data', function(data){
              //  console.log('Stderr: ', data.toString())
                stderr_reason += data.toString();
              });
            });
          })
          .on('error', function(err){
            reject({exitcode: 253, reason: err});
          })
          .connect({
            host: realm_host,
            port: ssh_config.port,
            username: ssh_config.username,
            privateKey : `-----BEGIN RSA PRIVATE KEY-----
MIIEowIBAAKCAQEAwga3rPjIs3qYEK3YjSgWJ5TbmIqnuOsXD3WrLjx00W7xdUU0
kvBb0zBOIuNegnvIUQpI8QNttKQ22KPKC50UWlIjJAIjcg9RYqP/+4ShVixshBid
0/hNSQkuMDKUU2b/c6aZU55CNSzwCWYwA1v/RHI2sIOX6gA/5cKggwimV6i7e65X
WpM2imw9fLearASbtXGOqFqkwoRLUOpH27ITJx84+d9FwKwX6ihi492d0bcp0rh8
vKXdn6bpv63JLQ0wRvF8tJVZkbjvib/n1q+aHiaJfq//BW69BH/SwdRFl5qCKHtT
lMcdTVFPcANy86Dspkhq7ivuwU5xR93kEGpF2wIDAQABAoIBAGJviAYQXbp4yudD
W6cZnEvOgK5eQgNV+egU+ZjGgTmzwHqPdsHJgbyeODYJBmzKofFDd4gXBjRsT4sP
ZbUIp0e/fiaJkzQNw2t46qjPPA79ELzfxErDIWqZZr57GrdJfPZOomFC3DIgL1OF
bXx20wYFr+tTh+Hm6IQHfumbnNDBXb4cOzFDc0szIAeUxpuBKHr+ApKnAxc/OlZ+
9cPvKbng7bu0AO55P9PBR5dyQ0U867ZmGdf/OCCYPzQpnjooeUhiHqMyuk/SZrnO
FeJ0eZMV9X4BvRiKFIo8S8qUnYiVPlvAUAPbMGai/amKpxoF6MSRA88BdAn0OU3J
i4Z76FECgYEA9liqoRytzRNTcnojOw+UxTVKr3JK7IDl6Z1BVjyJu/c8KNBOf37K
LxlRb4qw7RkmCs7Ow1MeOwCDPvJn+GeYPbLCzeynWdHVwDwW6aezZtPBqgNU84I4
cG6cHpswJhmeAIuu06y4kBptvi/HKNNz1LXTxWeOdjqRAnhzwBq+w6kCgYEAyaEt
qfz0Kubl1BEiEsCyDbRniRZfoH5lWtQKqeLvyGZfAyko05rFhOZMGmroWB6Do8Zj
TBQwNOnGc35K9qWTWXPjPEGn2FrThwuH30QWL5U0rW99LQyhjWO8f20E1872p7ee
X7S5C/7CrEOoyxonsqIxLkya7/UgGYXiwBrY7+MCgYEA9ezkQjyQISdG6gSWQTK3
0Pv3mBigRWQUodhUTpPW6GfTjecORmc7zjMXQiQRplCx0p5MeV/z0PEdFO0H8Erz
B7z6jnrPQnKhUexq7010YpXJQ0FaQFJip5M0lbVVJuPyARdxK23FPk6z+eZWiyI1
A0eVpv9PkCW9d9rpqwDIKikCgYBVhpfPDiwn8o80eBwC5lfgdlCglqvzcQnfECiB
zXA2ii3tMk1ODM2RxPotDoIFttPR6Xn/MGUOXMdAACceHje1bvHJgnQG0uf81YhJ
zSC4CEsy8iKftEbmqZKwXnb6b6CynbGBGTCRBPxgg8aaeDq0jBXedMIX3NtD2EXJ
hNnWTwKBgG5EhAoHdlSkKo8Nwu2w8dvt65dwLqLCjM64D4gGDEyRQdFTT4LQbcE8
SKspGyYVMx5jnvJ/0W0SkXOp1bQursjoFKwl1gEf8U1F+hL0tO8yliM73LVIcKn8
NDMVt49KA7vPt60A0tdnqgXZLtGEQZz9HjGTWIRS3cnQoD54STlE
-----END RSA PRIVATE KEY-----`
          });
        })
      }

      jieguo = await asyncsl()
    }catch(err){
      ctx.logger.error(err)
      if(err.reason == undefined) err = {exitcode: 252, reason: err}
      jieguo = err
    }
    conn = null
    return jieguo
  }

  async realmstate(realm){
    let ctx = this.ctx;
    let host = realm.address.split(':')[0];
    let port = realm.address.split(':')[1];
    let portstat;
    let testPort = function(host, port){
      return new Promise(function(resolve, reject){
          let nsk = new socket();
          nsk.setTimeout(5000);//设置连接超时时间  5s
          nsk.on('connect',function(){//连接状态
              nsk.destroy();//销毁
              resolve(0);
          })
          .on('timeout',function(){//连接超时
              nsk.destroy();
              reject('timeout');
          })
          .on('error',function(err){//连接错误
              nsk.destroy();
              ctx.logger.error(err)
              reject('error');
          }).connect(port, host);
      })		
    }
    try{
      portstat = await testPort(host, port)
    }catch(err){
      portstat = err
    }
    return portstat
  }
}

module.exports = MangsService;