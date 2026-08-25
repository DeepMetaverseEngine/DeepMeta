
using DeepCore;
using DeepCore.IO;
using DeepEditorConsole;

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
            Console.ForegroundColor = ConsoleColor.Green;
            Exec.Run("git", "init");
            Console.ResetColor();
        }
        if (!Directory.Exists(SlnPath))
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("### Make Solution folder ###");
            Console.WriteLine(SlnPath);
            Directory.CreateDirectory(SlnPath);
            Console.ResetColor();
        }
        else
        {
            Console.WriteLine($"Solution Folder : {SlnPath}");
        }
        if (!Directory.Exists(Path.Combine(SlnPath, "DeepMeta")))
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Exec.Run("git", $"submodule add {gitURL} DeepMeta", SlnPath);
            CFiles.ShellXCopy(root, $"{projName}SLN\\DeepMeta\\_Temp_*\\", $"{projName}SLN\\{projName}Src\\", "*.*");
            Console.ResetColor();
        }
        if (!Directory.Exists(UnityPath))
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("### Make Unity Project folder ###");
            Console.WriteLine(UnityPath);
            Directory.CreateDirectory(UnityPath);
            Console.ResetColor();
        }
        else
        {
            Console.WriteLine($"Unity Project Folder : {UnityPath}");
        }
        if (!Directory.Exists(GameEditorPath))
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("### Make Game Editor folder ###");
            Console.WriteLine(GameEditorPath);
            Directory.CreateDirectory(GameEditorPath);
            Console.ResetColor();
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