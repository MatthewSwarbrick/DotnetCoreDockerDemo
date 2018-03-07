#!/bin/bash

docker pull microsoft/mssql-server-linux:2017-GA

if [ ! "$(docker ps -q -f name=DockerDemoApiTest)" ]; then
    echo DockerDemoApiTest Container is not running
    if [ "$(docker ps -aq -f status=exited -f name=DockerDemoApiTest)" ]; then
        docker rm DockerDemoApiTest
    fi
    echo Starting mssql-server-linux container image
    docker run -e 'ACCEPT_EULA=Y' -e 'MSSQL_SA_PASSWORD=<YourStrong!Passw0rd>' \
       -p 1433:1433 --name DockerDemoApiTest \
       -v  /data/mssql/log:/var/opt/mssql \
       -d microsoft/mssql-server-linux:2017-GA
else
    echo DockerDemoApiTest container already running
fi

specsDbExists="$(/opt/mssql-tools/bin/sqlcmd -S localhost,1433 -U SA -P '<YourStrong!Passw0rd>' -Q 'select NAME from sys.Databases' | grep DockerDemoApiSpecs)"
if [ ! $specsDbExists ]; then
    echo Creating database DockerDemoApiSpecs
    /opt/mssql-tools/bin/sqlcmd -S localhost,1433 -U SA -P '<YourStrong!Passw0rd>' -Q 'CREATE DATABASE DockerDemoApiSpecs'
fi

specsLoginExists="$(/opt/mssql-tools/bin/sqlcmd -S localhost,1433 -U SA -P '<YourStrong!Passw0rd>' -Q 'select name from sys.server_principals' | grep DockerDemoApiUser)"
if [ ! $specsLoginExists ]; then
    echo Creating Login DockerDemoApiUser
    /opt/mssql-tools/bin/sqlcmd -S localhost,1433 -U SA -P '<YourStrong!Passw0rd>' -Q 'CREATE LOGIN DockerDemoApiUser WITH PASSWORD = '"'"'2fc148d2-2e5b-4dca-9a39-7f69e3e09c42'"'"''
fi

specsUserExists="$(/opt/mssql-tools/bin/sqlcmd -S localhost,1433 -U SA -P '<YourStrong!Passw0rd>' -Q 'use DockerDemoApiSpecs select name from sys.database_principals' | grep DockerDemoApiUser)"
if [ ! $specsUserExists ]; then
    echo Creating User for Login DockerDemoApiUser
    /opt/mssql-tools/bin/sqlcmd -S localhost,1433 -U SA -P '<YourStrong!Passw0rd>' -Q 'use DockerDemoApiSpecs CREATE USER DockerDemoApiUser FOR LOGIN DockerDemoApiUser'
    /opt/mssql-tools/bin/sqlcmd -S localhost,1433 -U SA -P '<YourStrong!Passw0rd>' -Q 'use DockerDemoApiSpecs EXEC sys.sp_addrolemember '"'"'db_owner'"'"', '"'"'DockerDemoApiUser'"'"''
fi

echo DockerDemoApiSpecs is running and available at localhost port 1433 u: DockerDemoApiUser p: 2fc148d2-2e5b-4dca-9a39-7f69e3e09c42
echo Applying Migrations...
cd ../DockerDemoApi/DockerDemoApi.Migrations/bin/Debug/netcoreapp2.0/ && dotnet DockerDemoApi.Migrations.dll "Server=localhost,1433;Database=DockerDemoApiSpecs;User Id=DockerDemoApiUser;Password=2fc148d2-2e5b-4dca-9a39-7f69e3e09c42"

echo Exiting
exit 0
