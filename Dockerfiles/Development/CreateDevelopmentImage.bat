rd /s /q publish
cd ..\..\DockerDemoApi
dotnet clean
dotnet build
dotnet publish -o ..\..\Dockerfiles\Development\publish
cd ..\Dockerfiles\Development
docker build -t mswarbrick/dockerdemoapi:dev .