#!/bin/bash
echo ${rId}
echo "started"
echo ${TargetDir}

echo ${rId} build step
if [[ $rId = "win-x64" ]]
then
    echo ${rId} build step
fi
echo ${TargetName}
cp ../Release/pre/${TargetName}.dll ./obj/${TargetName}.dll
cp ../Release/pre/MyProject.Repository.dll ../MyProject.Repository/bin/Release/net9.0/${rId}/MyProject.Repository.dll
echo ":::end post build:::"
