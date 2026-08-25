
using DeepCore;
using DeepCore.IO;
using DeepEditorConsole;
using System.Reflection.Metadata;
using System.Security.Cryptography;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

class Program
{
    static string DEFAULT_GIT_URL = "git@github.com:DeepMetaverseEngine/DeepMeta.git";

    static string USAGE = @"
Usage: DeepMetaGame.CLI <command> [options]
Commands:
  init               Initialize the project
Options:
  -git-url=<url>    Specify the Git URL for the submodule
  -root=<path>      Specify the root directory for the project
";

    [STAThread]
    static int Main(string[] args)
    {
        try
        {
            var pargs = Properties.ParseArgs(args);
            var root = new DirectoryInfo(".");
            var gitURL = DEFAULT_GIT_URL;
            if (pargs.TryGetValue("-git-url", out var _git))
            {
                gitURL = _git;
            }
            if (pargs.TryGetValue("-root", out var _root))
            {
                root = new DirectoryInfo(_root);
            }
            if (args.Length > 0)
            {
                var cmd = args[0];
                switch (cmd)
                {
                    case "init": return init(pargs, root, gitURL);
                    default:
                        Console.WriteLine($"Unknown command: {cmd}");
                        Console.WriteLine(USAGE);
                        return 0;
                }
            }
            Console.WriteLine(USAGE);
            return init(pargs, root, gitURL);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Exception: " + ex.Message);
            return -1;
        }
    }

    static int init(Properties pargs, DirectoryInfo root, string gitURL)
    {
        var projName = root.Name;
        Environment.CurrentDirectory = root.FullName;
        Console.WriteLine($"Current Path: {root.FullName}");
        Console.WriteLine($"Current Project Name: {projName}");

        var GitPath = Path.Combine(root.FullName, ".git");
        var SlnPath = Path.Combine(root.FullName, $"{projName}SLN");
        var UnityPath = Path.Combine(root.FullName, $"{projName}Unity");
        var GameEditorPath = Path.Combine(root.FullName, "GameEditor");
        if (!Directory.Exists(GitPath))
        {
            Exec.Run("git", "init");
        }
        if (!Directory.Exists(SlnPath))
        {
            Console.WriteLine("### Make Solution folder ###");
            Console.WriteLine(SlnPath);
            Directory.CreateDirectory(SlnPath);
        }
        else
        {
            Console.WriteLine($"Solution Folder : {SlnPath}");
        }
        var DeepMetaPath = Path.Combine(SlnPath, "DeepMeta");
        if (!Directory.Exists(DeepMetaPath))
        {
            Console.WriteLine("### Clone DeepMeta ###");
            Exec.Run("git", $"submodule add {gitURL} DeepMeta", SlnPath);
            Exec.Run("git", $"git pull \"origin\"  master:master", DeepMetaPath);
            Exec.Run("git", $"git lfs pull", DeepMetaPath);
        }
        var SrcPath = Path.Combine(SlnPath, $"{projName}Src");
        if (!Directory.Exists(SrcPath))
        {
            try
            {
                Console.WriteLine("### Copy Source Files ###");
                var temp_dirs = new DirectoryInfo(DeepMetaPath).GetDirectories();
                foreach (var dir in temp_dirs)
                {
                    if (dir.Name.StartsWith("_Temp_"))
                    {
                        var target_proj = $"{projName}SLN\\{projName}Src\\{dir.Name.Replace("_Temp_", projName)}";
                        CFiles.ShellXCopy(root, $"{projName}SLN\\DeepMeta\\{dir.Name}", target_proj);
                        var subfiles = new DirectoryInfo(target_proj).GetFiles("*", SearchOption.AllDirectories);
                        foreach (var sub in subfiles)
                        {
                            if (sub.Name.StartsWith("_Temp_"))
                            {
                                var dstname = sub.Name.Replace("_Temp_", projName);
                                Console.WriteLine($"    {sub.FullName} -> {dstname}");
                                CFiles.ShellRename(sub.Directory, sub.Name, dstname);
                                var dst = Path.Combine(sub.Directory.FullName, sub.Name.Replace("_Temp_", projName));
                                var content = Resource.LoadData(dst);
                                var text = CUtils.DecodeUTF8(content, out var encoding);
                                text = text.ReplaceAll("_Temp_", projName);
                                CFiles.WriteAllText(dst, text, encoding);
                            }
                            else if (sub.Name.EndsWith(".cs")
                                || sub.Name.EndsWith(".txt") 
                                || sub.Name.EndsWith(".bat") 
                                || sub.Name.EndsWith(".json") 
                                || sub.Name.EndsWith(".config"))
                            {
                                var content = Resource.LoadData(sub.FullName);
                                var text = CUtils.DecodeUTF8(content, out var encoding);
                                text = text.ReplaceAll("_Temp_", projName);
                                CFiles.WriteAllText(sub, text, encoding);
                            }
                        }

                    }
                }
            }
            catch (Exception err)
            {
                CFiles.Delete(SrcPath);
                Console.WriteLine($"Error: {err}");
            }
            //CFiles.ShellXCopy(root, $"{projName}SLN\\DeepMeta\\_Temp_*", $"{projName}SLN\\{projName}Src");
        }
        var SlnFilePath = Path.Combine(SlnPath, $"{projName}.slnx");
        if (!Directory.Exists(SlnFilePath))
        {
            var srcSLNX = Resource.LoadFromAssembly(typeof(Program), "_Temp_.slnx");
            var text = CUtils.DecodeUTF8(srcSLNX, out var encoding);
            text = text.ReplaceAll("_Temp_", projName);
            CFiles.WriteAllText(SlnFilePath, text, encoding);
        }
        if (!Directory.Exists(UnityPath))
        {
            Console.WriteLine("### Make Unity Project folder ###");
            Console.WriteLine(UnityPath);
            Directory.CreateDirectory(UnityPath);
        }
        else
        {
            Console.WriteLine($"Unity Project Folder : {UnityPath}");
        }
        if (!Directory.Exists(GameEditorPath))
        {
            Console.WriteLine("### Make Game Editor folder ###");
            Console.WriteLine(GameEditorPath);
            Directory.CreateDirectory(GameEditorPath);
        }
        else
        {
            Console.WriteLine($"Game Editor Folder : {GameEditorPath}");
        }
        return 0;
    }



}

/*
@echo off

cd %~dp0

echo Current Path: %~dp0
for %%I in (.) do set "DIR_NAME=%%~nxI"
echo Current Project Name: %DIR_NAME%


SET PROJECT_NAME=%DIR_NAME%

if not exist .git (

echo ----------------------------------------------------------------------
echo ### Make Solution folder ### 
git init
if not exist %PROJECT_NAME%SLN (
    md %PROJECT_NAME%SLN
)
cd %PROJECT_NAME%SLN

git submodule add git@github.com:DeepMetaverseEngine/DeepMeta.git DeepMeta

cd ..
)
echo ----------------------------------------------------------------------
echo ### Make Unity Project folder ### 
if not exist %PROJECT_NAME%Unity (
    md %PROJECT_NAME%Unity
)

echo ----------------------------------------------------------------------
echo ### Make Game Editor folder ### 
if not exist GameEditor (
    md GameEditor
)

echo ----------------------------------------------------------------------
pause
 */