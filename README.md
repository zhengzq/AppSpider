# AppSpider

## 简介

**AppSpider** 是一个使用Android 模拟器，使用Appium 模拟用户操作，通过代理拦截请求获取api接口数据的解决方案。

## 声明

代码、教程仅限于学习交流，请勿用于任何商业用途！

## 结构说明

![](https://github.com/zhengzq/AppSpider/blob/master/images/structure.jpg)

## 视屏预览

![image](http://p1.pstatp.com/large/2e87e00077f85d5b29ef1)

地址 [https://www.toutiao.com/i6770340660668006920/](https://www.toutiao.com/i6770340660668006920/)


## 使用说明

一、环境准备
1. 执行 npm i 安装依赖包

2. 安装模拟器

3. 安装 appium 并启动

     http://appium.io/

4. 安装ADB工具

    a) 下载最新的Android SDK，地址是：http://developer.android.com/sdk/index.html；

    b) ADB在解压目录下的\sdk\platform-tools

    c) 添加到 环境变量的 PATH 里面

    d) cmd 执行 adb devices 查看连接设备

5. 获取 appPackage 和 appActivity

    a) cmd 执行 adb shell
    
    b) 打开 apk 到开始的页面
    
    c) 执行 dumpsys window windows | grep -E mCurrentFocus 
    
      【例子： mCurrentFocus=Window{bdc75aa u0 com.luojilab.player/com.luojilab.business.HomeTabActivity}】

6. 设置模拟器 Wifi 代理
    ip: 127.0.0.1
    port: 8001

7. 使用 appium 记录操作功能
    记录操作的脚本复制过来使用
    记得切换, 使用的是  javascript wd 的脚本

二、 配置

config.js

配置消息队列连接地址

三、 脚本

1. 在 appiums\apps 创建 脚本文件

    类似：
    
    appiums\apps\wx.js
    
    说明：
    
        const capabilities = {
        
            platformName: "Android",
            
            platformVersion: "6.0.1", //安卓版本
            
            deviceName: "127.0.0.1:7555", //安卓连接（具体模拟器端口 下面有说明）
            
            // app: "D:/Users/zq/Desktop/Processing/focus/auto/appiums/apks/weixin708android1540.apk", // app 安装包
            
            appPackage: "com.tencent.mm",
            
            appActivity: ".ui.LauncherUI",
            
            noReset: true,
            
            chromeOptions: { 'androidProcess': 'com.tencent.mm:tools' },
            
            unicodeKeyboard: true,//使用Unicode编码方式发送字符串
            
            resetKeyboard: true,//使用Unicode编码方式发送字符串
            
            newCommandTimeout: 1200
        };
        
        这里是连接 appium 软件
        
        var driver = await wd.promiseChainRemote({
            host: '127.0.0.1',
            port: 4723
        });

2. 在 anyproxy\interceptors 创建拦截脚本

四、 启动

npm run start


## 捐赠

![](https://github.com/zhengzq/AppSpider/blob/master/images/wx.jpg)
![](https://github.com/zhengzq/AppSpider/blob/master/images/zfb.jpg)

## 其他

如需指导可加QQ 4062499, 有偿提供指导.

## License

MIT License
