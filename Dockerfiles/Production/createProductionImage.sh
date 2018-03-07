#!/bin/bash

echo Removing existing published files
rm -rf publish
cd ../../DockerDemoApi

echo Publishing docker demo api
dotnet clean
dotnet build
dotnet publish -o ../../Dockerfiles/Production/publish
cd ..\Dockerfiles\Production

echo Building production image
docker build -t mswarbrick/dockerdemoapi:prod .