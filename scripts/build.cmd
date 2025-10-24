echo %rId%

echo '%rId% build step'
if '%rId%' == 'win-x64' (
    echo '%rId% build step'
)
echo %TargetName%
copy "..\Release\pre\%TargetName%.dll" ".\obj\%TargetName%.dll" /y
copy "..\Release\pre\MyProject.Repository.dll" "..\MyProject.Repository\bin\Release\net9.0\%rId%\MyProject.Repository.dll" /y
echo ":::end post build:::"