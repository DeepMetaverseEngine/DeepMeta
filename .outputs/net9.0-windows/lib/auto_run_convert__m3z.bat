
@echo ---------------------------------------------------------
@echo - 资源转换
@echo ---------------------------------------------------------
@SET RES_DIR=%1\.
@SET FILTER_GUI=%2
@SET FILTER_M3Z=%3
@SET REBUILD="0"
@SET RM_TEMP=-rt:true
@SET PVR_LOG=-h:".png>.pvr.m3z|.jpg>.pvr.m3z"
@SET ETC_LOG=-h:".png>.etc.m3z|.jpg>.etc.m3z"
@SET DXT_LOG=-h:".png>.dxt.m3z|.jpg>.dxt.m3z"
@SET G3Z_LOG=-h:".m3z>.g3z"

@SET REBUILD="0"
@SET RM_TEMP=

@echo ---------------------------------------------------------
@echo - 创建所有CPJ BIN文件
@echo ---------------------------------------------------------
@java -client -Xmx1000m -cp ./*.jar;g2d_studio.jar CellResourceXmlToBin %RES_DIR% %FILTER_GUI% ""

@echo ---------------------------------------------------------
@echo - 创建所有UI BIN文件
@echo ---------------------------------------------------------
@gui xml2bin %RES_DIR%\UIEdit

@echo ---------------------------------------------------------
@echo - 创建所有M3Z文件，只更新差异化图片
@echo ---------------------------------------------------------
@echo RES_DIR=%RES_DIR%
@call %~dp0\libs.bat

@java -classpath g2d_studio.jar -Xmx1000m M3ZConvert %RM_TEMP% -d:%RES_DIR% -w:%~dp0\bin -c:m3z_cmdline.txt -e:.pvr.m3z %PVR_LOG% -p:%FILTER_M3Z% -f:ASTC -pow
@java -classpath g2d_studio.jar -Xmx1000m M3ZConvert %RM_TEMP% -d:%RES_DIR% -w:%~dp0\bin -c:m3z_cmdline.txt -e:.etc.m3z %ETC_LOG% -p:%FILTER_M3Z% -f:ETC2 -yflip:t -pow
@rem @java -classpath g2d_studio.jar -Xmx1000m M3ZConvert %RM_TEMP% -d:%RES_DIR% -w:%~dp0\bin -c:m3z_cmdline.txt -e:.dxt.m3z %DXT_LOG% -p:%FILTER_M3Z% -f:DXT3 -pow:t 

@echo ----------------------------------------------------------------
@echo nicely done !
@echo ----------------------------------------------------------------
