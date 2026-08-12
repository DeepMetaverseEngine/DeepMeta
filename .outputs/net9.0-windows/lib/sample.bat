
@call %~dp0\libs.bat

java -classpath g2d_studio.jar -Xmx1000m CellResourceXmlToBin
@echo ------------------------------------------------------------------------------------------------
java -classpath g2d_studio.jar -Xmx1000m M3ZConvert
@echo ------------------------------------------------------------------------------------------------
java -classpath g2d_studio.jar -Xmx1000m ZipFiles
@echo ------------------------------------------------------------------------------------------------

cmd