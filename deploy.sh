#!/bin/bash

# 部署脚本 - 使用 Docker 部署文章管理系统

echo "开始部署文章管理系统..."

# 构建 Docker 镜像
echo "构建 Docker 镜像..."
docker build -t article-api:latest .

# 停止并删除旧的容器（如果存在）
echo "清理旧容器..."
docker-compose down

# 启动新的容器
echo "启动新容器..."
docker-compose up -d

echo "部署完成！"
echo "应用访问地址: http://localhost:8080"
echo "API 文档: http://localhost:8080/swagger"

# 显示容器状态
echo "容器状态:"
docker-compose ps
