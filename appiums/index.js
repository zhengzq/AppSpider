var cmd = require('node-cmd');

cmd.run('appium');

var wx = require('./apps/' + process.argv[2] + '')

wx().then(function () {
    console.log(['end', arguments])
})