print("luascript is doing")



function Test1()
	print("---------------------------")
	print(api)
end

function Test(sss)
	--api:saveMap();                        --保存地图
	--[[第一章]]--
	map = api:setMapData(1001,9,23);                                      --设置地图信息，并获取该地图(地图资源ID,出生点坐标X,出生点坐标Y)
	map:changeMap();	
	                                                --切换地图
	--api:changeWeather(yu,1);
	user = api:getUser();                                               --获取玩家
	user:setDirectory(3);                                               --设置面向 0上1左2下3右
	user:setHp(2000);
	user:setMaxHp(2000);
	api:backupFast();
	
	--user:changeClothes(1);                                              --更换上身(资源ID)
 	--user:changePants(1);                                                --更换下身(资源ID)
 	--user:changeHair(1);                                                 --更换头发(资源ID)
 	--user:changeWeapon(1,1);                                             --更换武器(资源ID,武器类型)
	--user:changeHead(1);                                                 --更换头部(资源ID)
	
	m1 = api:createMonster(1006,28,21,1,false,true);                        --设置怪物数据，并获取该APC(资源ID,出生点X,出生点Y,面向,是否重生)
	m1:setHp(200);
	m1:setMaxHp(200);
	m1:setAICamp(10);                                                   --设置AI的阵营,user0-9,monster10-19
	m1:setAIId(3);                                                      --设置AI脚本(脚本编号)
	m2 = api:createMonster(1006,28,25,1,false,true);
	m2:setHp(200);
	m2:setMaxHp(200);
	m2:setAICamp(10);
	m2:setAIId(3);
	

	
	camera = api:getCamera();                                           --获取镜头
	--camera:moveToPoint(0,0,0);                                          --将镜头移动到指定点(X,Y,每帧移动的像素0表示瞬移)
	camera:moveToUnit(user,6);
	
	api:waitAll();	                                                     --等待当前执行的所有动作结束
	
	api:blackScreenExit(0,true); 
	api:movieModeShow();	                                             --开启电影模式，屏幕上下载入黑幕条
	user:moveTo(14,23,3);                                               --设置自动移动到目标点(x,y,面向)
	api:waitAll();                                                      --等待当前执行的所有动作结束
	
	--[[显示一个对话
	参数dialogName：说话者姓名
    参数dialogStr：话语内容
	参数isLeft：人物图片是否在屏幕左边
	参数rolePic：人物图片ID
	参数facePic：表情图片ID
	参数closeDialog：说完后是否关闭对话框]]--

	api:dialog("<name>","这是什么地方？我是谁......为什么脑子里一团乱......",true,0,0,false);   
	camera:followUnit(m1,15); 
	api:dialog("帝国魔兵A","看，一个人类!别让他跑了，领主大人一定很因为多了一个健壮的奴隶而奖赏我的!",false,4,0,false);    
   	api:dialog("帝国魔兵B","呃......这次你又想要独占功劳吗？",false,4,0,false);
   	api:dialog("帝国魔兵A","少啰嗦，快抓起来，别让他跑了!",false,4,0,true);

	--api:dialog("<name>","......你们、你们要干什么？",true,0,0,false); 
	--api:dialog("帝国魔兵A","虽然没有这个义务，但还是让你死个明白，我们是大名鼎鼎的赤焰帝国...血色狮子军团!",false,4,0,false);
	--api:dialog("<name>","赤焰帝国...很熟悉的名字...到底是什么...你们到底是谁？",true,0,0,true); 
	
	
	
	
	--api:blackScreenExit(1,true);                                         --在若干时间内将屏幕由黑变透明(时间S,是否需要等待这个动作结束后才执行下一个动作)
	camera:followUnit(user,15);                                           --将镜头移动到单位当前所在的点(目标单位,speed)
   	api:waitAll();                                                       --等待当前执行的所有动作结束
	camera:restoreCamera();                                              --将镜头还原，即跟随玩家移动
	api:waitAll();  
		
	
  	
	
	
	
	
	--api:waitAll();                                                        --等待当前执行的所有动作结束
	--设置玩家技能

	fastSkill1_user = api:createSkill();                                   --创建一个技能，并获取该技能
 	fastSkill1_user:setShowIcon(0);  
	fastSkill1_user:setCD(12);                                              --设置CD时间 
 	fastSkill1_user:setTargetRange(4);                                     --设置释放距离(地图格数)
 	fastSkill1_user:setTargetType(0);                                      --设置目标类型 0-敌对目标1-友方目标2-自己 3-任意目标4-无需目标
	fastSkill1_user:setDamage(40,50,50);                                --设置伤害数值范围(min,max,暴击概率)
	
	
	fastSkill2_user = api:createSkill();                                   --创建一个技能，并获取该技能
 	fastSkill2_user:setCD(50);                                              --设置CD时间 
	fastSkill2_user:setShowIcon(101);
 	fastSkill2_user:setDamage(40,50,50);                                 --设置伤害数值范围(min,max,暴击概率)
 	fastSkill2_user:setTargetRange(4);                                     --设置释放距离(地图格数)
 	fastSkill2_user:setTargetType(0);                                      --设置目标类型 0-敌对目标
	fastSkill2_user:setFireEffect(15,0);                                    --设置释放时自身特效(效果ID,音效ID)
 	fastSkill2_user:setFinishEffect(1,0);                                  --设置命中特效(效果ID,音效ID)
 	fastSkill2_user:setRange(5,5,0); 
 	
	
	 fastSkill3_user = api:createSkill();
 	fastSkill3_user:setCD(60);
	fastSkill3_user:setShowIcon(102);
 	fastSkill3_user:setDamage(40,50,50);
	fastSkill3_user:setTargetRange(4);  
 	fastSkill3_user:setTargetType(0);
	fastSkill3_user:setFireEffect(16,0);                                    --设置释放时自身特效(效果ID,音效ID)                               
 	fastSkill3_user:setExecutorEffect(2,0);                                --设置持续性特效(效果ID,音效ID)
 	fastSkill3_user:setRange(5,5,0);                                        --设置效果范围和偏移(w,h,offset)
 	fastSkill3_user:setFinishEffect(17,0)                                       --设置命中特效(效果ID,音效ID)
 	
	
	fastSkill4_user = api:createSkill();
 	fastSkill4_user:setCD(80);
 	fastSkill4_user:setDamage(40,50,50);
 	fastSkill4_user:setTargetRange(4);
 	fastSkill4_user:setTargetType(0);
 	fastSkill4_user:setFireEffect(0,0);
 	fastSkill4_user:setShowIcon(103);
	fastSkill4_user:setFinishEffect(1,0)  
 	
	
	fastSkill5_user = api:createSkill();
 	fastSkill5_user:setCD(100);
 	fastSkill5_user:setDamage(40,50,50);
 	fastSkill5_user:setTargetRange(7);
 	fastSkill5_user:setTargetType(0);
 	fastSkill5_user:setFireEffect(20,0);
 	fastSkill5_user:setRange(4,4,0);
 	fastSkill5_user:setShowIcon(104);
	fastSkill5_user:setFinishEffect(1,0)  
 	
	
	local pro = user:getPro();
	--local pro = 21;
	
	if (pro == 1) then

		
	
		fastSkill2_user:setShowIcon(101);                                    --设置显示图标(图标ID)
		fastSkill3_user:setShowIcon(102);  
		fastSkill4_user:setShowIcon(103);  
		fastSkill5_user:setShowIcon(104);  
		fastSkill5_user:setExecutorEffect(202,0);
	elseif (pro == 11) then
	
		
		fastSkill3_user:setTargetRange(8);
		fastSkill5_user:setTargetRange(0);
		
		fastSkill2_user:setCD(40); 
		
		fastSkill5_user:setTargetType(4); 
	
		fastSkill2_user:setShowIcon(1101);                                    --设置显示图标(图标ID)
		fastSkill3_user:setShowIcon(1102);  
		fastSkill4_user:setShowIcon(1103);  
		fastSkill5_user:setShowIcon(1104);  
		
		fastSkill2_user:setFireEffect(22,0); 
		fastSkill3_user:setFireEffect(0,0); 
		fastSkill4_user:setFireEffect(26,0); 
		fastSkill5_user:setFireEffect(1104,0); 
	
	elseif (pro == 21) then
	
		fastSkill1_user:setTargetRange(8); 
		fastSkill2_user:setTargetRange(8);
		fastSkill3_user:setTargetRange(2);
		fastSkill4_user:setTargetRange(6);
		fastSkill5_user:setTargetRange(10);
		
		fastSkill2_user:setCD(30); 
		fastSkill3_user:setCD(60); 
		fastSkill4_user:setCD(90); 
		fastSkill5_user:setCD(60); 
		
		fastSkill3_user:setTargetType(4); 
		
		fastSkill2_user:setChargeTime(10)
		fastSkill3_user:setChargeTime(5)
		fastSkill5_user:setChargeTime(20)
		
		fastSkill4_user:setChannelTime(20)
		
		--fastSkill5_user:setChargeEffect(39)                                 -- 暂未实现

		fastSkill2_user:setShowIcon(2101);                                    --设置显示图标(图标ID)
		fastSkill3_user:setShowIcon(2102);  
		fastSkill4_user:setShowIcon(2103);  
		fastSkill5_user:setShowIcon(2104);  
		
		fastSkill2_user:setFireEffect(30,0); 
		fastSkill3_user:setFireEffect(0,0); 
		fastSkill4_user:setFireEffect(36,0); 
		fastSkill5_user:setFireEffect(0,0); 
		
		fastSkill2_user:setFinishEffect(32,0);
		fastSkill5_user:setFinishEffect(41,0);
		
		fastSkill1_user:setFlyEffect(52000,0)
		fastSkill2_user:setFlyEffect(31,0)
		fastSkill5_user:setFlyEffect(40,0)
	end
	
	user:addAttackSkill(fastSkill1_user);                                  --添加普通功能技能(技能对象)	
	user:addFastSkill(fastSkill2_user,0);                                  --添加一个快捷键技能(技能对象,位置索引)
	--user:addFastSkill(fastSkill3_user,1);
	--user:addFastSkill(fastSkill4_user,2);
	--user:addFastSkill(fastSkill5_user,3);
	
	--设置怪物技能
	fastSkill1_monster = api:createSkill();                                   --创建一个技能，并获取该技能
 	fastSkill1_monster:setCD(20);                                              --设置CD时间 
 	fastSkill1_monster:setDamage(5,10,10);                                   --设置伤害数值范围(min,max,暴击概率)
 	fastSkill1_monster:setTargetRange(4);                                     --设置释放距离(地图格数)
 	fastSkill1_monster:setTargetType(0);    
	m1:addAttackSkill(fastSkill1_monster);  
	m2:addAttackSkill(fastSkill1_monster); 
	m1:setAIId(2);
	m2:setAIId(2);
	api:movieModeExit();	                                             --关闭电影模式，屏幕上下载入黑幕条
	api:teachMove();                                             --启动移动教学
	api:waitAll();
	api:teachAttack();                                           --启动攻击教学	
	
	 
	api:checkKillAllMonster();                            --检测是否所有的怪物都被消灭
	api:blackScreenShow(1,true);
	api:waitAll(); 
	api:movieModeShow();	                                             --开启电影模式，屏幕上下载入黑幕条
	--[[
  	user:doAttackAction(3);                                               --做一次攻击动作(面向)
	m1:addEffect(3,0,false);                                              
	m2:addEffect(3,0,false);                                               --释放一个技能效果(技能ID,音效ID,结束动作true挥砍,false无动作)
  	api:waitByFrame(8);                                                   --等待若干帧后执行下一个动作(帧数)
  	m2:doDeadAction(2);                                                   --设置为死亡模式(面向)
	m1:doDeadAction(2);
  	api:waitByFrame(10);
	]]--
	m3 = api:createMonster(1006,28,23,1,false,true); 
	api:blackScreenExit(0.4,true);
	api:waitAll(); 
	m3:moveTo(46,18,3);
	--api:waitAll(); 
	
	api:dialog("帝国魔兵A","你、你等着，赤焰帝国不会放过你的!",false,4,0,false);
	m3:setVisible(false,0); 
	api:dialog("<name>","赤焰帝国？",true,0,0,true);
  	user:moveTo(45,18,3);                                                 --设置自动移动到目标点(x,y,面向)
  	api:waitAll();                                                        --等待当前执行的所有动作结束
	api:waitByFrame(5);                                                   --等待若干帧后执行下一个动作(帧数)
  	user:setVisible(false,0);                                               --设置为是否可见

  	
	
  	api:waitAll();                                                       --等待当前执行的所有动作结束
	
	--[[第二章]]--
	
	map = api:setMapData(1002,10,39);
	map:changeMap();
	api:changeWeather(yu,2);
	user = api:getUser();
	user:setDirectory(3);
	user:setHp(2000);
	user:setMaxHp(2000);
	api:movieModeShow();	
	
	api:blackScreenExit(0,true);	
	
	user:moveTo(20,25,3);                                          --设置自动移动到目标点(x,y,面向)
	api:waitAll();		
	
	--6个怪
	m1 = api:createMonster(1006,43,26,2,false,true);
	m1:setAICamp(10);
	m1:setAIId(3);	
	m2 = api:createMonster(1006,43,24,2,false,true);
	m2:setAICamp(10);
	m2:setAIId(3);
	m3 = api:createMonster(1006,42,25,2,false,true);
	m3:setAICamp(10);
	m3:setAIId(3);
	m4 = api:createMonster(1006,42,26,2,false,true);
	m4:setAICamp(10);
	m4:setAIId(3);	
	m5 = api:createMonster(1006,41,26,2,false,true);
	m5:setAICamp(10);
	m5:setAIId(3);
	m6 = api:createMonster(1006,41,22,2,false,true);
	m6:setAICamp(10);
	m6:setAIId(3);
	
	--[[
	map = api:setMapData(1,11,21);
	map:changeMap();
	
	map:addPortalEffect(49,14,"","",0);                                  --添加一个传送门效果
	
	api:movieModeShow();	                                              --开启电影模式，屏幕上下载入黑幕条
	
	user = api:getUser();
	user:setDirectory(3);]]--
	
	--[[玩家技能]]--
		 	

 
 		
		
	
	--[[
	--刷小怪
	m1 = api:createMonster(1003,30,20,2,false);
	m1:setAICamp(10);
	m1:setAIId(3);
	m1:setUnitName(TextParse:getString(18,50));                           --设置显示的名称
	
	m2 = api:createMonster(1003,39,22,1,false);
	m2:setAICamp(10);
	m2:setAIId(3);
	m2:setUnitName(TextParse:getString(18,50));
	
	m3 = api:createMonster(1003,44,17,2,false);
	m3:setAICamp(10);
	m3:setAIId(3);
	m3:setUnitName(TextParse:getString(18,50));
	
	
	api:waitAll();                                               --等待当前执行的所有动作结束
	
	api:blackScreenExit(0,true);                                 --在若干时间内将屏幕由黑变透明(时间S,是否需要等待这个动作结束后才执行下一个动作)
	
	user:moveTo(13,22,3);
	api:waitAll();
	api:movieModeExit();                                         --关闭电影模式，上下的黑幕条移出屏幕
	
	api:teachMove();                                             --启动移动教学
	
	triggerZone = api:createTriggerZone(17,15,10,15);            --设置触发区域，并获得该区域(x,y,w,h)
 	triggerZone:start();                                         --启动区域判断
	
	user:moveTo(25,20,3);
	api:waitAll();         
	api:teachAttack();                                           --启动攻击教学
	api:createDirectoryCue(3,5);                                 --创建方向箭头提示(方向,持续时间)
	
	triggerZone = api:createTriggerZone(48,13,3,3);              --设置触发区域，并获得该区域(x,y,w,h)
 	triggerZone:start();                                          --启动区域判断
	]]--
	
	--[[第三章]]--
	--[[
	map = api:setMapData(2,10,39);
	map:changeMap();
	
	user = api:getUser();
	user:setDirectory(3);
	
	api:movieModeShow();	
	
	api:blackScreenExit(0,true);	
	
	user:moveTo(26,25,0);                                          --设置自动移动到目标点(x,y,面向)
	api:waitAll();		
	
	--6个怪
	m1 = api:createMonster(1003,31,26,2,false);
	m1:setAICamp(10);
	m1:setAIId(3);	
	m2 = api:createMonster(1003,33,26,2,false);
	m2:setAICamp(10);
	m2:setAIId(3);
	m3 = api:createMonster(1003,31,24,2,false);
	m3:setAICamp(10);
	m3:setAIId(3);
	m4 = api:createMonster(1003,35,26,2,false);
	m4:setAICamp(10);
	m4:setAIId(3);	
	m5 = api:createMonster(1003,31,22,2,false);
	m5:setAICamp(10);
	m5:setAIId(3);
	m6 = api:createMonster(1003,34,23,2,false);
	m6:setAICamp(10);
	m6:setAIId(3);
	]]--
	
	api:waitAll();
	api:waitByFrame(5);                            --等待若干帧后执行下一个动作
	
	--user:addFacePic(0);
	camera:followUnit(m3,15); 
	api:dialog("魔兵队长A","真该死，我正心情不好呢，又出现一个人类，刚洗干净的衣服又要染血了。",false,4,0,false);
	api:dialog("魔兵队长B","等等，我觉得不对劲，这个人类身上是不是有什么奇怪的力量？",false,4,0,false);
	api:dialog("魔兵队长A","别管了，一个人类而已。我要他戴的那条红色宝石项链，其他都归你。",false,4,0,false);
	api:dialog("魔兵队长B","就这么办。",false,4,0,false);
	api:waitAll();
	camera:followUnit(user,15); 
	api:dialog("<name>","这条项链......一路上很多人都想抢走它。",true,0,0,false);
	api:waitAll();
	camera:followUnit(m3,15); 
	api:dialog("魔兵队长A","以后就不会，让老子替你保管吧!",false,4,0,true);
	api:waitAll();
	--[[

	]]--
	camera:followUnit(user,15);
	api:waitAll();
	camera:restoreCamera();
	user:moveTo(26,25,3);	                               --瞬移到目标点(x,y,面向)
	api:waitAll();
	
	api:blackScreenShow(0.3,true);
	api:dialogScreen("敌人的数量和实力都远胜于你，正当你力不从心之际，一个背影挡在了你的面前",26);
	

	--普攻技能
	attackSkill = api:createSkill();
	attackSkill:setCD(20);
 	attackSkill:setTargetRange(8);
 	attackSkill:setDamage(40,50,50);
	
	--刷出APC
	--apc = api:createAPC(2001,20,30,3,true);
	apc = api:createAPC(2001,32,25,3,true);
	--apc:addEffect(11,0,false);
	apc:setHp(50000);
	apc:setMaxHp(50000);
	apc:addAttackSkill(attackSkill);
	apc:addFastSkill(fastSkill2_user,0);                                  --添加一个快捷键技能(技能对象,位置索引)
	apc:addFastSkill(fastSkill3_user,1);
	apc:addFastSkill(fastSkill4_user,2);
	apc:addFastSkill(fastSkill5_user,3);
	apc:setAICamp(0);
	apc:setAIId(3);
	apc:setUnitName("神秘人");

	api:blackScreenExit(0.1,true);
	
	api:waitAll();
	
	
	m1:moveTo(37,24,2);
 	m2:moveTo(36,24,2);
	m3:moveTo(35,25,2);
	m4:moveTo(35,25,2);
	m5:moveTo(36,26,2);
	m6:moveTo(37,26,2);
	api:waitAll();
	--user:setDirectory(2);
	--user:addFacePic(14);
	--m1:addFacePic(16);
 	--m2:addFacePic(16);
	api:waitAll();
	--api:waitByFrame(5);
	
	--camera:followUnit(apc,5);                                --将镜头移动到单位所在的点，之后跟随单位移动(目标单位,帧数)
	--api:waitAll();
	--apc:moveTo(32,25,3);
 	--api:waitAll();
	--user:setDirectory(3);
	
	apc:doAttackAction(3);
	apc:addEffect(15,0,true);
	--m1:addEffect(2,0,false);                             --释放一个技能效果(技能ID,音效ID,结束动作true挥砍,false无动作)
	--m2:addEffect(2,0,false);                             --释放一个技能效果(技能ID,音效ID,结束动作true挥砍,false无动作)
 	api:waitByFrame(5);
 	m1:doDeadAction(1);
 	m2:doDeadAction(1);
	m3:doDeadAction(1);
	m4:doDeadAction(1);
	m5:doDeadAction(1);
	m6:doDeadAction(1);
 	api:waitAll();
	
	camera:restoreCamera();                                  -- 将镜头还原，即跟随玩家移动
	
	api:dialog("神秘人","年轻人，没事吧？这里刚刚撤走一批魔兵主力，后续部队正在四处扫荡，你一个人非常危险。",false,1,0,false);
	api:dialog("<name>","多谢你救了我，请问你是？",true,0,0,true);



 	--user:addFacePic(16);
 	--apc:addFacePic(16);

 	api:removeAllMonster();
	--刷出BOSS
 	boss = api:createMonster(1044,43,25,2,false,true);	
 	--boss:addEffect(11,0,false);
	boss:setUnitName("魔族首领");
	camera:followUnit(boss,15);
	api:waitAll();
	api:dialog("魔族首领","可恶，一个卑弱的人类，竟敢伤害我的部属，我要把你们撕成碎片!",false,13,0,false);
	camera:followUnit(user,15);
 	api:dialog("<name>","真抱歉，刚认识就连累你......",true,0,0,false);
	api:dialog("神秘人","别分心，握紧你的武器，这只就交给我了!",false,1,0,true);
	boss:moveTo(36,25,1);
	api:waitAll();
	
	camera:restoreCamera();    
	--map:setTerrianBlock(18,21,1,20);                        -- 设置地图块为不可行走区域(x,y,w,h)
	api:movieModeExit();
	api:waitAll();
 	api:changeWeather(yu,2);
	
	api:waitByFrame(5);	
	
	--BOSS技能
	fastSkill1_boss = api:createSkill();
 	fastSkill1_boss:setCD(20);
 	fastSkill1_boss:setDamage(10,20,50);
 	fastSkill1_boss:setTargetRange(4);
 	fastSkill1_boss:setTargetType(0);
 	fastSkill1_boss:setFireEffect(16,0);
 	fastSkill1_boss:setShowIcon(50004);
 	boss:addFastSkill(fastSkill1_boss,2);
 
 	fastSkill2_boss = api:createSkill();
 	fastSkill2_boss:setCD(20);
 	fastSkill2_boss:setDamage(10,20,50);
 	fastSkill2_boss:setTargetRange(4);
 	fastSkill2_boss:setTargetType(0);
 	--fastSkill2_boss:setFireEffect(17,0);
 	fastSkill2_boss:setRange(4,4,0);
 	fastSkill2_boss:setShowIcon(50005);
 	boss:addFastSkill(fastSkill2_boss,3);
	
	boss:addAttackSkill(attackSkill);
	boss:setAICamp(10);
	boss:setAIId(2);
	--刷出随从
	--[[
	m1 = api:createMonster(1006,37,23,1,false,true);
	m1:addAttackSkill(attackSkill);
	m1:setAICamp(10);
	m1:setAIId(2);	
	m1:setUnitName("魔兵随从");
	m2 = api:createMonster(1006,37,27,1,false,true);
	m2:addAttackSkill(attackSkill);
	m2:setAICamp(10);
	m2:setAIId(2);
	m2:setUnitName("魔兵随从");
	]]--
	apc:setAIId(1);
	
	
	api:checkKillAllMonster();                            --检测是否所有的怪物都被消灭
	apc:setAIId(3);
	api:movieModeShow();
		--刷出apc赛维拉
	swl = api:createAPC(2006,50,27,1,true);
	
	--camera:followUnit(swl,15);
	swl:setAICamp(0);
	swl:setAIId(3);
 	swl:setUnitName("赛维拉");


	
	api:waitAll();
	user:setDirectory(3);
	user:moveTo(29,27,0);
	apc:moveTo(34,25,2);
 	api:waitAll();
	swl:moveTo(34,27,1);
	api:waitAll();
	
	api:dialog("赛维拉","我好像来晚了。这位是......不管你是谁，我得说，你真走运，不是每个被魔族追捕的奴役都能遇上威灵顿将军的。怎么样，跟我们去喝一杯庆祝一下？",false,2,0,false);
	api:dialog("威灵顿大将军","赛维拉，现在不是开玩笑的时候。对了，你说你不知道自己是谁，是什么意思？",false,1,0,false);
 	api:dialog("<name>","我......我不知道怎么解释。我的记忆似乎是从我醒来后开始的，而过去的一切，名字，身份，家事，一切的记忆似乎都消失了。",true,0,0,false);
	api:dialog("赛维拉","哈，听起来像某类传奇人物的故事。假如真的是的话，最好提前通知我，我可不想做主角的炮灰。",false,2,0,false);
	api:dialog("威灵顿大将军","我看你身手不错，反正你也没有别的地方去，不如来联盟军吧!",false,1,0,true);
 	--user:addFacePic(6);
	api:blackScreenShow(0.3,true);                         --在若干时间内将屏幕由透明变黑(s,是否等到这个动作结束后执行下一个动作)
	api:dialogScreen("还没来得及回应威灵顿的邀请，你感到体力不支，失去意识晕倒在地。威灵顿俯身检查你的伤势，在见到你的项链时脸上隐约露出了惊讶的神色......",26);
	api:waitAll();
	--api:loadMap();        --读取地图
	--api:blackScreenExit(1,true);
	api:movieModeExit();
	api:restoreFast();
	
	api:waitAll();
	api:storyOver();                                 --剧情脚本结束
	api:apiEnd();                                    --事件全部结束，关闭解析器

end





Test1()
api=ScriptExecutorLua:getGameFunInterface()
Test1()

Test(3)

