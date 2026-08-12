using DeepEditor.Common.Drawing;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace DeepEditor.Common.Net
{
    public interface IListViewSession
    {
        string ID { get; }
        long TotalRecvBytes { get; }
        long TotalSentBytes { get; }
        string Status { get; }
        string IP { get; }
        int TracerSeconds { get; }

        void Update();
    }

    public partial class NetSessionListTracer : UserControl
    {
        public delegate void GetSessionsEventHandler(object sender, SessionList list);

        private GetSessionsEventHandler event_CreateSessions;

        public NetSessionListTracer()
        {
            InitializeComponent();
        }

        public SessionTag SelectedSessionItemTag
        {
            get
            {
                if (this.listViewSessions.SelectedItems.Count > 0) { return this.listViewSessions.SelectedItems[0].Tag as SessionTag; }
                return null;
            }
        }
        public IListViewSession SelectedSession
        {
            get
            {
                var item = SelectedSessionItemTag;
                return (item != null) ? item.Session : null;
            }
        }

        public event GetSessionsEventHandler CreateSessions
        {
            add { event_CreateSessions += value; }
            remove { event_CreateSessions -= value; }
        }
        protected virtual ListViewItem CreateListViewItem(IListViewSession session)
        {
            return new ListViewItem(session.ID);
        }
        protected virtual SessionTag CreateListViewItemTag(ListViewItem item, IListViewSession session)
        {
            return new SessionTag(item, session);
        }
        public void ForEachSessionItemTags(Action<ListViewItem, SessionTag> action)
        {
            foreach (ListViewItem item in listViewSessions.Items)
            {
                var tag = (item.Tag as SessionTag);
                action.Invoke(item, tag);
            }
        }

        protected virtual void timer3_Tick(object sender, EventArgs e)
        {
            SessionList sessions = new SessionList();
            if (event_CreateSessions != null)
                event_CreateSessions.Invoke(this, sessions);
            {
                FormUtils.ListViewItemTagRefresh(listViewSessions, sessions,
                    (item, dst) => { return (item.Tag as SessionTag).Session.Equals(dst); },
                    (dst) =>
                    {
                        var s = dst as IListViewSession;
                        var item = CreateListViewItem(s);
                        var tag = CreateListViewItemTag(item, s);
                        item.Tag = tag;
                        return item;
                    },
                    (item) => { });
            }
        }
        protected virtual void timer1_Tick(object sender, EventArgs e)
        {
            foreach (ListViewItem item in listViewSessions.Items)
            {
                (item.Tag as SessionTag).Refresh();
            }
            pictureBox.Refresh();
        }
        protected virtual void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            var item = SelectedSessionItemTag;
            if (item != null)
            {
                item.DrawTracker(e.Graphics, pictureBox.DisplayRectangle);
            }
        }

        public class SessionTag
        {
            public IListViewSession Session { get; private set; }
            public ListViewItem Item { get; private set; }
            internal BytesSecondRateTracker TrackerRead { get; private set; }
            internal BytesSecondRateTracker TrackerWrite { get; private set; }
            public SessionTag(ListViewItem item, IListViewSession session)
            {
                this.Session = session;
                this.Item = item;
                this.Item.SubItems.Add(session.Status);
                this.Item.SubItems.Add(session.IP);
                this.TrackerRead = new BytesSecondRateTracker(session.TracerSeconds, item.Font, Pens.Blue, Brushes.Blue);
                this.TrackerRead.Title = "Read";
                this.TrackerWrite = new BytesSecondRateTracker(session.TracerSeconds, item.Font, Pens.Red, Brushes.Red);
                this.TrackerWrite.Title = "Write";
            }
            internal protected virtual void Refresh()
            {
                this.Session.Update();
                this.TrackerRead.Record(Session.TotalRecvBytes);
                this.TrackerWrite.Record(Session.TotalSentBytes);
                this.Item.SubItems[1].Text = Session.Status;
                this.Item.SubItems[2].Text = Session.IP;
            }
            internal protected virtual void DrawTracker(Graphics g, Rectangle rect)
            {
                var sh = rect.Height / 2;
                this.TrackerRead.DrawGrap(g, rect.X + 2, rect.Y + 2, rect.Width - 4, sh - 4);
                rect.Y += sh;
                this.TrackerWrite.DrawGrap(g, rect.X + 2, rect.Y + 2, rect.Width - 4, sh - 4);
            }
            public override bool Equals(object obj)
            {
                return Session.Equals(obj);
            }
            public override int GetHashCode()
            {
                return Session.GetHashCode();
            }
        }

        public class SessionList : List<IListViewSession> { }


    }

}
