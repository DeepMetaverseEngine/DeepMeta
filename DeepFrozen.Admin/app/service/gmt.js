const Service = require('egg').Service;
const util = require('util');

class GMTService extends Service {

  async send_command_attach(obj, postData, msg) {
    return this.send_command(obj, msg, postData)
  }

  async send_command_post(obj, data, msg) {
    var post_data = JSON.stringify(data)
    var content = new Buffer(post_data).toString("base64");
    return this.send_command(obj, msg, {data: { content:encodeURIComponent(content) }})
  }

  async send_command(obj, msg, postData) {
  	let realm = await this.service.realmselector.get_realm()
    if(obj.hasOwnProperty('realm_id')) {
        realm = await this.service.realmselector.get_realm_by_id(obj.realm_id)
      }
  	if(!this.ctx.helper.is_empty(realm)){
  		var params = this.generate_cmd(obj, realm.gmt_key);
      this.ctx.logger.info("====================params=======================")
      this.ctx.logger.info(obj)
      var header = {
        method: 'POST',
        dataType: 'json',
        timeout: 30000,
      }

  		try{
  			const result = await this.ctx.curl(realm.gmt_url + params, {...header, ...postData });
        this.ctx.logger.info(result.data);
        if(result.status != 200)return {state: false, reason: this.ctx.__('page_gmt_command_internal_error',result.status)}
		    if(result.data.state){
		    	return {state: true, ext: result.data.ext, reason: msg ? msg : this.ctx.__('page_gmt_command_success')}
		    }else {
		    	return {state: false, reason: result.data.reason}
		    }
  		}catch(e){
  			this.ctx.logger.error(e);
  			return this.handle_error(e);
  		}
  	}else {
  		return {state: false, reason: this.ctx.__('page_gmt_select_realm_first')}
  	}
  }

  generate_cmd(obj, api_key){
  	var timestamp = Math.floor(new Date() / 1000);
  	var command = JSON.stringify(obj)
  	var token = this.ctx.helper.get_sha1(api_key + command + timestamp)
  	var content = new Buffer(command).toString("base64");
  	var params = util.format('?token=%s&content=%s&stamp=%s', token, encodeURIComponent(content), timestamp);
  	return params;
  }

  handle_error(e){
  	if (e.name == 'ConnectionTimeoutError') {
  		return {state: false, reason: this.ctx.__('page_gmt_command_timeout')}
  	}else if(e.name == 'RequestError') {
  		return {state: false, reason: this.ctx.__('page_gmt_command_request_error')}
  	}else if(e.name == 'JSONResponseFormatError') {
  		return {state: false, reason: this.ctx.__('page_gmt_command_response_error')}
  	}else {
  		return {state: false, reason: this.ctx.__('page_gmt_command_unknown_error') + ' ' + e.name}
  	}
  }

  async changebanstate(add_type, is_enable, address){
	  var cmd_type = {
		  ip: 'ServerBanIpRequest',
		  mac: 'ServerBanMacRequest',
	  }
	  var command = {
		  op_type: is_enable,
		  cmd: cmd_type[add_type],
	  };
	  command[add_type] = address;
	  var result = await this.ctx.service.gmt.send_command(command, 'success');
	  return result
  }
}

module.exports = GMTService;