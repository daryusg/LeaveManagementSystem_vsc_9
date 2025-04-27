@echo off
setlocal

REM Set the path to the publish directory
set PUBLISH_DIR=publish

REM Delete the publish directory if it exists
if exist "%PUBLISH_DIR%" (
    echo Deleting existing %PUBLISH_DIR% directory...
    rmdir /s /q "%PUBLISH_DIR%"
)

REM Recreate the publish directory
echo Creating %PUBLISH_DIR% directory...
mkdir "%PUBLISH_DIR%"

REM Publish the app
echo Publishing the app...
dotnet publish -c Release /p:EnvironmentName=Production -o "%PUBLISH_DIR%"

echo Done.
endlocal
pause
