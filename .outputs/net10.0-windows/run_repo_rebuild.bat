@echo off
@echo Rebuild All CPJ
@SET cell=C:\"Program Files (x86)"\Cell\CellGameEditor\CellGameOutput.exe
@FOR /R . %%i IN (*.cpj) DO @%cell% %%i addimg -repo="*.repo" -remove:true
@echo Done!
@pause