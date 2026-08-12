'use strict';

function delay(span) {
  return new Promise(resolve => {
    setTimeout(resolve, span);
  })
}

module.exports = app => {
  app.role.use('user', ctx => !!ctx.user);

  //管理员
  app.role.use('admin', async function (ctx) {
    //await delay(2000);
    return ctx.user && ctx.user.privilege == 1;
  });

  //运维
  app.role.use('operator', async function (ctx) {
    return ctx.user && ctx.user.privilege <= 2;
  });

  //运营
  app.role.use('product', async function (ctx) {
    return ctx.user && ctx.user.privilege <= 3;
  });

  //台湾运营
  app.role.use('tw_product', async function (ctx) {
    return ctx.user && ctx.user.privilege <= 4;
  });

  //公共权限
  app.role.use('common', async function (ctx) {
    return ctx.user && ctx.user.privilege <= 999;
  });
  //url详细权限
  app.role.use('detail', async function(ctx){
    if(! ctx.user) return false
    if(ctx.user.privilege == 0){
      return true
    }else{
      let privileges
      if(ctx.user.privilege == 1){ 
        privileges = ctx.user.privileges.split(',')
      }else{
        privileges = app.group_p[ctx.user.privilege]
      }
      for(let pi = 0; pi < privileges.length; pi++){
        if(app.urlists[privileges[pi]] && app.urlists[privileges[pi]].includes(ctx._matchedRoute)){
          return true
        }
      }
    }
    return false
    //return ctx.user && ctx.user.privileges.includes(app.urlists[ctx._matchedRoute])
  })

  app.role.failureHandler = function(ctx, action) {
    const message = 'Forbidden, required role: ' + action;
    if (ctx.acceptJSON) {
      ctx.body = {
        message: message,
        stat: 'deny',
      };
    } else {
      if(ctx.user) {
        ctx.status = 404;
      }else {
        ctx.redirect('/login');
      }
      //ctx.body = message;
    }
  };
};