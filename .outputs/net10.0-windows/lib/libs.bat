@echo WORKING DIR : %~dp0
@set J2D_TRACE_LEVEL=4
@set PATH=%~dp0.\jre\bin;%PATH%
@set PATH=%~dp0.\bin;%PATH%
@set PATH=%~dp0.;%PATH%
@set G2DLIB=%~dp0.
@set G2DJAR=%~dp0.\g2d_studio.jar
@set JARS=mina-core.jar;slf4j-api.jar;slf4j-simple.jar;xpp3_min.jar;xstream.jar;jxl.jar;joal.jar;gluegen-rt.jar;jogg.jar;jorbis.jar;jcsv-1.4.0.jar;king3_plugin.jar;g2d_studio.jar
@set CLASSPATH=%~dp0.;%CLASSPATH%
