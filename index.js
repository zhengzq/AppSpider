var nrc = require('node-run-cmd');


// nrc.run('dotnet run --project store/Zq.Runner/Zq.Runner.csproj', {
//     verbose: true
// }).then(function () {
//     console.log(arguments)
// })


nrc.run('node anyproxy/index.js', {
    verbose: true
}).then(function () {
    console.log(arguments)
})


nrc.run('node appiums/index.js dd', {
    verbose: true
}).then(function () {
    console.log(arguments)
})
