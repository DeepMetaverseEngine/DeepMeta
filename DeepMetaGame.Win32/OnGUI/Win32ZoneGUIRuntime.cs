using DeepCore;
using DeepCore.Game3D.Host.Instance;
using DeepCore.Game3D.Slave.Layer;
using DeepCore.GUI.Data;
using DeepCore.GUI.Display.SceneGraph.GUI;
using DeepCore.GUI.SceneGraph;
using DeepMetaGame.Data.Message.UI;
using DeepMetaGame.Data.Template;
using DeepMetaGame.Data.ZoneEditor;
using DeepMetaGame.Display.GUI;
using DeepMetaGame.Slave.GUI;
using System;
namespace DeepEditor.Plugin3D.BattleClient
{
    public class Win32ZoneGUIRuntime : Disposable, ZoneGUIRuntime
    {
        public EditorTemplates DataRoot { get; private set; }
        public BattleView3D View { get; private set; }
        public void Init(BattleView3D view, EditorTemplates root)
        {
            this.View = view;
            this.DataRoot = root;
            view.Layer.GUIRuntime = this;
        }
        protected override void Disposing()
        {
            View = null;
        }
        //----------------------------------------------------------------------------------------------------
        public IZoneGUIDialog ShowDialog(ILayerZoneListener zone, string guid, BattleUITemplate Data)
        {
            var GUIFactory = CreateFactory(DataRoot.EditorRoot);
            var form = new OnGUIForm(GUIFactory, zone, guid, Data);
            form.IsDialog = true;
            View.HUDRootNode.AddChild(form.RootNode);
            return form;
        }
        public IZoneGUIForm ShowForm(ILayerZoneListener zone, string guid, BattleUITemplate Data)
        {
            var GUIFactory = CreateFactory(DataRoot.EditorRoot);
            var form = new OnGUIForm(GUIFactory, zone, guid, Data);
            form.IsDialog = false;
            View.HUDRootNode.AddChild(form.RootNode);
            return form;
        }
        protected virtual Win32UIFactory CreateFactory(string editorRoot)
        {
            return new Win32UIFactory(editorRoot);
        }
        //----------------------------------------------------------------------------------------------------
        public class Win32UIFactory : UIFactory
        {
            public Win32UIFactory(string rootDir) : base(rootDir)
            {
                this.IsEditor = false;
            }
            protected override UEComponentNode DoCreateUI(UEComponentMeta meta)
            {
                //                 var bnode = BattleUIFactory.CreateUI(this, meta);
                //                 if (bnode != null)
                //                 {
                //                     return bnode;
                //                 }
                return base.DoCreateUI(meta);
            }
        }
        class OnGUIRoot : DisplayNode
        {

        }
        class OnGUIForm : ZoneGUIForm
        {
            public override DisplayNode RootNode { get; }
            public OnGUIForm(UIFactory factory, ILayerZoneListener zone, string guid, BattleUITemplate gui) : base(factory, zone, guid, gui)
            {
                this.RootNode = new OnGUIRoot();
            }
            protected override void Disposing()
            {
                base.Disposing();
                RootNode.RemoveFromParent(true);
            }
        }
        //----------------------------------------------------------------------------------------------------
#if false
        //----------------------------------------------------------------------------------------------------
        class OnGUIForm : DisplayNode, IZoneGUIForm
        {
            public ILayerZoneListener Zone { get; }
            public Win32UIFactory GUIFactory { get; private set; }
            public BattleUITemplate Template { get; }
            public UEComponentMeta Meta { get; private set; }
            public IZoneGUIForm Form { get => this; }
            public OnGUIForm(Win32ZoneGUIRuntime runtime, ILayerZoneListener zone, string guid, BattleUITemplate gui)
            {
                this.Zone = zone;
                this.Template = gui;
                this.GUIFactory = new Win32UIFactory(runtime.DataRoot.EditorRoot);
                this.GUIFactory.OnCreateNode += GUIFactory_OnCreateNode;
                this.Name = guid;
                this.Meta = new UERootMeta()
                {
                    Layout = new UILayoutMeta() { Style = UILayoutStyle.NULL, },
                    Dock = DeepCore.GUI.Data.DockStyle.Fill,
                };
                if (Template.Forms != null)
                {
                    foreach (var ui in Template.Forms)
                    {
                        var form = GUIFactory.CreateUI(ui);
                        this.AddChild(form);
                        form.Decode();
                    }
                }
            }
            public void Show(Action shown)
            {
                Zone.QueueTask(() =>
                {
                    shown();
                    this.OnShown?.Invoke(this);
                });
            }

            private void GUIFactory_OnCreateNode(UEComponentMeta arg1, UEComponentNode arg2)
            {
                if (arg2 is UEComponentNode child)
                {
                    var comp = child.Components.AddComponent<OnGUINode>();
                    comp.Init(this);
                    if (arg1 is UERootMeta rootMeta)
                    {
                        arg2.Rect.IsDragMoveable = true;
                    }
                    else
                    {
                        arg2.Rect.IsDragMoveable = false;
                    }
                    OnAddChild(child, comp);
                }
            }
            protected virtual void OnAddChild(UEComponentNode childNode, OnGUINode child)
            {
                childNode.Rect.MouseClick += (sender, args) =>
                {
                    OnChildNodeClick(child);
                };
                if (childNode.Components.TryGetComponentAs<InputComponent>(out var input))
                {
                    input.OnTextChanged += (sender, newText, oldText) =>
                    {
                        OnChildNodeDataChanged(child);
                    };
                }
            }
            protected virtual void OnChildNodeClick(OnGUINode child)
            {
                Zone.QueueTask(() =>
                {
                    OnNodeClick?.Invoke(child);
                });
            }
            protected virtual void OnChildNodeDataChanged(OnGUINode child)
            {
                Zone.QueueTask(() =>
                {
                    OnNodeDataChanged?.Invoke(child);
                });
            }
            public void Close()
            {
                Zone.QueueTask(() =>
                {
                    OnClose?.Invoke(this);
                    this.Canvas.Invoke(() =>
                    {
                        this.RemoveFromParent(true);
                    });
                });
            }
            public IZoneGUINode GetChild(string name)
            {
                var node = this.FindNodeByName(name);
                return node?.Components.GetComponentAs<OnGUINode>();
            }
            public bool TryGetNode(string name, out IZoneGUINode node)
            {
                var enode = this.FindNodeByName(name);
                var comp = enode?.Components.GetComponentAs<OnGUINode>();
                node = comp;
                return comp != null;
            }
            public event UIFormHandler OnShown;
            public event UIFormHandler OnClose;
            public event UINodeHandler OnNodeClick;
            public event UINodeHandler OnNodeDataChanged;
        }
        //----------------------------------------------------------------------------------------------------
        class OnGUIDialog : OnGUIForm, IZoneGUIDialog
        {
            public OnGUIDialog(Win32ZoneGUIRuntime runtime, ILayerZoneListener zone, string guid, BattleUITemplate gui) : base(runtime, zone, guid, gui)
            {
            }
            protected override void OnChildNodeClick(OnGUINode child)
            {
                base.OnChildNodeClick(child);
                if (!string.IsNullOrEmpty(child.Meta.DialogResult))
                {
                    Zone.QueueTask(() =>
                    {
                        this.OnSelectDialog?.Invoke(child, child.Meta.DialogResult);
                        this.Close();
                    });
                }
            }
            public event SelectDialogHandler OnSelectDialog;
        }
#endif
        //----------------------------------------------------------------------------------------------------
    }
}
