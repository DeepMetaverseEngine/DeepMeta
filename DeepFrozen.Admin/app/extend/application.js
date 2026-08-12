const orderStatus = {
	Create: 0,
	PaySuccess: 1,
	OrderValid: 2,
	OrderInvalid: 3,
	OrderFinish:4
}

const consts_bi = {
    enter: 'role.login',         //帐号登录
    quit: 'role.logout',             //帐号登出
    createrole: 'role.create',       //创建角色
    login: 'role.login',            //登录角色
    logout: 'role.logout',           //登出角色
    pay: 11,              //充值付费
    prop: 12,             //道具变更
    //inventory:13,       //道具存量
    guide: 14,            //新手引导
    currency: 15,        //代币变量
    ser: 16,             //游戏服务器在线数
    delrole: 17,         //删除角色
    curremain: 18,       //代币存量
    accregi: 19,        //游戏内新增帐号
    acclogin: 20,        //游戏内帐号登录
    adt: 21,    //用户自定义事件
    currgold: 22,        //金币变量
    propinfos: 26,       //道具列表
    rolechat: 27,        //聊天消息
    empty: 0
    }



const action = {
	create: 0,
	destroy: 1,
	update: 2,
	info: 3,
}


const srvColor = {
  normal: '0xFFFFFFFF',
  hot: '0xFFFFFFFF',
  full: '0xFFFFFFFF',
  preserve: '0x595151FF',
}

const srvIconAlias = {
	normal:'#dynamic/TL_login/output/TL_login.xml|TL_login|46',
	hot: '#dynamic/TL_login/output/TL_login.xml|TL_login|47',
	full: '#dynamic/TL_login/output/TL_login.xml|TL_login|45',
	preserve: '#dynamic/TL_login/output/TL_login.xml|TL_login|48',
	new: '#dynamic/TL_login/output/TL_login.xml|TL_login|76',
	recommend: '#dynamic/TL_login/output/TL_login.xml|TL_login|75',
}

const rolePrivileges = [
    {privilege:0, title:'page_account_privilege_player'},
    {privilege:1, title:'page_account_privilege_powerplayer'},
    {privilege:2, title:'page_account_privilege_vip'},
    {privilege:3, title:'page_account_privilege_programer'},
    {privilege:4, title:'page_account_privilege_disgner'},
    {privilege:5, title:'page_account_privilege_wizard'},
    {privilege:6, title:'page_account_privilege_powerwizard'},
    {privilege:7, title:'page_account_privilege_superwizard'},
    {privilege:8, title:'page_account_privilege_leader'},
    {privilege:9, title:'page_account_privilege_manager'},
    {privilege:10, title:'page_account_privilege_supermanager'},
    {privilege:11, title:'page_account_privilege_bogboss'}
  ];

module.exports = {
	  action,
	  srvColor,
	  srvIconAlias,
	  rolePrivileges,
	  orderStatus,
    consts_bi,
}