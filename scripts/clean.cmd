echo "cleaning dlls"
echo %TargetDir%
Rmdir /S "obj\Release"  /Q
Rmdir /S "..\MyProject.Repository\obj\Release"  /Q
echo "end clean"