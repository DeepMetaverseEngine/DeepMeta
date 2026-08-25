using DeepCore;
using DeepCore.IO;
using DeepCore.Reflection;

Console.OutputEncoding = System.Text.Encoding.UTF8;

var clientDLL = new string[] {
    "DeepCore.dll",
    "DeepCore.Event.dll",
    "DeepCore.GUI.dll",
    "DeepMetaGame.Data.dll" ,
    "DeepMetaGame.Host.dll",
    "Gate.Data.dll" ,
    "_Temp_.Battle.dll" ,
    "_Temp_.Protocol.dll"
};

var serverDLL = CUtils.ArrayAppend(clientDLL, new string[] {
    "DeepCrystal.dll" ,
    "DeepCore.EventDebug.dll",
});

var GenClient = "_Temp_.Codec";
var GenClientNS = "_Temp_.Codec";
ReflectionUtil.LoadDlls();
try
{
    if (null == DeepTools.CodeGen.Program.TryGenSimple_REF(GenClient, GenClientNS, clientDLL))
    {
        return -1;
    }
    if (null == DeepTools.CodeGen.Program.TryGenSimple_MSG(GenClient, GenClientNS, clientDLL))
    {
        return -1;
    }
    if (DeepTools.CodeGen.Program.TryFindSolutionProjectDir(GenClient, out var pdir))
    {
        CFiles.Delete(Path.Combine(pdir.FullName, "gen.error.cs"));
    }

}
catch (Exception err)
{
    if (DeepTools.CodeGen.Program.TryFindSolutionProjectDir(GenClient, out var pdir))
    {
        CFiles.WriteAllText(Path.Combine(pdir.FullName, "gen.error.cs"), err.Message);
    }
    else
    {
        CFiles.WriteAllText(Path.Combine(Environment.CurrentDirectory, "gen.error.cs"), err.Message);
    }
    return -1;
}

return 0;

