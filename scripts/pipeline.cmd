echo running build pipeline
echo %rIds%
for %%a in ("%rIds:,=" "%") do (
    echo running single build
    echo %%~a
    echo :::::dotnet publish -c Release -r %%~a -p:PublishSingleFile=true -p:PublishTrimmed=true --self-contained true /p:rId=%%~a:::::
    dotnet publish -c Release -r %%~a -p:PublishSingleFile=true -p:PublishTrimmed=true --self-contained true /p:rId=%%~a
)

echo pipeline finished