@md c:\dtools
@xcopy /s /e /y %~dp0*.* c:\dtools

@reg add HKEY_CLASSES_ROOT\*\shell\aopen_mpq /ve /d "´ò¿ªMPQ" /f
@reg add HKEY_CLASSES_ROOT\*\shell\aopen_mpq\command /ve /d "c:\dtools\mpq.exe \"%%%L\"" /f

@reg add HKEY_CLASSES_ROOT\*\shell\extract_mpq /ve /d "½âÑ¹ËõMPQ" /f
@reg add HKEY_CLASSES_ROOT\*\shell\extract_mpq\command /ve /d "c:\dtools\mpq.exe E \"%%%L\"" /f

@pause