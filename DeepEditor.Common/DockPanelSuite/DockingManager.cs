using DeepCore;
using MaterialSkin;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace DeepEditor.Common.DockPanelSuite
{
    public class DockingManager 
    {
        private HashMap<string, DockContent> mdiForms = new HashMap<string, DockContent>();
        private DockPanel panel;
        private FileInfo saveFile;
        public DockPanel Panel { get => panel; }
        public event Action<DockingManager> ResetDockingLayout;
        public event Func<DockingManager, string, IDockContent> CreateDefaultContent;
        public ICollection<DockContent> Dockings { get => mdiForms.Values; }
        public DockingManager (DockPanel panel, FileInfo saveFile)
        {
            this.panel = panel;
            this.saveFile = saveFile;
        }

        public T AddMainMdiForm<T>() where T : DockContent, new()
        {
            var mdi = new T();
            mdiForms.Add(mdi.GetType().FullName, mdi);
            return mdi;
        }
        public T AddMainMdiForm<T>(T mdi) where T : DockContent
        {
            mdiForms.Add(mdi.GetType().FullName, mdi);
            return mdi;
        }

        public void Load()
        {
            try
            {
                if (!LoadDockingLayout())
                {
                    ResetDockingLayout?.Invoke(this);
                }
            }
            catch (Exception err)
            {
                ResetDockingLayout?.Invoke(this);
            }
            finally
            {
                foreach (var mdi in mdiForms.Values)
                {
                    if (mdi.DockPanel == null)
                    {
                        mdi.Show(this.panel, DockState.Document);
                    }
                }
            }          
        }

        private bool LoadDockingLayout()
        {
            try
            {
                if (File.Exists(saveFile.FullName))
                {
                    panel.LoadFromXml(saveFile.FullName, (persistString) =>
                    {
                        if (mdiForms.TryGetValue(persistString, out var mdi))
                        {
                            return mdi;
                        }
                        else
                        {
                            return CreateDefaultContent?.Invoke(this, persistString);
                        }
                    });
                    return true;
                }
            }
            catch { }
            return false;
        }
        public bool Save()
        {
            try
            {
                panel.SaveAsXml(saveFile.FullName);
                return true;
            }
            catch (Exception err)
            {
                err.ShowMessageBox();
            }
            return false;
        }
    }
}
