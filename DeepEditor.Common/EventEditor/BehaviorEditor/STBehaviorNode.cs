using DeepCore;
using DeepCore.EventTrigger;
using DeepCore.EventTrigger.Data;
using DeepCore.Log;
using DeepCore.Reflection;
using DeepEditor.Common.G2D.DataGrid;
using ST.Library.UI.NodeEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Reflection;
using System.Windows.Forms;

namespace DeepEditor.Common.EventEditor.BehaviorEditor
{
    //---------------------------------------------------------------------------------------------------------------------------------------
    public interface STBehaviorLayout
    {
        //---------------------------------------------------------------------------------------
        public static HashMap<Type, Color> S_COLOR_TABLE = new()
        {
            {typeof(EventLocalVar), Color.Blue},
            {typeof(AbstractTrigger), Color.Orange},
            {typeof(AbstractCondition), Color.Orchid},
            {typeof(AbstractAction), Color.RoyalBlue},
            {typeof(AbstractValue), Color.Gray},
            {typeof(BehaviorGroup), Color.WhiteSmoke},
        };
        static STBehaviorLayout()
        {
            foreach (var vt in ValueTypeNameSpace.Instance.ValueTypes)
            {
                S_COLOR_TABLE.Put(vt.ValueType.OwnerType, Color.FromArgb((int)vt.ColorARGB));
            }
        }
        public static Type GetBaseValueType(Type type)
        {
            if (typeof(AbstractTrigger).IsAssignableFrom(type)) return typeof(AbstractTrigger);
            if (typeof(AbstractAction).IsAssignableFrom(type)) return typeof(AbstractAction);
            if (typeof(AbstractValue).IsAssignableFrom(type)) return typeof(AbstractValue);
            if (typeof(BehaviorGroup).IsAssignableFrom(type)) return typeof(BehaviorGroup);
            return type;
        }
        public static Color GetValueColor(Type type)
        {
            if (S_COLOR_TABLE.TryGetValue(type, out var color))
            {
                return Color.FromArgb(200, color);
            }
            return Color.FromArgb(200, Color.Gray);
        }
        public static string GetValueTypeName(Type valueType)
        {
            var vt = ValueTypeNameSpace.Instance.GetValueType(valueType);
            if (vt != null)
            {
                return vt.Alias;
            }
            var gargs = valueType.GetGenericArguments();
            if (gargs.Length == 1)
            {
                return $"{gargs[0].Name}";
            }
            return valueType.Name;
        }
        public static string GetValueText(object value)
        {
            if (value != null)
            {
                if (value is ICollection list)
                {
                    return "集合:[" + list.Count + "]";
                }
                if (value.GetType().IsEnum)
                {
                    return PropertyUtil.ToEnumDesc(value);
                }
            }
            return $"{value}";
        }
        public static Type GetPrimitiveValueType(Type type)
        {
            var fvtype = ValueTypeNameSpace.Instance.GetValueTypeWithDataType(type)?.ValueType.OwnerType;
            if (fvtype == null)
            {
                fvtype = type;
            }
            return fvtype;
        }
        public static bool TryAcceptPropertyField(G2DTypeDescriptor desc, MemberInfo member, object owner)
        {
            return EventBehaviorNode.IsPrimitiveField(member);
        }

        //-------------------------------------------------------------------------------------------

        public static STBehaviorNode CreateNode(EventBehaviorNode data)
        {
            if (data is EventBehaviorLocalVar var) return new STLocalVarNode(var);
            if (data is EventBehaviorTrigger trigger) return new STTriggerNode(trigger);
            if (data is EventBehaviorAction action) return new STActionNode(action);
            if (data is EventBehaviorValue value) return new STValueNode(value);
            if (data is EventBehaviorGroup group) return new STGroupNode(group);
            return null;
        }
        public static STBehaviorNode CreateNode(EventExternalizable data, List<LinkOption> inputs, List<LinkOption> outputs)
        {
            var nodeData = EventBehaviorNode.CreateNode(data);
            nodeData.Inputs = inputs;
            nodeData.Outputs = outputs;
            return CreateNode(nodeData);
        }

        //-------------------------------------------------------------------------------------------
    }

    //---------------------------------------------------------------------------------------------------------------------------------------
    public abstract class STBehaviorNode : STNode, STBehaviorLayout
    {
        private static Logger log = new LazyLogger(typeof(STBehaviorNode));
        //---------------------------------------------------------------------------------------
        public EventBehaviorNode NodeData { get; }
        public EventExternalizable EventData { get => NodeData.EventData; }
        public STNodeOption MainInput { get; protected set; }
        public STNodeOption MainOutput { get; protected set; }

        public STBehaviorNode(EventBehaviorNode data)
        {
            this.NodeData = data;
            if (!string.IsNullOrEmpty(data.GUID))
            {
                this.Guid = new Guid(NodeData.GUID);
            }
            this.m_sf = new StringFormat();
            this.m_sf.LineAlignment = StringAlignment.Center;
            this.TitleColor = Color.FromArgb(200, Color.CornflowerBlue);
            if (PropertyUtil.TryGetAttribute<DescAttribute>(NodeData.EventData.GetType(), out var desc))
            {
                this.Title = desc.Desc;
            }
            else
            {
                this.Title = NodeData.EventData.GetType().Name;
            }
            this.TitleColor = STBehaviorLayout.GetValueColor(NodeData.EventData.BaseType);
            this.InitFields();
            if (NodeData.Inputs != null)
            {
                foreach (STNodeOption m_in in this.InputOptions.ToArray())
                {
                    if (m_in is STDataOption field)
                    {
                        field.Init(NodeData.EventData, NodeData.Inputs);
                    }
                }
            }
            if (NodeData.Outputs != null)
            {
                foreach (STNodeOption m_out in this.OutputOptions.ToArray())
                {
                    if (m_out is STDataOption field)
                    {
                        field.Init(NodeData.EventData, NodeData.Outputs);
                    }
                }
            }
        }
        public string NewGuid()
        {
            this.Guid = Guid.NewGuid();
            this.NodeData.GUID = this.Guid.ToString();
            return this.NodeData.GUID;
        }
        public void Refresh()
        {
            this.RefreshFields();
            //this.ResetHelp();
        }
        public void Load(BehaviorNodeEditor panel, bool select = false)
        {
            this.Guid = new Guid(NodeData.GUID);
            this.Location = new Point(
                NodeData.EditorX,
                NodeData.EditorY);
            if (NodeData.EditorARGB != 0)
            {
                this.TitleColor = Color.FromArgb(200, Color.FromArgb(unchecked((int)NodeData.EditorARGB)));
            }
            {
                this.LockLocation = BitMask.BitGetMask(this.NodeData.EditorTag, 0);
                this.LockOption = BitMask.BitGetMask(this.NodeData.EditorTag, 1);
            }
            if (select)
            {
                this.IsSelected = BitMask.BitGetMask(this.NodeData.EditorTag, 2);
                this.IsActive = BitMask.BitGetMask(this.NodeData.EditorTag, 3);
            }
            this.LoadFields(panel);
        }
        public void Save(BehaviorNodeEditor panel, bool select = false)
        {
            this.NodeData.GUID = this.Guid.ToString();
            this.NodeData.EditorX = this.Location.X;
            this.NodeData.EditorY = this.Location.Y;
            this.NodeData.EditorARGB = unchecked((uint)this.TitleColor.ToArgb());
            this.NodeData.EditorTag = 0;
            {
                BitMask.BitSetMask(ref this.NodeData.EditorTag, 0, this.LockLocation);
                BitMask.BitSetMask(ref this.NodeData.EditorTag, 1, this.LockOption);
            }
            if (select)
            {
                BitMask.BitSetMask(ref this.NodeData.EditorTag, 2, this.IsSelected);
                BitMask.BitSetMask(ref this.NodeData.EditorTag, 3, this.IsActive);
            }
            this.SaveFields(panel);
        }
        protected virtual void InitFields()
        {
            var data = NodeData.EventData;
            var typeinfo = data.GetTypeInfo();
            foreach (var field in typeinfo.GetFields())
            {
                if (typeof(AbstractTrigger).IsAssignableFrom(field.Field.FieldType))
                {
                    this.OutputOptions.Add(new STNodeOption($"{DescAttribute.GetDesc(field.Field)}", field.Field.FieldType, false)
                    {
                        DotColor = STBehaviorLayout.GetValueColor(field.Field.FieldType),
                        Tag = field,
                    });
                }
                else if (EventBehaviorNode.IsMonoList(field.Field.FieldType, out var memberType))
                {
                    if (typeof(AbstractAction).IsAssignableFrom(memberType))
                    {
                        this.OutputOptions.Add(new STFieldMonoActionsOption(field, memberType));
                    }
                    else if (typeof(AbstractValue).IsAssignableFrom(memberType))
                    {
                        this.InputOptions.Add(new STFieldMonoValuesOption(field, memberType));
                    }
                    else
                    {
                        this.InputOptions.Add(new STFieldPrimitiveOption(data, field));
                    }
                }
                else if (EventBehaviorNode.IsStereoList(field.Field.FieldType, out memberType, out var attr))
                {
                    if (typeof(AbstractValue).IsAssignableFrom(attr.InputType) && typeof(AbstractAction).IsAssignableFrom(attr.OutputType))
                    {
                        this.OutputOptions.Add(new STFieldStereoValueAction(field, attr, memberType));
                    }
                    else if (typeof(AbstractValue).IsAssignableFrom(attr.InputType) && typeof(AbstractValue).IsAssignableFrom(attr.OutputType))
                    {
                        this.OutputOptions.Add(new STFieldStereoValueValue(field, attr, memberType));
                    }
                    else
                    {
                        this.InputOptions.Add(new STFieldPrimitiveOption(data, field));
                    }
                }
                else if (typeof(AbstractAction).IsAssignableFrom(field.Field.FieldType))
                {
                    this.OutputOptions.Add(new STFieldActionOption(field));
                }
                else if (typeof(AbstractValue).IsAssignableFrom(field.Field.FieldType))
                {
                    this.InputOptions.Add(new STFieldValueOption(field));
                }
                else
                {
                    this.InputOptions.Add(new STFieldPrimitiveOption(data, field));
                }
            }
        }
        protected virtual void RefreshFields()
        {
            var data = NodeData.EventData;
            foreach (STNodeOption m_in in this.InputOptions)
            {
                if (m_in is STDataOption bop)
                {
                    bop.Refresh(data);
                }
            }
        }
        protected virtual void LoadFields(BehaviorNodeEditor panel)
        {
            if (NodeData.Inputs != null)
            {
                foreach (STNodeOption m_in in this.InputOptions.ToArray())
                {
                    if (m_in is STDataOption field)
                    {
                        var list = new List<LinkOption>(NodeData.Inputs);
                        field.Load(panel, list);
                    }
                }
            }
            if (NodeData.Outputs != null)
            {
                foreach (STNodeOption m_out in this.OutputOptions.ToArray())
                {
                    if (m_out is STDataOption field)
                    {
                        var list = new List<LinkOption>(NodeData.Outputs);
                        field.Load(panel, list);
                    }
                }
            }
        }
        protected virtual void SaveFields(BehaviorNodeEditor panel)
        {
            this.NodeData.Inputs = new();
            this.NodeData.Outputs = new();
            foreach (STNodeOption m_in in this.InputOptions.ToArray())
            {
                if (m_in is STDataOption field)
                {
                    var list = new List<LinkOption>();
                    field.Save(panel, list);
                    NodeData.Inputs.AddRange(list);
                }
            }
            foreach (STNodeOption m_out in this.OutputOptions.ToArray())
            {
                if (m_out is STDataOption field)
                {
                    var list = new List<LinkOption>();
                    field.Save(panel, list);
                    NodeData.Outputs.AddRange(list);
                }
            }
        }

        public LinkDock GetFieldDock(IDynamicFieldInfo field)
        {
            if (typeof(AbstractTrigger).IsAssignableFrom(field.Field.FieldType))
            {
                return LinkDock.Input;
            }
            else if (EventBehaviorNode.IsMonoList(field.Field.FieldType, out var memberType))
            {
                if (typeof(AbstractAction).IsAssignableFrom(memberType))
                {
                    return LinkDock.Output;
                }
                else if (typeof(AbstractValue).IsAssignableFrom(memberType))
                {
                    return LinkDock.Input;
                }
                else
                {
                    return LinkDock.Input;
                }
            }
            else if (EventBehaviorNode.IsStereoList(field.Field.FieldType, out memberType, out var attr))
            {
                if (typeof(AbstractValue).IsAssignableFrom(attr.InputType) && typeof(AbstractAction).IsAssignableFrom(attr.OutputType))
                {
                    return LinkDock.Output;
                }
                else
                {
                    return LinkDock.Input;
                }
            }
            else if (typeof(AbstractAction).IsAssignableFrom(field.Field.FieldType))
            {
                return LinkDock.Output;
            }
            else if (typeof(AbstractValue).IsAssignableFrom(field.Field.FieldType))
            {
                return LinkDock.Input;
            }
            else
            {
                return LinkDock.Input;
            }
        }

        public bool TryGetNodeFieldOption(LinkDock dock, string fieldName, out STBehaviorOption op)
        {
            var opts = dock == LinkDock.Input ? this.InputOptions : this.OutputOptions;
            foreach (STNodeOption m_in in opts)
            {
                if (m_in is STBehaviorOption field)
                {
                    if (field.FieldName == fieldName)
                    {
                        op = field;
                        return true;
                    }
                }
            }
            op = null;
            return false;
        }
        public bool TryGetNodeFieldOption(string fieldName, out IDynamicFieldInfo field, out LinkDock dock, out STBehaviorOption op)
        {
            if (EventBehaviorNode.TryGetValueField(NodeData.EventData, fieldName, out field))
            {
                dock = GetFieldDock(field);
                if (TryGetNodeFieldOption(dock, fieldName, out op))
                {
                    return true;
                }
            }
            dock = LinkDock.Input;
            op = null;
            return false;
        }
        public void ForEachBehaviorFields(Action<IDynamicFieldInfo> action)
        {
            var typeinfo = NodeData.EventData.GetTypeInfo();
            EventBehaviorNode.ForEachEventDataValueFields(NodeData.EventData, (src, field) =>
            {
                action(field);
            });
        }
        public void ForEachFieldLinks(Action<LinkDock, IDynamicFieldInfo, STBehaviorOption> action)
        {
            var typeinfo = NodeData.EventData.GetTypeInfo();
            EventBehaviorNode.ForEachEventDataValueFields(NodeData.EventData, (src, field) =>
            {
                if (TryGetNodeFieldOption(LinkDock.Input, field.Field.Name, out var input))
                {
                    action(LinkDock.Input, field, input);
                }
                else if (TryGetNodeFieldOption(LinkDock.Output, field.Field.Name, out var output))
                {
                    action(LinkDock.Output, field, output);
                }
            });
        }
        public void ForEachFieldNodes(Action<IDynamicFieldInfo, LinkDock, STBehaviorNode> action)
        {
            var typeinfo = NodeData.EventData.GetTypeInfo();
            EventBehaviorNode.ForEachEventDataValueFields(NodeData.EventData, (src, field) =>
            {
                if (TryGetNodeFieldOption(LinkDock.Input, field.Field.Name, out var input))
                {
                    foreach (var next in input.ConnectedOption)
                    {
                        if (next.Owner is STBehaviorNode bnext)
                        {
                            action(field, LinkDock.Input, bnext);
                        }
                    }
                }
                else if (TryGetNodeFieldOption(LinkDock.Output, field.Field.Name, out var output))
                {
                    foreach (var next in output.ConnectedOption)
                    {
                        if (next.Owner is STBehaviorNode bnext)
                        {
                            action(field, LinkDock.Output, bnext);
                        }
                    }
                }
            });
        }
        public void ForEachPrimitiveFields(Action<IDynamicFieldInfo> action)
        {
            var typeinfo = NodeData.EventData.GetTypeInfo();
            foreach (var f in typeinfo.GetFields())
            {
                if (EventBehaviorNode.IsPrimitiveField(f.Member))
                {
                    action(f);
                }
            }
        }

        //-------------------------------------------------------------------------------------------
        #region Drawing
        public Image HelpIcon { get; set; }
        public Rectangle HelpBounds { get => new Rectangle(this.Right - TitleHeight - 2, this.Top + 2, TitleHeight - 4, TitleHeight - 4); }
        protected override void OnDrawTitle(DrawingTools dt)
        {
            base.OnDrawTitle(dt);
            if (HelpIcon != null)
            {
                var bounds = HelpBounds;
                dt.Graphics.DrawImage(HelpIcon, bounds);
            }
        }
        public bool Highlight { get; set; } = false;
        private Image highlight_img;
        private Pen highlight_pen = new Pen(Color.White, 2);
        protected internal override void OnDrawNode(DrawingTools dt)
        {
            base.OnDrawNode(dt);
            if (Highlight)
            {
                OnDrawHightLight(dt);
            }
        }
        protected virtual void OnDrawHightLight(DrawingTools dt)
        {
            if (highlight_img == null)
            {
                highlight_img = Owner.Drawing.CreateSolidImage(Color.White, 16);
            }
            Owner.Drawing.RenderBorder(dt.Graphics, this.Rectangle, highlight_img, 6);
        }
        protected internal override void OnDrawBezier(Graphics g, Pen m_p_line, Pen m_p_line_hover, STNodeOption op, STNodeOption next)
        {
            base.OnDrawBezier(g, m_p_line, m_p_line_hover, op, next);
            if (this.Highlight && next.Owner is STBehaviorNode nextNode && nextNode.Highlight)
            {
                base.OnDrawBezier(g, highlight_pen, m_p_line_hover, op, next);
            }
        }
        #endregion
        //-------------------------------------------------------------------------------------------
    }
    public abstract class STBehaviorNode<T, E> : STBehaviorNode, STBehaviorLayout where T : EventBehaviorNode where E : EventExternalizable
    {
        new public T NodeData { get => base.NodeData as T; }
        new public E EventData { get => NodeData.EventData as E; }
        public STBehaviorNode(T data) : base(data)
        {
        }
    }

    //---------------------------------------------------------------------------------------------------------------------------------------
    #region DataNodes ---------------------------------------------------------------------------------------------------------------------------------------

    public class STLocalVarNode : STBehaviorNode<EventBehaviorLocalVar, EventLocalVar>
    {
        public STLocalVarNode(EventBehaviorLocalVar data) : base(data)
        {
            this.Title = $"{data.VAR.Key} : {STBehaviorLayout.GetValueTypeName(data.VAR.ValueType)}";
        }
        protected override void InitFields()
        {
            base.InitFields();
        }
        protected override void RefreshFields()
        {
            var data = NodeData.EventData;
            this.Title = $"{NodeData.VAR.Key} : {STBehaviorLayout.GetValueTypeName(NodeData.VAR.ValueType)}";
            base.RefreshFields();
        }
    }


    public class STTriggerNode : STBehaviorNode<EventBehaviorTrigger, AbstractTrigger>
    {
        public STTriggerNode(EventBehaviorTrigger data) : base(data)
        {
            this.Title = $"当{Title}";
        }
        protected override void InitFields()
        {
            var data = NodeData.EventData;
            this.InputOptions.Add(MainInput = new STDataOption<AbstractAction>(EventBehaviorTrigger.KEY_ENTRY, "LISTEN", false));
            this.OutputOptions.Add(MainOutput = new STDataCollectionOption<AbstractAction, AbstractAction>(EventBehaviorTrigger.KEY_CALL, "CALL"));
            //this.OutputOptions.Add(MainOutput = new STDataOption<AbstractAction>(EventBehaviorTrigger.KEY_CALL, "CALL", false));
            base.InitFields();
            foreach (var method in data.GetType().GetMethods())
            {
                if (method.TryGetAttribute<TriggingArgAttribute>(out var arg))
                {
                    var valueType = typeof(AbstractValue<>).MakeGenericType(method.ReturnType);
                    this.OutputOptions.Add(MainOutput = new STDataOption($"arg:{method.Name}", $"参数：{arg.Desc}", valueType, false)
                    {
                        DotColor = STBehaviorLayout.GetValueColor(valueType)
                    });
                }
            }
        }

    }

    public class STActionNode : STBehaviorNode<EventBehaviorAction, AbstractAction>
    {
        public STActionNode(EventBehaviorAction data) : base(data)
        {
            this.Title = $"执行{Title}";
        }
        protected override void InitFields()
        {
            var data = NodeData.EventData;
            this.InputOptions.Add(MainInput = new STDataOption<AbstractAction>(EventBehaviorAction.KEY_ENTRY, "ENTRY", false));
            base.InitFields();
            foreach (var method in data.GetType().GetMethods())
            {
                if (method.TryGetAttribute<ReturnValueAttribute>(out var arg))
                {
                    var valueType = typeof(AbstractValue<>).MakeGenericType(method.ReturnType);
                    this.OutputOptions.Add(MainOutput = new STDataOption($"return:{method.Name}", $"返回：{arg.Desc}", valueType, false)
                    {
                        DotColor = STBehaviorLayout.GetValueColor(valueType)
                    });
                }
            }
            this.OutputOptions.Add(MainOutput = new STDataCollectionOption<AbstractAction, AbstractAction>(EventBehaviorAction.KEY_NEXT, "NEXT"));
            //this.OutputOptions.Add(MainOutput = new STDataOption<AbstractAction>(EventBehaviorAction.KEY_NEXT, "NEXT", true));
            foreach (var method in data.GetType().GetMethods())
            {
                if (method.TryGetAttribute<TriggingArgAttribute>(out var arg))
                {
                    var valueType = typeof(AbstractValue<>).MakeGenericType(method.ReturnType);
                    this.OutputOptions.Add(MainOutput = new STDataOption($"arg:{method.Name}", $"参数：{arg.Desc}", valueType, false)
                    {
                        DotColor = STBehaviorLayout.GetValueColor(valueType)
                    });
                }
            }
        }
    }

    public class STValueNode : STBehaviorNode<EventBehaviorValue, AbstractValue>
    {
        public STValueNode(EventBehaviorValue data) : base(data)
        {
            this.Title = $"{Title} : {STBehaviorLayout.GetValueTypeName(data.EventData.BaseType)}";
        }
        protected override void InitFields()
        {
            var data = NodeData.EventData;
            this.OutputOptions.Add(MainOutput = new STDataOption(EventBehaviorValue.KEY_OUT, "OUT", data.GetType(), false)
            {
                DotColor = STBehaviorLayout.GetValueColor(data.BaseType)
            });
            base.InitFields();
        }
    }


    //---------------------------------------------------------------------------------------------------------------------------------------
    public class STGroupNode : STBehaviorNode<EventBehaviorGroup, BehaviorGroup>
    {
        public STNode Node => this;
        public STGroupNode(EventBehaviorGroup data) : base(data)
        {
            this.IsDockingGroup = true;
            this.Priority = -1;
            this.Title = $"{data.Group?.Title}";
            this.AutoSize = false;          //此节点需要定制UI 所以无需AutoSize
            this.IsDragResizeable = true;
            this.Size = new Size(data.Width, data.Height);
            this.TitleColor = Color.FromArgb(64, 32, 32, 32);
            this.BackColor = Color.Black.SetAlpha(0.2f);
        }
        protected override void InitFields()
        {
            //base.InitFields(data);
        }
        protected override void RefreshFields()
        {
            var data = NodeData.Group;
            this.Title = $"{data?.Title}";
            //base.RefreshFields(data);
        }
        protected override void LoadFields(BehaviorNodeEditor panel)
        {
            base.LoadFields(panel);
            this.Width = NodeData.Width;
            this.Height = NodeData.Height;
        }
        protected override void SaveFields(BehaviorNodeEditor panel)
        {
            base.SaveFields(panel);
            base.NodeData.Width = this.Width;
            base.NodeData.Height = this.Height;
        }
        protected internal override bool HitTestBody(PointF pt)
        {
            if (STNodeEditor.PointInRectangle(this.Rectangle, pt.X, pt.Y))
            {
                if (this.Rectangle.Width >= STNodeEditor.DRAG_RESIZE_W * 2 && this.Rectangle.Height >= STNodeEditor.DRAG_RESIZE_W * 2)
                {
                    if (STNodeEditor.PointInRectangle(this.TitleRectangle, pt.X, pt.Y))
                    {
                        return true;
                    }
                    var inner = new RectangleF(
                        this.Rectangle.X + STNodeEditor.DRAG_RESIZE_W,
                        this.Rectangle.Y + STNodeEditor.DRAG_RESIZE_W,
                        this.Rectangle.Width - STNodeEditor.DRAG_RESIZE_W * 2,
                        this.Rectangle.Height - STNodeEditor.DRAG_RESIZE_W * 2);
                    if (STNodeEditor.PointInRectangle(inner, pt.X, pt.Y))
                    {
                        return false;
                    }
                }
                return true;
            }
            return false;
        }
        protected internal override bool SelectTestBody(RectangleF rect)
        {
            return rect.IntersectsWith(this.TitleRectangle);
        }
    }

    #endregion DataNodes 
    //---------------------------------------------------------------------------------------------------------------------------------------
    #region Options ---------------------------------------------------------------------------------------------------------------------------------------
    //---------------------------------------------------------------------------------------------------------------------------------------

    public abstract class STBehaviorOption : STNodeOption
    {
        public abstract string FieldName { get; }
        public LinkDock FieldDock { get => this.IsInput ? LinkDock.Input : LinkDock.Output; }
        new public STBehaviorNode Owner { get => base.Owner as STBehaviorNode; }
        public STBehaviorOption(string strText, Type dataType, bool bSingle) : base(strText, dataType, bSingle)
        {
            this.DotColor = STBehaviorLayout.GetValueColor(dataType);
        }
        public virtual void SaveLinks(BehaviorNodeEditor panel, List<LinkOption> links, bool keepDock = false)
        {
            if (keepDock)
            {
                if (this.ConnectedOption.Count == 0)
                {
                    links.Add(new LinkOption()
                    {
                        OwnerFieldName = this.FieldName,
                        OwnerFieldState = this.FieldDock,
                    });
                }
            }
            foreach (var out_opt in this.ConnectedOption)
            {
                if (out_opt is STBehaviorOption next)
                {
                    links.Add(new LinkOption()
                    {
                        NextGUID = next.Owner.Guid.ToString(),
                        NextFieldName = next.FieldName,
                        NextFieldState = next.FieldDock,
                        OwnerFieldName = this.FieldName,
                        OwnerFieldState = this.FieldDock,
                    });
                }
            }
        }
        public virtual void LoadLinks(BehaviorNodeEditor panel, List<LinkOption> links)
        {
            foreach (var link in links)
            {
                if (link.OwnerFieldName == this.FieldName)
                {
                    if (panel.TryGetNodeFieldByGUID(link.NextGUID, link.NextFieldName, link.NextFieldState, out var next, out var next_op))
                    {
                        ConnectOption(next_op);
                    }
                    else if (next != null)
                    {
                        if (this.FieldDock == LinkDock.Input)
                        {
                            ConnectOption(next.MainOutput);
                        }
                        else if (this.FieldDock == LinkDock.Output)
                        {
                            ConnectOption(next.MainInput);
                        }
                    }
                }
            }
        }
    }
    //---------------------------------------------------------------------------------------------------------------------------------------
    public class STDataOption : STBehaviorOption
    {
        public override string FieldName { get; }
        public STDataOption(string fieldName, string strText, Type dataType, bool bSingle) : base(strText, dataType, bSingle)
        {
            this.FieldName = fieldName;
        }
        public virtual void Init(EventExternalizable owner, List<LinkOption> options) { }
        public virtual void Refresh(EventExternalizable owner) { }
        public virtual void Save(BehaviorNodeEditor panel, List<LinkOption> options)
        {
            SaveLinks(panel, options);
        }
        public virtual void Load(BehaviorNodeEditor panel, List<LinkOption> options)
        {
            var links = options.FindAll(link => link.OwnerFieldName == FieldName);
            LoadLinks(panel, links);
        }
    }
    public class STDataOption<T> : STDataOption
    {
        public STDataOption(string fieldName, string strText, bool bSingle) : base(fieldName, strText, typeof(T), bSingle) { }
    }
    public class STDataCollectionOption<T, M> : STDataOption<T>
    {
        public STDataOptionMonoCollection Mono { get; }
        public STDataCollectionOption(string fieldName, string strText) : base(fieldName, strText, true)
        {
            this.Mono = new STDataOptionMonoCollection(this);
        }
        public override void Init(EventExternalizable owner, List<LinkOption> options)
        {
            base.Init(owner, options);
            Mono.Init(owner, options);
        }
        public override void Save(BehaviorNodeEditor panel, List<LinkOption> options)
        {
            //base.Save(panel, options);
            Mono.Save(panel, options);
        }
        public override void Load(BehaviorNodeEditor panel, List<LinkOption> options)
        {
            //base.Load(panel, options);
            Mono.Load(panel, options);
        }
    }
    //---------------------------------------------------------------------------------------------------------------------------------------

    public class STMemberOption : STBehaviorOption
    {
        public override string FieldName { get => _FieldName; }
        public object Index { get; private set; }
        private string _FieldName;
        public STMemberOption(object index, string fieldName, string strText, Type dataType) : base(strText, dataType, true)
        {
            SetIndex(index, fieldName, strText);
        }
        public void SetIndex(object index, string fieldName, string strText)
        {
            this.Index = index;
            this._FieldName = fieldName;
            this.Text = strText;
        }
    }
    //---------------------------------------------------------------------------------------------------------------------------------------
    public abstract class STFieldOption : STDataOption
    {
        public IDynamicFieldInfo Field { get; }
        public STFieldOption(IDynamicFieldInfo field, string strText) : base(field.Field.Name, strText, field.Field.FieldType, true)
        {
            this.Field = field;
            this.Tag = field;
        }
        public STFieldOption(IDynamicFieldInfo field, string strText, Type dataType) : base(field.Field.Name, strText, dataType, true)
        {
            this.Field = field;
            this.Tag = field;
        }
    }
    public class STFieldActionOption : STFieldOption
    {
        public STFieldActionOption(IDynamicFieldInfo field) : base(field, $"{DescAttribute.GetDesc(field.Field)} CALL")
        {
        }
        public override void Load(BehaviorNodeEditor panel, List<LinkOption> options)
        {
            Field.SetValue(Owner.NodeData.EventData, null);
            base.Load(panel, options);
        }
        public override void Save(BehaviorNodeEditor panel, List<LinkOption> options)
        {
            Field.SetValue(Owner.NodeData.EventData, null);
            base.Save(panel, options);
        }
    }
    public class STFieldValueOption : STFieldOption
    {
        public STFieldValueOption(IDynamicFieldInfo field) : base(field, $"IN {DescAttribute.GetDesc(field.Field)} : {STBehaviorLayout.GetValueTypeName(field.Field.FieldType)}")
        {
        }
        public override void Load(BehaviorNodeEditor panel, List<LinkOption> options)
        {
            Field.SetValue(Owner.NodeData.EventData, null);
            base.Load(panel, options);
        }
        public override void Save(BehaviorNodeEditor panel, List<LinkOption> options)
        {
            Field.SetValue(Owner.NodeData.EventData, null);
            base.Save(panel, options);
        }
    }
    public class STFieldPrimitiveOption : STFieldOption
    {
        public STFieldPrimitiveOption(object ownerData, IDynamicFieldInfo field) : base(field, $"IN {DescAttribute.GetDesc(field.Field)} : {STBehaviorLayout.GetValueText(field.Field.GetValue(ownerData))}", typeof(void))
        {
            this.OwnerDrawDot += STFieldPrimitiveOption_OwnerDrawDot;
        }
        private void STFieldPrimitiveOption_OwnerDrawDot(DrawingTools dt, STNodeOption op, Rectangle bounds)
        {
            int rx = bounds.Width / 2;
            int ry = bounds.Height / 2;
            dt.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            var poly = new Point[3] {
                new Point(  bounds.X + rx, bounds.Y),
                new Point(  bounds.X, bounds.Y + bounds.Height),
                new Point(  bounds.X + bounds.Width, bounds.Y + bounds.Height),
                };
            dt.Graphics.FillPolygon(dt.SolidBrush, poly);
        }
        public override void Refresh(EventExternalizable owner)
        {
            this.Text = $"IN {DescAttribute.GetDesc(Field.Field)} : {STBehaviorLayout.GetValueText(Field.Field.GetValue(owner))}";
        }
    }
    #endregion Options 
    //---------------------------------------------------------------------------------------------------------------------------------------
    #region  MonoOptions ---------------------------------------------------------------------------------------------------------------------------------------

    //---------------------------------------------------------------------------------------------------------------------------------------
    public delegate bool ToMonoMemberText(int index, out string text);
    public class STDataOptionMonoCollection
    {
        private List<STMemberOption> members = new List<STMemberOption>();
        public STDataOption Option { get; }
        public int MemberCount { get => members.Count; }
        public event ToMonoMemberText ToMemberText;
        public STDataOptionMonoCollection(STDataOption Option)
        {
            this.Option = Option;
            Option.OwnerDrawDot += Option_OwnerDrawDot;
            Option.Connected += Option_Connected;
        }

        public virtual void Init(EventExternalizable owner, List<LinkOption> options)
        {
            var links = options.FindAll(link => EventBehaviorNode.TryParseMonoField(link.OwnerFieldName, out var monoFieldName, out var index) && monoFieldName == Option.FieldName);
            foreach (var link in links)
            {
                if (EventBehaviorNode.TryParseMonoField(link.OwnerFieldName, out var monoFieldName, out var index))
                {
                    AddHub(index);
                }
            }
        }
        public virtual void Load(BehaviorNodeEditor panel, List<LinkOption> options)
        {
            var links = options.FindAll(link => EventBehaviorNode.TryParseMonoField(link.OwnerFieldName, out var monoFieldName, out var index) && monoFieldName == Option.FieldName);
            foreach (var link in links)
            {
                if (members.TryFind(m => m.FieldName == link.OwnerFieldName, out var member))
                {
                    member.LoadLinks(panel, links);
                }
            }
        }
        public virtual void Save(BehaviorNodeEditor panel, List<LinkOption> options)
        {
            ReIndex();
            foreach (var member in members)
            {
                member.SaveLinks(panel, options, true);
            }
        }
        public bool ContainsMember(STNodeOption other)
        {
            foreach (var member in members)
            {
                if (member == other)
                {
                    return true;
                }
            }
            return false;
        }
        protected virtual void Option_Connected(object sender, STNodeOptionEventArgs e)
        {
            Option.DisConnectionAll();
            e.TargetOption.DisConnectOption(Option);
            var conn = this.AddHub(NewIndex());
            if (conn != null)
            {
                conn.ConnectOption(e.TargetOption);
            }
        }
        protected virtual void Option_OwnerDrawDot(DrawingTools dt, STNodeOption op, Rectangle bounds)
        {
            op.DrawDefaultDot(dt);
            var r = bounds.Width / 2;
            var pen = new Pen(this.Option.Owner.Owner.HighLineColor, 2);
            dt.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;
            //dt.Graphics.DrawRectangle(new Pen(dt.SolidBrush.Color), bounds.X, bounds.Y, bounds.Width, bounds.Height);
            dt.Graphics.DrawCross(pen, bounds.X + r, bounds.Y + r, r - 1);
        }
        protected virtual STMemberOption AddHub(int index)
        {
            if (members.Exists(e => e.Index.Equals(index)))
            {
                return null;
            }
            EventBehaviorNode.ToMonoMemberFieldName(Option.FieldName, index, out var fname);
            GetMemberText(index, out var tname);
            var hub = new STMemberOption(index, fname, tname, Option.DataType);
            hub.DisConnected += Hub_DisConnected;
            hub.BeforeConnecting += Hub_BeforeConnecting;
            hub.OnMouseDown += Hub_OnMouseDown;
            hub.OwnerDrawDot += Hub_OwnerDrawDot;
            if (Option.IsInput)
            {
                Option.Owner.InputOptions.Insert(Option.Owner.InputOptions.IndexOf(Option), hub);
            }
            else
            {
                Option.Owner.OutputOptions.Insert(Option.Owner.OutputOptions.IndexOf(Option), hub);
            }
            members.Add(hub);
            if (Option.Owner.Owner != null) Option.Owner.Owner.BuildLinePath();
            if (Option.Owner.AutoSize) Option.Owner.RebuildSize();
            return hub;
        }

        private void Hub_OnMouseDown(object sender, MouseEventArgs e)
        {
            if (sender is STMemberOption hub)
            {
                if (e.Button == MouseButtons.Right)
                {
                    RemoveHub(hub);
                }
            }
        }
        private void Hub_OwnerDrawDot(DrawingTools dt, STNodeOption op, Rectangle bounds)
        {
            op.DrawDefaultDot(dt);
            if (op == op.Owner.Owner.HoverOption)
            {
                var r = bounds.Width / 2;
                var pen = new Pen(this.Option.Owner.Owner.HighLineColor, 2);
                dt.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;
                dt.Graphics.DrawLine(pen, bounds.X + 1, bounds.Y + r, bounds.X + r + r - 1, bounds.Y + r);
            }
        }
        protected virtual void Hub_DisConnected(object sender, STNodeOptionEventArgs e)
        {
            //             if (sender is STMemberOption member)
            //             {
            //                 RemoveHub(member);
            //                
            //             }
        }
        private bool Hub_BeforeConnecting(STNodeOption src, STNodeOption dst)
        {
            if (src != dst && src.Owner == dst.Owner && src.Owner == Option.Owner && src.OwnerOptions == Option.OwnerOptions)
            {
                if (ContainsMember(src) && ContainsMember(dst))
                {
                    // swap option
                    if (Option.OwnerOptions.Swap(src, dst))
                    {
                        this.members.Swap(src, dst);
                        ReIndex();
                        Option.Owner.Owner.Refresh();
                        return false;
                    }
                }
            }
            return true;
        }

        protected bool RemoveHub(STMemberOption hub)
        {
            if (members.Remove(hub))
            {
                if (hub.IsInput)
                {
                    Option.Owner.InputOptions.Remove(hub);
                }
                else
                {
                    Option.Owner.OutputOptions.Remove(hub);
                }
                if (Option.Owner.Owner != null) Option.Owner.Owner.BuildLinePath();
                if (Option.Owner.AutoSize) Option.Owner.RebuildSize();
                ReIndex();
                return true;
            }
            return false;
        }
        protected void GetMemberText(int index, out string textName)
        {
            if (ToMemberText != null && ToMemberText(index, out textName))
            {
                return;
            }
            textName = $"{Option.FieldName}[{index}]";
        }
        protected virtual int NewIndex()
        {
            return MemberCount;
        }
        protected virtual void ReIndex()
        {
            int count = 0;
            foreach (var member in members)
            {
                var index = count;
                EventBehaviorNode.ToMonoMemberFieldName(Option.FieldName, index, out var fname);
                GetMemberText(index, out var tname);
                member.SetIndex(index, fname, tname);
                count++;
            }
        }
    }
    public abstract class STFieldMonoCollectionOption : STFieldOption
    {
        public Type MemberType { get; }
        public STDataOptionMonoCollection Mono { get; }
        protected STFieldMonoCollectionOption(IDynamicFieldInfo field, string strText, Type memberType) : base(field, strText, memberType)
        {
            this.MemberType = memberType;
            this.Mono = new STDataOptionMonoCollection(this);
            //this.OwnerDrawDot += STFieldMonoCollectionOption_OwnerDrawDot;
        }
        //         private void STFieldMonoCollectionOption_OwnerDrawDot(DrawingTools dt, STNodeOption op, Rectangle bounds)
        //         {
        //             var r = bounds.Width / 2;
        //             var pen = new Pen(Owner.Owner.HighLineColor);
        //             dt.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
        //             dt.Graphics.DrawRectangle(new Pen(dt.SolidBrush.Color), bounds.X, bounds.Y, bounds.Width, bounds.Height);
        //             dt.Graphics.DrawCross(pen, bounds.X + r, bounds.Y + r, r - 2);
        //         }
        // 
        //         protected internal override void OnConnected(STNodeOptionEventArgs e)
        //         {
        //             base.OnConnected(e);
        //             base.DisConnectionAll();
        //             e.TargetOption.DisConnectOption(this);
        //             var conn = this.AddHub(NewIndex());
        //             if (conn != null)
        //             {
        //                 conn.ConnectOption(e.TargetOption);
        //             }
        //         }
        //         protected virtual void Hub_DisConnected(object sender, STNodeOptionEventArgs e)
        //         {
        //             if (sender is STMemberOption member)
        //             {
        //                 RemoveHub(member);
        //                 ReIndex();
        //             }
        //         }
        // 
        //         protected List<STMemberOption> members = new List<STMemberOption>();
        //         protected int MemberCount { get => members.Count; }
        //         protected STMemberOption AddHub(int index)
        //         {
        //             if (members.Exists(e => e.Index.Equals(index)))
        //             {
        //                 return null;
        //             }
        //             EventBehaviorNode.ToMonoMemberFieldName(this.FieldName, index, out var fname);
        //             ToMemberText(index, out var tname);
        //             var hub = new STMemberOption(index, fname, tname, MemberType);
        //             hub.DisConnected += Hub_DisConnected;
        //             hub.BeforeConnecting += Hub_BeforeConnecting;
        //             if (this.IsInput)
        //             {
        //                 Owner.InputOptions.Insert(Owner.InputOptions.IndexOf(this), hub);
        //             }
        //             else
        //             {
        //                 Owner.OutputOptions.Insert(Owner.OutputOptions.IndexOf(this), hub);
        //             }
        //             members.Add(hub);
        //             if (this.Owner.Owner != null) this.Owner.Owner.BuildLinePath();
        //             if (Owner.AutoSize) Owner.RebuildSize();
        //             return hub;
        //         }
        // 
        //         private bool Hub_BeforeConnecting(STNodeOption src, STNodeOption dst)
        //         {
        //             if (src.Owner == dst.Owner && src.Owner == this.Owner && src.OwnerOptions == this.OwnerOptions)
        //             {
        //                 // swap option
        //                 if (this.OwnerOptions.Swap(src, dst))
        //                 {
        //                     this.members.Swap(src, dst); 
        //                     ReIndex();
        //                     this.Owner.Owner.Refresh();
        //                     return false;
        //                 }
        //             }
        //             return true;
        //         }
        // 
        //         protected bool RemoveHub(STMemberOption hub)
        //         {
        //             if (members.Remove(hub))
        //             {
        //                 if (hub.IsInput)
        //                 {
        //                     Owner.InputOptions.Remove(hub);
        //                 }
        //                 else
        //                 {
        //                     Owner.OutputOptions.Remove(hub);
        //                 }
        //                 if (this.Owner.Owner != null) this.Owner.Owner.BuildLinePath();
        //                 if (Owner.AutoSize) Owner.RebuildSize();
        //                 return true;
        //             }
        //             return false;
        //         }
        // 
        //         protected virtual void ToMemberText(int index, out string textName)
        //         {
        //             textName = $"{FieldName}[{index}]";
        //         }
        // 
        //         protected virtual int NewIndex()
        //         {
        //             return MemberCount;
        //         }
        //         protected virtual void ReIndex()
        //         {
        //             int count = 0;
        //             foreach (var member in members)
        //             {
        //                 var index = count;
        //                 EventBehaviorNode.ToMonoMemberFieldName(this.FieldName, index, out var fname);
        //                 ToMemberText(index, out var tname);
        //                 member.SetIndex(index, fname, tname);
        //                 count++;
        //             }
        //         }
        public override void Init(EventExternalizable owner, List<LinkOption> options)
        {
            base.Init(owner, options);
            Mono.Init(owner, options);
            //             var links = options.FindAll(link => EventBehaviorNode.TryParseMonoField(link.OwnerFieldName, out var monoFieldName, out var index) && monoFieldName == this.FieldName);
            //             foreach (var link in links)
            //             {
            //                 if (EventBehaviorNode.TryParseMonoField(link.OwnerFieldName, out var monoFieldName, out var index))
            //                 {
            //                     AddHub(index);
            //                 }
            //             }
        }
        public override void Load(BehaviorNodeEditor panel, List<LinkOption> options)
        {
            base.Load(panel, options);
            Mono.Load(panel, options);
            //             var links = options.FindAll(link => EventBehaviorNode.TryParseMonoField(link.OwnerFieldName, out var monoFieldName, out var index) && monoFieldName == this.FieldName);
            //             foreach (var link in links)
            //             {
            //                 if (members.TryFind(m => m.FieldName == link.OwnerFieldName, out var member))
            //                 {
            //                     member.LoadLinks(panel, links);
            //                 }
            //             }
        }
        public override void Save(BehaviorNodeEditor panel, List<LinkOption> options)
        {
            base.Save(panel, options);
            Mono.Save(panel, options);
            //             ReIndex();
            //             foreach (var member in members)
            //             {
            //                 member.SaveLinks(panel, options);
            //             }
        }
    }
    public class STFieldMonoActionsOption : STFieldMonoCollectionOption
    {
        public STFieldMonoActionsOption(IDynamicFieldInfo field, Type memberType) : base(field, $"{DescAttribute.GetDesc(field.Field)} +", memberType)
        {
            base.Mono.ToMemberText += this.ToMemberText;
        }
        bool ToMemberText(int index, out string text)
        {
            text = $"{DescAttribute.GetDesc(Field.Field)}[{index}] CALL";
            return true;
        }
    }
    public class STFieldMonoValuesOption : STFieldMonoCollectionOption
    {
        public STFieldMonoValuesOption(IDynamicFieldInfo field, Type memberType) : base(field, $"+ {STBehaviorLayout.GetValueTypeName(memberType)} : {DescAttribute.GetDesc(field.Field)}", memberType)
        {
            base.Mono.ToMemberText += this.ToMemberText;
        }
        bool ToMemberText(int index, out string text)
        {
            text = $"IN {STBehaviorLayout.GetValueTypeName(MemberType)} : {DescAttribute.GetDesc(Field.Field)}[{index}]";
            return true;
        }
    }

    #endregion MonoOptions 
    //---------------------------------------------------------------------------------------------------------------------------------------
    #region  StereoOptions ---------------------------------------------------------------------------------------------------------------------------------------
    public delegate bool ToStereoMemberText(int index, out string inText, out string outText);
    public class STDataOptionStereoCollection
    {
        private List<KeyValuePair<STMemberOption, STMemberOption>> members = new();
        public STDataOption Option { get; }
        public StereoOptionAttribute StereoAttr { get; }
        public int MemberCount { get => members.Count; }
        public event ToStereoMemberText ToMemberText;
        public STDataOptionStereoCollection(STDataOption owner, StereoOptionAttribute attr)
        {
            this.Option = owner;
            this.StereoAttr = attr;
            Option.IsFullLine = true;
            Option.OwnerDrawDot += Option_OwnerDrawDot;
            Option.OwnerDrawText += Option_OwnerDrawText;
            Option.Connected += Option_Connected;
        }
        public virtual void Init(EventExternalizable owner, List<LinkOption> options)
        {
            var linksIn = options.FindAll(link => EventBehaviorNode.TryParseStereoField(link.OwnerFieldName, out var dataFieldName, out var attrName, out var index) && dataFieldName == Option.FieldName && attrName == StereoAttr.InputName);
            var linksOut = options.FindAll(link => EventBehaviorNode.TryParseStereoField(link.OwnerFieldName, out var dataFieldName, out var attrName, out var index) && dataFieldName == Option.FieldName && attrName == StereoAttr.OutputName);
            foreach (var linkIn in linksIn)
            {
                if (EventBehaviorNode.TryParseStereoField(linkIn.OwnerFieldName, out var dataFieldName, out var stereoName, out var index))
                {
                    AddHub(index, out var _in, out var _out);
                }
            }
            foreach (var linkOut in linksOut)
            {
                if (EventBehaviorNode.TryParseStereoField(linkOut.OwnerFieldName, out var dataFieldName, out var stereoName, out var index))
                {
                    AddHub(index, out var _in, out var _out);
                }
            }
        }
        public virtual void Load(BehaviorNodeEditor panel, List<LinkOption> options)
        {
            var linksIn = options.FindAll(link => EventBehaviorNode.TryParseStereoField(link.OwnerFieldName, out var dataFieldName, out var attrName, out var index) && dataFieldName == Option.FieldName && attrName == StereoAttr.InputName);
            var linksOut = options.FindAll(link => EventBehaviorNode.TryParseStereoField(link.OwnerFieldName, out var dataFieldName, out var attrName, out var index) && dataFieldName == Option.FieldName && attrName == StereoAttr.OutputName);
            foreach (var linkIn in linksIn)
            {
                if (members.TryFind(m => m.Key.FieldName == linkIn.OwnerFieldName, out var member))
                {
                    member.Key.LoadLinks(panel, linksIn);
                }
            }
            foreach (var linkOut in linksOut)
            {
                if (members.TryFind(m => m.Value.FieldName == linkOut.OwnerFieldName, out var member))
                {
                    member.Value.LoadLinks(panel, linksOut);
                }
            }
        }
        public virtual void Save(BehaviorNodeEditor panel, List<LinkOption> options)
        {
            ReIndex();
            foreach (var member in members)
            {
                member.Key.SaveLinks(panel, options);
                member.Value.SaveLinks(panel, options);
            }
        }
        protected virtual void Option_OwnerDrawDot(DrawingTools dt, STNodeOption op, Rectangle bounds)
        {
            var r = bounds.Width / 2;
            var pen = new Pen(Option.Owner.Owner.HighLineColor);
            dt.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            dt.Graphics.DrawRectangle(new Pen(dt.SolidBrush.Color), bounds.X, bounds.Y, bounds.Width, bounds.Height);
            dt.Graphics.DrawCross(pen, bounds.X + r, bounds.Y + r, r - 2);
        }
        protected virtual void Option_OwnerDrawText(DrawingTools dt, STNodeOption op, Rectangle bounds)
        {
            var penH = new Pen(Option.Owner.Owner.HighLineColor);
            penH.Width = 2f;
            var pen = new Pen(Option.TextColor.ToDark(0.5f));
            var h1 = op.Editor.HoverConnection.Input;
            var h2 = op.Editor.HoverConnection.Output;
            foreach (var m in members)
            {
                var pos1 = m.Key.TextRectangle.Location + m.Key.TextRectangle.Size;
                var pos2 = m.Value.TextRectangle.Location + new Size(0, m.Value.TextRectangle.Height);
                if (h1 == m.Key || h1 == m.Value || h2 == m.Key || h2 == m.Value)
                {
                    dt.Graphics.DrawLine(penH, pos1, pos2);
                    m.Key.DrawBeziers(dt.Graphics, penH);
                    m.Value.DrawBeziers(dt.Graphics, penH);
                }
                else
                {
                    dt.Graphics.DrawLine(pen, pos1, pos2);
                    m.Key.DrawBeziers(dt.Graphics, penH);
                }
            }
        }
        protected virtual void Option_Connected(object sender, STNodeOptionEventArgs e)
        {
            Option.DisConnectionAll();
            e.TargetOption.DisConnectOption(Option);
            this.AddHub(NewIndex(), out var _in, out var _out);
            if (_out != null)
            {
                _out.ConnectOption(e.TargetOption);
            }
        }
        protected virtual bool AddHub(int index, out STMemberOption _in, out STMemberOption _out)
        {
            _out = null;
            _in = null;
            if (members.Exists(e => e.Key.Index.Equals(index)))
            {
                return false;
            }
            EventBehaviorNode.ToStereoMemberFieldName(Option.FieldName, StereoAttr, index, out var inFieldName, out var outFieldName);
            GetMemberText(index, out var inText, out var outText);
            var input = new STMemberOption(index, inFieldName, inText, StereoAttr.InputType);
            var output = new STMemberOption(index, outFieldName, outText, StereoAttr.OutputType);
            input.DisConnected += Hub_DisConnected;
            output.DisConnected += Hub_DisConnected;
            input.Counterpart = output;
            output.Counterpart = input;
            Option.Owner.InputOptions.Add(input);
            Option.Owner.OutputOptions.Insert(Option.Owner.OutputOptions.IndexOf(Option), output);
            members.Add(new KeyValuePair<STMemberOption, STMemberOption>(input, output));
            _in = input;
            _out = output;
            if (Option.Owner.Owner != null) Option.Owner.Owner.BuildLinePath();
            if (Option.Owner.AutoSize) Option.Owner.RebuildSize();
            return true;
        }
        protected virtual void Hub_DisConnected(object sender, STNodeOptionEventArgs e)
        {
            if (sender is STMemberOption opt)
            {
                if (opt.IsInput)
                {
                    var m_in = opt;
                    if (m_in.ConnectionCount == 0 && m_in.Counterpart is STMemberOption m_out && m_out.ConnectionCount == 0)
                    {
                        RemoveHub(m_in, m_out);
                    }
                }
                if (opt.IsOutput)
                {
                    var m_out = opt;
                    if (m_out.ConnectionCount == 0 && m_out.Counterpart is STMemberOption m_in && m_in.ConnectionCount == 0)
                    {
                        RemoveHub(m_in, m_out);
                    }
                }
            }
        }
        protected virtual bool RemoveHub(STMemberOption _in, STMemberOption _out)
        {
            if (members.RemoveAll(e => e.Key == _in) > 0)
            {
                Option.Owner.InputOptions.Remove(_in);
                Option.Owner.OutputOptions.Remove(_out);
                if (Option.Owner.Owner != null) Option.Owner.Owner.BuildLinePath();
                if (Option.Owner.AutoSize) Option.Owner.RebuildSize();
                return true;
            }
            return false;
        }
        private void GetMemberText(int index, out string inText, out string outText)
        {
            if (ToMemberText != null && ToMemberText(index, out inText, out outText))
            {
                return;
            }
            inText = $"IN {StereoAttr.InputName}";
            outText = $"{StereoAttr.OutputName} OUT";
        }
        protected virtual int NewIndex()
        {
            return MemberCount;
        }
        protected virtual void ReIndex()
        {
            int count = 0;
            foreach (var member in members)
            {
                var index = count;
                EventBehaviorNode.ToStereoMemberFieldName(Option.FieldName, StereoAttr, index, out var inFieldName, out var outFieldName);
                GetMemberText(index, out var inText, out var outText);
                member.Key.SetIndex(index, inFieldName, inText);
                member.Value.SetIndex(index, outFieldName, outText);
                count++;
            }
        }
    }
    public abstract class STFieldStereoCollectionOption : STFieldOption
    {
        public Type MemberType { get; }
        public STDataOptionStereoCollection Stereo { get; }
        public StereoOptionAttribute StereoAttr { get; }
        protected STFieldStereoCollectionOption(IDynamicFieldInfo field, StereoOptionAttribute attr, string strText, Type memberType) : base(field, strText, attr.OutputType)
        {
            this.MemberType = memberType;
            this.StereoAttr = attr;
            this.Stereo = new STDataOptionStereoCollection(this, attr);
            //             this.IsFullLine = true;
            //             this.OwnerDrawDot += STFieldStereoCollectionOption_OwnerDrawDot;
            //             this.OwnerDrawText += STFieldStereoCollectionOption_OwnerDrawText;
        }
        // 
        //         private void STFieldStereoCollectionOption_OwnerDrawDot(DrawingTools dt, STNodeOption op, Rectangle bounds)
        //         {
        //             var r = bounds.Width / 2;
        //             var pen = new Pen(Owner.Owner.HighLineColor);
        //             dt.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
        //             dt.Graphics.DrawRectangle(new Pen(dt.SolidBrush.Color), bounds.X, bounds.Y, bounds.Width, bounds.Height);
        //             dt.Graphics.DrawCross(pen, bounds.X + r, bounds.Y + r, r - 2);
        //         }
        //         private void STFieldStereoCollectionOption_OwnerDrawText(DrawingTools dt, STNodeOption op, Rectangle bounds)
        //         {
        //             var pen = new Pen(this.TextColor.ToDark(0.5f));
        //             foreach (var m in members)
        //             {
        //                 var pos1 = m.Key.TextRectangle.Location + m.Key.TextRectangle.Size;
        //                 var pos2 = m.Value.TextRectangle.Location + new Size(0, m.Value.TextRectangle.Height);
        //                 dt.Graphics.DrawLine(pen, pos1, pos2);
        //             }
        //         }
        //         protected internal override void OnConnected(STNodeOptionEventArgs e)
        //         {
        //             base.OnConnected(e);
        //             base.DisConnectionAll();
        //             e.TargetOption.DisConnectOption(this);
        //             this.AddHub(NewIndex(), out var _in, out var _out);
        //             if (_out != null)
        //             {
        //                 _out.ConnectOption(e.TargetOption);
        //             }
        //         }
        //         protected virtual void Hub_DisConnected(object sender, STNodeOptionEventArgs e)
        //         {
        //             if (sender is STMemberOption opt)
        //             {
        //                 if (opt.IsInput)
        //                 {
        //                     var m_in = opt;
        //                     if (m_in.ConnectionCount == 0 && m_in.Counterpart is STMemberOption m_out && m_out.ConnectionCount == 0)
        //                     {
        //                         RemoveHub(m_in, m_out);
        //                     }
        //                 }
        //                 if (opt.IsOutput)
        //                 {
        //                     var m_out = opt;
        //                     if (m_out.ConnectionCount == 0 && m_out.Counterpart is STMemberOption m_in && m_in.ConnectionCount == 0)
        //                     {
        //                         RemoveHub(m_in, m_out);
        //                     }
        //                 }
        //             }
        //         }
        //         protected List<KeyValuePair<STMemberOption, STMemberOption>> members = new();
        //         protected int MemberCount { get => members.Count; }
        //         protected bool AddHub(int index, out STMemberOption _in, out STMemberOption _out)
        //         {
        //             _out = null;
        //             _in = null;
        //             if (members.Exists(e => e.Key.Index.Equals(index)))
        //             {
        //                 return false;
        //             }
        //             EventBehaviorNode.ToStereoMemberFieldName(this.FieldName, StereoAttr, index, out var inFieldName, out var outFieldName);
        //             ToMemberText(index, out var inText, out var outText);
        //             var input = new STMemberOption(index, inFieldName, inText, StereoAttr.InputType);
        //             var output = new STMemberOption(index, outFieldName, outText, StereoAttr.OutputType);
        //             input.DisConnected += Hub_DisConnected;
        //             output.DisConnected += Hub_DisConnected;
        //             input.Counterpart = output;
        //             output.Counterpart = input;
        //             Owner.InputOptions.Add(input);
        //             Owner.OutputOptions.Insert(Owner.OutputOptions.IndexOf(this), output);
        //             members.Add(new KeyValuePair<STMemberOption, STMemberOption>(input, output));
        //             _in = input;
        //             _out = output;
        //             if (this.Owner.Owner != null) this.Owner.Owner.BuildLinePath();
        //             if (Owner.AutoSize) Owner.RebuildSize();
        //             return true;
        //         }
        //         protected bool RemoveHub(STMemberOption _in, STMemberOption _out)
        //         {
        //             if (members.RemoveAll(e => e.Key == _in) > 0)
        //             {
        //                 Owner.InputOptions.Remove(_in);
        //                 Owner.OutputOptions.Remove(_out);
        //                 if (this.Owner.Owner != null) this.Owner.Owner.BuildLinePath();
        //                 if (Owner.AutoSize) Owner.RebuildSize();
        //                 return true;
        //             }
        //             return false;
        //         }
        //         protected virtual void ToMemberText(int index, out string inText, out string outText)
        //         {
        //             inText = $"IN {StereoAttr.InputName}";
        //             outText = $"{StereoAttr.OutputName} OUT";
        //         }
        //         protected virtual int NewIndex()
        //         {
        //             return MemberCount;
        //         }
        //         protected virtual void ReIndex()
        //         {
        //             int count = 0;
        //             foreach (var member in members)
        //             {
        //                 var index = count;
        //                 EventBehaviorNode.ToStereoMemberFieldName(this.FieldName, StereoAttr, index, out var inFieldName, out var outFieldName);
        //                 ToMemberText(index, out var inText, out var outText);
        //                 member.Key.SetIndex(index, inFieldName, inText);
        //                 member.Value.SetIndex(index, outFieldName, outText);
        //                 count++;
        //             }
        //         }
        public override void Init(EventExternalizable owner, List<LinkOption> options)
        {
            base.Init(owner, options);
            Stereo.Init(owner, options);
            //           var linksIn = options.FindAll(link => EventBehaviorNode.TryParseStereoField(link.OwnerFieldName, out var dataFieldName, out var attrName, out var index) && dataFieldName == FieldName && attrName == StereoAttr.InputName);
            //           var linksOut = options.FindAll(link => EventBehaviorNode.TryParseStereoField(link.OwnerFieldName, out var dataFieldName, out var attrName, out var index) && dataFieldName == FieldName && attrName == StereoAttr.OutputName);
            //           foreach (var linkIn in linksIn)
            //           {
            //               if (EventBehaviorNode.TryParseStereoField(linkIn.OwnerFieldName, out var dataFieldName, out var stereoName, out var index))
            //               {
            //                   AddHub(index, out var _in, out var _out);
            //               }
            //           }
            //           foreach (var linkOut in linksOut)
            //           {
            //               if (EventBehaviorNode.TryParseStereoField(linkOut.OwnerFieldName, out var dataFieldName, out var stereoName, out var index))
            //               {
            //                   AddHub(index, out var _in, out var _out);
            //               }
            //           }
        }
        public override void Load(BehaviorNodeEditor panel, List<LinkOption> options)
        {
            base.Load(panel, options);
            Stereo.Load(panel, options);
            //             var linksIn = options.FindAll(link => EventBehaviorNode.TryParseStereoField(link.OwnerFieldName, out var dataFieldName, out var attrName, out var index) && dataFieldName == FieldName && attrName == StereoAttr.InputName);
            //             var linksOut = options.FindAll(link => EventBehaviorNode.TryParseStereoField(link.OwnerFieldName, out var dataFieldName, out var attrName, out var index) && dataFieldName == FieldName && attrName == StereoAttr.OutputName);
            //             foreach (var linkIn in linksIn)
            //             {
            //                 if (members.TryFind(m => m.Key.FieldName == linkIn.OwnerFieldName, out var member))
            //                 {
            //                     member.Key.LoadLinks(panel, linksIn);
            //                 }
            //             }
            //             foreach (var linkOut in linksOut)
            //             {
            //                 if (members.TryFind(m => m.Value.FieldName == linkOut.OwnerFieldName, out var member))
            //                 {
            //                     member.Value.LoadLinks(panel, linksOut);
            //                 }
            //             }
        }
        public override void Save(BehaviorNodeEditor panel, List<LinkOption> options)
        {
            base.Save(panel, options);
            Stereo.Save(panel, options);
            //             ReIndex();
            //             foreach (var member in members)
            //             {
            //                 member.Key.SaveLinks(panel, options);
            //                 member.Value.SaveLinks(panel, options);
            //             }
        }
    }
    public class STFieldStereoValueAction : STFieldStereoCollectionOption
    {
        public STFieldStereoValueAction(IDynamicFieldInfo field, StereoOptionAttribute attr, Type memberType)
           : base(field, attr, $"{DescAttribute.GetDesc(field.Field)} +", memberType)
        {
            Stereo.ToMemberText += this.ToMemberText;
        }
        bool ToMemberText(int index, out string inText, out string outText)
        {
            inText = $"IN {StereoAttr.InputName}";
            outText = $"{StereoAttr.OutputName} CALL";
            return true;
        }
    }
    public class STFieldStereoValueValue : STFieldStereoCollectionOption
    {
        public STFieldStereoValueValue(IDynamicFieldInfo field, StereoOptionAttribute attr, Type memberType)
           : base(field, attr, $"{DescAttribute.GetDesc(field.Field)} +", memberType)
        {
            Stereo.ToMemberText += this.ToMemberText;
        }
        bool ToMemberText(int index, out string inText, out string outText)
        {
            inText = $"IN {StereoAttr.InputName}";
            outText = $"OUT {StereoAttr.OutputName}";
            return true;
        }
    }

    #endregion StereoOptions 
    //---------------------------------------------------------------------------------------------------------------------------------------
}
