# 文章管理系统部署指南

## 部署选项

### 1. Docker 部署（推荐）

#### 前提条件
- 安装 Docker Desktop
- 安装 Docker Compose

#### 快速部署
```bash
# Windows
./deploy.bat

# Linux/Mac
chmod +x deploy.sh
./deploy.sh
```

#### 手动部署
```bash
# 构建镜像
docker build -t article-api:latest .

# 启动服务
docker-compose up -d

# 查看日志
docker-compose logs -f

# 停止服务
docker-compose down
```

访问地址：
- 应用主页：http://localhost:8080
- API 文档：http://localhost:8080/swagger

### 2. 云服务器部署

#### Azure Web App
1. 在 Azure 门户创建 Web App
2. 配置容器设置，使用构建的 Docker 镜像
3. 配置环境变量和数据库连接字符串

#### AWS ECS
1. 创建 ECS 集群
2. 推送镜像到 ECR
3. 创建任务定义和服务

#### 腾讯云/阿里云
1. 购买云服务器
2. 安装 Docker
3. 上传项目文件并运行部署脚本

### 3. 传统服务器部署

#### 安装 .NET 8 运行时
```bash
# Ubuntu
wget https://packages.microsoft.com/config/ubuntu/20.04/packages-microsoft-prod.deb -O packages-microsoft-prod.deb
sudo dpkg -i packages-microsoft-prod.deb
sudo apt-get update
sudo apt-get install -y aspnetcore-runtime-8.0

# Windows Server
# 下载并安装 .NET 8 ASP.NET Core Runtime
```

#### 发布应用
```bash
# 生成发布文件
dotnet publish -c Release -o ./publish

# 复制到服务器
scp -r ./publish user@server:/path/to/app/

# 在服务器上运行
cd /path/to/app/
dotnet ArticleAPI.dll
```

#### 配置反向代理（Nginx）
```nginx
server {
    listen 80;
    server_name your-domain.com;
    
    location / {
        proxy_pass http://localhost:5000;
        proxy_http_version 1.1;
        proxy_set_header Upgrade $http_upgrade;
        proxy_set_header Connection keep-alive;
        proxy_set_header Host $host;
        proxy_cache_bypass $http_upgrade;
        proxy_set_header X-Forwarded-For $proxy_add_x_forwarded_for;
        proxy_set_header X-Forwarded-Proto $scheme;
    }
}
```

## 环境变量配置

### 生产环境
```bash
ASPNETCORE_ENVIRONMENT=Production
ASPNETCORE_URLS=http://+:80
ConnectionStrings__DefaultConnection="Server=your-server;Database=ArticleDB;User=sa;Password=YourPassword;TrustServerCertificate=true;"
```

### 开发环境
```bash
ASPNETCORE_ENVIRONMENT=Development
ASPNETCORE_URLS=http://localhost:5000
```

## 数据库配置

### 使用 SQL Server
1. 修改 `appsettings.Production.json` 中的连接字符串
2. 确保数据库服务器可访问
3. 应用会自动创建数据库和表结构

### 使用其他数据库
1. 安装相应的 EF Core 提供程序
2. 修改 `Program.cs` 中的数据库配置
3. 更新连接字符串

## 监控和日志

### 应用日志
日志输出到控制台，可以通过以下方式查看：
```bash
# Docker
docker-compose logs -f articleapi

# 系统服务
journalctl -u your-app-service -f
```

### 健康检查
访问 `/health` 端点检查应用状态

## 故障排除

### 常见问题
1. **端口冲突**：确保端口 80/8080 未被占用
2. **数据库连接失败**：检查连接字符串和网络访问
3. **权限问题**：确保应用有足够的文件系统权限
4. **内存不足**：分配足够的内存资源

### 日志分析
```bash
# 查看错误日志
docker-compose logs articleapi | grep ERROR

# 查看性能指标
docker stats
```

## 安全建议

1. 使用 HTTPS（配置 SSL 证书）
2. 设置强密码和安全的连接字符串
3. 限制网络访问（防火墙规则）
4. 定期更新依赖包
5. 配置适当的 CORS 策略
6. 使用环境变量存储敏感信息
