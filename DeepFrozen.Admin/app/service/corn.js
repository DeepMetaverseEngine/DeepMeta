const Service = require('egg').Service;
const moment = require('moment');

class CornService extends Service {

	randomNum(minNum,maxNum){ 
	    switch(arguments.length){ 
	        case 1: 
	            return parseInt(Math.random()*minNum+1,10); 
	        break; 
	        case 2: 
	            return parseInt(Math.random()*(maxNum-minNum+1)+minNum,10); 
	        break; 
	            default: 
	                return 0; 
	            break; 
	    } 
	} 

  async update_five_minutes(){
    //5分钟视图更新
    this.five_views()
  }

  async send_notification() {

    var list = await this.ctx.model.EarlyWarningRecord.find()
    var arr = []
    var idx = []
    list.forEach(function(entity) {
      idx.push(entity.id)
      if(entity.type == 0){
        if(entity.sub_type == 0){
          var item_name = '道具(' + entity.condition + ')'
          if(entity.condition == 3){
            item_name = '元宝'
          }
          arr.push('服务器ID:' + entity.server_id + ' 角色名称:' + entity.role_name + ' ID:' + entity.role_id + ' 触发[' + item_name + ']预警' + ',阈值' + entity.value + ',实际值' + entity.actual + ',来源:' + entity.reason)
        }
      }

    });

    if(arr.length > 0){
      var content = '报警!\n' + arr.join('\n')
      await this.ctx.service.talkRobot.send_message('https://oapi.dingtalk.com/robot/send?access_token=3785f489b5b8580822292b1e56e2f9c23e4c672c47113c18ceb87a0cc95dac8f',content);
      await this.ctx.service.warnmail.send_message(content);
      await this.ctx.model.EarlyWarningRecord.finish(idx)
    }
  }


  async update_early_warning(){
    //单次监控预警作业 检测前5分钟变化
    var list = await this.ctx.model.EarlyWarningSettings.findByType(0, 0);
    var monitor_arr = []
   
    var date=new Date()
    // dt1 = '2019-11-12 00:00:00'
    // dt2 = '2019-11-12 23:59:59'
    var dt1 = moment(date).add(-10,'minutes').format('YYYY-MM-DD HH:mm:00');
    var dt2 = moment(date).add(-6,'minutes').format('YYYY-MM-DD HH:mm:59')
    var insert_list = []
    for (var i = 0; i < list.length; i++) {
      var monitor = list[i]
      
      if(monitor.condition == 3){
        var entity_list = await this.ctx.biModel.YuanbaoGain.get_once_overflow(monitor.value,dt1,dt2)
        entity_list.forEach(function(entity) {
          insert_list.push({
            time: entity.time,
            type: monitor.type,
            sub_type: monitor.sub_type,
            condition: monitor.condition,
            server_id: entity.server_id,
            role_id: entity.role_id,
            role_name: entity.role_name,
            value: monitor.value,
            actual: entity.AddDiamond,
            total: entity.Diamond,
            reason: entity.reason,
            status: 0
          })
        });
      } else{
        var entity_list = await this.ctx.biModel.Itemgain.get_once_overflow(dt1,dt2,monitor.condition, monitor.value)
        entity_list.forEach(function(entity) {
          insert_list.push({
            time: entity.time,
            type: monitor.type,
            sub_type: monitor.sub_type,
            condition: monitor.condition,
            server_id: entity.server_id,
            role_id: entity.role_id,
            role_name: entity.role_name,
            value: monitor.value,
            actual: entity.AddValue,
            reason: entity.reason,
            status: 0
          })
        });
      }
    }
    try {
      await this.ctx.model.EarlyWarningRecord.bulkCreate(insert_list)
    }catch(err){
      this.ctx.logger.error('bulk create EarlyWarningRecord error.')
      this.ctx.logger.error(err)
    }
  }


  async five_views() {
    var datetime =  moment().format('YYYY-MM-DD HH:mm:00');
    var ymd = datetime.split(' ')[0]
    var ctx = this.ctx
    var insert_list = []
    let server_list = await this.ctx.service.serverlist.get_all_servers()
    for (var i = 0; i < server_list.length; i++) {
      var server_id = server_list[i].id
      //在线账号数量/在线角色数量
      var online_id = await this.ctx.biModel.LogOnline.getOnline(ymd, server_list[i].realm_id, server_list[i].group)
      //今日累计登陆账号
      var total_logged_openid = await ctx.biModel.LogLoginRole.countByType(ymd, server_id, 'openid')
       //今日累计登陆角色
      var total_logged_id = await ctx.biModel.LogLoginRole.countByType(ymd, server_id, 'role_id')
      //今日充值额度
      var total_recharge_amount = await ctx.biModel.LogPrepaid.sumBy(ymd, server_id, -1)
      //今日充值账号
      var recharge_openid = await ctx.biModel.LogPrepaid.chargecount(ymd, server_id, 'openid')
      //今日充值角色
      var recharge_id = await ctx.biModel.LogPrepaid.chargecount(ymd, server_id, 'role_id')
      //今日注册账号
      var total_reg_openid = await ctx.biModel.LogCreateRole.countByType(ymd, server_id, 'openid')
      //今日注册角色
      var total_reg_id = await ctx.biModel.LogCreateRole.countByType(ymd, server_id, 'role_id')

      insert_list.push({
        ymd: ymd,
        time: datetime,
        server_id: server_id,
        online_openid: online_id,
        online_id: online_id,
        total_logged_openid: total_logged_openid,
        total_logged_id: total_logged_id,
        total_recharge_amount: total_recharge_amount,
        recharge_openid : recharge_openid,
        recharge_id : recharge_id,
        total_reg_openid : total_reg_openid,
        total_reg_id : total_reg_id
      })
    }

    try {
      await this.ctx.biModel.LogMintuesRecord.bulkCreate(insert_list)
    }catch(err){
      ctx.logger.error('bulk create error.')
      ctx.logger.error(err)
    }
  }
}

module.exports = CornService;