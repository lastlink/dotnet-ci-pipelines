#!/bin/bash
echo $rIds
array=(${rIds//,/ })
for i in "${!array[@]}"
do
    echo "$i=>${array[i]}"
    echo :::::dotnet publish -c Release -r ${array[i]} -p:PublishSingleFile=true -p:PublishTrimmed=true --self-contained true /p:rId=${array[i]}:::::
    dotnet publish -c Release -r ${array[i]} -p:PublishSingleFile=true -p:PublishTrimmed=true --self-contained true /p:rId=${array[i]}
done