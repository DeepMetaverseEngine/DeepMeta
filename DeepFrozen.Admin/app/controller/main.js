'use strict';
const Controller = require('egg').Controller;

class MainController extends Controller {

  async player_query_acc() {
    let server_list = await this.ctx.service.serverlist.get_all_servers()
    await this.ctx.render('page/player/player_query_acc.html',{
      title: this.ctx.__('page_player_query'),
      server_list: server_list,
    });
  }
  async player_query_role() {
    let server_list = await this.ctx.service.serverlist.get_all_servers()
    await this.ctx.render('page/player/player_query_role.html',{
      title: this.ctx.__('page_player_rolequery'),
      server_list: server_list,
    });
  }
  async player_charge(){
    await this.ctx.render('page/player/player_charge.html',{
      title: this.ctx.__('page_player_charge')
    })
  }

  async player_ability(){
    let server_list = await this.ctx.service.serverlist.get_all_servers()
    await this.ctx.render('page/player/player_ability.html', {
      title: this.ctx.__('page_player_abilityquery'),
      server_list: server_list,
    })
  }

  async player_yuanbao(){
    let server_list = await this.ctx.service.serverlist.get_all_servers()
    await this.ctx.render('page/player/player_yuanbao.html', {
      title: this.ctx.__('page_player_yuanbao'),
      server_list: server_list,
      gain_list: ['recharge_reward',
        'RECHARGE_5101_1',
        'RECHARGE_5001_1',
        'RECHARGE_3003_1',
        'RECHARGE_3002_1',
        'RECHARGE_3001_1',
        'RECHARGE_1006_1',
        'RECHARGE_1005_1',
        'RECHARGE_1004_1',
        'RECHARGE_1003_1',
        'RECHARGE_1002_1',
        'RECHARGE_1001_1',
        'mail_get',
        'GM',
        'business_total_recharge',
        'business_single_recharge'
      ],
      use_list: [ 'AddWorldBossBuff_2',
        'AddWorldBossBuff_4',
        'bossbufftype',
        'business_level',
        'business_wish',
        'ClientBuyDailyDungeonTicketsRequest',
        'CostRewardBackReward_OneKey',
        'guild_create',
        'guildwanted',
        'mining_levelup',
        'revive',
        'reward_back',
        'send_welfare',
        'shop/100',
        'shop/101',
        'shop/102',
        'shop/103',
        'shop/104',
        'shop/105',
        'shop/106',
        'treasureunit_wash',
        'TurnTable',
        'useitem',
        'vip_reward']
    })
  }

  async player_tongbi(){
    let server_list = await this.ctx.service.serverlist.get_all_servers()
    await this.ctx.render('page/player/player_tongbi.html', {
      title: this.ctx.__('page_player_tb'),
      server_list: server_list,
      gain_list: ['didujinku/2602106',
        'didujinku/2602107',
        'GM',
        'mail_get',
        'martialartstargetreward',
        'newopen_boss_person',
        'pet_release',
        'QuestReward',
        'reward_back',
        'sell',
        'sweep/MoneyRoomSweep_1',
        'sweep/MoneyRoomSweep_2',
        'sweep/MoneyRoomSweep_3',
        'sweep/MoneyRoomSweep_4',
        'sweep/MoneyRoomSweep_5',
        'sweep/MoneyRoomSweep_6',
        'sweep/MoneyRoomSweep_8',
        'sweepreward',
        'SystemDrop',
        'TrunTableDailyFree/1',
        'TurnTable',
        'useitem',
        'worldbossdrop'],
      use_list: ['itemcompose_cost',
        'AddWorldBossBuff_1',
        'RechargeMedicinePool',
        'AddWorldBossBuff_3',
        'reward_back',
        'CostRewardBackReward_OneKey',
        'pet_activation',
        'bossbufftype'],
    })
  }

  async player_yinliang(){
    let server_list = await this.ctx.service.serverlist.get_all_servers()
    await this.ctx.render('page/player/player_yinliang.html', {
      title: this.ctx.__('page_player_yinliang'),
      server_list: server_list,
      gain_list: ['auction_bid',
        'auction_buy',
        'auction_extract',
        'BActivity_FirstRecharge_1',
        'BActivity_FirstRecharge_2',
        'BActivity_FirstRecharge_3',
        'business_level',
        'business_login',
        'business_single_recharge',
        'business_total_recharge',
        'business_wish',
        'exchange',
        'first_recharge',
        'GiftAddReward',
        'GM',
        'guild_create',
        'mail_get',
        'newopen_boss_redbag',
        'newopen_rank',
        'QuestReward',
        'receive_welfare',
        'RECHARGE_1001_1',
        'RECHARGE_1002_1',
        'RECHARGE_1003_1',
        'RECHARGE_1004_1',
        'RECHARGE_1005_1',
        'RECHARGE_1006_1',
        'RECHARGE_2001_1',
        'RECHARGE_2002_1',
        'RECHARGE_2003_1',
        'recharge_reward',
        'sell',
        'soul_palace',
        'tehui_card',
        'tehui_fund',
        'TLClientReceiveAssistanceAcceptGiftResponse',
        'TLClientReceiveAssistanceGiftResponse',
        'TrunTableDailyFree/1',
        'TurnTable',
        'TURNTABLE_REWARD',
        'useitem',
        'vip_reward'],
      use_list: ['auction_bid',
        'auction_buy',
        'CostRewardBackReward_OneKey',
        'DoClientAuctionNewHelpRequest',
        'grid_pattern',
        'guild_create',
        'pet_activation',
        'pet_star_up',
        'reward_back',
        'shop/200',
        'shop/201',
        'shop/202',
        'shop/203',
        'shop/204'],
    })
  }

  async player_item(){
    let server_list = await this.ctx.service.serverlist.get_all_servers()
    await this.ctx.render('page/player/player_item.html', {
      title: this.ctx.__('page_player_item'),
      server_list: server_list,
    })
  }

  async player_vip(){
    await this.ctx.render('page/player/player_vip.html', {
      title: this.ctx.__('common_instructions_vip_level')
    })
  }

  async player_levelup(){
    let server_list = await this.ctx.service.serverlist.get_all_servers()
    await this.ctx.render('page/player/player_levelup.html', {
      title: this.ctx.__('page_player_levelup'),
      server_list: server_list,
    })
  }

  async player_device(){
    let server_list = await this.ctx.service.serverlist.get_all_servers()
    await this.ctx.render('page/player/player_device.html', {
      title: this.ctx.__('page_player_device'),
      server_list: server_list,
    })
  }

  async player_roledetail(){
    let server_list = await this.ctx.service.serverlist.get_all_servers()
    await this.ctx.render('page/player/player_roledetail.html', {
      title: this.ctx.__('page_player_roledetail'),
      server_list: server_list,
    })
  }

  async player_online(){
    let server_list = await this.ctx.service.serverlist.get_all_servers()
    await this.ctx.render('page/player/player_online.html', {
      title: this.ctx.__('page_player_online'),
      server_list: server_list,
    })
  }

  async player_chat(){
    let server_list = await this.ctx.service.serverlist.get_all_servers()
    await this.ctx.render('page/player/player_chat.html', {
      title: this.ctx.__('page_player_chat'),
      server_list: server_list,
    })
  }

  async operate_forbidip(){
    await this.ctx.render('page/operate/operate_forbidip.html', {
      title: this.ctx.__('page_operate_forbidip')
    })
  }

  async operate_forbidmac(){
    await this.ctx.render('page/operate/operate_forbidmac.html', {
      title: this.ctx.__('page_operate_forbidmac')
    })
  }
  async operate_changename(){
    let server_list = await this.ctx.service.serverlist.get_all_servers()
    await this.ctx.render('page/operate/operate_changename.html', {
      title: this.ctx.__('page_operate_changename'),
      server_list: server_list,
    })
  }

  async operate_kick(){
    let server_list = await this.ctx.service.serverlist.get_all_servers()
    await this.ctx.render('page/operate/operate_kick.html', {
      title: this.ctx.__('page_operate_kick'),
      server_list: server_list,
    })
  }
  async operate_changeguild(){
    let server_list = await this.ctx.service.serverlist.get_all_servers()
    await this.ctx.render('page/operate/operate_guild.html', {
      title: this.ctx.__('page_operate_changeguild'),
      server_list: server_list,
    })
  }
  async operate_changeguildnotice(){
    let server_list = await this.ctx.service.serverlist.get_all_servers()
    await this.ctx.render('page/operate/operate_guildnotice.html', {
      title: this.ctx.__('page_operate_changeguildnotice'),
      server_list: server_list,
    })
  }
  async operate_changeqianming(){
    let server_list = await this.ctx.service.serverlist.get_all_servers()
    await this.ctx.render('page/operate/operate_changeqianming.html', {
      title: this.ctx.__('page_operate_changeqianming'),
      server_list: server_list,
    })
  }
  async operate_changescene(){
    let server_list = await this.ctx.service.serverlist.get_all_servers()
    await this.ctx.render('page/operate/operate_changescene.html', {
      title: this.ctx.__('page_operate_changescene'),
      server_list: server_list,
    })
  }

  async op_history(){
    var log_type_id = this.ctx.params[0];
    var log_type = await this.app.model.LogType.find(log_type_id);
    await this.ctx.render('page/log_histroy.html',{
      title: log_type.title_i18n + '记录',
      log_type_id: log_type_id,
    });
  }

  async gm_account_list(){
    let realmlist = await this.app.model.Realm.findAll();
    await this.ctx.render('page/server_resource_gm_account_list.html', {
      title: this.ctx.__('page_account_whitelist_title'),
      realmlist: realmlist,
    })
  }

}

module.exports = MainController;
