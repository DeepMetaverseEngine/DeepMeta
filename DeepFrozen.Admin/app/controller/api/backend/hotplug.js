'use strict';
const Controller = require('egg').Controller;
const sendToWormhole = require('stream-wormhole');
const awaitStreamReady = require('await-stream-ready').write;
var path = require("path")
var fs = require("fs")
var AdmZip = require('adm-zip');

//验证规则
const editRule = {
    type: 'string',
    role: 'string',
    reason: 'string',
    server_id: 'id',
  };

class HotPlugController extends Controller {

  async reload(){
    var ctx = this.ctx
    var params = ctx.request.body;
    ctx.logger.info(params)

    var realm_list = params.realm_list
    var tables = params.tables

    try {
      for (var i = 0; i < realm_list.length; i++) {
        var command = {
          cmd: "ServerHotPlugReload",
          realm_id: realm_list[i],
          tables: params.tables,
        }
      var result = await this.service.gmt.send_command(command,this.ctx.__('common_instructions_success'));
      ctx.logger.info(result)
      if(!result.state){
        this.ctx.body = {state: false, reason: result.reason}
        return
      }

      }
    }catch(e){
      this.ctx.body = {state: false, reason: 'Unknow error.'}
      return
    }

    this.ctx.body = {state: true, reason: 'success.'}
  }
  async upload(){
    const { ctx } = this;

    try {
      const stream = await ctx.getFileStream();
      var parmas = stream.fields
      const filename = Date.now() + '' + Number.parseInt(Math.random() * 10000) + path.extname(stream.filename);
      const target = path.join(this.config.baseDir, 'app/public/upload/', filename);
      const writeStream = fs.createWriteStream(target);
      try { 
        await awaitStreamReady(stream.pipe(writeStream));
      } catch (err) {
        await sendToWormhole(stream);
        this.ctx.logger.error(e)
      }

      var results = []
      var success = true
      var reason = 'SUCCESS.'
      var realm_list = parmas.realm_list.split(',')
      for (var i = 0; i < realm_list.length; i++) {
        var result = await this.send_to_gs(realm_list[i], target)
        if(!result.state){
          success = false
          reason = result.reason
          results.push({realm_id: realm_list[i], reason: result.reason})
          break;
        }
      }

      if(results.length == 0){
        var zip = new AdmZip(target);
        var zipEntries = zip.getEntries();
        zipEntries.forEach(function(zipEntry) {
          if(!zipEntry.isDirectory && zipEntry.name != '_luaversion_.lua' && zipEntry.name.endsWith('.lua')){
            var prefix = zipEntry.entryName.substring(zipEntry.entryName.indexOf('/') + 1,zipEntry.entryName.lastIndexOf('/'))
            var ends = zipEntry.name.substring(0,zipEntry.name.length - 4)
            results.push(prefix + ' ' + ends)
          }
        });
        this.ctx.body = {state: true, ext: results, reason: reason};
      }else {
        this.ctx.body = {state: false, ext: results[0], reason: reason};
      }


    }catch(e){
      this.ctx.logger.error(e)
      this.ctx.body = {state: false, reason: 'Unknow error.'}
      return
    }

    
  }

  async send_to_gs(realm_id, target) {

    try {
      var command = {
        cmd: "ServerHotPlugUpload",
        realm_id: realm_id
      }
      var result = await this.service.gmt.send_command_attach(command,{files: target});
      return result;
    } catch(err) {
      this.ctx.logger.error(err)
    }
    return {state: false, reason: this.ctx.__('page_gmt_command_unknown_error')}
  }


}

module.exports = HotPlugController;
