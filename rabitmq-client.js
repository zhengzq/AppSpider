var amqp = require('amqplib');
var when = require('when');
var config = require('./config')

module.exports = {
  sent: sent
}

function sent(q, msg) {
  amqp.connect(config.rabitmq.url).then(function (conn) {
    return when(conn.createChannel().then(function (ch) {
      return ch.assertQueue(q, { durable: false }).then(function (_qok) {
        ch.sendToQueue(q, Buffer.from(msg));
        return ch.close()
      });
    })).ensure(function () {
      conn.close();
    });
  }).then(null, console.warn);
}
