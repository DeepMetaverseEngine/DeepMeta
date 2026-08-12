using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;

namespace ST.Library.UI.NodeEditor
{
    public class STNodeOption
    {
        #region Properties

        public static readonly STNodeOption Empty = new STNodeOption();

        private STNode _Owner;
        /// <summary>
        /// 获取当前 Option 所属的 Node
        /// </summary>
        public STNode Owner
        {
            get { return _Owner; }
            internal set
            {
                if (value == _Owner) return;
                if (_Owner != null) this.DisConnectionAll();        //当所有者变更时 断开当前所有连接
                _Owner = value;
            }
        }
        public STNodeEditor Editor => Owner.Owner;
        public STNodeOptionCollection OwnerOptions
        {
            get
            {
                if (IsInput) return Owner.InputOptions;
                else return Owner.OutputOptions;
            }
        }
        private bool _IsSingle;
        /// <summary>
        /// 获取当前 Option 是否仅能被连接一次
        /// </summary>
        public bool IsSingle
        {
            get { return _IsSingle; }
        }

        private bool _IsInput;
        /// <summary>
        /// 获取当前 Option 是否是输入选项
        /// </summary>
        public bool IsInput
        {
            get { return _IsInput; }
            internal set { _IsInput = value; }
        }
        public bool IsOutput => !_IsInput;

        public STDockStatus DockStatus => IsInput ? STDockStatus.Input : STDockStatus.Output;

        public STNodeOption Counterpart { get; set; } = null;
        public bool IsFullLine { get; set; } = false;

        //         private Rectangle _Bounding;
        //         public Rectangle Bounding
        //         {
        //             get => _Bounding;
        //             internal set => _Bounding = value;
        //         }

        private Color _TextColor = Color.White;
        /// <summary>
        /// 获取或设置当前 Option 文本颜色
        /// </summary>
        public Color TextColor
        {
            get { return _TextColor; }
            internal set
            {
                if (value == _TextColor) return;
                _TextColor = value;
                this.Invalidate();
            }
        }

        private Color _DotColor = Color.Transparent;
        /// <summary>
        /// 获取或设置当前 Option 连接点的颜色
        /// </summary>
        public Color DotColor
        {
            get { return _DotColor; }
            internal set
            {
                if (value == _DotColor) return;
                _DotColor = value;
                this.Invalidate();
            }
        }

        private string _Text;
        /// <summary>
        /// 获取或设置当前 Option 显示文本
        /// 当AutoSize被设置时 无法修改此属性
        /// </summary>
        public string Text
        {
            get { return _Text; }
            internal set
            {
                if (value == _Text) return;
                _Text = value;
                if (this._Owner == null) return;
                this._Owner.BuildSize(true, true, true);
            }
        }

        private int _DotLeft;
        /// <summary>
        /// 获取当前 Option 连接点的左边坐标
        /// </summary>
        public int DotLeft
        {
            get { return _DotLeft; }
            internal set { _DotLeft = value; }
        }
        private int _DotTop;
        /// <summary>
        /// 获取当前 Option 连接点的上边坐标
        /// </summary>
        public int DotTop
        {
            get { return _DotTop; }
            internal set { _DotTop = value; }
        }

        private int _DotSize;
        /// <summary>
        /// 获取当前 Option 连接点的宽度
        /// </summary>
        public int DotSize
        {
            get { return _DotSize; }
            protected set { _DotSize = value; }
        }

        private Rectangle _TextRectangle;
        /// <summary>
        /// 获取当前 Option 文本区域
        /// </summary>
        public Rectangle TextRectangle
        {
            get { return _TextRectangle; }
            internal set { _TextRectangle = value; }
        }

        private object _Data;
        /// <summary>
        /// 获取或者设置当前 Option 所包含的数据
        /// </summary>
        public object Data
        {
            get { return _Data; }
            set
            {
                if (value != null)
                {
                    if (this._DataType == null) return;
                    var t = value.GetType();
                    if (t != this._DataType && !t.IsSubclassOf(this._DataType))
                    {
                        throw new ArgumentException("无效数据类型 数据类型必须为指定的数据类型或其子类");
                    }
                }
                _Data = value;
            }
        }

        private Type _DataType;
        /// <summary>
        /// 获取当前 Option 数据类型
        /// </summary>
        public Type DataType
        {
            get { return _DataType; }
            internal set { _DataType = value; }
        }

        //private Rectangle _DotRectangle;
        /// <summary>
        /// 获取当前 Option 连接点的区域
        /// </summary>
        public Rectangle DotRectangle
        {
            get
            {
                return new Rectangle(this._DotLeft, this._DotTop, this._DotSize, this._DotSize);
            }
        }
        /// <summary>
        /// 获取当前 Option 被连接的个数
        /// </summary>
        public int ConnectionCount
        {
            get { return m_hs_connected.Count; }
        }
        /// <summary>
        /// 获取当前 Option 所连接的 Option 集合
        /// </summary>
        internal HashSet<STNodeOption> ConnectedOption
        {
            get { return m_hs_connected; }
        }

        private object _Tag;
        /// <summary>
        /// 获取或设置用户自定义保存的数据
        /// </summary>
        public object Tag
        {
            get { return _Tag; }
            set { _Tag = value; }
        }


        /// <summary>
        /// 保存已经被连接的点
        /// </summary>
        protected HashSet<STNodeOption> m_hs_connected;

        //---------------------------------------------------------------------------------------------------------------------------
        public event DrawOptionDot OwnerDrawDot;
        public bool IsOwnerDrawDot { get => OwnerDrawDot != null; }
        internal bool TryOwnerDrawDot(DrawingTools dt, STNodeOption op, Rectangle bounds)
        {
            if (OwnerDrawDot != null)
            {
                OwnerDrawDot.Invoke(dt, op, bounds);
                return true;
            }
            return false;
        }
        public virtual void DrawDefaultDot(DrawingTools dt)
        {
            STNodeOption op = this;
            var t = typeof(object);
            Pen pen = dt.Pen;
            SolidBrush brush = dt.SolidBrush;
            Graphics g = dt.Graphics;
            if (op.IsSingle)
            {                              //单连接 圆形
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                if (op.DataType == t)
                {                     //未知类型绘制 否则填充
                    g.DrawEllipse(pen, op.DotRectangle.X, op.DotRectangle.Y, op.DotRectangle.Width - 1, op.DotRectangle.Height - 1);
                }
                else
                {
                    g.FillEllipse(brush, op.DotRectangle);
                }
//                 if (Owner.Owner.HoverOption == op)
//                 {
//                     brush.Color = Owner.Owner.HighLineColor;
//                     g.FillEllipse(brush, op.DotRectangle.X + 1, op.DotRectangle.Y + 1, op.DotRectangle.Width - 2, op.DotRectangle.Height - 2);
//                 }
            }
            else
            {                                        //多连接 矩形
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;
                if (op.DataType == t)
                {
                    g.DrawRectangle(pen, op.DotRectangle.X, op.DotRectangle.Y, op.DotRectangle.Width - 1, op.DotRectangle.Height - 1);
                }
                else
                {
                    g.FillRectangle(brush, op.DotRectangle);
                }
//                 if (Owner.Owner.HoverOption == op)
//                 {
//                     brush.Color = Owner.Owner.HighLineColor;
//                     g.FillRectangle(brush, op.DotRectangle.X + 1, op.DotRectangle.Y + 1, op.DotRectangle.Width - 2, op.DotRectangle.Height - 2);
//                 }
            }
        }

        //---------------------------------------------------------------------------------------------------------------------------
        public event DrawOptionText OwnerDrawText;
        public bool IsOwnerDrawText { get => OwnerDrawText != null; }
        internal bool TryOwnerDrawText(DrawingTools dt, STNodeOption op, Rectangle bounds)
        {
            if (OwnerDrawText != null)
            {
                OwnerDrawText.Invoke(dt, op, bounds);
                return true;
            }
            return false;
        }
        public event DrawOptionBezier OwnerDrawBezier;
        public bool IsOwnerDrawBezier { get => OwnerDrawBezier != null; }
        internal bool TryOwnerDrawBezier(DrawingTools dt, STNodeOption op, STNodeOption next)
        {
            if (OwnerDrawBezier != null)
            {
                OwnerDrawBezier.Invoke(dt, op, next);
                return true;
            }
            return false;
        }

        //---------------------------------------------------------------------------------------------------------------------------
        #endregion Properties


        #region Constructor


        private STNodeOption() { }

        /// <summary>
        /// 构造一个 Option
        /// </summary>
        /// <param name="strText">显示文本</param>
        /// <param name="dataType">数据类型</param>
        /// <param name="bSingle">是否为单连接</param>
        public STNodeOption(string strText, Type dataType, bool bSingle)
        {
            if (dataType == null) throw new ArgumentNullException("指定的数据类型不能为空");
            this._DotSize = 10;
            m_hs_connected = new HashSet<STNodeOption>();
            this._DataType = dataType;
            this._Text = strText;
            this._IsSingle = bSingle;
        }

        #endregion Constructor

        #region Event

        public event STNodeOptionBeforeConnecting BeforeConnecting;

        /// <summary>
        /// 当被连接时候发生
        /// </summary>
        public event STNodeOptionEventHandler Connected;
        /// <summary>
        /// 当连接开始发生时发生
        /// </summary>
        public event STNodeOptionEventHandler Connecting;
        /// <summary>
        /// 当连接断开时候发生
        /// </summary>
        public event STNodeOptionEventHandler DisConnected;
        /// <summary>
        /// 当连接开始断开时发生
        /// </summary>
        public event STNodeOptionEventHandler DisConnecting;
        /// <summary>
        /// 当有数据传递时候发生
        /// </summary>
        public event STNodeOptionEventHandler DataTransfer;
        public event MouseEventHandler OnMouseDown;
        #endregion Event

        #region protected
        /// <summary>
        /// 重绘整个控件
        /// </summary>
        protected void Invalidate()
        {
            if (this._Owner == null) return;
            this._Owner.Invalidate();
        }
        /*
         * 开始我认为应当只有输入类型的选项才具有事件 因为输入是被动的 而输出则是主动的
         * 但是后来发现 比如在STNodeHub中输出节点就用到了事件
         * 以防万一所以这里代码注释起来了 也并不是很有问题 输出选项不注册事件也是一样的效果
         */
        protected internal virtual void OnConnected(STNodeOptionEventArgs e)
        {
            if (this.Connected != null/* && this._IsInput*/) this.Connected(this, e);
        }
        protected internal virtual void OnConnecting(STNodeOptionEventArgs e)
        {
            if (this.Connecting != null) this.Connecting(this, e);
        }
        protected internal virtual void OnDisConnected(STNodeOptionEventArgs e)
        {
            if (this.DisConnected != null/* && this._IsInput*/) this.DisConnected(this, e);
        }
        protected internal virtual void OnDisConnecting(STNodeOptionEventArgs e)
        {
            if (this.DisConnecting != null) this.DisConnecting(this, e);
        }
        protected internal virtual void OnDataTransfer(STNodeOptionEventArgs e)
        {
            if (this.DataTransfer != null/* && this._IsInput*/) this.DataTransfer(this, e);
        }
        protected void STNodeEidtorConnected(STNodeEditorOptionEventArgs e)
        {
            if (this._Owner == null) return;
            if (this._Owner.Owner == null) return;
            this._Owner.Owner.OnOptionConnected(e);
        }
        protected void STNodeEidtorDisConnected(STNodeEditorOptionEventArgs e)
        {
            if (this._Owner == null) return;
            if (this._Owner.Owner == null) return;
            this._Owner.Owner.OnOptionDisConnected(e);
        }
        internal void InternalOnMouseDown(MouseEventArgs e) {
            OnMouseDown?.Invoke(this, e);
        }
        /// <summary>
        /// 当前 Option 开始连接目标 Option
        /// </summary>
        /// <param name="op">需要连接的 Option</param>
        /// <returns>是否允许继续操作</returns>
        protected virtual bool ConnectingOption(STNodeOption op)
        {
            if (this._Owner == null) return false;
            if (this._Owner.Owner == null) return false;
            STNodeEditorOptionEventArgs e = new STNodeEditorOptionEventArgs(op, this, ConnectionStatus.Connecting);
            this._Owner.Owner.OnOptionConnecting(e);
            this.OnConnecting(new STNodeOptionEventArgs(true, op, ConnectionStatus.Connecting));
            op.OnConnecting(new STNodeOptionEventArgs(false, this, ConnectionStatus.Connecting));
            return e.Continue;
        }
        /// <summary>
        /// 当前 Option 开始断开目标 Option
        /// </summary>
        /// <param name="op">需要断开的 Option</param>
        /// <returns>是否允许继续操作</returns>
        protected virtual bool DisConnectingOption(STNodeOption op)
        {
            if (this._Owner == null) return false;
            if (this._Owner.Owner == null) return false;
            STNodeEditorOptionEventArgs e = new STNodeEditorOptionEventArgs(op, this, ConnectionStatus.DisConnecting);
            this._Owner.Owner.OnOptionDisConnecting(e);
            this.OnDisConnecting(new STNodeOptionEventArgs(true, op, ConnectionStatus.DisConnecting));
            op.OnDisConnecting(new STNodeOptionEventArgs(false, this, ConnectionStatus.DisConnecting));
            return e.Continue;
        }

        #endregion protected

        #region public
        public void MoveOffset(int x, int y)
        {
            _TextRectangle.X += x;
            _TextRectangle.Y += y;
            _DotTop += y;
            _DotLeft += x;
        }

        /// <summary>
        /// 当前 Option 连接目标 Option
        /// </summary>
        /// <param name="op">需要连接的 Option</param>
        /// <returns>连接结果</returns>
        public virtual ConnectionStatus ConnectOption(STNodeOption op)
        {
            if (op == this) return ConnectionStatus.SameOwner;
            if (BeforeConnecting != null && !BeforeConnecting.Invoke(this, op))
            {
                return ConnectionStatus.Reject;
            }
            if (!this.ConnectingOption(op))
            {
                this.STNodeEidtorConnected(new STNodeEditorOptionEventArgs(op, this, ConnectionStatus.Reject));
                return ConnectionStatus.Reject;
            }

            var v = this.CanConnect(op);
            if (v != ConnectionStatus.Connected)
            {
                this.STNodeEidtorConnected(new STNodeEditorOptionEventArgs(op, this, v));
                return v;
            }
            v = op.CanConnect(this);
            if (v != ConnectionStatus.Connected)
            {
                this.STNodeEidtorConnected(new STNodeEditorOptionEventArgs(op, this, v));
                return v;
            }

            var a = op.AddConnection(this, false);
            var b = this.AddConnection(op, true);
            this.ControlBuildLinePath();
            if (a) op.DoConnected(this, false);
            if (b) this.DoConnected(op, true);

            this.STNodeEidtorConnected(new STNodeEditorOptionEventArgs(op, this, v));
            return v;
        }
        public event STNodeTestConnect TestConnect;
        /// <summary>
        /// 检测当前 Option 是否可以连接目标 Option
        /// </summary>
        /// <param name="op">需要连接的 Option</param>
        /// <returns>检测结果</returns>
        public virtual ConnectionStatus CanConnect(STNodeOption op)
        {
            if (this == STNodeOption.Empty || op == STNodeOption.Empty) return ConnectionStatus.EmptyOption;
            if (this._IsInput == op.IsInput) return ConnectionStatus.SameInputOrOutput;
            if (op.Owner == null || this._Owner == null) return ConnectionStatus.NoOwner;
            if (op.Owner == this._Owner) return ConnectionStatus.SameOwner;
            if (this._Owner.LockOption || op._Owner.LockOption) return ConnectionStatus.Locked;
            if (this._IsSingle && m_hs_connected.Count == 1) return ConnectionStatus.SingleOption;
            if (op.IsInput && STNodeEditor.CanFindNodePath(op.Owner, this._Owner)) return ConnectionStatus.Loop;
            if (m_hs_connected.Contains(op)) return ConnectionStatus.Exists;
            if (this._IsInput && op._DataType != this._DataType && !op._DataType.IsSubclassOf(this._DataType)) return ConnectionStatus.ErrorType;
            if (TestConnect != null) return TestConnect(op);
            return ConnectionStatus.Connected;
        }
        /// <summary>
        /// 当前 Option 断开目标 Option
        /// </summary>
        /// <param name="op">需要断开的 Option</param>
        /// <returns></returns>
        public virtual ConnectionStatus DisConnectOption(STNodeOption op)
        {
            if (!this.DisConnectingOption(op))
            {
                this.STNodeEidtorDisConnected(new STNodeEditorOptionEventArgs(op, this, ConnectionStatus.Reject));
                return ConnectionStatus.Reject;
            }

            if (op.Owner == null) return ConnectionStatus.NoOwner;
            if (this._Owner == null) return ConnectionStatus.NoOwner;
            if (op.Owner.LockOption && this._Owner.LockOption)
            {
                this.STNodeEidtorDisConnected(new STNodeEditorOptionEventArgs(op, this, ConnectionStatus.Locked));
                return ConnectionStatus.Locked;
            }

            var a = op.RemoveConnection(this, false);
            var b = this.RemoveConnection(op, true);
            this.ControlBuildLinePath();
            if (a) op.DoDisConnected(this, false);
            if (b) this.DoDisConnected(op, true);

            this.STNodeEidtorDisConnected(new STNodeEditorOptionEventArgs(op, this, ConnectionStatus.DisConnected));
            return ConnectionStatus.DisConnected;
        }
        /// <summary>
        /// 断开当前 Option 的所有连接
        /// </summary>
        public void DisConnectionAll()
        {
            if (this._DataType == null) return;
            var arr = m_hs_connected.ToArray();
            foreach (var v in arr)
            {
                this.DisConnectOption(v);
            }
        }
        /// <summary>
        /// 获取当前 Option 所连接的 Option 集合
        /// </summary>
        /// <returns>如果为null 则表示不存在所有者 否则返回集合</returns>
        public List<STNodeOption> GetConnectedOption()
        {
            if (this._DataType == null) return null;
            if (!this._IsInput)
                return m_hs_connected.ToList();
            List<STNodeOption> lst = new List<STNodeOption>();
            if (this._Owner == null) return null;
            if (this._Owner.Owner == null) return null;
            foreach (var v in this._Owner.Owner.GetConnectionInfo())
            {
                if (v.Output == this) lst.Add(v.Input);
            }
            return lst;
        }
        /// <summary>
        /// 向当前 Option 所连接的所有 Option 投递数据
        /// </summary>
        public void TransferData()
        {
            if (this._DataType == null) return;
            foreach (var v in m_hs_connected)
            {
                v.OnDataTransfer(new STNodeOptionEventArgs(true, this, ConnectionStatus.Connected));
            }
        }
        /// <summary>
        /// 向当前 Option 所连接的所有 Option 投递数据
        /// </summary>
        /// <param name="data">需要投递的数据</param>
        public void TransferData(object data)
        {
            if (this._DataType == null) return;
            this.Data = data;   //不是this._Data
            foreach (var v in m_hs_connected)
            {
                v.OnDataTransfer(new STNodeOptionEventArgs(true, this, ConnectionStatus.Connected));
            }
        }
        /// <summary>
        /// 向当前 Option 所连接的所有 Option 投递数据
        /// </summary>
        /// <param name="data">需要投递的数据</param>
        /// <param name="bDisposeOld">是否释放旧数据</param>
        public void TransferData(object data, bool bDisposeOld)
        {
            if (bDisposeOld && this._Data != null)
            {
                if (this._Data is IDisposable) ((IDisposable)this._Data).Dispose();
                this._Data = null;
            }
            this.TransferData(data);
        }

        #endregion public

        #region internal

        private bool AddConnection(STNodeOption op, bool bSponsor)
        {
            if (this._DataType == null) return false;
            bool b = m_hs_connected.Add(op);
            //DoConnected(op, bSponsor);
            return b;
        }

        private bool RemoveConnection(STNodeOption op, bool bSponsor)
        {
            if (this._DataType == null) return false;
            bool b = false;
            if (m_hs_connected.Contains(op))
            {
                b = m_hs_connected.Remove(op);
                //DoDisConnected(op, bSponsor);
            }
            return b;
        }

        private void DoConnected(STNodeOption op, bool bSponsor)
        {
            this.OnConnected(new STNodeOptionEventArgs(bSponsor, op, ConnectionStatus.Connected));
            if (this._IsInput) this.OnDataTransfer(new STNodeOptionEventArgs(bSponsor, op, ConnectionStatus.Connected));
        }
        private void DoDisConnected(STNodeOption op, bool bSponsor)
        {
            if (this._IsInput) this.OnDataTransfer(new STNodeOptionEventArgs(bSponsor, op, ConnectionStatus.DisConnected));
            this.OnDisConnected(new STNodeOptionEventArgs(bSponsor, op, ConnectionStatus.Connected));
        }

        #endregion internal

        #region private

        private void ControlBuildLinePath()
        {
            if (this.Owner == null) return;
            if (this.Owner.Owner == null) return;
            this.Owner.Owner.BuildLinePath();
        }

        #endregion


        public void DrawBezier(Graphics g, Pen pen, STNodeOption next)
        {
            STNodeOption op = this;
            Owner.Owner.Drawing.DrawBezier(g, pen,
                op.DotLeft + op.DotSize, op.DotTop + op.DotSize / 2,
                next.DotLeft - 1, next.DotTop + next.DotSize / 2, Owner.Owner.Drawing.Curvature);
        }
        public void DrawBeziers(Graphics g, Pen pen)
        {
            STNodeOption op = this;
            foreach(var next in op.GetConnectedOption())
            {
                Owner.Owner.Drawing.DrawBezier(g, pen,
                    op.DotLeft + op.DotSize, op.DotTop + op.DotSize / 2,
                    next.DotLeft - 1, next.DotTop + next.DotSize / 2, Owner.Owner.Drawing.Curvature);
            }
        }
    }
}
