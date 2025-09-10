using Microsoft.EntityFrameworkCore;
using ArticleAPI.Data;
using ArticleAPI.Services;
using ArticleAPI.Models;

var builder = WebApplication.CreateBuilder(args);

// 添加服务到容器
builder.Services.AddControllers();

// 添加 Blazor Server 服务
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

// 配置Entity Framework - 统一使用SQLite
builder.Services.AddDbContext<ArticleDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// 配置HttpClient用于调用API
builder.Services.AddHttpClient<ArticleService>(client =>
{
    var baseAddress = builder.Environment.IsDevelopment() 
        ? "http://localhost:5000" 
        : "http://localhost:80";
    client.BaseAddress = new Uri(baseAddress);
});

// 配置Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "Article API", Version = "v1" });
    
    // 启用XML注释
    var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
    {
        c.IncludeXmlComments(xmlPath);
    }
});

// 配置CORS（如果需要）
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();



// 配置HTTP请求管道
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Article API V1");
        c.RoutePrefix = "swagger"; // 将Swagger UI移动到 /swagger 路径
    });
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseCors("AllowAll");
app.UseRouting();

app.UseAuthorization();

// 配置路由
app.MapControllers(); // API 控制器
app.MapBlazorHub(); // Blazor Hub
app.MapFallbackToPage("/_Host"); // Blazor 页面

// 初始化数据库并添加示例数据
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ArticleDbContext>();
    context.Database.EnsureCreated();
    
    // 添加示例数据
    if (!context.Articles.Any())
    {
        context.Articles.AddRange(
            new ArticleAPI.Models.Article
            {
                Title = "欢迎使用 ASP.NET Core Web API",
                Content = "这是一个使用 ASP.NET Core 8 和 Entity Framework Core 构建的文章管理 API。通过这个API，您可以轻松地管理文章内容，包括创建、读取、更新和删除操作。",
                CreatedAt = DateTime.UtcNow.AddDays(-2)
            },
            new ArticleAPI.Models.Article
            {
                Title = "Entity Framework Core 入门指南",
                Content = "Entity Framework Core 是一个轻量级、可扩展、开源和跨平台的数据访问技术。它支持 LINQ 查询、变更跟踪、更新和架构迁移。EF Core 可在 .NET Core 或 .NET Framework 上运行。",
                CreatedAt = DateTime.UtcNow.AddDays(-1)
            },
            new ArticleAPI.Models.Article
            {
                Title = "Blazor Server 应用开发",
                Content = "Blazor Server 是一种托管模型，其中组件在服务器上运行，UI 更新通过 SignalR 连接发送到浏览器。这种模式提供了丰富的交互性，同时保持了较小的客户端大小。",
                CreatedAt = DateTime.UtcNow.AddHours(-6)
            }
        );
        context.SaveChanges();
    }
}

app.Run();
