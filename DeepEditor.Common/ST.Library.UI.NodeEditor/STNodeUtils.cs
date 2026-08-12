using DeepEditor.Common;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace ST.Library.UI.NodeEditor
{

    public delegate bool SelectTreeNode(STNode src, STNodeOption srcOption, STNode dst, STNodeOption dstOption);

    public static class STNodeUtils
    {


        public static Rectangle GetFullBounds<T>(this ICollection<T> nodes) where T : STNode
        {
            if (nodes.Count == 0)
            {
                return Rectangle.Empty;
            }
            int x = int.MaxValue;
            int y = int.MaxValue;
            int r = int.MinValue;
            int b = int.MinValue;
            foreach (STNode n in nodes)
            {
                if (x > n.Left) x = n.Left;
                if (y > n.Top) y = n.Top;
                if (r < n.Right) r = n.Right;
                if (b < n.Bottom) b = n.Bottom;
            }
            return new Rectangle(x, y, r - x, b - y);
        }

        public static void MoveNodes<T>(this ICollection<T> nodes, Point offset) where T : STNode
        {
            foreach (STNode n in nodes)
            {
                n.Left += offset.X;
                n.Top += offset.Y;
            }
        }
        public static void MoveNode<T>(this T n, Point offset) where T : STNode
        {
            n.Left += offset.X;
            n.Top += offset.Y;
        }



        public static List<STNode> GetInputNodes(this STNode node, SelectTreeNode select, bool fullTree = false)
        {
            var ret = new List<STNode>();
            GetInputNodes(node, select, ret, fullTree);
            return ret;
        }
        public static List<STNode> GetInputNodes(this STNode node, bool fullTree = false)
        {
            var ret = new List<STNode>();
            GetInputNodes(node, null, ret, fullTree);
            return ret;
        }
        public static void GetInputNodes(this STNode node, SelectTreeNode select, List<STNode> ret, bool fullTree = false)
        {
            if (node == null) return;
            foreach (STNodeOption op in node.InputOptions)
            {
                foreach (var next in op.ConnectedOption)
                {
                    if (select == null || select.Invoke(node, op, next.Owner, next))
                    {
                        ret.Add(next.Owner);
                        if (fullTree)
                        {
                            GetInputNodes(next.Owner, select, ret, fullTree);
                        }
                    }
                }
            }
        }
        public static List<STNode> GetOutputNodes(this STNode node, SelectTreeNode select, bool fullTree = false)
        {
            var ret = new List<STNode>();
            GetOutputNodes(node, select, ret, fullTree);
            return ret;
        }
        public static List<STNode> GetOutputNodes(this STNode node, bool fullTree = false)
        {
            var ret = new List<STNode>();
            GetOutputNodes(node, null, ret, fullTree);
            return ret;
        }
        public static void GetOutputNodes(this STNode node, SelectTreeNode select, List<STNode> ret, bool fullTree = false)
        {
            if (node == null) return;
            foreach (STNodeOption op in node.OutputOptions)
            {
                foreach (var next in op.ConnectedOption)
                {
                    if (select == null || select.Invoke(node, op, next.Owner, next))
                    {
                        ret.Add(next.Owner);
                        if (fullTree)
                        {
                            GetOutputNodes(next.Owner, select, ret, fullTree);
                        }
                    }
                }
            }
        }

        public static List<STNode> GetTreeChilds(this STNode src, SelectTreeNode selection = null)
        {
            var exists = new List<STNode>();
            GetTreeNodesInternal(src, selection, exists);
            exists.RemoveAt(0);
            return exists;
        }
        public static List<STNode> GetTreeNodes(this STNode src, SelectTreeNode selection = null)
        {
            var exists = new List<STNode>();
            GetTreeNodesInternal(src, selection, exists);
            return exists;
        }
        private static void GetTreeNodesInternal(this STNode src, SelectTreeNode selection, List<STNode> exist)
        {
            if (src == null) return;
            if (exist.Contains(src)) return;
            exist.Add(src);
            if (src.InputOptions != null)
            {
                foreach (STNodeOption input in src.InputOptions)
                {
                    if (input.ConnectedOption != null)
                    {
                        foreach (STNodeOption in_next in input.ConnectedOption)
                        {
                            if (selection == null || selection(src, input, in_next.Owner, in_next))
                            {
                                GetTreeNodesInternal(in_next.Owner, selection, exist);
                            }
                        }
                    }
                }
            }
            if (src.OutputOptions != null)
            {
                foreach (STNodeOption output in src.OutputOptions)
                {
                    if (output.ConnectedOption != null)
                    {
                        foreach (STNodeOption out_next in output.ConnectedOption)
                        {
                            if (selection == null || selection(src, output, out_next.Owner, out_next))
                            {
                                GetTreeNodesInternal(out_next.Owner, selection, exist);
                            }
                        }
                    }
                }
            }
        }





    }
}
