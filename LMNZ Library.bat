@echo off
echo ==============================================
echo       LMNZ LIBRARY IS NOW OPENING
echo ==============================================
echo [1/2] Turning on the API Server...
start /min cmd /c "title API_Server_Libreria && dotnet run --project "%~dp0LibraryAPI.csproj""

echo In attesa che il server sia pronto (3 secondi)...
timeout /t 4 /nobreak >nul

echo [2/2] Opening the APP (CLI)...
"%~dp0LibraryCLI\bin\Release\net10.0\win-x64\publish\LibraryCLI.exe"

echo.
echo ==============================================
echo                  bye bye...
echo ==============================================
taskkill /FI "WINDOWTITLE eq API_Server_Libreria*" /T /F >nul 2>&1
