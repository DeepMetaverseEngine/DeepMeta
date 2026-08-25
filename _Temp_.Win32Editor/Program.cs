using DeepCore;
using DeepCore.Reflection;
using DeepCore.Template.NewtonJson;
using DeepEditor.Main;
using _Temp_.Battle.Data;
using _Temp_.Battle.Host;
using _Temp_.Battle.Slave;
using _Temp_.Codec;
using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace _Temp_.Win32Editor
{
    static class Program
    {
        /// <summary>
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            //Disposable.ENABLE_ALLOC_RECORD = true;
            new ReflectionTypes();
            try
            {
                Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                new NewtonJsonTemplateLoader(true);
            }
            catch (Exception ex) { ex.PrintStackTrace(); }
            {
                TemplateDataCenter.ENABLE_BATCH_LOAD = true;
                TemplateDataCenter.BATCH_CONCURRENT = 2;
                TemplateDataCenter.ENABLE_LOAD_FROM_BIN = false;
                TypeAllocRecorder.ENABLE_STATISTICS = true;

                Application.SetHighDpiMode(HighDpiMode.DpiUnaware);
                Application.VisualStyleState = System.Windows.Forms.VisualStyles.VisualStyleState.ClientAndNonClientAreasEnabled;
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                new _Temp_Win32Factory();
                {

                }
                GameEditorLauncher.OnMainBeginInit += argsp =>
                {
                    ReflectionUtil.LoadDlls(new DirectoryInfo(Application.StartupPath));
                };
                GameEditorLauncher.OnMainEndInit += (main, argsp) =>
                {
                    //new FillCartridges();
                    GameEditor.HostFactory.OnZoneCreate += (zone) =>
                    {
                        //ZuMaLocalBattle.OnZoneStart(zone);
                    };
                    GameEditor.SlaveFactory.OnLayerCreate += (layer) =>
                    {
                        //layer.LayerInit += ZuMaLocalBattle.OnLayerStart;
                    };
                };
                GameEditorLauncher.Main(args, new ZumaGamePlugin());
            }
        }
        class ZumaGamePlugin : GamePlugin
        {
            public ZumaGamePlugin() : base(
                new _Temp_BattleCodec(),
                new _Temp_ZoneDataFactory(),
                new _Temp_ZoneHostFactory(),
                new _Temp_ZoneSlaveFactory(),
                new _Temp_GamePlugin3D())
            { }          
        }


        [DllImport("user32.dll")]
        static extern bool SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);


    }
}