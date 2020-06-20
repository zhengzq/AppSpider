
var proxy = require("anyproxy");
const exec = require('child_process').exec;
var config = require("../config")

if (!proxy.utils.certMgr.ifRootCAFileExists()) {
  proxy.utils.certMgr.generateRootCA((error, keyPath) => {
    // let users to trust this CA before using proxy
    if (!error) {
      const certDir = require('path').dirname(keyPath);
      console.log('The cert is generated at', certDir);
      const isWin = /^win/.test(process.platform);
      if (isWin) {
        exec('start .', { cwd: certDir });
      } else {
        exec('open .', { cwd: certDir });
      }
    } else {
      console.error('error when generating rootCA', error);
    }
  });
}

var options = {
  type: "http",
  port: 8001,
  rule: require("./rule.js"),
  webInterface: {
    enable: true,
    webPort: 8002
  },
  dangerouslyIgnoreUnauthorized: false,
  forceProxyHttps: true,
  wsIntercept: true,
  silent: true
};
var server = new proxy.ProxyServer(options);
server.on('ready', function () {
  console.log('start anyproxy')
})
server.on('error', function () {
  console.log([error, arguments])
})
server.start();