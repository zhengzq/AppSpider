var cmd = require('node-cmd');
var wd = require("wd")

const capabilities = {
    platformName: "Android",
    platformVersion: "6.0.1",
    // deviceName: "127.0.0.1:62001",
    deviceName: "127.0.0.1:7555",
    // app: "D:/Users/zq/Desktop/Code/Focus/app spider/appiums/apks/com.android36kr.app.apk",
    appPackage: "com.android36kr.app",
    appActivity: "com.android36kr.app.ui.LogoActivity",
    noReset: true,
    chromeOptions: { 'androidProcess': 'com.tencent.mm:tools' },
    unicodeKeyboard: true,//使用Unicode编码方式发送字符串
    resetKeyboard: true,//使用Unicode编码方式发送字符串
};

module.exports = async function () {
    var driver = await wd.promiseChainRemote({
        host: '127.0.0.1',
        port: 4723
    });

    await driver.init(capabilities).setImplicitWaitTimeout(10000);

    let el1 = await driver.elementById("com.android36kr.app:id/main_tab_invest_text");
    await el1.click().sleep(3000);

    for (let index = 0; index < 200; index++) {
        var action = new wd.TouchAction(driver);
        action
            .press({ x: 421, y: 1256 })
            .wait(800)
            .moveTo({ x: 449, y: 326 })
            .release();
        await action.perform();
    }
}