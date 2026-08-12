'use strict';
const Controller = require('egg').Controller;

class CallbackController extends Controller {
	async onegame_test() {
    	var params = this.ctx.request.body;
    	this.ctx.body = await this.ctx.service.pay.onegame.parseOrder(params);
 	}

	async aligames() {
		var params = this.ctx.request.body;
		this.ctx.body = await this.ctx.service.pay.uc.parseOrder(params);
	}

	async xiaomi() {
		var params = this.ctx.query;
		this.ctx.body = await this.ctx.service.pay.xiaomi.parseOrder(params);
	}

	async egls() {
		var params = this.ctx.request.body;
		this.ctx.body = await this.ctx.service.pay.egls.parseOrder(params);
	}

	async foyo() {
		var params = this.ctx.request.body;
		this.ctx.body = await this.ctx.service.pay.foyo.parseOrder(params);
	}

	async syg() {
		var params = this.ctx.request.body;
		this.ctx.body = await this.ctx.service.pay.syg.parseOrder(params);
	}

	async cgame() {
		var params = this.ctx.query;
		this.ctx.body = await this.ctx.service.pay.cgame.parseOrder(params);
	}

	async Sanwan() {
		var params = this.ctx.query;
		this.ctx.body = await this.ctx.service.pay.sanwan.parseOrder(params);
	}

	async ewan() {
		var params = this.ctx.query;
		this.ctx.body = await this.ctx.service.pay.ewan.parseOrder(params);
	}

	async btyq() {
		var params = this.ctx.request.body;
		var secret = {
			md5_key: 'fdljdvzknbdht1k9fm26fkinjfzrbbxq',
			product_code: '51020458180842302608487655915520'
		}
		this.ctx.body = await this.ctx.service.pay.quick.parseOrder(params, secret);
	}

	async btll() {
		var params = this.ctx.request.body;
		var secret = {
			md5_key: 'iz3zduv1jpd1jsds4vnpvksjeyi9rfz9',
			product_code: '59662643829224265629395039099394'
		}
		this.ctx.body = await this.ctx.service.pay.quick.parseOrder(params, secret);
	}

	async btly() {
		var params = this.ctx.request.body;
		var secret = {
			md5_key: 'jnbbffjzrnypwchbjwrn5t2byiebkhp3',
			product_code: '12398805720931246568610600640002'
		}
		this.ctx.body = await this.ctx.service.pay.quick.parseOrder(params, secret);
	}

	async btyljx() {
		var params = this.ctx.request.body;
		var secret = {
			md5_key: 'qbxoibmzup56vnaj7a3izyfcmnsnzbbq',
			product_code: '70015569234016747357149368120718'
		}
		this.ctx.body = await this.ctx.service.pay.quick.parseOrder(params, secret);
	}

	async quick_jzhjc() {
		var params = this.ctx.request.body;
		var secret = {
			md5_key: '6qtc0j4aytpji5wpu3alnvfxyxsbfugl',
			product_code: '39295184523878661811473370832165'
		}
		this.ctx.body = await this.ctx.service.pay.quick.parseOrder(params, secret);
	}

	async quick_cnck() {
		var params = this.ctx.request.body;
		var secret = {
			md5_key: 'iextiiz88kad0ypietsbumk4detgdjjv',
			product_code: '93391435487042772241303714380980'
		}
		this.ctx.body = await this.ctx.service.pay.quick.parseOrder(params, secret);
	}
	async quick_agxx() {
		var params = this.ctx.request.body;
		var secret = {
			md5_key: 'zjzzaeeyg09xrln8djfoef5hqzq3irer',
			product_code: '24192961738881788995206512108490'
		}
		this.ctx.body = await this.ctx.service.pay.quick.parseOrder(params, secret);
	}
	async quick_ml() {
		var params = this.ctx.request.body;
		var secret = {
			md5_key: '7mirkmgimelghrs9jhbhlf4dcm4sfjxn',
			product_code: '86123348559430018840500723742420'
		}
		this.ctx.body = await this.ctx.service.pay.quick.parseOrder(params, secret);
	}
}

module.exports = CallbackController;
