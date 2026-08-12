using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Drawing;

namespace ST.Library.UI.NodeEditor
{
    public class STNodeCollection : IList<STNode>
    {
        public int Count { get { return m_nodes.Count; } }
        private readonly List<STNode> m_nodes = new List<STNode>();
        private STNodeEditor m_owner;
        public STNode First => m_nodes.FirstOrDefault();
        internal STNodeCollection(STNodeEditor owner)
        {
            if (owner == null) throw new ArgumentNullException("所有者不能为空");
            m_owner = owner;
        }
        internal void Sort()
        {
            m_nodes.Sort(static (a, b) => a.Priority - b.Priority);
        }
        //         public void MoveToEnd(STNode node) {
        //             if (this._Count < 1) return;
        //             if (m_nodes[this._Count - 1] == node) return;
        //             bool bFound = false;
        //             for (int i = 0; i < _Count - 1; i++) {
        //                 if (m_nodes[i] == node) {
        //                     bFound = true;
        //                 }
        //                 if (bFound) m_nodes[i] = m_nodes[i + 1];
        //             }
        //             m_nodes[this._Count - 1] = node;
        //         }

        public int Add(STNode node)
        {
            if (node == null) throw new ArgumentNullException("添加对象不能为空");
            int nIndex = this.IndexOf(node);
            if (-1 == nIndex)
            {
                node.Owner = m_owner;
                //node.BuildSize(true, true, false);
                m_nodes.Add(node);
                Sort();
                m_owner.BuildBounds();
                m_owner.OnNodeAdded(new STNodeEditorEventArgs(node));
                //m_owner.MoveEndCheckDockingNodes([node]);
                m_owner.Invalidate();
                //m_owner.Invalidate(m_owner.CanvasToControl(new Rectangle(node.Left - 5, node.Top - 5, node.Width + 10, node.Height + 10)));
                //Console.WriteLine(node.Rectangle);
            }
            return nIndex;
        }

        public void AddRange(STNode[] nodes)
        {
            if (nodes == null) throw new ArgumentNullException("添加对象不能为空");
            foreach (var n in nodes)
            {
                if (n == null) throw new ArgumentNullException("添加对象不能为空");
                if (-1 == this.IndexOf(n))
                {
                    n.Owner = m_owner;
                    m_nodes.Add(n);
                }
                m_owner.OnNodeAdded(new STNodeEditorEventArgs(n));
            }
            Sort();
            // m_owner.MoveEndCheckDockingNodes(nodes);
            m_owner.Invalidate();
            m_owner.BuildBounds();
        }

        public void Remove(STNode node)
        {
            int nIndex = this.IndexOf(node);
            if (nIndex != -1)
            {
                this.RemoveAt(nIndex);
            }
        }

        public void RemoveAt(int nIndex)
        {
            if (nIndex < 0 || nIndex >= this.Count)
                throw new IndexOutOfRangeException("索引越界");
            m_nodes[nIndex].Owner = null;
            m_owner.InternalRemoveSelectedNode(m_nodes[nIndex]);
            m_owner.RemoveCheckDocking(m_nodes[nIndex]);
            if (m_owner.ActiveNode == m_nodes[nIndex]) m_owner.SetActiveNode(null);
            m_owner.OnNodeRemoved(new STNodeEditorEventArgs(m_nodes[nIndex]));
            m_nodes.RemoveAt(nIndex);
            Sort();
            if (this.Count == 0)
            {             //当不存在节点时候 坐标系回归
                m_owner.ScaleCanvas(1, 0, 0);
                m_owner.MoveCanvas(10, 10, true, CanvasMoveArgs.All);
            }
            else
            {
                m_owner.Invalidate();
                m_owner.BuildBounds();
            }
        }

        public void Clear()
        {
            for (int i = 0; i < this.Count; i++)
            {
                m_nodes[i].Owner = null;
                foreach (STNodeOption op in m_nodes[i].InputOptions) op.DisConnectionAll();
                foreach (STNodeOption op in m_nodes[i].OutputOptions) op.DisConnectionAll();
                m_owner.OnNodeRemoved(new STNodeEditorEventArgs(m_nodes[i]));
                m_owner.InternalRemoveSelectedNode(m_nodes[i]);
                m_owner.RemoveCheckDocking(m_nodes[i]);
            }
            m_nodes.Clear();
            m_owner.SetActiveNode(null);
            m_owner.BuildBounds();
            m_owner.ScaleCanvas(1, 0, 0);       //当不存在节点时候 坐标系回归
            m_owner.MoveCanvas(10, 10, true, CanvasMoveArgs.All);
            m_owner.Invalidate();               //如果画布位置和缩放处于初始状态 上面两行代码并不会造成控件重绘
        }

        public bool Contains(STNode node)
        {
            return this.IndexOf(node) != -1;
        }

        public int IndexOf(STNode node)
        {
            return m_nodes.IndexOf(node);
        }

        public void Insert(int nIndex, STNode node)
        {
            if (nIndex < 0 || nIndex >= this.Count)
                throw new IndexOutOfRangeException("索引越界");
            if (node == null)
                throw new ArgumentNullException("插入对象不能为空");
            node.Owner = m_owner;
            m_nodes.Insert(nIndex, node);
            Sort();
            //node.BuildSize(true, true,false);
            m_owner.Invalidate();
            m_owner.BuildBounds();
        }

        public bool IsFixedSize
        {
            get { return false; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public STNode this[int nIndex]
        {
            get
            {
                if (nIndex < 0 || nIndex >= this.Count)
                    throw new IndexOutOfRangeException("索引越界");
                return m_nodes[nIndex];
            }
            set { throw new InvalidOperationException("禁止重新赋值元素"); }
        }

        public void CopyTo(STNode[] array, int index)
        {
            if (array == null)
                throw new ArgumentNullException("数组不能为空");
            m_nodes.CopyTo(array, index);
        }

        //============================================================================

        public STNode[] ToArray()
        {
            return m_nodes.ToArray();
        }

        void ICollection<STNode>.Add(STNode item)
        {
            ((ICollection<STNode>)m_nodes).Add(item);
        }

        bool ICollection<STNode>.Remove(STNode item)
        {
            return ((ICollection<STNode>)m_nodes).Remove(item);
        }

        public IEnumerator<STNode> GetEnumerator()
        {
            return ((IEnumerable<STNode>)m_nodes).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)m_nodes).GetEnumerator();
        }
    }
}
