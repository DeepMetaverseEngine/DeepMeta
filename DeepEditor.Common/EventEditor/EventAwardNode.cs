using DeepCore.EventTrigger;
using DeepCore.EventTrigger.Data;
using DeepCore.Reflection;
using System;
using System.Drawing;
using System.Windows.Forms;
using System.Xml;

namespace DeepEditor.Common.EventEditor
{

    public abstract class EventTypeNode : TreeNode
    {
        public EventExternalizable Data { get; private set; }
        public Type BaseDataType { get; private set; }
        public TypeDescAttribute BaseDesc { get; private set; }
        public XmlDocument TextDocument { get; private set; }
        public EventTypeNode(Type valueType)
        {
            this.Name = valueType.FullName;
            if (TryCheckEventType(valueType, out var baseType, out var imageKey))
            {
                this.BaseDataType = baseType;
                this.ImageKey = this.SelectedImageKey = imageKey;
            }
            else
            {
                throw new Exception("对象必须为，临时变量，事件，条件，动作，值。");
            }
            this.BaseDesc = new TypeDescAttribute(BaseDataType);
        }

        public void SetData(object data)
        {
            if (data == null)
            {
                this.Data = null;
                this.Text = $"NULL as {BaseDataType.Name};";
                this.Nodes.Clear();
            }
            else
            {
                if (!BaseDataType.IsAssignableFrom(data.GetType())) { throw new Exception(); }
                this.Data = data as EventExternalizable;
                try
                {
                    this.TextDocument = EventStringBuilder.FunctionDocument(Data);
                    this.Text = TextDocument.InnerText;
                }
                catch (Exception err)
                {
                    this.Text = err.Message;
                }
                //CollectSubNodes(this);
            }
        }

        public static bool TryCheckEventType(Type valueType, out Type baseType, out string imageKey)
        {
            if (typeof(EventLocalVar).IsAssignableFrom(valueType))
            {
                baseType = typeof(EventLocalVar);
                imageKey = "icon_var.png";
                return true;
            }
            else if (typeof(AbstractTrigger).IsAssignableFrom(valueType))
            {
                baseType = typeof(AbstractTrigger);
                imageKey = "icon_quest_event.png";
                return true;
            }
            else if (typeof(AbstractCondition).IsAssignableFrom(valueType))
            {
                baseType = typeof(AbstractCondition);
                imageKey = "icon_quest_condition.png";
                return true;
            }
            else if (typeof(AbstractAction).IsAssignableFrom(valueType))
            {
                baseType = typeof(AbstractAction);
                imageKey = "icon_quest_result.png";
                return true;
            }
            else if (typeof(AbstractValue).IsAssignableFrom(valueType))
            {
                baseType = typeof(AbstractValue);
                imageKey = "icon_value.png";
                return true;
            }
            else
            {
                imageKey = null;
                baseType = null;
                return false;
            }
        }
        public static void CollectSubNodes(EventTypeNode dataNode)
        {
            var data = dataNode.Data;
            var dt = data != null ? data.GetType() : dataNode.BaseDataType;
            foreach (var field in dt.GetFields())
            {
                var ft = field.FieldType;
                if (TryCheckEventType(ft, out var btype, out var kimage))
                {
                    var fv = field.GetValue(data);
                    var fn = new EventSubNode(fv, ft);
                    dataNode.Nodes.Add(fn);
                    CollectSubNodes(fn);
                }
            }
        }

    }

    public class EventAwardNode : EventTypeNode
    {
        public EventAwardNode(object data) : base(data.GetType())
        {
            this.SetData(data);
        }
        public void Refresh()
        {
            this.Text = Data.ToString();
        }
        public void Draw(DrawTreeNodeEventArgs e)
        {
            // Draw the background and node text for a selected node.
            if ((e.State & TreeNodeStates.Selected) != 0)
            {
                // Draw the background of the selected node. The NodeBounds
                // method makes the highlight rectangle large enough to
                // include the text of a node tag, if one is present.
                e.Graphics.FillRectangle(Brushes.Green, NodeBounds(e.Node));

                // Retrieve the node font. If the node font has not been set,
                // use the TreeView font.
                Font nodeFont = e.Node.NodeFont;
                if (nodeFont == null) nodeFont = TreeView.Font;

                // Draw the node text.
                e.Graphics.DrawString(e.Node.Text, nodeFont, Brushes.White,
                    Rectangle.Inflate(e.Bounds, 2, 0));
            }
            // Use the default background and node text.
            else
            {
                e.DrawDefault = true;
            }

            // If a node tag is present, draw its string representation 
            // to the right of the label text.
            if (e.Node.Tag != null)
            {
                e.Graphics.DrawString(e.Node.Tag.ToString(), TreeView.Font,
                    Brushes.Yellow, e.Bounds.Right + 2, e.Bounds.Top);
            }

            // If the node has focus, draw the focus rectangle large, making
            // it large enough to include the text of the node tag, if present.
            if ((e.State & TreeNodeStates.Focused) != 0)
            {
                using (Pen focusPen = new Pen(Color.Black))
                {
                    focusPen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dot;
                    Rectangle focusBounds = NodeBounds(e.Node);
                    focusBounds.Size = new Size(focusBounds.Width - 1,
                    focusBounds.Height - 1);
                    e.Graphics.DrawRectangle(focusPen, focusBounds);
                }
            }
        }
        private Rectangle NodeBounds(TreeNode node)
        {
            // Set the return value to the normal node bounds.
            Rectangle bounds = node.Bounds;
            if (node.Tag != null)
            {
                // Retrieve a Graphics object from the TreeView handle
                // and use it to calculate the display width of the tag.
                Graphics g = TreeView.CreateGraphics();
                int tagWidth = (int)g.MeasureString(node.Tag.ToString(), TreeView.Font).Width + 6;

                // Adjust the node bounds using the calculated value.
                bounds.Offset(tagWidth / 2, 0);
                bounds = Rectangle.Inflate(bounds, tagWidth / 2, 0);
                bounds.Height += 100;
                g.Dispose();
            }

            return bounds;

        }
    }


    public class EventSubNode : EventTypeNode
    {
        public EventSubNode(object data, Type baseType) : base(baseType)
        {
            this.SetData(data);
        }
    }
}
