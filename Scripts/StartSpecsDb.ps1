$checkContainerIsRunning = docker ps -f name=DockerDemoApiSpecsDb | sls DockerDemoApiSpecsDb

If($checkContainerIsRunning) {
    echo "Container is running"
}
else {
    echo "Container is not running"
    docker run -d -p 1433:1433 -e sa_password=1StrongPassword! -e ACCEPT_EULA=Y --name=DockerDemoApiSpecsDb microsoft/mssql-server-windows-developer:2017-GA
}

$specsDbExists = sqlcmd -S localhost,1433 -U SA -P '1StrongPassword!' -Q 'select NAME from sys.Databases' | sls DockerDemoApiSpecs
If($specsDbExists) {
    echo "Specs database already exists"
}
else {
    echo "Creating database"
    sqlcmd -S localhost,1433 -U SA -P '1StrongPassword!' -Q 'CREATE DATABASE DockerDemoApiSpecs'
}

$specsLoginExists = sqlcmd -S localhost,1433 -U SA -P '1StrongPassword!' -Q 'select name from sys.server_principals' | sls DockerDemoApiUser
If($specsLoginExists) {
    echo "Login DockerDemoApiUser already exists"
}
else {
    echo "Creating DockerDemoApiUser Login"
    sqlcmd -S localhost,1433 -U SA -P '1StrongPassword!' -Q 'CREATE LOGIN DockerDemoApiUser WITH PASSWORD = "2fc148d2-2e5b-4dca-9a39-7f69e3e09c42"'
}

$specsUserExists = sqlcmd -S localhost,1433 -U SA -P '1StrongPassword!' -Q 'use DockerDemoApiSpecs select name from sys.database_principals' | sls DockerDemoApiUser
If($specsUserExists) {
    echo "User DockerDemoApiUser already exists"
}
else {
    echo "Creating DockerDemoApiUser User for Login DockerDemoApiUser"
    sqlcmd -S localhost,1433 -U SA -P '1StrongPassword!' -Q 'use DockerDemoApiSpecs CREATE USER DockerDemoApiUser FOR LOGIN DockerDemoApiUser'
    sqlcmd -S localhost,1433 -U SA -P '1StrongPassword!' -Q "use DockerDemoApiSpecs EXEC sys.sp_addrolemember 'db_owner', 'DockerDemoApiUser'"
}

echo "DockerDemoApiSpecs SQL server is running and available at localhost port 1433 u: DockerDemoApiUser p: 2fc148d2-2e5b-4dca-9a39-7f69e3e09c42"
echo "Applying Migrations..."

cd ..\DockerDemoApi\DockerDemoApi.Migrations\
dotnet build
dotnet run "Server=localhost,1433;Database=DockerDemoApiSpecs;User Id=DockerDemoApiUser;Password=2fc148d2-2e5b-4dca-9a39-7f69e3e09c42"

echo "Exiting"
cd ..\..\Scripts