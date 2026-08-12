const LocalStrategy = require('passport-local').Strategy;
const pluck = require('arr-pluck');
const ms = require('ms');

module.exports = app => {

  app.locals = {
    test:[1,2,3,4,5]
  }

  let communicator;

  // 挂载 strategy
  app.passport.use(new LocalStrategy({
    passReqToCallback: true,
  }, (req, username, password, done) => {
    req.ctx.model.User.findOne({
      where: {username: username} 
    }).then(db_user => {
      if (!db_user) { return done(null, false); }
      if (!db_user.verifyPassword(password)) { return done(null, false); }
      return done(null, db_user);
    })
  }));


  

  // 将用户信息序列化后存进 session 里面，一般需要精简，只保存个别字段
  app.passport.serializeUser(async (ctx, user) => {

    //设置session有效期
    if (ctx.request.body.remberme) ctx.session.maxAge = ms('30d')

    //var last = await user.get({plain: true})
    //app.logger.info(last)
    await user.loginUpdate();
    //app.logger.info(user)
    await ctx.write_log(ctx.app.action.create, {customType:'login', filter:['pwd_encrypt','salt'], after:user})
     //app.logger.info("serializeUser..ctx ");
     //app.logger.info(ctx);
     //app.logger.info(user);
  // 处理 user
  // ...
    return user;
  });

  // 反序列化后把用户信息从 session 中取出来，反查数据库拿到完整信息
  app.passport.deserializeUser(async (ctx, user) => {
    //app.logger.info("deserializeUser..",ctx,user);
  // 处理 user
  // ...
    return user;
  });

  app.validator.addRule('chkip', (rule, value) => {
    const ipp = /^((2(5[0-5]|[0-4]\d))|[0-1]?\d{1,2})(\.((2(5[0-5]|[0-4]\d))|[0-1]?\d{1,2})){3}$/;
    if(! ipp.test(value)){
      return 'error'
    }
  });

  app.validator.addRule('chkmac', (rule, value) => {
    const macp = /^([a-f0-9]{2}:?){5}[a-f0-9]{2}$/i
    if(! macp.test(value)){
      return 'error'
    }
  });
  app.messenger.on('update_var', data => {
    app[data.varname] = data.value
  })
  app.beforeStart(async () => {
    // 应用会等待这个函数执行完成才启动
    // try{
    //   communicator = Ice.initialize();
    //   const proxy = communicator.stringToProxy("AdminServer:default -p 17000").ice_twoway().ice_secure(false);
    //   app.ice_proxy = await DeepFrozenIceImpl.IAdminServiceAdapterPrx.checkedCast(proxy);
    // }
    // catch(e)
    // {
    //     console.log(e);
    // }
    let urls = await app.model.Urllist.findAll({where: {enable: 1}})
    let group_l = await app.model.Usergroup.findAll()
    let gmacc_list = await app.model.Gmaccountlist.findAll();
    let nvg = new Set(pluck(urls, 'nevigate'))
    let urlsl = {}, titlel = {}, menu_list = {}, group_p = {}

    nvg.forEach(function(sv){
      menu_list[sv] = []
    })

    group_l.forEach(function(v, i, a){
      group_p[v.dataValues.id] = v.dataValues.group_privileges.split(',')
    })

    urls.forEach(function(v, i, a){
      urlsl[v.dataValues.id] = v.dataValues.urls.split(',');
      titlel[v.dataValues.title] = v.dataValues.id;
      menu_list[v.dataValues.nevigate].push(v.dataValues.title);
    })
    app.urlists = urlsl;
    app.titlelists = titlel;
    app.menu_list = menu_list;
    app.group_p = group_p;
    app.gm_acc_list = pluck(gmacc_list, 'role_id');
  });
  app.beforeClose(async () => {
    // if(communicator)
    // {
    //     await communicator.destroy();
    // }
     await this.app.service.backend.destroy();
  });
};
