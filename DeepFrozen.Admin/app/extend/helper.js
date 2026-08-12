var crypto = require('crypto');
const xml2js = require('xml2js');
const util = require('util');
const moment = require('moment');

module.exports = {
  //get_permission(param) {
  //	return this.ctx.user &&
  //	this.ctx.user.privilege <= this.config.menu_privileges[param].privilege
  //},
  sleepms(ms) {
    return new Promise((resolve) => setTimeout(resolve, ms));
  },

  get_detail_permission(param) {
    if(this.ctx.user.privilege == 0) return true
    let privileges
    if(this.ctx.user.privilege == 1){ 
      privileges = this.ctx.user.privileges.split(',')
    }else{
      privileges = this.app.group_p[this.ctx.user.privilege]
    }   
    return this.ctx.user && privileges.includes(this.app.titlelists[param].toString());     
  },

  object_add(obj, key, value){
    if(obj[key]){
      obj[key].push(value)
    }else {
      obj[key] = [value];
    }
  },
  is_empty(obj) {
  	if(!obj)return true;
	return !Object.keys(obj).length;
  },
  is_local_ip(addr){
    if(addr.startsWith('103.242.169.212') || addr.startsWith('192.168.') || addr.startsWith('127.0.0.1')){
      return true
    }
    return false
  },
  get_sha1(str) {
  	return crypto.createHash('sha1').update(str).digest('hex')
  },
  extend(target) {
    var sources = [].slice.call(arguments, 1);
    sources.forEach(function (source) {
        for (var prop in source) {
            target[prop] = source[prop];
        }
    });
    return target;
  },
  /** 通过状态码获取对应服务器参数 **/
  get_srv_state(state) {
    var view_rgba,icon,text
    if(state == '1'){
      rgba = this.ctx.app.srvColor.normal
      icon = this.ctx.app.srvIconAlias.normal
      text = this.ctx.__('page_serverlist_state_normal')
    }else if(state == '2'){
      rgba = this.ctx.app.srvColor.hot
      icon = this.ctx.app.srvIconAlias.hot
      text = this.ctx.__('page_serverlist_state_hot')
    }else if(state == '3'){
      rgba = this.ctx.app.srvColor.full
      icon = this.ctx.app.srvIconAlias.full
      text = this.ctx.__('page_serverlist_state_full')
    }else if(state == '4'){
      rgba = this.ctx.app.srvColor.preserve
      icon = this.ctx.app.srvIconAlias.preserve
      text = this.ctx.__('page_serverlist_state_preserve')
    }else if(state == '5'){
      rgba = this.ctx.app.srvColor.normal
      icon = this.ctx.app.srvIconAlias.new
      text = this.ctx.__('page_serverlist_state_new')
    }else if(state == '6'){
      rgba = this.ctx.app.srvColor.normal
      icon = this.ctx.app.srvIconAlias.recommend
      text = this.ctx.__('page_serverlist_state_recommend')
    }else {
      rgba = this.ctx.app.srvColor.normal
      icon = this.ctx.app.srvIconAlias.normal
      text = this.ctx.__('page_serverlist_state_normal')
    }
    return {view_rgba: rgba, icon: icon, state_text: text}
  },
  /** 批量检查订单状态 **/
  getManyOrderValid(orders) {
    var valid_orders = {}
    for ( var i = 0; i <orders.length; i++){
      if(this.getOrderValid(orders[i])){
        this.object_add(valid_orders, orders[i].role_id, orders[i].cp_order_id)
      }
    }
    return valid_orders;
  },
  /** 检查订单状态 **/
  getOrderValid(order) {
    if(order.status == this.ctx.app.orderStatus.OrderValid || order.status == this.ctx.app.orderStatus.OrderFinish){
      return true;
    }else {
      return false;
    }
  },
 /** XML to Json**/
  async xmlToJs(xml) {
    xml2js.parseStringPromise = util.promisify(xml2js.parseString);
    return await xml2js.parseStringPromise(xml);
  },
  isNumber(nubmer) {
　　var re = /^[0-9]+.?[0-9]*$/; //判断字符串是否为数字 //判断正整数 /^[1-9]+[0-9]*]*$/ 
　　if (re.test(nubmer)) {
　　　　return true;
　　}
    return false;
  },
  /** 获取日期区间 返回array **/
  getDateRange(startDate, stopDate) {
    var dateArray = [];
    var currentDate = moment(startDate);
    var stopDate = moment(stopDate);
    while (currentDate <= stopDate) {
        dateArray.push( moment(currentDate).format('YYYY-MM-DD') )
        currentDate = moment(currentDate).add(1, 'days');
    }
    return dateArray;
  },
  getTimeRange(stfx, idx, mutx) {
    var timeArray = [];
    var h,m
    for (var j = 0; j < 24; j++) {
      var i = 0
      for (var k = 0; k < mutx; k++) {
        h = j
        m = i
        if(h < 10){
          h = '0' + h
        }

        if(m < 10){
          m = '0' + m
        }
        timeArray.push(stfx + ' ' + h + ':' + m + ':00')
        i+= idx
      }
    }
    return timeArray;
  },
  /** 日期 + **/
  addDate(date, days) {
    return moment(date).add(days, 'days');
  },
  addDateFormat(date, days) {
    return moment(date).add(days, 'days').format('YYYY-MM-DD');
  },

};