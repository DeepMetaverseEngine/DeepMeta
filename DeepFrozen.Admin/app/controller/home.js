'use strict';
const Controller = require('egg').Controller;

class HomeController extends Controller {
  async index() {
    await this.ctx.render('page/welcome.html',{
        title: this.ctx.__('page_index_title')
      });
  }

  async channel_selector() {
   var selected = this.ctx.query.select.split(',').map(Number);
    let channel_list = await this.ctx.service.update.show()
    var channels = []
    channel_list.forEach(function (channel) {
      var has = -1
      if(selected.length == 1 && selected[0] == -1)
        has = 0
      else
        has = selected.indexOf(channel.id)
      channels.push({id:channel.id, sdk_name:channel.sdk_name, os_type: channel.os_type, selected: has})
    });
    
    await this.ctx.render('widgets/channel_selector.html', {
      channel_list:channels
    });
  }

  async server_selector() {
   var selected = this.ctx.query.select.split(',').map(Number);
   let realm_list = await this.ctx.service.serverlist.show_realmlist()
    let server_list = await this.ctx.service.serverlist.get_all_servers()

    if(selected.length == 1 && selected[0] == -1){
      selected = []
      server_list.forEach(function (server) {
        selected.push(server.id)
      });
    }
    await this.ctx.render('widgets/server_selector.html', {
      realm_list: realm_list,
      server_list:server_list,
      selected:selected
    });
  }

  async call() {
    this.ctx.body = 'ok\n' + (await this.ctx.service.backend.call("111", this.ctx.request.body.cmd));
  }


  async changpwd()
  {
    var result = await this.ctx.service.users.find_one(1)

    result.updatePassword('123321ps2');
  }

  async login() {
    await this.ctx.render('login',{
      title: this.ctx.__('page_login_title')
    });
  }

  async server_notice() {
    await this.ctx.render('page/server_notice.html',{
        title: this.ctx.__('page_notice_title')
      });
  }

  async server_broadcast() {
    await this.ctx.render('page/server_broadcast.html',{
        title: this.ctx.__('page_broadcast_title')
      });
  }


  async server_im_broadcast() {
    let server_list = await this.ctx.service.serverlist.get_all_servers()
    await this.ctx.render('page/server_im_broadcast.html',{
        title: this.ctx.__('page_im_broadcast_title'),
        server_list: server_list,
      });
  }
  
  async sdk_manage() {
    await this.ctx.render('page/sdk_manage.html',{
        title: this.ctx.__('page_sdk_title')
      });
  }

  
  async update_manage() {
    await this.ctx.render('page/update/update_manage.html',{
        title: this.ctx.__('page_update_title')
      });
  }

  async mpq_manage() {
    await this.ctx.render('page/update/mpq_manage.html',{
        title: this.ctx.__('page_mpq_title')
      });
  }

  async server_mail_apply() {
    await this.ctx.render('page/server_mail_apply.html',{
        title: this.ctx.__('page_email_apply_title')
      });
  }

  async server_mail_review() {
    await this.ctx.render('page/server_mail_review.html',{
        title: this.ctx.__('page_email_review')
      });
  }

  async server_mail_record() {
    let server_list = await this.ctx.service.serverlist.get_all_servers()
    await this.ctx.render('page/server_mail_record.html',{
        server_list:server_list,
        title: this.ctx.__('page_email_record')
      });
  }

  async server_gmactivity() {
    var time = await this.ctx.service.sysconfig.get('activity_sync_time')
    await this.ctx.render('page/server_gmactivity.html',{
        time: time,
        title: this.ctx.__('page_gmactivity_title')
      });
  }

  async server_activity() {
    await this.ctx.render('page/server_activity.html',{
        title: this.ctx.__('page_activity_title')
      });
  }

  async server_cdkey() {
    let activity_list = await this.ctx.service.activity.show()
    await this.ctx.render('page/server_cdkey.html',{
        title: this.ctx.__('page_cdkey_title'),
        activitys:activity_list
      });
  }

   async bi_remain() {
    let server_list = await this.ctx.service.serverlist.get_all_servers()
    let channel_list = await this.ctx.service.update.show()
    await this.ctx.render('page/server_data_remain.html',{
        title: this.ctx.__('page_server_data_remain'),
        server_list:server_list,
        channel_list:channel_list
      });
  }

  async bi_preview() {
    let server_list = await this.ctx.service.serverlist.get_all_servers()
    let channel_list = await this.ctx.service.update.show()
    await this.ctx.render('page/bi/preview.html',{
        title: this.ctx.__('page_bi_preview_title'),
        server_list:server_list,
        channel_list:channel_list
      });
  }

  async bi_ltv() {
    let server_list = await this.ctx.service.serverlist.get_all_servers()
    let channel_list = await this.ctx.service.update.show()
    await this.ctx.render('page/bi/ltv.html',{
        title: this.ctx.__('page_bi_ltv_title'),
        server_list:server_list,
        channel_list:channel_list
      });
  }

  async server_data_today() {
    let server_list = await this.ctx.service.serverlist.get_all_servers()
    let channel_list = await this.ctx.service.update.show()
    await this.ctx.render('page/server_data_today.html',{
        title: this.ctx.__('page_server_data_today'),
        server_list:server_list,
        channel_list:channel_list
      });
  }

  async server_data_chargelist() {
    let server_list = await this.ctx.service.serverlist.get_all_servers()
    let channel_list = await this.ctx.service.update.show()
    await this.ctx.render('page/server_data_chargelist.html',{
        title: this.ctx.__('page_server_data_chargelist'),
        server_list:server_list,
        channel_list:channel_list
      });
  }

  async white_list() {
    await this.ctx.render('page/account/white_list.html',{
        title: this.ctx.__('page_account_whitelist_title')
      });
  }

  async recharge_list() {
    await this.ctx.render('page/recharge_list.html',{
        title: this.ctx.__('page_recharge_list_title')
      });
  }

  async hot_plug() {
    var realm_list = await this.ctx.service.serverlist.show_realmlist()
    await this.ctx.render('page/hot_plug.html',{
        title: this.ctx.__('page_hot_plug_title'),
        realm_list: realm_list
      });
  }

  async order_grant() {
    await this.ctx.render('page/order_grant.html',{
        title: this.ctx.__('page_order_grant_title')
      });
  }

  async resend_order() {
    await this.ctx.render('page/resend_order.html',{
        title: this.ctx.__('page_order_resend_title')
      });
  }

  async ip_list() {
    await this.ctx.render('page/ip_list.html',{
        title: this.ctx.__('page_account_iplist_title')
      });
  }

  async role_info(){
    await this.ctx.render('page/role/role_info.html',{
        title: this.ctx.__('page_role_roleinfo_title'),
      });
  }

  async role_email(){
    await this.ctx.render('page/role/role_email.html',{
        title: this.ctx.__('page_role_roleemail_title')
      });
  }

  async role_bag(){
    let server_list = await this.ctx.service.serverlist.get_all_servers()
    await this.ctx.render('page/role/role_bag.html',{
        title: this.ctx.__('page_role_bag_title'),
        server_list: server_list,
      });
  }

  async role_blacklist(){
    let server_list = await this.ctx.service.serverlist.get_all_servers()
    await this.ctx.render('page/role/role_blacklist.html',{
        title: this.ctx.__('page_role_blacklist_title'),
        server_list: server_list,
      });
  }

  async role_ban(){
    let server_list = await this.ctx.service.serverlist.get_all_servers()
    await this.ctx.render('page/role/role_ban.html',{
        title: this.ctx.__('page_role_ban_title'),
        server_list: server_list,
      });
  }

  async role_privilege(){
    var ctx = this.ctx
    var options = []
    this.ctx.app.rolePrivileges.forEach(function (privilege) {
      options.push({label:ctx.__(privilege.title),value:privilege.privilege})
    });
    await this.ctx.render('page/role/role_privilege.html',{
        title: this.ctx.__('page_role_privilege_title'),
        privileges:options
      });
  }

  async realmlist() {
    await this.ctx.render('page/realmlist.html',{
        title: this.ctx.__('page_realmlist_title')
      });
  }

  async realmop() {
    await this.ctx.render('page/realmop.html',{
        title: this.ctx.__('page_realmlist_title')
      });
  }

  async serverlist() {
  	await this.ctx.render('page/serverlist.html',{
        title: this.ctx.__('page_serverlist_title')
      });
  }

  async users() {
    await this.ctx.render('page/users.html',{
      title: this.ctx.__('page_users_title'),
      userlist: await this.ctx.model.User.findAll({where: {privilege: 1}}),
      grouplist: await this.ctx.model.Usergroup.findAll({where: {id: {[this.ctx.model.Op.gt]: 1 }}}),
      titlelist: this.app.titlelists,
      menu_list: this.app.menu_list,
    });
  }

  async logs() {
    await this.ctx.render('page/logs.html',{
      title: this.ctx.__('page_logs_title')
    });
  }



  async server_data_view5m() {
    let server_list = await this.ctx.service.serverlist.get_all_servers()
    await this.ctx.render('page/server_data_view5m.html',{
      title: this.ctx.__('page_server_data_view5m'),
      server_list: server_list
    });
  }


  async server_resource_gm_apply() {
    let role_list = await this.ctx.model.Gmaccountlist.findAll();
    await this.ctx.render('page/server_resource_gm_apply.html',{
      title: this.ctx.__('page_resource_gm_apply'),
      role_list: role_list,
    });
  }

  async server_resource_gm_review() {
    await this.ctx.render('page/server_resource_gm_review.html',{
        title: this.ctx.__('page_resource_gm_review')
      });
  }

  async server_resource_gm_record() {
    let server_list = await this.ctx.service.serverlist.get_all_servers()
    await this.ctx.render('page/server_resource_gm_record.html',{
        server_list:server_list,
        title: this.ctx.__('page_resource_gm_record')
      });
  }

  async server_resource_order_relay_apply() {
    let server_list = await this.ctx.service.serverlist.get_all_servers()
    await this.ctx.render('page/server_resource_order_relay_apply.html',{
      title: this.ctx.__('page_resource_order_relay_apply'),
      server_list: server_list
    });
  }

  async server_resource_order_relay_review() {
    await this.ctx.render('page/server_resource_order_relay_review.html',{
        title: this.ctx.__('page_resource_order_relay_review')
      });
  }

  async server_resource_order_relay_record() {
    let server_list = await this.ctx.service.serverlist.get_all_servers()
    await this.ctx.render('page/server_resource_order_relay_record.html',{
        server_list:server_list,
        title: this.ctx.__('page_resource_order_relay_record')
      });
  }

  async server_data_remain() {
    let server_list = await this.ctx.service.serverlist.get_all_servers()
    let channel_list = await this.ctx.service.update.show()
    await this.ctx.render('page/server_data_remain.html',{
        title: this.ctx.__('page_server_data_remain'),
        server_list:server_list,
        channel_list:channel_list
      });
  }

  async server_data_active() {
    let server_list = await this.ctx.service.serverlist.get_all_servers()
    let channel_list = await this.ctx.service.update.show()
    await this.ctx.render('page/server_data_active.html',{
        title: this.ctx.__('page_server_data_active'),
        server_list:server_list,
        channel_list:channel_list
      });
  }

  async server_data_singleability() {
    let server_list = await this.ctx.service.serverlist.get_all_servers()
    await this.ctx.render('page/server_data_singleability.html',{
        server_list:server_list,
        title: this.ctx.__('page_server_data_singleability')
      });
  }

  async server_data_singledata() {
    let server_list = await this.ctx.service.serverlist.get_all_servers()
    await this.ctx.render('page/server_data_singledata.html',{
        server_list:server_list,
        title: this.ctx.__('page_server_data_singledata')
      });
  }

  async server_data_yuanbao() {
    let server_list = await this.ctx.service.serverlist.get_all_servers()
    await this.ctx.render('page/server_data_ybxh.html',{
        server_list:server_list,
        title: this.ctx.__('page_server_xiaohao_yuanbao')
      });
  }

  async server_data_tongbi() {
    let server_list = await this.ctx.service.serverlist.get_all_servers()
    await this.ctx.render('page/server_data_tbxh.html',{
        server_list:server_list,
        title: this.ctx.__('page_server_xiaohao_tongbi')
      });
  }

  async early_warning_setting() {
    await this.ctx.render('page/monitor/early_warning.html',{
        title: this.ctx.__('page_early_warning_setting')
      });
  }


  async early_warning_record() {
    await this.ctx.render('page/monitor/early_warning_record.html',{
        title: this.ctx.__('page_early_warning_record')
      });
  }

  async warning_mail_list() {
    await this.ctx.render('page/monitor/warning_mail_list.html',{
        title: this.ctx.__('page_early_warning_mailist')
      });
  }


  
}

module.exports = HomeController;
