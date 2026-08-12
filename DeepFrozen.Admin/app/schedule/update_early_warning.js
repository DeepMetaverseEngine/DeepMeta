const Subscription = require('egg').Subscription;


class UpdateEarlyWarning extends Subscription {
  // 通过 schedule 属性来设置定时任务的执行间隔等配置
  static get schedule() {
    return {
      interval: '1m', // 1 分钟间隔
      type: 'worker',
      disable: true
    };
  }

  // subscribe 是真正定时任务执行时被运行的函数
  async subscribe() {
    var min=new Date().getMinutes()
    if(min % 5 != 0){
      return
    }
    
    await this.ctx.service.corn.update_early_warning()

    await this.ctx.service.corn.send_notification()

    this.ctx.logger.info('update_early_warning execute.')
  }
}

module.exports = UpdateEarlyWarning;