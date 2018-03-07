rd /s /q publish
cd ..\..\DockerDemoApi
dotnet clean
dotnet build
dotnet publish -o ..\..\Dockerfiles\Development\publish
cd ..\Dockerfiles\Development
@echo off
FOR /F "tokens=4 delims= " %%i in ('route print ^| find " 0.0.0.0"') do set localIp=%%i
echo %localIp%
powershell -Command "(gc publish\appsettings.json) -replace 'localhost', '%localIp%' | sc publish\appsettings.json"
docker build -t mswarbrick/dockerdemoapi:dev .
docker push mswarbrick/dockerdemoapi:dev