@chcp 65001
@echo Date: %1
@echo copy target to "..\..\..\Platform\Android\bin\"
@copy /Y   ".\bin\unityplugin.jar"                 "..\..\..\Platform\Android\bin\"
@copy /Y   ".\libs\armeabi\libUnityPlugin.so"      "..\..\..\Platform\Android\bin\armeabi"
@copy /Y   ".\libs\armeabi-v7a\libUnityPlugin.so"  "..\..\..\Platform\Android\bin\armeabi-v7a"
@echo done