# IndustrialDeviceLog

[![.NET Version](https://img.shields.io/badge/.NET-9.0-blue.svg)](https://dotnet.microsoft.com/)
[![License](https://img.shields.io/badge/license-MIT-green.svg)](LICENSE)


这是一个轻量级、开箱即用的工业设备日志管理工具，专为 AGV、机械臂、复合机器人等调试场景设计的实践项目。

## 功能特性

- **日志录入**：快捷录入：设备编号、运行状态、自定义内容等
- **筛选查询**：按设备 ID、时间段、运行状态等多维检索
- **Excel 导出**：一键导出当前筛选结果，自动生成带格式的报表
- **轻量级部署**：单文件可执行，无需安装数据库（内置 SQLite）；适用于现场调试笔记本、工控机、平板

## 技术栈

- **后端框架**： ASP.NET Core 9.0 MVC
- **ORM框架**： Entity Framework Core 9.0.10
- **数据库**： SQL Server
- **Excel导出**： NPOI 2.7.5
- **配置管理**：Microsoft.Extensions.Configuration.Json 9.0.10
- **开发工具**： Visual Studio 2022, Git

## 截图/演示

![主界面](/screenshots/MainWindowsView.png)  
*主界面：设备列表与日志概览*

![日志录入](/screenshots/SaveLogView.png)  

![日志查询](/screenshots/QueryView.png)  

![Excel导出](/screenshots/ExportLogRecordView.png)  
![Excel导出](/screenshots/ExportLogRecordExcel.png)  


## 快速开始


### 先决条件

- [.NET 9.0 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- [Visual Studio 2022](https://visualstudio.microsoft.com/) 或 [VS Code](https://code.visualstudio.com/)
- SQL Server (本地或 Express 版本)

### 安装与运行

1.  **克隆仓库**
---bash
git clone https://github.com/wuwy999/IndustrialDeviceLog.git
cd IndustrialDeviceLog

2.  **还原 NuGet 包**
---bash
dotnet restore

3.  **配置数据库**
- 打开 `appsettings.json` 文件，修改 `DefaultConnection` 连接字符串，使其指向自己的本地 SQL Server。

---bash
dotnet ef database update

4.  **运行项目**
---bash
dotnet run

项目将启动，并在控制台输出一个网址（如 `https://localhost:5001`）。

5.  **打开浏览器**
访问控制台输出的网址即可。



## 项目结构

IndustrialDeviceLog/

├── Controllers/         # MVC 控制器

├── Models/              # 数据模型

├── ModelViews/          # 业务模型

├── Views/               # 视图页面

├── Data/                # 数据库上下文（DbContext）和迁移文件

├── screenshots/         # 项目截图

└── Program.cs           # 应用程序入口

## 许可证

本项目基于 [MIT](LICENSE) 许可证开源。
