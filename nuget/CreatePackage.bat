@echo off
SETLOCAL EnableDelayedExpansion
for /F "tokens=1,2 delims=#" %%a in ('"prompt #$H#$E# & echo on & for %%b in (1) do rem"') do (
  set "DEL=%%a"
)

REM md "%~dp0\lib" 2>nul
REM copy "%~dp0..\Utilities\bin\Release\Structura.Shared.Utilities.dll" "%~dp0lib\"
REM if ERRORLEVEL 1 goto :eof

"%~dp0\nuget" pack -Prop Configuration=Release

md "%~dp0NuGetPackages" 2>nul
for %%i in ("*.nupkg") do if exist "%~dp0NuGetPackages\%%i" (
	call :errorExists %%i
	goto :eof
)


MOVE *.nupkg "%~dp0NuGetPackages"
goto :eof

:errorExists
echo[
echo(
call :ColorText 0C "Package %1 already exists, possibly the version must be increased..."
del "%~dp0*.nupkg"
echo[
echo[
goto :eof


:ColorText
<nul set /p ".=%DEL%" > "%~2"
findstr /v /a:%1 /R "^$" "%~2" nul
del "%~2" > nul 2>&1


pause

