
SET TargetDir=%1
rem md %~dp0..\..\..\Data\Tools\bin
xcopy /Y/E  %TargetDir%*   %~dp0..\..\..\Data\GameEditor\bin
rem echo [%~dp0..\..\..\Data\Tools\bin]
rem echo [%TargetDir%*.*]
