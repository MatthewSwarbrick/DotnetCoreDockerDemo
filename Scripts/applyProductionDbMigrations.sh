#!/bin/bash

echo Applying Migrations...
cd ../DockerDemoApi/DockerDemoApi.Migrations/bin/Debug/netcoreapp2.0/ && dotnet DockerDemoApi.Migrations.dll "Server=dockerdemosqlserver.database.windows.net,1433;Database=DockerDemoApiDbProd;User Id=DockerDemoApiUser;Password=e25a01da-4093-4cd3-8664-ea156c04c9b0"

echo Exiting
exit 0