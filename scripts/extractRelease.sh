#!/bin/bash
# copy dlls wanted to artifact folder
folder=Release/pre
echo ${folder}
mkdir -p ${folder}

./tools/obfuscar.console ./scripts/obfuscator/unix/obfuscar-Api.xml ./scripts/obfuscator/unix/obfuscar-console.xml ./scripts/obfuscator/unix/obfuscar-Repository.xml

cp MyProject/bin/Release/netcoreapp2.0/publish/ReadyForDeployment/MyProject.dll ${folder}
cp MyProject.Api/bin/Release/netcoreapp2.2/publish/ReadyForDeployment/MyProject.Api.dll ${folder}
cp MyProject.Repository/bin/Release/netcoreapp2.2/publish/ReadyForDeployment/MyProject.Repository.dll ${folder}