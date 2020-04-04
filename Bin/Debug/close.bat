echo "close server ..."

taskkill /f /t /im CenterServer.exe
taskkill /f /t /im LoginServer.exe
taskkill /f /t /im GateServer.exe
taskkill /f /t /im SSCombatServer.exe

ping -n 1 127.0 >nul

echo "close server finish"

pause