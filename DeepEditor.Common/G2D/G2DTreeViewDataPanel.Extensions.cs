using DeepCore;
using DeepCore.Concurrent;
using DeepCore.IO;
using DeepCore.Reflection.Modeling;
using DeepCore.Xml;
using DeepEditor.Common.G2D.DataGrid;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace DeepEditor.Common.G2D
{
    public partial class G2DTreeViewDataPanel<T> : G2DTreeViewDataPanel where T : class, new()
    {
        new public G2DTreeNodeRoot<T> TreeRoot { get { return base.TreeRoot as G2DTreeNodeRoot<T>; } }
        public G2DTreeViewDataPanel()
        {
        }
        public override void Init(IExternalizableFactory codec, Type dataType, string category, string dir, string set_dir, ImageList imageList = null, string groupImageKey = "", string childImageKey = "")
        {
            base.Init(codec, typeof(T), category, dir, set_dir, imageList, groupImageKey, childImageKey);
        }
        public virtual void Init(IExternalizableFactory codec, string category, string dir, string set_dir, ImageList imageList = null, string groupImageKey = "", string childImageKey = "")
        {
            base.Init(codec, typeof(T), category, dir, set_dir, imageList, groupImageKey, childImageKey);
        }
        //-------------------------------------------------------------------------------------------
        override public T CreateData()
        {
            return new T();
        }
        override protected G2DTreeNodeRoot CreateRoot(string dir, string set_dir)
        {
            return new G2DTreeNodeRoot<T>(CategoryText, dir, set_dir);
        }
        //         override protected G2DTreeNode<T> CreateDataNode(object data)
        //         {
        //             G2DTreeNode<T> ret = new G2DTreeNode<T>(data as T);
        //             return ret;
        //         }
        override protected Type GetDataNodeType()
        {
            return typeof(G2DTreeNode<T>);
        }
        //-------------------------------------------------------------------------------------------
        public T GetNodeData(object id)
        {
            return base.GetNodeData($"{id}") as T;
        }
        public bool TryGetNodeData(object id, out G2DTreeNode<T> node, out T data)
        {
            if (base.TryGetNodeData($"{id}", out var _node, out var _data))
            {
                node = _node as G2DTreeNode<T>;
                data = _data as T;
                return true;
            }
            node = null;
            data = null;
            return false;
        }
        new public List<T> GetAllNodeData()
        {
            return base.GetAllNodeData().ConvertAll(a => (T)a);
        }
        new public List<G2DTreeNode<T>> GetAllDataNode()
        {
            return base.GetAllDataNode().ConvertAll(a => (G2DTreeNode<T>)a);
        }

        //         public void SaveAll(AtomicInteger progress, bool check, SavingAction<T> saving = null, SavedAction<T> saved = null)
        //         {
        //             base.SaveAll(progress, check,
        //                 (a) => saving?.Invoke((T)a),
        //                 (b, src, dst) => saved?.Invoke((T)b, src, dst));
        //         }
        //         public void SaveNode(G2DTreeNode<T> node, SavingAction<T> saving = null, SavedAction<T> saved = null)
        //         {
        //             base.SaveNode(node,
        //                 (a) => saving?.Invoke((T)a),
        //                 (b, src, dst) => saved?.Invoke((T)b, src, dst));
        //         }


        new public T GetSelectedData()
        {
            return base.GetSelectedData() as T;
        }
        new public G2DTreeNode<T> GetSelectedNode()
        {
            return base.GetSelectedNode() as G2DTreeNode<T>;
        }
        new public T ShowSelectTemplateDialog(object obj)
        {
            G2DListSelectEditor<T> dialog = new G2DListSelectEditor<T>(
                   this.TreeRoot, this.TreeView.ImageList, obj);
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                return dialog.SelectedTag;
            }
            return null;
        }
    }
}