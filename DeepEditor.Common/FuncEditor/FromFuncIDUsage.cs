using DeepCore;
using DeepCore.FuncData;
using DeepCore.IO;
using DeepEditor.Common.G2D;
using System;
using System.Drawing;
using System.Windows.Forms;


namespace DeepEditor.Common.FuncEditor
{
    public partial class FromFuncIDUsage : Form
    {
        public FromFuncIDUsage()
        {
            InitializeComponent();
            this.treeView1.ImageList = FuncEditorPlugin.Instance.GetTempaltesImageList();
            this.RefreshTemplates();
        }

        public void RefreshTemplates()
        {
            try
            {
                this.treeView1.SuspendLayout();
                this.treeView1.Nodes.Clear();
                var templates = FuncEditorPlugin.Instance.GetEditorTemplatesData();
                var affects = FuncDataManager.Instance.GenAffectBindings(templates);
                var t_i_templates = FuncDataManager.ToTypeIDTemplateMap(templates);
                foreach (var affect in affects.FuncIDAffects.ToSorted())
                {
                    var tempf = FuncDataManager.Instance.GetTemplate(affect.Key);
                    var funcNode = CreateFuncNode(affect.Value, tempf);
                    var sheetGroup = GetOrCreateSheetNodeCollection(tempf);
                    sheetGroup.Add(funcNode);
                    foreach (var types in affect.Value.TypeTemplateIDAffects.ToSorted())
                    {
                        if (t_i_templates.TryGetValue(types.Key, out var i_templates))
                        {
                            foreach (var binding in types.Value.ToSorted())
                            {
                                if (i_templates.TryGetValue(binding.Key, out var tempd))
                                {
                                    var usageNode = CreateFieldsNode(binding.Value, tempd);
                                    funcNode.Nodes.Add(usageNode);
                                }
                            }
                        }
                    }
                    if (tempf != null)
                    {
                        foreach (var attribute in tempf.Attributes)
                        {
                            var attr = funcNode.Nodes.Add($"{attribute.Key} = {attribute.Value}");
                            attr.ImageKey = attr.SelectedImageKey = "icon_var.png";
                        }
                    }
                }
#if false
                var templates = Editor.Instance.GetEditorTemplatesData();
                var affects = FuncDataManager.Instance.GenAffectBindings(templates);
                foreach (var affect in affects.Affects.ToSorted())
                {
                    var tempf = FuncDataManager.Instance.GetTemplate(affect.Key);
                    var funcNode = CreateFuncNode(affect.Value, tempf);
                    var sheetGroup = GetOrCreateSheetNodeCollection(tempf);
                    sheetGroup.Add(funcNode);
                    foreach (var binding in affect.Value.AffectScenes.ToSorted())
                    {
                        var tempd = templates.Scenes.Get(binding.Key);
                        var usageNode = CreateFieldsNode(binding.Value, tempd);
                        funcNode.Nodes.Add(usageNode);
                    }
                    foreach (var binding in affect.Value.AffectUnits.ToSorted())
                    {
                        var tempd = templates.Units.Get(binding.Key);
                        var usageNode = CreateFieldsNode(binding.Value, tempd);
                        funcNode.Nodes.Add(usageNode);
                    }
                    foreach (var binding in affect.Value.AffectSkills.ToSorted())
                    {
                        var tempd = templates.Skills.Get(binding.Key);
                        var usageNode = CreateFieldsNode(binding.Value, tempd);
                        funcNode.Nodes.Add(usageNode);
                    }
                    foreach (var binding in affect.Value.AffectSpells.ToSorted())
                    {
                        var tempd = templates.Spells.Get(binding.Key);
                        var usageNode = CreateFieldsNode(binding.Value, tempd);
                        funcNode.Nodes.Add(usageNode);
                    }
                    foreach (var binding in affect.Value.AffectBuffs.ToSorted())
                    {
                        var tempd = templates.Buffs.Get(binding.Key);
                        var usageNode = CreateFieldsNode(binding.Value, tempd);
                        funcNode.Nodes.Add(usageNode);
                    }
                    foreach (var binding in affect.Value.AffectAuras.ToSorted())
                    {
                        var tempd = templates.Auras.Get(binding.Key);
                        var usageNode = CreateFieldsNode(binding.Value, tempd);
                        funcNode.Nodes.Add(usageNode);
                    }
                    foreach (var binding in affect.Value.AffectUnitEvents.ToSorted())
                    {
                        var tempd = templates.UnitEvents.Get(binding.Key);
                        var usageNode = CreateFieldsNode(binding.Value, tempd);
                        funcNode.Nodes.Add(usageNode);
                    }
                    foreach (var binding in affect.Value.AffectItems.ToSorted())
                    {
                        var tempd = templates.Items.Get(binding.Key);
                        var usageNode = CreateFieldsNode(binding.Value, tempd);
                        funcNode.Nodes.Add(usageNode);
                    }
                    if (tempf != null)
                    {
                        foreach (var attribute in tempf.Attributes)
                        {
                            var attr = funcNode.Nodes.Add($"{attribute.Key} = {attribute.Value}");
                            attr.ImageKey = attr.SelectedImageKey = "icon_var.png";
                        }
                    }
                }
#endif
                TreeNodeCollection GetOrCreateSheetNodeCollection(FuncDataTemplate tempf)
                {
                    if (tempf != null)
                    {
                        var xnodes = treeView1.Nodes;
                        if (!string.IsNullOrEmpty(tempf.FileName))
                        {
                            var xkey = "File: " + tempf.FileName;
                            var xgroup = treeView1.Nodes.Find(xkey, false);
                            if (xgroup.Length > 0)
                            {
                                xnodes = xgroup[0].Nodes;
                            }
                            else
                            {
                                var fileGroup = new TreeNode(xkey);
                                fileGroup.Name = xkey;
                                treeView1.Nodes.Add(fileGroup);
                                xnodes = fileGroup.Nodes;
                            }
                        }
                        var gkey = "Sheet: " + tempf.SheetName;
                        var group = xnodes.Find(gkey, false);
                        if (group.Length == 0)
                        {
                            var sheetGroup = new TreeNode(gkey);
                            sheetGroup.Name = gkey;
                            xnodes.Add(sheetGroup);
                            return sheetGroup.Nodes;
                        }
                        else
                        {
                            return group[0].Nodes;
                        }
                    }
                    return treeView1.Nodes;
                }
                TreeNode CreateFuncNode(AffectBindingTemplate func, FuncDataTemplate tempf)
                {
                    var funcNode = new TreeNode($"Func: {func.FuncID} ({tempf?.FuncName})");
                    if (tempf != null)
                    {
                        funcNode.ToolTipText = tempf.FuncDesc;
                    }
                    funcNode.ImageKey = funcNode.SelectedImageKey = "icon_event.png";
                    funcNode.Tag = func;
                    return funcNode;
                }
                TreeNode CreateFieldsNode(AffectFieldsUsage fields, IFuncTemplateData tempd)
                {
                    var bindingNode = new TreeNode($"{tempd.GetType().Name}: {fields.TemplateID} ({tempd.TemplateName})");
                    if (tempd != null)
                    {
                        bindingNode.ImageKey = bindingNode.SelectedImageKey = FuncEditorPlugin.Instance.GetImageKeyByTemplateType(tempd.GetType());
                    }
                    bindingNode.Tag = fields;
                    foreach (var usage in fields.FieldsUsage)
                    {
                        var sub = bindingNode.Nodes.Add(usage);
                        sub.ImageKey = sub.SelectedImageKey = "icons_tool_bar1.png";
                    }
                    return bindingNode;
                }
                this.treeView1.ExpandAll();
            }
            catch (Exception err)
            {
                err.ShowMessageBox();
            }
            finally
            {
                this.treeView1.ResumeLayout();
            }
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            treeView1.ExpandAll();
        }
        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            treeView1.CollapseAll();
        }

        private void btnTestFillAllDataToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new G2DProgressDialog(btn_TestFillAllData.Text, (pct) =>
            {
                var templates = FuncEditorPlugin.Instance.GetEditorTemplatesData();
                pct.SetMax(templates.Count);
                foreach (var data in templates)
                {
                    try
                    {
                        try
                        {
                            var ownerFunc = FuncDataManager.Instance.GetFuncUsage(data);
                            var filled = FuncDataManager.Instance.Codec.Clone(data);
                            FuncDataManager.Instance.FillData(filled, ownerFunc);
                        }
                        catch (Exception err)
                        {
                            throw new Exception(data.ToString() + " : " + err.Message, err);
                        }
                    }
                    catch (Exception err)
                    {
                        this.Invoke(new System.Action(() => { err.ShowMessageBox(); }));
                    }
                    finally
                    {
                        pct.Add(1);
                    }
                }
            }).ShowDialog();
        }

        private void treeView1_DrawNode(object sender, DrawTreeNodeEventArgs e)
        {
            if (e.Node.ImageKey == "icons_tool_bar1.png" && !e.Node.IsSelected)
            {
                var split = e.Node.Text.LastIndexOf("<-");
                if (split > 0)
                {
                    e.DrawDefault = false;
                    var prifix = e.Node.Text.Substring(0, split);
                    var suffix = e.Node.Text.Substring(split);
                    var size1 = (int)e.Graphics.MeasureString(prifix, this.Font).Width;
                    {
                        var b1 = e.Bounds;
                        e.Graphics.DrawString(prifix, this.Font, Brushes.Black, b1);
                    }
                    {
                        var b2 = e.Bounds;
                        b2.X += size1 - 1;
                        e.Graphics.DrawString(suffix, this.Font, Brushes.Blue, b2);
                    }
                    return;
                }
            }
            e.DrawDefault = true;
        }
    }
}
