var mqClient = require('../../../rabitmq-client.js');
var untils = require('../../../untils.js')

var config = {
  queue: '36k',
  listURL: 'https://gateway.36kr.com/api/mis/nav/copartner/flow'
}

module.exports = {
  beforeSendResponse: function (requestDetail, responseDetail) {
    if (requestDetail.url.indexOf(config.listURL) === 0) {
      try {
        mqClient.sent(config.queue, untils.byteToString(responseDetail.response.body))
      } catch (e) {
        console.log(e)
      }
    }
  }
}