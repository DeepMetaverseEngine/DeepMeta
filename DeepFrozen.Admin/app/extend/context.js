module.exports = {
  /** 获取请求data**/
  get_request_primary_data(datas) {
  	for (var cell in datas) {
  		if (datas.hasOwnProperty(cell)) {
  			return datas[cell];
  		}
  	}
  },

  async write_log(action, instance) {
    var _type;
    var logs = null;
    var _internalfilter = ['created_at', 'updated_at'];
    instance._internalfilter = _internalfilter;
    if(instance.hasOwnProperty('customType')){
     var log_type = await this.app.model.LogType.findByName(instance.customType);
      _type = log_type.id;
    }else {
      _type = await instance.after.LogType();
    }

    if(instance.hasOwnProperty('filter')) {
      instance._internalfilter = _internalfilter.concat(instance.filter)
    }

  	switch(action){
  		case this.app.action.create:
  			logs = this.get_format_created_logs(instance);
  			break;
  		case this.app.action.destroy:
  			logs = this.get_format_destroyed_logs(instance);
  			break;
  		case this.app.action.update:
  			logs = this.get_format_updated_logs(instance);
  			break;
      case this.app.action.info:
        logs = this.get_format_info_logs(instance);
        break;
    }
  	var task = await this.model.Log.create({
  		user_id:this.user == null ? 0 : this.user.id, 
  		operator_ip: this.ip,
  		log_type_id: _type, 
  		operation: JSON.stringify({act:action,logs:logs})
  	})
  },

  get_format_created_logs(instance) {
  	var _logs = instance.after.dataValues;
    instance._internalfilter.forEach(function(element) {
      delete _logs[element];
    });
  	return _logs;
  },

  get_format_info_logs(instance) {
    var _logs = this.helper.extend({},this.session.realm, instance.command, instance.result)
    instance._internalfilter.forEach(function(element) {
      delete _logs[element];
    });
    return _logs;
  },



  get_format_destroyed_logs(instance) {
  	var _logs = instance.after.dataValues;
    instance._internalfilter.forEach(function(element) {
      delete _logs[element];
    });
  	return _logs;
  },

  get_format_updated_logs(instance) {
  	var _changed = {};
  	var _previous = instance.before;
  	var _next = instance.after.dataValues;
  	var _first = Object.keys(_previous)[0];
    instance._internalfilter.forEach(function(element) {
      delete _previous[element];
    });
  	_changed[_first] = {oldVal:_previous[_first],newVal:_next[_first]}
  	for(var k in _previous) {
  		if (_next.hasOwnProperty(k)) {
	  		if(_next[k] != _previous[k]){
	  			_changed[k] = {oldVal:_previous[k],newVal:_next[k]}
	  		}
  		}
  	}
  	return _changed;
  },
};