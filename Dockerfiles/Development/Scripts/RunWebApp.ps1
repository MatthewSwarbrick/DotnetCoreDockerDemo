cd C:\\app
dotnet DockerDemoApi.Migrations.dll "Server=mssql;Database=DockerDemoApiDev;User Id=DockerDemoApiUser;Password=f1719ef5-cde1-463f-a0a8-b1d8baa0c26d"

while($LastExitCode -ne 0) {
    echo "Migrations failed - trying again in 10 seconds"
    Start-Sleep -Seconds 10
    dotnet DockerDemoApi.Migrations.dll "Server=mssql;Database=DockerDemoApiDev;User Id=DockerDemoApiUser;Password=f1719ef5-cde1-463f-a0a8-b1d8baa0c26d"
    echo $LastExitCode
}

dotnet DockerDemoApi.dll