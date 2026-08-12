'use strict';
const Controller = require('egg').Controller;
const moment = require('moment');
const pluck = require('arr-pluck');

class BiController extends Controller {
  async get_remain() {
    let ctx = this.ctx;
    var params = ctx.request.body
    var server_id = params.server_id
    var channel = params.channel
    var start_date = params.date1
    var end_date = params.date2
    var channel_combine = ! channel.includes('-1') && ! eval(params.channel_combine)
    var server_combine = ! eval(params.server_combine) && ! server_id.includes('-1')
    var data_range = ctx.helper.getDateRange(start_date, end_date)
    var query_result = []
    for (var i = 0; i < data_range.length; i++) {
      let today_reg = await ctx.biModel.LogCreateRole.Todayreg(data_range[i], server_id, channel)
      let today_reg_num = 0, pp = {}
      //if(today_reg.length == 0){
      //  query_result.push({ymd: data_range[i], server_id: server_id, channel: channel, regnum: 0})
      //  continue
      //}
      let reglist = pluck(today_reg, 'role_id');
      let ymds = [
          moment(data_range[i]).add('d', 1).format('YYYY-MM-DD'), 
          moment(data_range[i]).add('d', 2).format('YYYY-MM-DD'), 
          moment(data_range[i]).add('d', 3).format('YYYY-MM-DD'), 
          moment(data_range[i]).add('d', 4).format('YYYY-MM-DD'), 
          moment(data_range[i]).add('d', 7).format('YYYY-MM-DD'),
          moment(data_range[i]).add('d', 14).format('YYYY-MM-DD'), 
          moment(data_range[i]).add('d', 29).format('YYYY-MM-DD'), 
          moment(data_range[i]).add('d', 59).format('YYYY-MM-DD'), 
          moment(data_range[i]).add('d', 89).format('YYYY-MM-DD'), 
          moment(data_range[i]).add('d', 179).format('YYYY-MM-DD'), 
      ]
      ymds.filter(function(yv){
        return Date.parse(yv) <= Date.now()
      })
      var countlogin = await ctx.biModel.LogLoginRole.countdetail(ymds, reglist, server_combine, channel_combine)
      if(countlogin.length == 0){
        if(server_combine && channel_combine){
          countlogin.push({ dataValues: {server_id: '', channel: ''} })
        }else if(server_combine){
          countlogin.push({ dataValues: {server_id: ''} })
        }else if(channel_combine){
          countlogin.push({ dataValues: {channel: ''} })
        }else{
          query_result.push({ymd: data_range[i], server_id: server_id, channel: channel, regnum: today_reg.length})
          continue
        }
      }
      if( countlogin[0].dataValues.server_id !== undefined && countlogin[0].dataValues.channel !== undefined){
        server_id.forEach((sr,sindex,sarray)=>{
          channel.forEach((cr,cindex,carray)=>{
            today_reg_num = 0
            today_reg.forEach((trr,tindex,tarray)=>{
              if(trr.dataValues.server_id == sr && trr.dataValues.channel == cr){
                today_reg_num = today_reg_num + 1
              }
            })
            pp = {server_id: sr, channel: cr, regnum: today_reg_num, ymd: data_range[i]};
            countlogin.forEach((vr,rindex,rarray)=>{
              if(vr.dataValues.server_id == sr && vr.dataValues.channel == cr && today_reg_num >0){
                var diffdd = moment(vr.dataValues['ymd']).diff(moment(data_range[i]), 'days')
                pp[diffdd + 1] = (vr.dataValues['countlogin'] / today_reg_num *100).toFixed(2)
              }
            })
            query_result.push(pp)
          })
        })
      }else if( countlogin[0].dataValues.server_id !== undefined){
        server_id.forEach((sr,sindex,sarray)=>{
          today_reg_num = 0
          today_reg.forEach((trr,tindex,tarray)=>{
            if(trr.dataValues.server_id == sr){
              today_reg_num = today_reg_num + 1
            }
          })
          pp = {server_id: sr, channel: channel, regnum: today_reg_num, ymd: data_range[i]};
          countlogin.forEach((vr,rindex,rarray)=>{
            if(vr.dataValues.server_id == sr && today_reg_num >0){
              var diffdd = moment(vr.dataValues['ymd']).diff(moment(data_range[i]), 'days')
              pp[diffdd + 1] = (vr.dataValues['countlogin'] / today_reg_num *100).toFixed(2)
          }
          })
          query_result.push(pp)
        })
      }else if(countlogin[0].dataValues.channel !== undefined){
        channel.forEach((cr,cindex,carray)=>{
          today_reg_num = 0
          today_reg.forEach((trr,tindex,tarray)=>{
            if(trr.dataValues.channel == cr){
              today_reg_num = today_reg_num + 1
            }
          })
          pp = {server_id: server_id, channel: cr , regnum: today_reg_num, ymd: data_range[i]};
          countlogin.forEach((vr,rindex,rarray)=>{
            if(vr.dataValues.channel == cr && today_reg_num >0){
              var diffdd = moment(vr.dataValues['ymd']).diff(moment(data_range[i]), 'days')
              pp[diffdd + 1] = (vr.dataValues['countlogin'] / today_reg_num *100).toFixed(2)
          }
          })
          query_result.push(pp)
        })
      }else{
        pp = { ymd: data_range[i], server_id: server_id, channel: channel, regnum: today_reg.length }
        countlogin.forEach((vr,rindex,rarray)=>{
          var diffdd = moment(vr.dataValues['ymd']).diff(moment(data_range[i]), 'days')
          pp[diffdd + 1] = (vr.dataValues['countlogin'] / today_reg.length *100).toFixed(2)
  })
        query_result.push(pp)
      }
    }
    ctx.body = {state: true, info:query_result, reason: ctx.__('common_instructions_success')}
  };


  async get_preview() {
    let ctx = this.ctx;
    var params = ctx.request.body
    var server_id = params.server_id
    var channel = params.channel
    var start_date = params.date1
    var end_date = params.date2

    var data_range = ctx.helper.getDateRange(start_date, end_date)


    var query_result = []
    for (var i = 0; i < data_range.length; i++) {

      //当日活跃
      var today_active_num = await ctx.model.Bi.countByType(ctx.app.consts_bi.login, data_range[i], server_id, channel)
      //当日注册
      // var today_reg_num = await ctx.model.Bi.countByType(ctx.app.consts_bi.createrole, data_range[i], server_id, channel)
      var today_reg = await ctx.model.Bi.findAndCountByType(ctx.app.consts_bi.createrole, data_range[i], server_id, channel)
      var today_reg_num = today_reg.length

      //当日创角
      var today_create_num = today_reg_num
      //创角率
      var today_create_rato = (today_create_num / today_reg_num).toFixed(2)
      //当日总付费
      var today_pay_num = await ctx.model.Order.AllMoneyByDate(data_range[i], server_id, channel) / 100

      //当日付费人数
      var today_pay_role_num = await ctx.model.Order.countByDate(data_range[i], server_id, channel)
      //当日付费率
      var today_pay_rato = (today_pay_role_num / today_active_num).toFixed(2)
      //ARPU
      var arpu = today_pay_num / today_active_num
      //ARRPU
      var arrpu = today_pay_num / today_pay_role_num

      //新增付费额
      var split_range = []
      today_reg.forEach(function(val) {
          var ext = val.split(':')
          if(ext.length > 1){
            split_range.push(ext[1])
          }else {
             split_range.push(ext[0])
          }
          
      });
      var new_today_pay_num = await ctx.model.Order.findByRangeSum(split_range, data_range[i], server_id, channel) / 100
      
      //新增付费人数
      var today_new_pay_num = await ctx.model.Order.countByRange(split_range, data_range[i], server_id, channel)
      //新增付费率
      var today_new_pay_rato = (today_new_pay_num / today_create_num).toFixed(2)
      //新arpu
      var new_arpu = (new_today_pay_num / today_create_num).toFixed(2) / 100
      //新arppu
      var new_arppu = (new_today_pay_num / today_new_pay_num).toFixed(2) / 100
      
      query_result.push({date: data_range[i],
                    today_active_num: today_active_num,
                    today_reg_num: today_reg_num,
                    today_create_num: today_create_num,
                    today_create_rato: today_create_rato,
                    today_pay_num: today_pay_num,
                    today_pay_role_num: today_pay_role_num,
                    today_pay_rato: today_pay_rato,
                    arpu: arpu,
                    arrpu: arrpu,
                    new_today_pay_num: new_today_pay_num,
                    today_new_pay_num: today_new_pay_num,
                    today_new_pay_rato: today_new_pay_rato,
                    new_arpu: new_arpu,
                    new_arppu: new_arppu
      })


    }
    ctx.body = {state: true, info:query_result, reason: ctx.__('common_instructions_success')}
  }


  async server_data_active() {
    let ctx = this.ctx;
    var params = ctx.request.body
    ctx.logger.info(params)
    var start_date = params.date1
    var end_date = params.date2
    var server_list = params.server_id
    var channel_list = params.channel
    var server_length = server_list.length
    var channel_length = channel_list.length
    var data_range = ctx.helper.getDateRange(start_date, end_date)

    
    if(params.server_combine == 'true' && server_length > 1){
      server_length = 1
      server_list = []
      server_list.push(params.server_id)
    }

    if(params.channel_combine == 'true' && channel_length > 1){
      channel_length = 1
      channel_list = []
      channel_list.push(params.channel)
    }

    var query_result = []
    for (var i = 0; i < data_range.length; i++) {

      for (var k = 0; k < server_list.length; k++) {
        for (var o = 0; o < channel_list.length; o++) {

          //注册数
          var today_reg = await ctx.biModel.LogCreateRole.findAndCount(data_range[i], server_list[k], channel_list[o])
          var today_reg_num = today_reg.length
          //活跃人数(登陆)
          var today_active_num = await ctx.biModel.LogLoginRole.countBy(data_range[i], server_list[k], channel_list[o])
          //老用活跃人数
          var today_old_login_num = Math.max(today_active_num - today_reg_num, 0)

          //付费人数
          var today_pay_num = await ctx.biModel.LogPrepaid.countBy(data_range[i], server_list[k], channel_list[o])
          //总付费金额
          var today_pay_total = await ctx.biModel.LogPrepaid.sumBy(data_range[i], server_list[k], channel_list[o])
          
          //活跃付费率
          var today_pay_rato = (today_pay_num / today_active_num *100).toFixed(2)
          //活跃ARPU
          var active_arpu = (today_pay_total / today_active_num /100).toFixed(2)
          //付费ARPU
          var pay_arpu = (today_pay_total / today_pay_num /100).toFixed(2)

          //新用户付费人数
          var today_new_pay_num = await ctx.biModel.LogPrepaid.countByRange(today_reg, data_range[i], server_list[k], channel_list[o])
          //新用户付费率
          var today_new_pay_rato = (today_new_pay_num / today_reg_num *100).toFixed(2)
          //新用户付费金额
          var today_new_pay_total = await ctx.biModel.LogPrepaid.rangeSum(today_reg, data_range[i], server_list[k], channel_list[o])
          //新用户ARPU
          var new_active_arpu = (today_new_pay_total / today_reg_num /100).toFixed(2)
          //新用户付费ARPU
          var new_pay_arpu = (today_new_pay_total / today_new_pay_num /100).toFixed(2)
          //老用户付费人数
          var today_old_pay_num = today_pay_num - today_new_pay_num
          //老用户付费金额
          var today_old_pay_total = today_pay_total - today_new_pay_total
          //老用户付费率
          var today_old_pay_rato = (today_old_pay_num / today_old_login_num * 100).toFixed(2)
          //老用户ARPU
          var old_active_arpu = (today_old_pay_total / today_old_login_num /100).toFixed(2)
          //老用户付费ARPU
          var old_pay_arpu = (today_old_pay_total / today_old_pay_num /100).toFixed(2)
    

          query_result.push({date: data_range[i],
                        channel:channel_list[o],
                        server_id:server_list[k],
                        today_reg_num: today_reg_num,
                        today_old_login_num: today_old_login_num,
                        today_active_num: today_active_num,
                        today_pay_num: today_pay_num,
                        today_pay_total: today_pay_total /100,
                        today_pay_rato: today_pay_rato,
                        active_arpu: active_arpu,
                        pay_arpu: pay_arpu,
                        today_new_pay_num: today_new_pay_num,
                        today_new_pay_rato: today_new_pay_rato,
                        today_new_pay_total: today_new_pay_total /100,
                        new_active_arpu: new_active_arpu,
                        new_pay_arpu: new_pay_arpu,
                        today_old_pay_num: today_old_pay_num,
                        today_old_pay_total: today_old_pay_total /100,
                        today_old_pay_rato: today_old_pay_rato,
                        old_active_arpu: old_active_arpu,
                        old_pay_arpu: old_pay_arpu
          })
        };
      };

    }
    ctx.body = {state: true, info:query_result, reason: ctx.__('common_instructions_success')}
  }

  async get_ltv() {
    let ctx = this.ctx;
    var params = ctx.request.body
    var start_date = params.date1
    var end_date = params.date2
    var server_list = params.server_id
    var server_length = server_list.length
    var channel_list = params.channel
    var channel_length = channel_list.length
    var data_range = ctx.helper.getDateRange(start_date, end_date)
    var now = moment().format('YYYY-MM-DD')
    var now_date = Date.parse(now)
    if(params.server_combine == 'true' && server_length > 1){
      server_length = 1
      server_list = []
      server_list.push(params.server_id)
    }

    if(params.channel_combine == 'true' && channel_length > 1){
      channel_length = 1
      channel_list = []
      channel_list.push(params.channel)
    }

    var query_result = []
    var total_pay = 0

    for (var i = 0; i < data_range.length; i++) {

      for (var k = 0; k < server_length; k++) {
        for (var o = 0; o < channel_length; o++) {
          //当日注册
          var today_reg = await ctx.biModel.LogCreateRole.findAndCount(data_range[i], server_list[k], channel_list[o])
          var today_reg_num = today_reg.length
          //付费额
          var pay_result = []
          var days = [0, 1, 2, 3, 4, 5, 6, 14, 29, 59, 89, 179]
          var temp = data_range[i]
          for (var j = 0; j < days.length; j++) {
            var started = temp
            var ended = ctx.helper.addDateFormat(data_range[i], days[j])
            var ended_date =  Date.parse(ended)
            var pay_num = 0
            if(today_reg_num > 0){
              if(ended_date <= now_date)
                pay_num = await ctx.biModel.LogPrepaid.findByRangeSum(today_reg, started, ended)
              else
                total_pay = 0
            }
            total_pay += pay_num
            var ltv = (total_pay / today_reg_num /100).toFixed(2)
            temp = ctx.helper.addDateFormat(ended, 1)
            pay_result.push(ltv)
          }

          query_result.push({date: data_range[i],
                        channel:channel_list[o],
                        server_id:server_list[k],
                        today_reg_num: today_reg_num,
                        day1: pay_result[0],
                        day2: pay_result[1],
                        day3: pay_result[2],
                        day4: pay_result[3],
                        day5: pay_result[4],
                        day6: pay_result[5],
                        day7: pay_result[6],
                        day15: pay_result[7],
                        day30: pay_result[8],
                        day60: pay_result[9],
                        day90: pay_result[10],
                        day180: pay_result[11],
          })
        };
      };

    }
    ctx.body = {state: true, info:query_result, reason: ctx.__('common_instructions_success')}
  }

  async view5m() {
    var ctx = this.ctx
    var params = ctx.request.body
    // ctx.logger.info(params)
    var data = []

    var idx = 5
    var mutx = 12
    if(params.interval == 1){
      idx = 10
      mutx = 6
    }else if(params.interval == 2){
      idx = 30
      mutx = 2
    }else if(params.interval == 3){
      idx = 60
      mutx = 1
    }

    var type = 'online_id'
    if(params.type == 1){
      type = 'total_recharge_amount'
    }else if(params.type == 2){
      type = 'total_reg_id'
    }

    var timeRange1 = ctx.helper.getTimeRange(params.dt1, idx, mutx)

    var timeRange2 = ctx.helper.getTimeRange(params.dt2, idx, mutx)

    var line1 = await ctx.biModel.LogMintuesRecord.findByType(timeRange1, type, params.server)

    var line2 = await ctx.biModel.LogMintuesRecord.findByType(timeRange2, type, params.server)

    
    var line1_date_arr = pluck(line1, 'time');
    var line1_date_Val = pluck(line1, type);

    var line2_date_arr = pluck(line2, 'time');
    var line2_date_Val = pluck(line2, type);


    var now = moment().format('YYYY-MM-DD HH:mm:ss');
    var index1 = 0
    var index2 = 0
    for (var i = 0; i < timeRange1.length; i++) {
      var t1 = 0
      var t2 = 0
      if(line1_date_arr.indexOf(timeRange1[i]) > -1){
        t1 = line1_date_Val[index1]
        index1 ++
      }

      if(line2_date_arr.indexOf(timeRange2[i]) > -1){
        t2 = line2_date_Val[index2]
        index2 ++
      }

      if(timeRange2[i] < now){
        data.push({time : timeRange1[i], val1:t1, val2:t2})
      }else{
        data.push({time : timeRange1[i], val1:t1})
      }
    }

    ctx.body = data
  }


  async server_data_today() {
    let ctx = this.ctx;
    var params = ctx.request.body
    ctx.logger.info(params)
    var server_list = params.server_id
    var query_result = []

    for (var k = 0; k < server_list.length; k++) {
      var today_ = moment().format('YYYY-MM-DD')
      //今日注册数
      var today_reg = await ctx.biModel.LogCreateRole.findAndCount(today_, server_list[k], -1)
      var today_reg_num = today_reg.length
      //累计注册数
      //var sum_reg = await ctx.biModel.query('select count(distinct role_id) as reg_num,os_name FROM  log_createrole where server_id = ' + server_list[k].split(';')[0] + ' group by os_name')
      var sum_reg = await ctx.biModel.LogCreateRole.sum_reg(server_list[k])
      var sum_reg_ios = 0, sum_reg_and = 0;
      sum_reg.forEach(function(v, i, arr){
        if(v.dataValues.os_name == 5){
          sum_reg_ios = v.dataValues.reg_num;
        }else if(v.dataValues.os_name == 6){
          sum_reg_and = v.dataValues.reg_num;
        }
      });
      //今日登录人数
      var today_active = await ctx.biModel.query('select count(distinct role_id) as active_num,os_name FROM  log_loginrole where server_id = ' + server_list[k].split(';')[0] + ' and ymd = "' + today_ + '" group by os_name')
      var today_active_num_ios = 0, today_active_num_and = 0;
      today_active[0].forEach(function(v, i, arr){
        if(v.os_name == 5){
          today_active_num_ios = v.active_num;
        }else if(v.os_name == 6){
          today_active_num_and = v.active_num;
        }
      });
      var today_active_num = today_active_num_and + today_active_num_ios
      //在线人数;
      var lserver_id = await ctx.model.Server.find(server_list[k])
      var online_sum = await ctx.biModel.LogOnline.getonlinenow(lserver_id)
      online_sum = online_sum.dataValues.count
      //今日充值额
      var today_pay_total = await ctx.biModel.LogPrepaid.sumBy(today_, server_list[k], -1)
      //今日注册付费人数
      var today_new_pay_num = await ctx.biModel.LogPrepaid.countByRange(today_reg, today_, server_list[k], -1)
      //今日充值人数
      var today_pay_num = await ctx.biModel.LogPrepaid.countBy(today_, server_list[k], -1)
      //今日注册付费率
      var today_new_pay_rato = (today_new_pay_num / today_reg_num *100).toFixed(2)
      //活跃付费率
      var today_pay_rato = (today_pay_num / today_active_num *100).toFixed(2)
      //活跃ARPU
      var active_arpu = (today_pay_total / today_active_num).toFixed(2)
      //充值总额
      var server_pay_num = await ctx.biModel.LogPrepaid.sumByServer(server_list[k])
      //充值top50
      var top_list = await ctx.biModel.query('SELECT role_id,role_name, sum(totalPay) as topay FROM log_prepaid where server_id = '+ server_list[k].split(';')[0] +' and role_id not in (' + ctx.app.gm_acc_list.toString() + ') group by role_id,role_name order by topay desc limit 50')
      query_result.push({
        server_id:server_list[k],
        today_reg_num: today_reg_num,
        sum_reg_and: sum_reg_and,
        sum_reg_ios: sum_reg_ios,
        today_active_num_and: today_active_num_and,
        today_active_num_ios: today_active_num_ios,
        today_active_num: today_active_num,
        online_sum: online_sum,
        today_pay_total: today_pay_total,
        today_new_pay_num: today_new_pay_num,
        today_new_pay_rato: today_new_pay_rato,
        today_pay_num: today_pay_num,
        today_pay_rato: today_pay_rato,
        active_arpu: active_arpu,
        server_pay_num: server_pay_num,
        top_list: top_list[0]
      });
    };
    ctx.body = {state: true, info:query_result, reason: ctx.__('common_instructions_success')}
  }

  async server_data_chargelist() {
    let ctx = this.ctx;
    var params = ctx.request.body
    ctx.logger.info(params)
    var channel = params.channel
    var server_id = params.server_id
    var date = params.date

    var chargelist = await ctx.biModel.LogPrepaid.findChargelist(null, server_id, channel, []);
    var role_list =  pluck(chargelist, 'role_id');
    var chargelist_d = await ctx.biModel.LogPrepaid.findChargelist(moment(date).format('YYYY-MM-DD'), server_id, channel, role_list);
    if(moment(date).format('E') == '1'){
      var chargelist_w = chargelist_d
    }else{
      var this_w = moment(date).subtract(moment(date).format('E') - 1, 'days').format('YYYY-MM-DD')
      var chargelist_w = await ctx.biModel.LogPrepaid.findChargelist([this_w, date], server_id, channel, role_list);
    }
    if(moment(date).format('D') == '1'){
      var chargelist_m = chargelist_d
    }else if(moment(date).format('D') == moment(date).format('E')){
      var chargelist_m = chargelist_w
    }else{
      var this_m = moment(date).subtract(moment(date).format('D') - 1, 'days').format('YYYY-MM-DD')
      var chargelist_m = await ctx.biModel.LogPrepaid.findChargelist([this_m, date], server_id, channel, role_list);
    }
    chargelist.forEach(function(vc, ic, ac){
      vc.dataValues.payd = 0
      vc.dataValues.payw = 0
      vc.dataValues.paym = 0
      chargelist_d.forEach(function(vd, id, ad){
        if(vc.dataValues.role_id == vd.dataValues.role_id){
          vc.dataValues.payd = vd.dataValues.topay
        }
      });
      chargelist_w.forEach(function(vw, iw, aw){
        if(vc.dataValues.role_id == vw.dataValues.role_id){
          vc.dataValues.payw = vw.dataValues.topay
        }
      });
      chargelist_m.forEach(function(vm, im, am){
        if(vc.dataValues.role_id == vm.dataValues.role_id){
          vc.dataValues.paym = vm.dataValues.topay
        }
      });
    })
    ctx.body = {state: true, info:chargelist, reason: ctx.__('common_instructions_success')}
  }

  async server_data_singledata() {
    let ctx = this.ctx;
    var params = ctx.request.body
    var start_date = params.date1
    var end_date = params.date2
    var server_id = params.server_id
    var data_range = ctx.helper.getDateRange(start_date, end_date)

    var query_result = []
    for (var i = 0; i < data_range.length; i++) {

        //活跃人数(登陆)
        var today_active_num = await ctx.biModel.LogLoginRole.countBy(data_range[i], server_id, -1)
        //付费人数
        var today_pay_num = await ctx.biModel.LogPrepaid.countBy(data_range[i], server_id, -1)
        //总付费金额
        var today_pay_total = await ctx.biModel.LogPrepaid.sumBy(data_range[i], server_id, -1)
        //活跃ARPU
        var active_arpu = (today_pay_total / today_active_num /100).toFixed(2)
        //付费ARPU
        var pay_arpu = (today_pay_total / today_pay_num /100).toFixed(2)
        //付费率
        var today_pay_rato = (today_pay_num / today_active_num *100).toFixed(2)

        query_result.push({
		            date: data_range[i],
                    server_id: server_id,
                    today_active_num: today_active_num,
                    today_pay_num: today_pay_num,
                    today_pay_total: today_pay_total /100,
                    today_pay_rato: today_pay_rato,
                    active_arpu: active_arpu,
                    pay_arpu: pay_arpu,
        })
    };
    ctx.body = {state: true, info:query_result, reason: ctx.__('common_instructions_success')}
  }

  async server_data_yuanbao() {
    let ctx = this.ctx;
    var params = ctx.request.body
    ctx.logger.info(params)
    var server_id = params.server_id
    var q_date = moment(params.date).format('YYYY-MM-DD')
    var query_sum = await ctx.biModel.query('select sum(CostDiamond) as sumc from log_yuanbao_use where ymd = "' + q_date + '"');
    var query_summ = query_sum[0][0].sumc
    var query_result = await ctx.biModel.query('select rr.reason, rr.sumc, rr.countr, rr.sumc / rr.countr as avgcost, rr.sumc / '+ query_summ +' as costratio from (select reason,sum(CostDiamond) as sumc,count(distinct role_id) as countr from log_yuanbao_use where ymd = "' + q_date + '" and server_id = '+ server_id +' group by reason) rr;')
    ctx.body = {state: true, info:query_result[0], reason: ctx.__('common_instructions_success')}
  }

  async server_data_tongbi() {
    let ctx = this.ctx;
    var params = ctx.request.body
    ctx.logger.info(params)
    var server_id = params.server_id
    var q_date = moment(params.date).format('YYYY-MM-DD')
    var query_sum = await ctx.biModel.query('select sum(CostCopper) as sumc from log_jinbi_use where ymd = "' + q_date + '"');
    var query_summ = query_sum[0][0].sumc
    var query_result = await ctx.biModel.query('select rr.reason, rr.sumc, rr.countr, rr.sumc / rr.countr as avgcost, rr.sumc / '+ query_summ +' as costratio from (select reason,sum(CostCopper) as sumc,count(distinct role_id) as countr from log_jinbi_use where ymd = "' + q_date + '" and server_id = '+ server_id +' group by reason) rr;')
    ctx.body = {state: true, info:query_result[0], reason: ctx.__('common_instructions_success')}
  }


}

module.exports = BiController;
