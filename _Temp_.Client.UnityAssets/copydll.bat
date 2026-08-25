@echo off

SET TargetDir=%1

SET SRC_ASSETS1=%~dp0..\..\DeepMeta\DeepCore.Unity.Assets
SET SRC_ASSETS2=%~dp0..\..\DeepMeta\DeepMetaGame.UnityAssets

SET DST_ASSETS=%~dp0..\..\..\ScopeUnity\Assets
SET DST_RUNTIME=%~dp0..\..\..\GameEditor\_Temp_Unity\_Temp__Data\Managed

rem echo ----------------------------------------------------------------------------------
echo - AOT Assets TO: %DST_ASSETS%
rem xcopy /Y/E    %SRC_ASSETS1%\Assets\*    %DST_ASSETS%\
rem xcopy /Y/E    %SRC_ASSETS2%\Assets\*    %DST_ASSETS%\

echo ----------------------------------------------------------------------------------
echo - AOT DLL TO: %DST_ASSETS%\Plugins\Library\
rem xcopy /Y/E /EXCLUDE:%~dp0copydll.exclude.unity.txt    %TargetDir%*.dll    %DST_ASSETS%\Plugins\Library\
rem xcopy /Y/E /EXCLUDE:%~dp0copydll.exclude.unity.txt    %TargetDir%*.pdb    %DST_ASSETS%\Plugins\Library\

echo ----------------------------------------------------------------------------------
if EXIST %DST_RUNTIME% (
echo - RUNTIME £º¸´ÖÆDLLµ½Runtime
rem xcopy /Y/E /EXCLUDE:%~dp0copydll.exclude.unity.txt    %TargetDir%*.dll    %DST_RUNTIME%
rem del /F %DST_RUNTIME%\DeepCore.UnityEditor.dll
)
echo ----------------------------------------------------------------------------------
rem xcopy /Y/E /EXCLUDE:%~dp0\copydll.exclude.txt   %TargetDir%\*.dll       %~dp0\..\..\..\UnityRogueRun\Assets\Plugins\Import\
rem xcopy /Y/E /EXCLUDE:%~dp0\copydll.exclude.txt   %TargetDir%\*.pdb       %~dp0\..\..\..\UnityRogueRun\Assets\Plugins\Import\
rem xcopy /Y/E /EXCLUDE:%~dp0\copydll.exclude.txt   %TargetDir%\*.dll       %~dp0\..\..\..\UnityRogueArt\Assets\Plugins\Import\
rem xcopy /Y/E /EXCLUDE:%~dp0\copydll.exclude.txt   %TargetDir%\*.pdb       %~dp0\..\..\..\UnityRogueArt\Assets\Plugins\Import\
rem xcopy /Y/E /EXCLUDE:%~dp0\copydll.exclude.txt   %TargetDir%\*.dll       %~dp0\..\..\..\Data\GameEditor\UnityRun\RogueRuntime_Data\Managed\
