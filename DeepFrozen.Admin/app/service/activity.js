const Service = require('egg').Service;

const letters = ["a", "c", "d", "e", "f", "g", "h", "j", "k", "m", "n", "p", "r",
 "t", "u", "v", "w", "x", "y", "3", "4", "7"]
const max_length = 11

function generate_one_key(hash, prefix){
  let length = prefix.length
  var key = prefix
  for (var i = length; i < max_length; i++) {
    key = key + letters[Math.floor(Math.random()*letters.length)];
  }
  hash.set(key, true);
}

class ActivityService extends Service {
  async show() {
    //let activity = await this.ctx.model.Activity.findAll();
    let activity = await this.app.model.query('select * from activity as act left join (SELECT count(*) as cdksum,count(if(status=0, false, null)) as cdkrem, activity_id FROM cdkey group by activity_id) as cdk on act.id = cdk.activity_id');
    return activity[0];
  }

  async generate_cdkey(activity_id, prefix, qty) {
    let act = await this.ctx.model.Activity.find(activity_id);
    let batch_id = act.last_batch + 1;
    await  await this.ctx.model.Activity.updateBatch(activity_id, batch_id);
    console.log("generate_cdkey " + activity_id + " " + prefix + " " + qty + " " + batch_id);
    let cdkeys = new Map();
    while (cdkeys.size < qty) {
      generate_one_key(cdkeys, prefix+batch_id);
    }
    var d = new Date();
    var records = [];
    cdkeys.forEach(function(value, key) {
      records.push(
        {
          id: key,
          status: 0,
          created_at: d,
          updated_at: d,
          batch: batch_id,
          activity_id: activity_id
        }
      );
    });
    try {
      let task = await this.ctx.model.Cdkey.bulkCreate(records);
    } catch (e) {
      console.log(e);
    } finally {

    }

    return cdkeys;
  }
}

module.exports = ActivityService;
