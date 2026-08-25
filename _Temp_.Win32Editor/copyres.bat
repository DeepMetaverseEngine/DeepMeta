@echo off

SET TargetDir=%1

md %~dp0..\..\..\GameEditor\bin

xcopy /Y/E  %TargetDir%\*   %~dp0..\..\..\GameEditor\bin

exit 0
