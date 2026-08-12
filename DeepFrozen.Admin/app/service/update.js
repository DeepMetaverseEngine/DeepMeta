const Service = require('egg').Service;


class UpdateService extends Service {
  async show() {
    let updates = await this.ctx.model.Update.findAll({where: {id: {[this.app.model.Op.ne]: 0}}});
    return updates;
  }


  async check_update() {
  	let ctx = this.ctx;
  	var params = ctx.request.body;
  	ctx.logger.info(params)
    var sdk_name = params.sdkName;
    var channel = params.channel;
    var os_type = params.ostype;
    var version = params.version;
    var response_data = {}
    if(os_type != 0){
    	var data = await ctx.model.Update.find(channel);
    	if(!ctx.helper.is_empty(data)){
    		if(data.is_enable){
                var cdn_url = null;
    			var repair_notice = await ctx.service.notice.get_server_notice(ctx.request);
    			var reapir_state = 0;
    			var repair_content;
    			if(!ctx.helper.is_empty(repair_notice)){
    				reapir_state = 1;
    				repair_content = repair_notice;
    			}
                var mpq_resource = await this.ctx.service.mpq.find_mpq(version);
                if(!ctx.helper.is_empty(mpq_resource)){
                    cdn_url = mpq_resource.cdn_url;
                }else {
                    cdn_url = null;
                }
    			if(version < data.least_build){
    				response_data = {status:1, message:'force update', update_type:2, update_url:data.update_url}
    			}else if(version < data.current_build){
    				response_data = {
    					status:1, 
    					message:'regular update', 
    					update_type:1, 
    					update_url:data.update_url,
                        cdn_url:cdn_url,
    					repair_notice_state:reapir_state,
    					repair_content:repair_content
    				}
    			}else {
    				response_data = {
    					status:1, 
    					message:'no update', 
    					update_url:data.update_url,
                        cdn_url:cdn_url,
    					repair_notice_state:reapir_state,
    					repair_content:repair_content
    				}
    			}

                if(reapir_state == 0){
                    delete response_data['repair_content'];
                }
    		}else {
    			response_data = {status:1, message:'check update is disabled.'}
    		}
    	}else {
            var mpq_resource = await this.ctx.service.mpq.find_mpq(version);
            if(!ctx.helper.is_empty(mpq_resource)){
                cdn_url = mpq_resource.cdn_url;
            }else {
                cdn_url = null;
            }
            response_data = {status:1, cdn_url:cdn_url, message:'skip default platform.'}
    		// response_data = {status:0, message:'channel not exist.'}
    	}
    }else {
    	response_data = {status:1, message:'skip default platform.'}
    }
    this.ctx.response.rsp_xml_object(response_data);
  }
}

module.exports = UpdateService;