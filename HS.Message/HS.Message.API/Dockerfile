# 添加基础镜像：docker会从网络下载这个镜像
FROM asp.net:8.0

#容器中系统的工作空间
WORKDIR /app

#拷贝当前文件夹下的文件到容器中系统的工作空间（注意：“.”后面有空格）
COPY . .

#设置Docker容器对外暴露的端口
EXPOSE 80

#容器中使用 ["dotnet","系统启动的dll"] 来运行应用程序
ENTRYPOINT ["dotnet", "HS.Message.API.dll"]
CMD ["--environment=docker"]