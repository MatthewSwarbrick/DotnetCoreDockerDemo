#!/bin/bash
echo Installing required packages
sudo apt-get update
sudo apt-get -y install \
    apt-transport-https \
    ca-certificates \
    curl \
    software-properties-common

echo Installing dotnet core sdk 2.1.4
curl https://packages.microsoft.com/keys/microsoft.asc | gpg --dearmor > microsoft.gpg
sudo mv microsoft.gpg /etc/apt/trusted.gpg.d/microsoft.gpg
sudo sh -c 'echo "deb [arch=amd64] https://packages.microsoft.com/repos/microsoft-ubuntu-trusty-prod trusty main" > /etc/apt/sources.list.d/dotnetdev.list'
sudo apt-get update
sudo apt-get -y install dotnet-sdk-2.1.4
dotnet --version
cd /usr/share/dotnet && sudo find / -xdev 2>/dev/null -name "Microsoft.Docker.Sdk" /usr/share/dotnet/sdk/2.1.4/Sdks/Microsoft.Docker.Sdk
export DOTNET_CLI_TELEMETRY_OPTOUT=1

echo Install sqlcmd
curl https://packages.microsoft.com/keys/microsoft.asc | sudo apt-key add -
curl https://packages.microsoft.com/config/ubuntu/14.04/prod.list | sudo tee /etc/apt/sources.list.d/msprod.list
sudo apt-get update 
sudo ACCEPT_EULA=Y apt-get -y install mssql-tools msodbcsql

echo 'export PATH="$PATH:/opt/mssql-tools/bin"' >> ~/.bash_profile
echo 'export PATH="$PATH:/opt/mssql-tools/bin"' >> ~/.bashrc
source ~/.bashrc

sudo locale-gen en_US.UTF-8

echo Exiting
exit 0
