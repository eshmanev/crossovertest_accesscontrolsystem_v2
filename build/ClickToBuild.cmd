FOR %%b in ( 
       "%VS140COMNTOOLS%..\..\VC\vcvarsall.bat"
       "%ProgramFiles(x86)%\Microsoft Visual Studio 14.0\VC\vcvarsall.bat"
       "%ProgramFiles%\Microsoft Visual Studio 14.0\VC\vcvarsall.bat" 

       "%VS120COMNTOOLS%..\..\VC\vcvarsall.bat"
       "%ProgramFiles(x86)%\Microsoft Visual Studio 12.0\VC\vcvarsall.bat"
       "%ProgramFiles%\Microsoft Visual Studio 12.0\VC\vcvarsall.bat" 

       "%VS110COMNTOOLS%..\..\VC\vcvarsall.bat"
       "%ProgramFiles(x86)%\Microsoft Visual Studio 11.0\VC\vcvarsall.bat"
       "%ProgramFiles%\Microsoft Visual Studio 11.0\VC\vcvarsall.bat" 
    ) do (
    if exist %%b ( 
       call %%b x86
       goto build
    )
)
  
echo "Unable to detect suitable environment. Build may not succeed."

:build

SET target=%1
SET project=%2

IF "%target%" == "" SET target=Build
IF "%project%" =="" SET project=build.proj

msbuild /t:%target% %project%

pause

