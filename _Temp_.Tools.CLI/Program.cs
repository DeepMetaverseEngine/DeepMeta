using DeepCore.IO;
using DeepMetaGame.Tools.SaveAll;
using _Temp_.Battle.Data;
using _Temp_.Battle.Host;
using _Temp_.Battle.Slave;
using _Temp_.Codec;

class Program
{
    [STAThread]
    static int Main(string[] args)
    {
        try
        {
            if (CFiles.TryFindParentDirectory(Environment.CurrentDirectory, Path.Combine("GameEditor"), out var editor_root))
            {
                var save = new _Temp_SaveMain();
                save.Main(new DirectoryInfo(editor_root), args);
            }
        }
        catch (Exception err)
        {
            err.PrintStackTrace();
            System.Environment.Exit(-1);
            return -1;
        }
        return 0;
    }
}

public class _Temp_SaveMain : SaveMain<_Temp_Types, _Temp_BattleCodec, _Temp_ZoneDataFactory, _Temp_ZoneHostFactory, _Temp_ZoneSlaveFactory, _Temp_DataCenter>
{

}