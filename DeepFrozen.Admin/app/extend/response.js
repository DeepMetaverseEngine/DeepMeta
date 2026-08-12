const builder = require('xmlbuilder');



module.exports = {
  /** 返回table特定格式错误信息 **/
  rsp_table_field_errors(errors) {
    var table_errors = [];
    for (var key in errors) {
        table_errors.push({name:errors[key].field,status:errors[key].message});
    }
    this.body = {fieldErrors:table_errors};
  },

  rsp_body_errors(errors) {
    var table_errors = [];
    for (var key in errors) {
        table_errors.push("filed: " + errors[key].field + " code: " + errors[key].code);
    }
    this.body = {state: false, reason: table_errors.join('\n')}
  },

  rsp_table_error(e) {
    var error = e[0];
    var message = error.message + " value:" + error.value;
    this.body = {error:message}
  },

  rsp_xml_object(obj) {
    var xml = builder.create('root').ele({root:obj}).toString();
    this.body = xml;
  }
};