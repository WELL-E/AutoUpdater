## WPF AutoUpdater

### Description:

WPF+MVVM实现的自动更新程序

- 支持更新包文件验证（比较文件MD5码）
- 支持区分x86与x64程序的更新
- 支持更新程序的版本号
- 支持执行更新策略

### Screenshots:

![](http://i.imgur.com/oWcbNhb.png)

### Usage:

程序启动需要6个参数

1. 当前版本号（0.9.0.0）
2. 升级的版本号（1.0.0.0）
3. 更新说明URL（https://github.com/WELL-E）
4. 更新包文件URL（ http://localhost:9090/UpdateFile.zip）
5. 更新后文件释放路径（E:\PlatformPath）
6. 更新包文件MD5码（2b406701f8ad92922feb537fc789561a）

如调试时参数时，参数可设为：`0.9.0.0 1.0.0.0 https://github.com/WELL-E http://localhost:9090/UpdateFile.zip E:\PlatformPath 2b406701f8ad92922feb537fc789561a`

**注：** `http://localhost:9090/UpdateFile.zip`

- `http://localhost:9090/`：为自己在本地搭建的文件服务器地址
- UpdateFile.zip：更新包文件名

如有什么疑问，加QQ群130108655

### Special thanks:
@[zhuzhen723723](https://github.com/zhuzhen723723)

### Acknowledgements:

- MahApps.Metro: [https://github.com/MahApps/MahApps.Metro](https://github.com/MahApps/MahApps.Metro "https://github.com/MahApps/MahApps.Metro")
- Source of software icon: [https://www.iconfinder.com/icons/314711/cloud_download_icon#size=128](https://www.iconfinder.com/icons/314711/cloud_download_icon#size=128 "https://www.iconfinder.com/icons/314711/cloud_download_icon#size=128")

### License:

Open sourced under the MIT license.

