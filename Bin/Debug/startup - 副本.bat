echo "close pre servers ..."

taskkill /f /t /im CenterServer.exe
taskkill /f /t /im LoginServer.exe
taskkill /f /t /im GateServer.exe
::taskkill /f /t /im SSCombatServer.exe

echo "close pre finish"

echo "start all servers ..."

ping -n 1 127.0 >nul
start "center server" "CenterServer.exe"

ping -n 1 127.0 >nul
start "login server" "LoginServer.exe"

::ping -n 1 127.0 >nul
::start "scene combat server" "SSCombatServer.exe"

ping -n 1 127.0 >nul
start "gate server" "GateServer.exe"

echo "start all servers finish"

