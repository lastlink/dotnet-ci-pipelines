echo copy releases to upload
set folder=Release\post
echo %folder%
mkdir %folder%
echo %rIds%
for %%a in ("%rIds:,=" "%") do (
    echo running single copy
    echo %%~a
    copy "MyProject\bin\Release\net9.0\%%~a\publish\*" "%folder%\%%~a" /y
    copy "MyProject.Api\bin\Release\net9.0\%%~a\publish\*" "%folder%\%%~a" /y
)