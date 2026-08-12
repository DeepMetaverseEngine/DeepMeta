'use strict';
const Controller = require('egg').Controller;
var crypto = require('crypto');
const moment = require('moment');

class InfoController extends Controller {

    async ban_role() {
        let ctx = this.ctx;
        var params = this.ctx.request.body
        var server_id = params.serverId
        var role_id = params.playerId
        var type = params.leixing
        var time = params.time
        var remarks = params.remarks
        var timestamp = params.timestamp
        var sign = params.sign;
        var sdk_name = 'EWAN_20534'


        if(!server_id || !role_id || !type || !time || !remarks || !sign){
            ctx.body = {success: false, msg:'Parameter cannot be empty'}
            ctx.logger.error('Parameter cannot be empty ' + JSON.stringify(params))
            return;
        }
        var realm_id = 0

        var secret_params = await ctx.model.Channel.findBySDKName(sdk_name)
        if(ctx.helper.is_empty(secret_params)){
            ctx.body = {success: false, msg:'not yet configuring'}
            ctx.logger.error('not yet configuring ' + JSON.stringify(params))
            return;
        }

        var local_sign = this.getSignature(secret_params.api_key + this.sortParams(params))
        this.ctx.logger.info(local_sign)
        if(local_sign != sign) {
            ctx.body = {success: false, msg:'signature error'}
            ctx.logger.error('signature error ' + this.sortParams(params) + ' local='+local_sign + ' sign=' + sign)
            return;
        }

        var has_server;
        let server_list = await this.ctx.service.serverlist.get_all_servers()
        server_list.forEach(function(val) {
              if(val.id == server_id) {
                has_server = true;
                realm_id = val.realm_id
              }
        });

        if(!has_server) {
            this.ctx.body = {success: false, msg:'serverId not exist'}
            ctx.logger.error('serverId not exist ' + JSON.stringify(params))
            return;
        }

        if(type == 0){
            try {
              var command = {
                cmd: "blacklist",
                channel: [0, 1, 2, 3, 4, 5],
                realm_id: realm_id,
                type: 6,
                minutes: time,
                role: role_id
              }
              var result = await this.service.gmt.send_command(command,'success');

              if(result.state){
                await this.ctx.write_log(this.ctx.app.action.info, {
                    customType:'blacklist', 
                    command: {server_id: params.serverId, digit_id: params.playerId, type: 6, op_reason: params.remarks}, 
                    result: result
                  });
                this.ctx.body = {success: true, msg:''}
              }else {
                this.ctx.body = {success: false, msg:'playerId not exist.'}
              }
            } catch(err) {
              this.ctx.logger.error(err)
              this.ctx.body = {success: 0, msg:'Internal Exception'}
            }
        }else {
            var date = moment().add(time, 'minutes')
            try {
              var command = {
                cmd: "ban",
                type: time == 0 ? -1 : 1,
                date: date,
                realm_id: realm_id,
                reason: remarks,
                minutes: time,
                role_id: role_id
              }
              var result = await this.service.gmt.send_command(command,'success');

              if(result.state){
                await this.ctx.write_log(this.ctx.app.action.info, {
                    customType:'ban', 
                    command: {server_id: params.serverId, digit_id: params.playerId, online_state: params.remarks, end_dt: time == 0 ? 0 : moment(date).format('YYYY-MM-DD HH:mm:ss'), action: time == 0 ? 'remove' : 'edit'}, 
                    result: result
                  });
                this.ctx.body = {success: true, msg:''}
              }else {
                this.ctx.body = {success: false, msg:'playerId not exist.'}
              }
            } catch(err) {
              this.ctx.logger.error(err)
              this.ctx.body = {success: false, msg:'Internal Exception'}
            }
        }
    }
	
 	async ban_ip() {
    	let ctx = this.ctx;
        var params = this.ctx.request.body
        var ip = params.ip
        var time = params.time
        var remarks = params.remarks
        var timestamp = params.timestamp
        var sign = params.sign;
        var sdk_name = 'EWAN_20534'


        if(!ip || !time || !remarks || !timestamp || !sign){
            ctx.body = {success: false, msg:'Parameter cannot be empty'}
            ctx.logger.error('Parameter cannot be empty ' + JSON.stringify(params))
            return;
        }

        var secret_params = await ctx.model.Channel.findBySDKName(sdk_name)
        if(ctx.helper.is_empty(secret_params)){
            ctx.body = {success: false, msg:'not yet configuring'}
            ctx.logger.error('not yet configuring ' + JSON.stringify(params))
            return;
        }

        var local_sign = this.getSignature(secret_params.api_key + this.sortParams(params))
        this.ctx.logger.info(local_sign)
        if(local_sign != sign) {
            ctx.body = {success: false, msg:'signature error'}
            ctx.logger.error('signature error ' + this.sortParams(params) + ' local='+local_sign + ' sign=' + sign)
            return;
        }

        try{
          var date = moment().add(time, 'minutes')
          var task = await this.ctx.model.Blocklist.create({
            address: ip,
            type: 'IP',
            remark: remarks,
            end_dt: date,
            created_dt: new Date(),
          })
          this.ctx.body = {success: true, msg:''}
        }catch(err){
          this.ctx.logger.error(err);
          this.ctx.body = {success: false, msg:'ip already exist.'}
        }
 	}

    //参数排序
    sortParams(obj)
    {
        var sorted_keys = Object.keys(obj).sort();
        var sorted_signMap = {};
        var signedStr = '';
        for(var i=0;i<sorted_keys.length;i++){
            sorted_signMap[sorted_keys[i]] = obj[sorted_keys[i]]
            if(obj[sorted_keys[i]] !=='undefined' && sorted_keys[i] != 'sign' && sorted_keys[i] != 'serverId'  && sorted_keys[i] != 'leixing'){
                signedStr += sorted_keys[i] + '=' + sorted_signMap[sorted_keys[i]];
            }
        }
        return signedStr;
    }

    getSignature(params) {
        this.ctx.logger.info('sign=' + params)
        return crypto.createHash('md5').update(params).digest("hex").toLowerCase();
    }
 	
}

module.exports = InfoController;
