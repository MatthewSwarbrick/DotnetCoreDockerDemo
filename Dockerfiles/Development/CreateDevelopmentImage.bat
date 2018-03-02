cd ..\..\DockerDemoApi
dotnet publish -o ..\..\Dockerfiles\Development\publish
cd ..\Dockerfiles\Development
docker build -t mswarbrick/dockerdemoapi .