@echo off
cd /d "C:\Users\pollo\Documents\01. Universidad\02. Progra Avanzada en Web\PAW"

REM Abrir PowerShell con PAW.API
start powershell -NoExit -Command "cd 'C:\Users\pollo\Documents\01. Universidad\02. Progra Avanzada en Web\PAW'; dotnet run --project PAW.API/PAW.API.csproj"

REM Esperar 8 segundos (más tiempo para compilar)
timeout /t 8 /nobreak

REM Abrir PowerShell con PAW.Web
start powershell -NoExit -Command "cd 'C:\Users\pollo\Documents\01. Universidad\02. Progra Avanzada en Web\PAW'; dotnet run --project PAW.Web/PAW.Web.csproj"

REM Esperar 8 segundos más (para que PAW.Web esté completamente listo)
timeout /t 8 /nobreak

REM Abrir el navegador en HTTP (más simple)
start http://localhost:5252

exit
