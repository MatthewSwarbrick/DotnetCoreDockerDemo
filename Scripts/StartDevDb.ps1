$checkContainerIsRunning = docker ps -f name=DockerDemoApiSpecsDb | sls DockerDemoApiSpecsDb

If($checkContainerIsRunning) {
    echo "Container is running"
}
else {
    echo "Container is not running"
    docker run -d -p 1433:1433 -e sa_password=1StrongPassword! -e ACCEPT_EULA=Y --name=DockerDemoApiSpecsDb microsoft/mssql-server-windows-developer:2017-GA
}

$devDbExists = sqlcmd -S localhost,1433 -U SA -P '1StrongPassword!' -Q 'select NAME from sys.Databases' | sls DockerDemoApiDev
If($devDbExists) {
    echo "Dev database already exists"
}
else {
    echo "Creating database"
    sqlcmd -S localhost,1433 -U SA -P '1StrongPassword!' -Q 'CREATE DATABASE DockerDemoApiDev'
}

$specsLoginExists = sqlcmd -S localhost,1433 -U SA -P '1StrongPassword!' -Q 'select name from sys.server_principals' | sls DockerDemoApiUser
If($specsLoginExists) {
    echo "Login DockerDemoApiUser already exists"
}
else {
    echo "Creating DockerDemoApiUser Login"
    sqlcmd -S localhost,1433 -U SA -P '1StrongPassword!' -Q 'CREATE LOGIN DockerDemoApiUser WITH PASSWORD = "2fc148d2-2e5b-4dca-9a39-7f69e3e09c42"'
}

$devUserExists = sqlcmd -S localhost,1433 -U SA -P '1StrongPassword!' -Q 'use DockerDemoApiDev select name from sys.database_principals' | sls DockerDemoApiUser
If($devUserExists) {
    echo "User DockerDemoApiUser already exists"
}
else {
    echo "Creating DockerDemoApiUser User for Login DockerDemoApiUser"
    sqlcmd -S localhost,1433 -U SA -P '1StrongPassword!' -Q 'use DockerDemoApiDev CREATE USER DockerDemoApiUser FOR LOGIN DockerDemoApiUser'
    sqlcmd -S localhost,1433 -U SA -P '1StrongPassword!' -Q "use DockerDemoApiDev EXEC sys.sp_addrolemember 'db_owner', 'DockerDemoApiUser'"
}

echo "DockerDemoApiDev SQL server is running and available at localhost port 1433 u: DockerDemoApiUser p: 2fc148d2-2e5b-4dca-9a39-7f69e3e09c42"
echo "Applying Migrations..."

cd ..\DockerDemoApi\DockerDemoApi.Migrations\
dotnet build
dotnet run "Server=localhost,1433;Database=DockerDemoApiDev;User Id=DockerDemoApiUser;Password=2fc148d2-2e5b-4dca-9a39-7f69e3e09c42"

echo "Exiting"
cd ..\..\Scripts