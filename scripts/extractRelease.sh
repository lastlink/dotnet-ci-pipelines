#!/bin/bash
# copy dlls wanted to artifact folder
folder=Release/pre
echo ${folder}
mkdir -p ${folder}

./tools/obfuscar.console ./scripts/obfuscator/unix/obfuscar-console.xml

cp MyProject/bin/Release/netcoreapp3.1/publish/ReadyForDeployment/MyProject.dll ${folder}
# cp MyProject.Api/bin/Release/netcoreapp3.1/publish/ReadyForDeployment/MyProject.Api.dll ${folder}
# cp MyProject.Repository/bin/Release/netcoreapp3.1/ReadyForDeployment/MyProject.Repository.dll ${folder}