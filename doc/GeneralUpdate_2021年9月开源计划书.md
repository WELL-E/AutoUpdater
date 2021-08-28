2021年9月开源计划

## 1.在线帮助文档网站开发 ##
该网站基于blazor技术开发，展示GeneralUpdate所有组件的介绍、用法、和类、方法等详细说明。

开源地址:

- https://github.com/JusterZhu/JusterSpaceStation
- https://gitee.com/Juster-zhu/JusterSpaceStation

## 2.GeneralUpdate组件开发 ##
在源码项目中“example”文件夹里有GeneralUpdate组件的使用demo示例。目前不够完善，需看懂源码和使用方法编写对应的使用示例，炫酷的更新界面等等。

开源地址:

- https://github.com/WELL-E/AutoUpdater
- https://gitee.com/Juster-zhu/GeneralUpdate


迭代日志:

2020-5-30

- https://mp.weixin.qq.com/s/pclAR3_G5IcvUkjy2D4xwg

2020-8-30

- https://mp.weixin.qq.com/s/BNugQ58MoqJS1OU_86A-VA

2021-3-18

- https://mp.weixin.qq.com/s/Qtbh9FFqh9Mrj37Zue3GZg

2021-8-21

- https://mp.weixin.qq.com/s/xBI5WNmDpnfAUBB-wTN0gQ

视频介绍

- https://www.bilibili.com/video/BV1aX4y137dd

### GeneralUpdate.Core ###
1. 需新增【增量更新功能】，客户端上传当前版本号服务端根据客户端版本号生成差异更新包提供下载。
1. 需新增【更新本地“sqlite数据库”，“配置文件”】，当数据库或配置文件有结构变化时，需保留客户端本地原有文件数据把文件结构更新后再把原有的数据或配置还原到新的文件结构当中。

### GeneralUpdate.ClientCore ###
1. 需新增【增量更新功能】，同上。

### GeneralUpdate.Single ###
1. 需新增【目前只支持dotnet-framework，需编写为.net standard框架且能支持所有的框架版本】

### GeneralUpdate.AspNetCore ###
1. 需新增【增量更新功能】，根据客户端提供的信息生成增量包以供下载更新。