
class Program
{
    [STAThread]
    static int Main(string[] args)
    {
        if (args.Length > 1)
        {
            var cmd = args[0];
            if (cmd == "gen")
            {

            }
        }
        return 0;
    }

    static void gen(string[] args, DirectoryInfo root)
    {
        var projName = root.Name;


    }
}