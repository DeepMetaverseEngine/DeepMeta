@echo off
@echo Rebuild All CPJ
@SET cell=C:\"Program Files (x86)"\Cell\CellGameEditor\CellGameOutput.exe
@FOR /R . %%i IN (*.cpj) DO @%cell% %%i expimg -repo="*.repo"
@echo Done!
@pause