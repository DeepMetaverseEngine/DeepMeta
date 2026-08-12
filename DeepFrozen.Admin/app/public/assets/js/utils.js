function changeSwitcheryState(el,value){
	if($(el).is(':checked')!=value){
		$(el).trigger("click")
	}
}

function loadGroup(){   
	registerCallbacks();
	$.ajax({    
	    type:'get',        
	    url:'/api/backend/get_all_group',    
	    cache:false,    
	    dataType:'json',    
	    success:function(obj){
	        if(!_.isEmpty(obj)) {
	        	server_list = obj;
	        	select_list = new Array();
	            $.each(obj, function(i){     
	                $('#group_selector').multiSelect('addOption', { value: obj[i].id, text: obj[i].name+ '(' + obj[i].group +')' });   
	             });   
	        }
	    }    
	});    
}

function registerCallbacks(){
	$('#group_selector').multiSelect({
	  afterSelect: function(values){
	  	if(!_.contains(select_list,values[0])){
	  		var arr = getSameGroupByServer(values);
	  		select_list = select_list.concat(arr);
	  		$('#group_selector').multiSelect('select', getSameGroupByServer(values));
	  	}
	  },
	  afterDeselect: function(values){
	  	var index = select_list.indexOf(values[0]);
		if (index !== -1) select_list.splice(index, 1);
	  	var arr = getSameGroupByServer(values);
	  	$('#group_selector').multiSelect('deselect', getSameGroupByServer(values));
	  }
	});
}



function getSameGroupByServer(val){
	var arr = new Array();
	var group_id = getCurrentServerInfo(val[0]);
	$.each(server_list, function(i){     
		  if(server_list[i].group == group_id)
		  	arr.push(server_list[i].id.toString())
	});

	return arr = _.without(arr, val.toString());
}

function getCurrentServerInfo(val){
	var group_id = 0;
	$.each(server_list, function(i){     
		  if(server_list[i].id == val){
		  	group_id = server_list[i].group
		  }
	}); 
	return group_id;
}

function getServerGroups()
{
	var arr = new Array();
	var servers = $('#group_selector').val()
	$.each(servers, function(i){ 
		var group_id = getCurrentServerInfo(servers[i]);   
		  if(group_id !=0 && !_.contains(arr,group_id))
		  	arr.push(group_id)
	});
	return arr;
}

function def_format(data){
	return moment(data).format('YYYY-MM-DD HH:mm:ss');
}

function unselect() {
    selecting = false
}

function open_channel_selector(title, selector) {
        layer.open({
          title: title,
          type: 2,
          area: ['800px', '400px'],
          fixed: false, //不固定
          content: '/widgets/channel_selector?select=' + $(selector).val()
        });
}

function open_server_selector(title, selector) {
    layer.open({
      title: title,
      type: 2,
      area: ['900px', '400px'],
      fixed: false, //不固定
      content: '/widgets/server_selector?select=' + $(selector).val()
    });
}

if ($.fn.DataTable) {
		$.extend( true, $.fn.dataTable.Editor.defaults, {
	    i18n: {
	        datetime: {
	            months:   [ '一月', '二月', '三月', '四月', '五月', '六月', '七月', '八月', '九月', '十月', '十一月', '十二月' ],
	            weekdays: [ '周日', '周一', '周二', '周三', '周四', '周五', '周六' ]
	        }
	    }
	} );
}

if ($.fn.datepicker) {
	$.fn.datepicker.dates['zh-CN'] = {
	    days: ["星期日", "星期一", "星期二", "星期三", "星期四", "星期五", "星期六", "星期日"],
	    daysShort: ["周日", "周一", "周二", "周三", "周四", "周五", "周六", "周日"],
	    daysMin: ["日", "一", "二", "三", "四", "五", "六", "日"],
	    months: ["一月", "二月", "三月", "四月", "五月", "六月", "七月", "八月", "九月", "十月", "十一月", "十二月"],
	    monthsShort: ["一月", "二月", "三月", "四月", "五月", "六月", "七月", "八月", "九月", "十月", "十一月", "十二月"],
	    today: "今天",
	    clear: "清除",
	    format: "yyyy-mm-dd",
	    weekStart: 1
	};
}


$(document).ready(function(){
	if($('#realm_selector').length ) {
		$.ajax({ 
			url:'/api/backend/get_operation_realm' , 
			dataType:"json",
			success:function(data){
				$.each(data, function (index, item) {  
				  var id = data[index].id; 
				  var name = data[index].name;
				  var selected = data[index].selected ? 'selected':'';
				  $("#realm_selector").append("<option value='" + id + "' " + " "+ selected + ">"+name+"</option>");
				}); 
			}
	    });

	    $("#realm_selector").change(function(){
			var id = $(this).val();
			$.ajax({
				url: '/api/backend/set_operation_realm',
				data: {id: id},
				success:function(data){
					if(data.state){
						$('#realm_selected').show();
						$('#realm_not_selected').hide();
						$('#realm_name_show').text('[' + data.name +']')
						$('#realm_id_show').text('[' + data.id + ']')
					}else {
						$('#realm_selected').hide();
						$('#realm_not_selected').show();
						$("#realm_selector").val($("#realm_selector option:first").val());
					}
					if ($.change_realm_event) {
						$.change_realm_event();
					}
				}
			});
	    });
	}
});