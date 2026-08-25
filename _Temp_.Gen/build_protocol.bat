
SET BD=%~dp0\bin\Debug\net6.0
SET TD=%~dp0\..\Gate.ServerLauncher\bin\Debug\netstandard2.0
SET PD=%~dp0\..\Gate.Test.Codec\

echo -------------------------------------------------------------------------------------------------------------------------------------------
set gen_ref=%TD%\DeepCore.dll
set gen_ref=%gen_ref%;%TD%\Gate.Data.dll
del /Q %PD%\gen_client\msg\*.cs
%BD%\codegen -ns:Gate.Data -wd:%TD% -if:%gen_ref% -od:%PD%\gen_client\msg -t:csharp-code.xml 
%BD%\codegen -ns:Gate.Data -wd:%TD% -if:%gen_ref% -od:%PD%\gen_client\msg -t:csharp-code-meta.xml   -of:%PD%\gen_client\msg\meta.cs            
%BD%\codegen -ns:Gate.Data -wd:%TD% -if:%gen_ref% -od:%PD%\gen_client\msg -t:json-response-code.xml -of:%PD%\gen_client\msg\response-code.json 

echo -------------------------------------------------------------------------------------------------------------------------------------------
set gen_ref=%TD%\DeepCore.dll
set gen_ref=%gen_ref%;%TD%\Gate.Data.dll
set gen_ref=%gen_ref%;%TD%\Gate.Server.dll
del /Q %PD%\gen_server\msg\*.cs
%BD%\codegen -ns:Gate.Server -wd:%TD% -if:%gen_ref% -od:%PD%\gen_server\msg -t:csharp-code.xml 
%BD%\codegen -ns:Gate.Server -wd:%TD% -if:%gen_ref% -od:%PD%\gen_server\msg -t:csharp-code-meta.xml   -of:%PD%\gen_server\msg\meta.cs            
%BD%\codegen -ns:Gate.Server -wd:%TD% -if:%gen_ref% -od:%PD%\gen_server\msg -t:json-response-code.xml -of:%PD%\gen_server\msg\response-code.json 

echo -------------------------------------------------------------------------------------------------------------------------------------------
