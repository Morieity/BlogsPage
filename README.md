# Article API

这是一个使用 ASP.NET Core 8 和 Entity Framework Core 构建的文章管理 Web API。

## 功能特性

- ✅ 获取所有文章 (GET /api/articles)
- ✅ 获取单个文章 (GET /api/articles/{id})
- ✅ 创建文章 (POST /api/articles)
- ✅ 更新文章 (PUT /api/articles/{id})
- ✅ 删除文章 (DELETE /api/articles/{id})

## 技术栈

- ASP.NET Core 8
- Entity Framework Core
- Swagger/OpenAPI 文档
- 内存数据库（可切换到 SQL Server）

## 快速开始

### 1. 恢复依赖包
```bash
dotnet restore
```

### 2. 运行项目
```bash
dotnet run
```

### 3. 访问 API 文档
项目启动后，访问 `https://localhost:5001` 或 `http://localhost:5000` 查看 Swagger UI 文档。

## API 端点

### 获取所有文章
```http
GET /api/articles
```

### 获取单个文章
```http
GET /api/articles/{id}
```

### 创建文章
```http
POST /api/articles
Content-Type: application/json

{
  "title": "文章标题",
  "content": "文章内容"
}
```

### 更新文章
```http
PUT /api/articles/{id}
Content-Type: application/json

{
  "title": "更新的标题",
  "content": "更新的内容"
}
```

### 删除文章
```http
DELETE /api/articles/{id}
```

## 数据模型

### Article
- `Id` (int): 文章ID（自动生成）
- `Title` (string): 文章标题（必填，最大200字符）
- `Content` (string): 文章内容（必填）
- `CreatedAt` (DateTime): 创建时间（自动设置）

## 配置说明

### 数据库配置
默认使用内存数据库进行演示。如需使用 SQL Server，请：

1. 修改 `Program.cs` 中的数据库配置：
```csharp
builder.Services.AddDbContext<ArticleDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
```

2. 运行数据库迁移：
```bash
dotnet ef migrations add InitialCreate
dotnet ef database update
```

### CORS 配置
项目已配置允许所有来源的跨域请求，生产环境中请根据需要调整。

## 开发说明

- 项目使用 DTO 模式分离内部模型和 API 接口
- 包含完整的错误处理和验证
- 支持 Swagger UI 进行 API 测试
- 代码包含中文注释便于理解
