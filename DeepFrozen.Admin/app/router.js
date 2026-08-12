'use strict';

/**
 * @param {Egg.Application} app - egg application
 */
module.exports = app => {
  const { router, controller } = app;


  /** Role Privilege Control **/
  const admin_privilege = app.role.can('admin');
  const operator_privilege = app.role.can('operator');
  const product_privilege = app.role.can('product');
  const tw_product_privilege = app.role.can('tw_product');
  const common_privilege = app.role.can('common');
  const detail_privilege = app.role.can('detail');




  /** 租赁市场 */
  router.post('/api/backend/rent/list', controller.api.backend.rent.show);
  router.post('/api/backend/rent/detail', controller.api.backend.rent.detail);
  router.post('/api/backend/rent/add', controller.api.backend.rent.add);
  router.post('/api/backend/rent/remove', controller.api.backend.rent.remove);
  router.post('/api/backend/rent/take', controller.api.backend.rent.take);



  /** Pages **/

  // router.get('/', common_privilege, controller.home.index);
  router.get('server_notice','/server_notice', detail_privilege, controller.home.server_notice);
  router.get('server_broadcast','/server_broadcast', detail_privilege, controller.home.server_broadcast);
  router.get('server_im_broadcast','/server_im_broadcast', detail_privilege, controller.home.server_im_broadcast);

  
  router.get('server_mail_apply','/server_mail_apply', detail_privilege, controller.home.server_mail_apply);
  router.get('server_mail_review','/server_mail_review', detail_privilege, controller.home.server_mail_review);
  router.get('server_mail_record','/server_mail_record', detail_privilege, controller.home.server_mail_record);


  router.get('server_gmactivity','/server_gmactivity', detail_privilege, controller.home.server_gmactivity);
  router.resources('/api/backend/gmactivity', detail_privilege, controller.api.backend.gmactivity);
  router.post('/api/backend/gmactivity_upload', detail_privilege, controller.api.backend.gmactivity.upload);
  router.post('/api/backend/gmactivity_sync', detail_privilege, controller.api.backend.gmactivity.sync);

  router.get('server_activity','/server_activity', detail_privilege, controller.home.server_activity);
  router.get('server_cdkey','/server_cdkey', detail_privilege, controller.home.server_cdkey);

  /** 充值管理 **/
  router.get('recharge_list','/recharge_list', detail_privilege, controller.home.recharge_list);
  router.get('order_grant','/order_grant', detail_privilege, controller.home.order_grant);
  router.get('resend_order','/resend_order', detail_privilege, controller.home.resend_order);
  /** 账号管理 **/
  //router.get('white_list','/white_list', detail_privilege, controller.home.white_list);
  router.get('ip_list','/ip_list', detail_privilege, controller.home.ip_list);
  /** 角色管理 Pages **/
  router.get('role_info','/role_info', detail_privilege, controller.home.role_info);
  router.get('role_email','/role_email', detail_privilege, controller.home.role_email);
  router.get('role_bag','/role_bag', detail_privilege, controller.home.role_bag);
  router.get('role_blacklist','/role_blacklist', detail_privilege, controller.home.role_blacklist);
  router.get('role_ban','/role_ban', detail_privilege, controller.home.role_ban);
  router.get('role_privilege','/role_privilege', detail_privilege, controller.home.role_privilege);
   /** 渠道管理 Pages **/
  router.get('sdk_manage','/sdk_manage', detail_privilege, controller.home.sdk_manage);
  router.get('update_manage','/update_manage', detail_privilege, controller.home.update_manage);
  router.get('update_manage','/mpq_manage', detail_privilege, controller.home.mpq_manage);

  router.get('realmlist','/realmlist', detail_privilege, controller.home.realmlist);
  router.get('serverlist','/serverlist', detail_privilege, controller.home.serverlist);
  router.get('users','/users', detail_privilege, controller.home.users);
  router.get('logs','/logs', detail_privilege, controller.home.logs);



  /** 服务器数据查询 **/
  //5分钟视图
  router.get('server_data_view5m','/server_data_view5m', detail_privilege, controller.home.server_data_view5m);
  router.post('/api/backend/bi/view5m', detail_privilege, controller.api.backend.bi.view5m);
  //LTV
  router.get('server_data_LTV','/server_data_LTV', detail_privilege, controller.home.bi_ltv);
  router.post('/api/backend/bi/get_ltv', detail_privilege, controller.api.backend.bi.get_ltv);

  //留存查询
  router.get('server_data_remain','/server_data_remain', detail_privilege, controller.home.server_data_remain);
  router.post('/api/backend/bi/get_remain', detail_privilege, controller.api.backend.bi.get_remain);
  
  //日活查询
  router.get('server_data_active','/server_data_active', detail_privilege, controller.home.server_data_active);
  router.post('/api/backend/server_data_active', detail_privilege, controller.api.backend.bi.server_data_active);
  
  //服务器今日情况
  router.get('server_data_today','/server_data_today', detail_privilege, controller.home.server_data_today);
  router.post('/api/backend/bi/server_data_today', detail_privilege, controller.api.backend.bi.server_data_today);

  //充值排行
  router.get('server_data_chargelist','/server_data_chargelist', detail_privilege, controller.home.server_data_chargelist);
  router.post('/api/backend/bi/server_data_chargelist', detail_privilege, controller.api.backend.bi.server_data_chargelist);

  //单服战力排行
  router.get('server_data_singleability','/server_data_singleability', detail_privilege, controller.home.server_data_singleability);
  router.post('/api/backend/server_data_singleability', detail_privilege, controller.api.backend.player.server_data_singleability);

  //单服每日数据
  router.get('server_data_singledata','/server_data_singledata', detail_privilege, controller.home.server_data_singledata);
  router.post('/api/backend/server_data_singledata', detail_privilege, controller.api.backend.bi.server_data_singledata);

  //元宝消耗
  router.get('server_data_yuanbao','/server_data_yuanbao', detail_privilege, controller.home.server_data_yuanbao);
  router.post('/api/backend/server_data_yuanbao', detail_privilege, controller.api.backend.bi.server_data_yuanbao);

  //铜币消耗
  router.get('server_data_tongbi','/server_data_tongbi', detail_privilege, controller.home.server_data_tongbi);
  router.post('/api/backend/server_data_tongbi', detail_privilege, controller.api.backend.bi.server_data_tongbi);

  /** 选择器 **/
  router.get('/api/backend/set_operation_realm', common_privilege, controller.api.backend.realmselector.set_realm);
  router.get('/api/backend/get_operation_realm', common_privilege, controller.api.backend.realmselector.get_realm);
  router.get('/api/backend/get_all_group', common_privilege, controller.api.backend.realmselector.get_all_group);
  router.get('/api/backend/get_all_realms', common_privilege, controller.api.backend.realmselector.get_all_realms);
  router.get('/api/backend/get_all_product_list', common_privilege, controller.api.backend.rechargelist.product_list);
  router.get('/api/backend/get_all_item', common_privilege, controller.api.backend.item.get_all_item);
  router.get('/api/backend/get_realm_item', common_privilege, controller.api.backend.item.get_realm_item);

  router.resources('/api/backend/notices', detail_privilege, controller.api.backend.notices);
  router.resources('/api/backend/broadcast', detail_privilege, controller.api.backend.broadcast);
  router.resources('/api/backend/system_broadcast', detail_privilege, controller.api.backend.sysbroadcast);
  router.resources('/api/backend/blacklist', detail_privilege, controller.api.backend.blacklist);
  /** 角色相关 **/
  router.post('/api/backend/role/role_info', detail_privilege, controller.api.backend.role.role_info);
  router.post('/api/backend/role/role_email', detail_privilege, controller.api.backend.role.role_email);
  router.post('/api/backend/role/role_bag', detail_privilege, controller.api.backend.role.role_bag);
  router.post('/api/backend/role/role_bag_edit', detail_privilege, controller.api.backend.role.role_bag_edit);
  router.resources('/api/backend/ban', detail_privilege, controller.api.backend.ban);
  router.resources('/api/backend/privilege', detail_privilege, controller.api.backend.privilege);
  router.resources('/api/backend/channels', detail_privilege, controller.api.backend.channels);
  router.resources('/api/backend/activity', detail_privilege, controller.api.backend.activity);
  router.resources('/api/backend/updates', detail_privilege, controller.api.backend.updates);
  router.resources('/api/backend/mpq', detail_privilege, controller.api.backend.mpq);



  router.resources('/api/backend/email', detail_privilege, controller.api.backend.email);
  router.post('/api/backend/email_review', detail_privilege, controller.api.backend.email.review);
  router.post('/api/backend/email_review_act', detail_privilege, controller.api.backend.email.review_act);
  router.post('/api/backend/email_record', detail_privilege, controller.api.backend.email.record);



  router.resources('/api/backend/recharge_list', detail_privilege, controller.api.backend.rechargelist);
  router.resources('/api/backend/whitelist', detail_privilege, controller.api.backend.account.whitelist);
  router.resources('/api/backend/iplist', detail_privilege, controller.api.backend.iplist);
  router.resources('/api/backend/realms', detail_privilege, controller.api.backend.realms);
  router.resources('/api/backend/servers', detail_privilege, controller.api.backend.servers);
  router.resources('/api/backend/users', detail_privilege, controller.api.backend.users);
  router.post('/api/backend/usersgroup', detail_privilege, controller.api.backend.users.group);
  router.resources('/api/backend/logs', detail_privilege, controller.api.backend.logs);

  /**运营数据**/
  router.post('/api/backend/bi/get_remain', detail_privilege, controller.api.backend.bi.get_remain);
  router.post('/api/backend/bi/get_preview', detail_privilege, controller.api.backend.bi.get_preview);
  

  //虚拟补单
  router.post('/api/backend/recharge/deal_order', detail_privilege, controller.api.backend.rechargelist.deal_order);
  router.post('/api/backend/recharge/reSendOrder', detail_privilege, controller.api.backend.rechargelist.reSendOrder);
  router.get('/api/backend/cdkey/generate', detail_privilege, controller.api.backend.cdkey.create);




  /** 资源管理 **/
  router.get('white_list','/white_list', detail_privilege, controller.main.gm_account_list);
  router.resources('/api/backend/gm_account_list', detail_privilege, controller.api.backend.gmaccountlist);

  router.get('resource_gm_apply','/resource_gm_apply', detail_privilege, controller.home.server_resource_gm_apply);
  router.post('/api/backend/resource_gm_apply', detail_privilege, controller.api.backend.rechargelist.resource_gm_apply);

  router.get('resource_gm_review','/resource_gm_review', detail_privilege, controller.home.server_resource_gm_review);
  router.post('/api/backend/resource_gm_review', detail_privilege, controller.api.backend.rechargelist.resource_gm_review);
  router.post('/api/backend/resource_gm_review_act', detail_privilege, controller.api.backend.rechargelist.review_act);


  router.get('resource_gm_record','/resource_gm_record', detail_privilege, controller.home.server_resource_gm_record);
  router.post('/api/backend/resource_gm_record', detail_privilege, controller.api.backend.rechargelist.resource_gm_record);
  

  router.get('resource_order_relay_apply','/resource_order_relay_apply', detail_privilege, controller.home.server_resource_order_relay_apply);
  router.post('/api/backend/resource_order_relay_apply', detail_privilege, controller.api.backend.rechargelist.resource_order_relay_apply);

  router.get('resource_order_relay_review','/resource_order_relay_review', detail_privilege, controller.home.server_resource_order_relay_review);
  router.post('/api/backend/resource_order_relay_review', detail_privilege, controller.api.backend.rechargelist.resource_order_relay_review);
  router.post('/api/backend/resource_order_relay_review_act', detail_privilege, controller.api.backend.rechargelist.resource_order_relay_review_act);


  router.get('resource_order_relay_record','/resource_order_relay_record', detail_privilege, controller.home.server_resource_order_relay_record);
  router.post('/api/backend/resource_order_relay_record', detail_privilege, controller.api.backend.rechargelist.resource_order_relay_record);


  router.get('hot_plug','/hot_plug', detail_privilege, controller.home.hot_plug);
  router.post('/api/backend/hot_plug_upload', detail_privilege, controller.api.backend.hotplug.upload);
  router.post('/api/backend/hot_plug_reload', detail_privilege, controller.api.backend.hotplug.reload);


  /** selector **/
  router.get('/widgets/channel_selector', common_privilege, controller.home.channel_selector);
  router.get('/widgets/server_selector', common_privilege, controller.home.server_selector);

  /** Public API **/
  //PayApi
  router.post('/api/server/getsignature', controller.api.pay.getsignature.index);
  router.post('/api/server/getorder', controller.api.pay.getorder.index);
  router.post('/api/server/getiaporder', controller.api.pay.getorder.iap);
  //router.get('/api/crontab/check_orders', controller.api.crontab.checkorder.index);

  /** SDK Pay Callback **/
  //OneGame Test
  //router.post('/api/public/callback/onegame_test', controller.api.pay.callback.onegame_test);
  router.get('/api/public/ipaddr', controller.api.client.ipaddr);
  //AliUC
  router.post('/api/public/callback/aligames', controller.api.pay.callback.aligames);
  //Xiaomi
  router.get('/api/public/callback/xiaomi', controller.api.pay.callback.xiaomi);
  //egls
  router.post('/api/public/callback/egls', controller.api.pay.callback.egls);
  //foyo
  router.post('/api/public/callback/foyo', controller.api.pay.callback.foyo);
  //SYG
  router.post('/api/public/callback/syg', controller.api.pay.callback.syg);
  //Quick
  router.post('/api/public/callback/btyq', controller.api.pay.callback.btyq);
  router.post('/api/public/callback/btll', controller.api.pay.callback.btll);
  router.post('/api/public/callback/btly', controller.api.pay.callback.btly);
  router.post('/api/public/callback/btyljx', controller.api.pay.callback.btyljx);
  router.post('/api/public/callback/quick_jzhjc', controller.api.pay.callback.quick_jzhjc);
  router.post('/api/public/callback/quick_cnck', controller.api.pay.callback.quick_cnck);
  router.post('/api/public/callback/quick_ml', controller.api.pay.callback.quick_ml);
  router.post('/api/public/callback/quick_agxx', controller.api.pay.callback.quick_agxx);
  //CGame
  router.get('/api/public/callback/cgame', controller.api.pay.callback.cgame);
  //SanWan
  router.get('/api/public/callback/Sanwan', controller.api.pay.callback.Sanwan);
  //益玩
  router.get('/api/public/callback/ewan', controller.api.pay.callback.ewan);



  /** Server API **/
  router.post('/call', controller.home.call);
  router.get('/api/server/server_list/:realm_id', controller.api.server.index);
  router.get('/api/server/server_list', controller.api.server.index);
  router.get('/api/server/ban_ip_list', controller.api.backend.forbidip.ban_ip_list);
  router.get('/api/server/ban_mac_list', controller.api.backend.forbidmac.ban_mac_list);
  router.get('/api/server/get_serverlist', controller.api.server.serverlist);
  router.get('/api/server/get_activities', controller.api.server.get_activities);
  router.post('/api/server/verify', controller.api.server.verify_account);
  /** Client API **/
  router.get('/api/client/server_list', controller.api.client.index);
  router.post('/api/client/test_notices', controller.api.client.get_server_notice);
  router.post('/api/client/check_update', controller.api.client.check_update);
  router.get('/api/client/check_update', controller.api.client.check_update);
  router.post('/api/client/create_guest', controller.api.client.create_guest);


  /* 操作realm */
  router.get('realmop','/realmop', detail_privilege, controller.home.realmop);
  router.post('/api/backend/server_adm', detail_privilege, controller.api.backend.serveradmin.serveradm);
  router.get('/api/backend/realmstat', detail_privilege, controller.api.backend.serveradmin.realmstat);

  /* 玩家数据查询 */
  router.get('/player_charge', detail_privilege, controller.main.player_charge);
  router.post('/api/backend/player_charge', detail_privilege, controller.api.backend.player.player_charge);

  router.get('/player_query', detail_privilege, controller.main.player_query_acc);
  router.get('/player_query_role', detail_privilege, controller.main.player_query_role);
  router.post('/api/backend/role_info', detail_privilege, controller.api.backend.player.player_query);

  router.get('/player_ability', detail_privilege, controller.main.player_ability);
  router.post('/api/backend/player_ability', detail_privilege, controller.api.backend.player.player_ability);

  router.get('/player_yuanbao', detail_privilege, controller.main.player_yuanbao);
  router.post('/api/backend/player_yuanbao', detail_privilege, controller.api.backend.player.player_yuanbao);

  router.get('/player_item', detail_privilege, controller.main.player_item);
  router.post('/api/backend/player_item', detail_privilege, controller.api.backend.player.player_item);

  router.get('/player_tongbi', detail_privilege, controller.main.player_tongbi);
  router.post('/api/backend/player_tongbi', detail_privilege, controller.api.backend.player.player_tongbi);

  router.get('/player_yinliang', detail_privilege, controller.main.player_yinliang);
  router.post('/api/backend/player_yinliang', detail_privilege, controller.api.backend.player.player_yinliang);

  router.get('/player_device', detail_privilege, controller.main.player_device);
  router.post('/api/backend/player_device', detail_privilege, controller.api.backend.player.player_device);

  router.get('/player_vip', controller.main.player_vip);
  router.get('/player_roledetail', controller.main.player_roledetail);
  
  router.get('/player_online', controller.main.player_online);
  router.post('/api/backend/player_online', detail_privilege, controller.api.backend.player.player_online);

  router.get('/player_levelup', detail_privilege, controller.main.player_levelup);
  router.post('/api/backend/player_levelup', detail_privilege, controller.api.backend.player.player_levelup);

  router.get('/player_chat', detail_privilege, controller.main.player_chat);
  router.post('/api/backend/player_chat', detail_privilege, controller.api.backend.player.player_chat);

  /* 操作处理 */
  router.get('/operate_forbidip', detail_privilege, controller.main.operate_forbidip);
  router.resources('/api/backend/operate_forbidip', detail_privilege, controller.api.backend.forbidip);

  router.get('/operate_forbidmac', detail_privilege, controller.main.operate_forbidmac);
  router.resources('/api/backend/operate_forbidmac', detail_privilege, controller.api.backend.forbidmac);

  router.get('/operate_changename', detail_privilege, controller.main.operate_changename);
  router.post('/api/backend/operate_changename', detail_privilege, controller.api.backend.player.operate_changename);

  router.get('/operate_changeqianming', detail_privilege, controller.main.operate_changeqianming);
  router.post('/api/backend/operate_changeqianming', detail_privilege, controller.api.backend.player.operate_changeqianming);

  router.get('/operate_kick', detail_privilege, controller.main.operate_kick);
  router.post('/api/backend/operate_kick', detail_privilege, controller.api.backend.player.operate_kick);

  router.get('/operate_changescene', detail_privilege, controller.main.operate_changescene);
  router.post('/api/backend/operate_changescene', detail_privilege, controller.api.backend.player.operate_changescene);

  router.get(/^\/ophistory\/backend\/([0-9]+)$/, common_privilege, controller.api.backend.player.op_history);
  router.get(/^\/ophistory_([0-9]+)$/, common_privilege, controller.main.op_history);


  /* 监控告警 */
  router.get('early_warning_setting','/early_warning_setting', detail_privilege, controller.home.early_warning_setting);
  router.post('/api/backend/early_warning_setting', detail_privilege, controller.api.backend.earlywarning.settings_show);
  router.post('/api/backend/early_warning_setting_edit', detail_privilege, controller.api.backend.earlywarning.settings_edit);

  router.get('early_warning_setting','/early_warning_record', detail_privilege, controller.home.early_warning_record);
  router.post('/api/backend/early_warning_record', detail_privilege, controller.api.backend.earlywarning.record_show);

  router.get('warning_mail_list','/warning_mail_list', detail_privilege, controller.home.warning_mail_list);
  router.get('/api/backend/warning_mail_list', detail_privilege, controller.api.backend.earlywarning.mail_list);
  router.post('/api/backend/warning_mail_list', detail_privilege, controller.api.backend.earlywarning.mail_list_eidt);



  router.post('/api/backend/show_guild', detail_privilege, controller.api.backend.guild.show_guild);
  router.get('/operate_changeguild', detail_privilege, controller.main.operate_changeguild);
  router.post('/api/backend/operate_changeguild', detail_privilege, controller.api.backend.guild.operate_changeguild);

  router.get('/operate_changeguildnotice', detail_privilege, controller.main.operate_changeguildnotice);
  router.post('/api/backend/operate_changeguildnotice', detail_privilege, controller.api.backend.guild.operate_changeguildnotice);







   //对外API接口
  router.post('/api/public/server/ban_role', controller.api.thirdparty.info.ban_role);
  router.post('/api/public/server/ban_ip', controller.api.thirdparty.info.ban_ip);


  // 测试用API，上线前需要注掉
  // router.post('/api/test/post', controller.api.test.post);

  /** Authentication **/
  // router.get('/login', controller.home.login);
  router.post('/login',
  app.passport.authenticate('local', { failureRedirect: '/login', successRedirect: '/index'}));
  app.get('/logout',
  function(req, res){
    req.logout();
    req.session = null;
    this.redirect('/login');
  });
};
