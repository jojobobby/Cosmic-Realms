#NoEnv  ; Recommended for performance and compatibility with future AutoHotkey releases.
; #Warn  ; Enable warnings to assist with detecting common errors.
SendMode Input  ; Recommended for new scripts due to its superior speed and reliability.
SetWorkingDir %A_ScriptDir%  ; Ensures a consistent starting directory.
run %A_ScriptDir%\Server-src\bin\RemoteLogger.exe, %A_ScriptDir%\Server-src\bin
run %A_ScriptDir%\Server-src\Redis-x64-3.2.100\redis-server.exe, %A_ScriptDir%\Server-src\Redis-x64-3.2.100
sleep 1000
run %A_ScriptDir%\Server-src\bin\wServer.exe, %A_ScriptDir%\Server-src\bin
run *runas %A_ScriptDir%\Server-src\bin\server.exe, %A_ScriptDir%\Server-src\bin