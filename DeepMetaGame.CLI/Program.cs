
using DeepEditorConsole;

class Program
{
    [STAThread]
    static int Main(string[] args)
    {
        if (args.Length > 1)
        {
            var cmd = args[0];
            if (cmd == "init")
            {
                init(args, new DirectoryInfo(args[1]));
            }
        }
        return 0;
    }

    static void init(string[] args, DirectoryInfo root)
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
            Exec.Run("git", "submodule add git@github.com:DeepMetaverseEngine/DeepMeta.git DeepMeta", SlnPath);
            Console.ResetColor();
        }
        else
        {
            Console.WriteLine($"Solution Folder : {SlnPath}");
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