$devDbExists = sqlcmd -S mssql -U SA -P '1Strongpassword!' -Q 'select NAME from sys.Databases' | sls DockerDemoApiDev
If($devDbExists) {
    echo "Dev database already exists"
}
else {
    echo "Creating database"
    sqlcmd -S mssql -U SA -P '1Strongpassword!' -Q 'CREATE DATABASE DockerDemoApiDev'
}

$loginExists = sqlcmd -S mssql -U SA -P '1Strongpassword!' -Q 'select name from sys.server_principals' | sls DockerDemoApiUser
If($loginExists) {
    echo "Login DockerDemoApiUser already exists"
}
else {
    echo "Creating DockerDemoApiUser Login"
    sqlcmd -S mssql -U SA -P '1Strongpassword!' -Q 'CREATE LOGIN DockerDemoApiUser WITH PASSWORD = "f1719ef5-cde1-463f-a0a8-b1d8baa0c26d"'
}

$devUserExists = sqlcmd -S mssql -U SA -P '1Strongpassword!' -Q 'use DockerDemoApiDev select name from sys.database_principals' | sls DockerDemoApiUser
If($devUserExists) {
    echo "User DockerDemoApiUser already exists"
}
else {
    echo "Creating DockerDemoApiUser User for Login DockerDemoApiUser"
    sqlcmd -S mssql -U SA -P '1Strongpassword!' -Q 'use DockerDemoApiDev CREATE USER DockerDemoApiUser FOR LOGIN DockerDemoApiUser'
    sqlcmd -S mssql -U SA -P '1Strongpassword!' -Q "use DockerDemoApiDev EXEC sys.sp_addrolemember 'db_owner', 'DockerDemoApiUser'"
}

echo "Complete"