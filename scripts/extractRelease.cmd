@REM copy dlls wanted to artifact folder
set folder=Release\pre
echo %folder%
mkdir %folder%

.\tools\obfuscar.console .\scripts\obfuscator\win\obfuscar-console.xml

copy "MyProject\bin\Release\netcoreapp3.1\publish\ReadyForDeployment\MyProject.dll" "%folder%\MyProject.dll" /y
@REM copy "MyProject.Api\bin\Release\netcoreapp3.1\publish\ReadyForDeployment\MyProject.Api.dll" "%folder%\MyProject.Api.dll" /y
@REM copy "MyProject.Repository\bin\Release\netcoreapp3.1\ReadyForDeployment\MyProject.Repository.dll" "%folder%\MyProject.Repository.dll" /y