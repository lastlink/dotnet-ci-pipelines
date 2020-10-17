#!/bin/bash
echo copy releases to upload
folder=Release/post
echo ${folder}
mkdir -p ${folder}
echo $rIds
array=(${rIds//,/ })
for i in "${!array[@]}"
do
    echo "$i=>${array[i]}"
    cp MyProject/bin/Release/netcoreapp3.1/${array[i]}/publish/* ${folder}/${array[i]}
    cp MyProject.Api/bin/Release/netcoreapp3.1/${array[i]}/publish/* ${folder}/${array[i]}
done