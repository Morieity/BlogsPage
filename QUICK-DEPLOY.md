# 简单部署指南

## 方案1：直接部署到服务器

### 1. 准备服务器
- Windows Server 或 Linux 服务器
- 安装 .NET 8 运行时

### 2. 发布项目
在你的本地机器上：
```bash
dotnet publish -c Release -o ./publish
```

### 3. 上传到服务器
将 `publish` 文件夹上传到服务器

### 4. 运行应用
在服务器上：
```bash
cd publish
dotnet ArticleAPI.dll --urls "http://0.0.0.0:80"
```

## 方案2：云平台部署

### Azure Web App
1. 登录 Azure 门户
2. 创建新的 Web App
3. 选择 .NET 8 运行时
4. 使用 Visual Studio 或 Azure CLI 发布

### 腾讯云 / 阿里云
1. 购买云服务器
2. 安装 .NET 8
3. 上传项目文件
4. 配置反向代理（可选）

## 方案3：使用 Docker（推荐）

### 前提条件
- 服务器安装 Docker

### 部署步骤
1. 将项目文件上传到服务器
2. 运行部署脚本：
   ```bash
   # Windows
   deploy.bat
   
   # Linux
   chmod +x deploy.sh
   ./deploy.sh
   ```

## 快速开始（推荐）

### 如果你有云服务器：
1. 在服务器上安装 .NET 8 运行时
2. 在本地运行：`dotnet publish -c Release -o ./publish`
3. 将 publish 文件夹上传到服务器
4. 在服务器运行：`dotnet ArticleAPI.dll --urls "http://0.0.0.0:80"`

### 访问应用
- 应用地址：http://你的服务器IP
- API 文档：http://你的服务器IP/swagger
