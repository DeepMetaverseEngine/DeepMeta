'use strict';

var _ = require("underscore")._;


const Controller = require('egg').Controller;
const path = require('path');
const moment = require('moment');
const sendToWormhole = require('stream-wormhole');
const excelToJson = require('convert-excel-to-json');
const fs = require('mz/fs');
const util = require('util');



//编辑规则
const editRule = {
    activity_key: 'string',
    activity_status: 'id',
    show_type: 'id',
    activity_type: 'id',
    activity_name: 'string',
    activity_id: 'id',
    limit_time: 'id',
    start_time: {type: 'string', allowEmpty: true},
    end_time: {type: 'string', allowEmpty: true},
    open_time: 'id',
    last_time: 'id',
    over_keep: 'id'
  };

class GMActivityController extends Controller {
  async index(){
    this.ctx.body = await this.ctx.service.gmactivity.show()
  };

  async upload(){
    const { ctx } = this;
    const stream = await ctx.getFileStream();
    try {
      var buffer = await this.streamToBuffer(stream)
      const activitys = excelToJson({
          source: buffer,
          header:{
              rows: 2
          },
          columnToKey: {
              '*': '{{columnHeader}}'
          }
      });

      try {
        for (var i = 0; i < activitys.all_activity.length; i++) {
          if (i == 0)continue
          var activity = {
            activity_key: activitys.all_activity[i].activity_key,
            activity_status: activitys.all_activity[i].activity_status,
            show_type: activitys.all_activity[i].show_type,
            activity_type: activitys.all_activity[i].activity_type,
            show_icon: activitys.all_activity[i].show_icon,
            activity_name: activitys.all_activity[i].activity_name,
            xlsx_name: activitys.all_activity[i].xlsx_name,
            sheet_name: activitys.all_activity[i].sheet_name,
            activity_id: activitys.all_activity[i].activity_id,
            server_type: activitys.all_activity[i].server_type,
            order: activitys.all_activity[i].order,
            client_lua: activitys.all_activity[i].client_lua,
            client_xml: activitys.all_activity[i].client_xml,
            goto_key: activitys.all_activity[i].goto_key,
            not_open_before: activitys.all_activity[i].not_open_before,
            not_open_after: activitys.all_activity[i].not_open_after,
            server_id: activitys.all_activity[i].server_id,
            limit_time: activitys.all_activity[i].limit_time,
            start_time: activitys.all_activity[i].start_time,
            end_time: activitys.all_activity[i].end_time,
            open_time: activitys.all_activity[i].open_time,
            last_time: activitys.all_activity[i].last_time,
            over_keep: activitys.all_activity[i].over_keep,
            need_Listener: activitys.all_activity[i].need_Listener,
            requesttype: activitys.all_activity[i].requesttype,
            open_red_point: activitys.all_activity[i].open_red_point,
            check_key: activitys.all_activity[i].check_key
          }
          var instance = await ctx.model.GmActivity.find(activity.activity_key)
          if(ctx.helper.is_empty(instance)){
            await ctx.model.GmActivity.create(activity)
          }else {
            await ctx.model.GmActivity.updateByKey(activity,activity.activity_key)
          }
        }
      }catch (err) {
        ctx.logger.error(err)
        this.ctx.body = {state: false, reason: this.ctx.__('page_gmactivity_upload_failed')}
      }

    } catch (err) {
      await sendToWormhole(stream);
      ctx.logger.error(err)
      this.ctx.body = {state: false, reason: this.ctx.__('page_gmt_command_unknown_error')}
    }

    this.ctx.body = {state: true, reason: this.ctx.__('common_instructions_success')}
  }

  async sync() {
    var last_time = await this.ctx.service.sysconfig.get('activity_sync_time')

    var activitys = await this.ctx.model.GmActivity.findChanged(last_time.value || '2000-01-01 00:00:00')


    //server_id format
    activitys.forEach(function(data) {
      if(typeof(data.server_id) == 'string'){
        data.server_id = data.server_id.split(',')
      }else if(typeof(data.server_id) == 'number'){
        data.server_id = [data.server_id]
      }

      if(data.check_key != null){
        data.check_key = data.check_key.split(',')
        if(!Array.isArray(data.check_key)){
            data.check_key = [data.check_key]
          }
      }
    });

    var group = _.groupBy(activitys,function(o){ return o.activity_status; })

    var remove_list = group[-1] || []

    var sync_list = []
    if(group[1]){
      sync_list = group[1].concat(group[0])
    }

    var now_date = moment(new Date()).format('YYYY-MM-DD HH:mm:ss')

    var realm_list = await this.ctx.service.serverlist.get_realms()

    try {
      var results = []
      for (var i = 0; i < realm_list.length; i++) {
        var command = {
          cmd: "ServerBusinessActivityData",
          realm_id: realm_list[i].id,
        }

        var data = {
          sync_list: sync_list,
          remove_list: remove_list
        }
        
        var result = await this.service.gmt.send_command_post(command, data, this.ctx.__('common_instructions_success'));
        if(!result.state){
          results.push({state: false, reason: result.reason + ' RealmId:' + realm_list[i].id + ' Name:' + realm_list[i].name})
        }

      }
    }catch(e){
      this.ctx.logger.error(e)
      this.ctx.body = {state: false, reason: 'Unknow error.'}
      return
    }

    if(results.length == 0) {
      await this.ctx.service.sysconfig.set('activity_sync_time', now_date)
      this.ctx.body = {state: true, ext:now_date, reason: this.ctx.__('common_instructions_success')}
    }else {
      this.ctx.body = {state: false, ext:now_date, reason: results}
    }


    

  }

  async streamToBuffer(stream) {  
    return new Promise((resolve, reject) => {
      let buffers = [];
      stream.on('error', reject);
      stream.on('data', (data) => buffers.push(data))
      stream.on('end', () => resolve(Buffer.concat(buffers)))
    });
  }  

  async create(){
    const ctx = this.ctx;
    let action = ctx.request.body.action;
    let data = ctx.get_request_primary_data(ctx.request.body.data)

    if(action == 'edit') {
      try {
        await ctx.validate(editRule, data);
        await ctx.model.GmActivity.updateByKey({
            activity_status: data.activity_status,
            show_type: data.show_type,
            activity_type: data.activity_type,
            activity_name: data.activity_name,
            activity_id: data.activity_id,
            limit_time: data.limit_time,
            start_time: data.start_time,
            end_time: data.end_time,
            open_time: data.open_time,
            last_time: data.last_time,
            over_keep: data.over_keep,
          },data.activity_key);

        var result = await this.ctx.model.GmActivity.find(data.activity_key);
        ctx.body = {data:[result]}
      } catch(err) {
        ctx.response.rsp_table_field_errors(err.errors);
        ctx.logger.error(err)
      }
    }else if(action == 'remove') {
      var task = await ctx.model.GmActivity.findOne({
         where: {activity_key: data.activity_key}
      });
      task.activity_status = -1
      await task.save();
      // await ctx.write_log(ctx.app.action.destroy, {after: task, filter: ['gmt_key']})
      ctx.body = {data:[]};
    }
  };


  async edit(){};

  async update(){};

  async destroy(){};

  async new() {
    this.ctx.body = 'new';
  }
}

module.exports = GMActivityController;
