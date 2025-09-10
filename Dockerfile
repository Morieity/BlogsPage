# 使用官方的 .NET 8 运行时镜像作为基础镜像
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# 使用官方的 .NET 8 SDK 镜像来构建应用
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["ArticleAPI.csproj", "."]
RUN dotnet restore "ArticleAPI.csproj"
COPY . .
WORKDIR "/src"
RUN dotnet build "ArticleAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ArticleAPI.csproj" -c Release -o /app/publish /p:UseAppHost=false

# 最终镜像
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ArticleAPI.dll"]
