using DeepCore.GameData.ZoneServer;
using DeepCore.IO;
using DeepCore.MPQ;
using System;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace DeepEditor.Plugin.ServerTest
{
    public abstract class TestClientLoader
    {
        public TestClientLoader()
        {
        }

        public void Init(DirectoryInfo mpq_path)
        {
            LoadDlls();
            Resource.SetLoader(new ResourceLoader(mpq_path));
        }

        public abstract CreateUnitInfoR2B GenUnitInfoR2B(int unitID);

        public abstract Type ZoneFactoryType { get; }

        public class ResourceLoader : DefaultResourceLoader
        {
            private MPQFileSystem mFileSystem;

            public ResourceLoader(DirectoryInfo dir)
            {
                mFileSystem = new MPQFileSystem();
                mFileSystem.init(dir);
            }
            public override bool TryOpenStream(string path, out Stream stream)
            {
                var e = mFileSystem.findEntry(path);
                if (e != null)
                {
                    stream = mFileSystem.openEntryStream(e);
                    return true;
                }
                return base.TryOpenStream(path, out stream);
            }
            public override bool TryLoadData(string path, out byte[] data)
            {
                data = mFileSystem.getData(path);
                if (data != null)
                {
                    return true;
                }
                return base.TryLoadData(path, out data);
            }
//             public override string[] ListFiles(string path)
//             {
//                 return base.ListFiles(path);
//             }
            //             public override bool ExistData(string path)
            //             {
            //                 if (mFileSystem.findEntry(path) != null)
            //                 {
            //                     return true;
            //                 }
            //                 return base.ExistData(path); 
            //             }
        }
        public static void LoadDlls()
        {
            AppDomain domain = AppDomain.CurrentDomain;

            FileInfo exefile = new FileInfo(Application.ExecutablePath);
            foreach (FileInfo file in exefile.Directory.GetFiles())
            {
                var ext = file.Extension.ToLower();
                if (ext.Equals(".dll") || ext.Equals(".exe"))
                {
                    try
                    {
                        Assembly asm = Assembly.ReflectionOnlyLoadFrom(file.FullName);
                        //Assembly asm = Assembly.ReflectionOnlyLoad(File.ReadAllBytes(file.FullName));
                        if (!ExistAssembly(domain, asm.FullName))
                        {
                            domain.Load(asm.FullName);
                        }
                    }
                    catch { }
                }
            }
        }

        private static bool ExistAssembly(AppDomain domain, string name)
        {
            foreach (Assembly asm in domain.GetAssemblies())
            {
                if (asm.FullName.Equals(name))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
