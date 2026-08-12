using DeepCore;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace DeepEditor.Common.Drawing
{

    public class Tracker
    {
        protected readonly Font font;
        protected readonly Pen drawLine;
        protected readonly Brush drawString;

        private double[] track;
        private PointF[] points;
        private string status = "";
        private string status_max = "";
        private string status_min = "";
        private double min, max, avg;
        private int r_count = 0;

        public string StatusInfo
        {
            get { return status; }
            set { status = value; }
        }
        public string StatusMaxInfo
        {
            get { return status_max; }
            set { status_max = value; }
        }
        public string StatusMinInfo
        {
            get { return status_min; }
            set { status_min = value; }
        }
        public object UserTag { get; set; }

        public double HistoryMin { get; private set; }
        public double HistoryMax { get; private set; }
        public double LastValue { get { return track[track.Length - 1]; } }
        public double Max { get { return max; } }
        public double Min { get { return min; } }
        public double Average { get { return avg; } }

        public Tracker(int tracklen, Font font, Pen drawLine, Brush drawString)
        {
            this.font = font;
            this.track = new double[tracklen];
            this.points = new PointF[tracklen];
            this.HistoryMin = Double.MaxValue;
            this.HistoryMax = Double.MinValue;
            this.drawLine = drawLine;
            this.drawString = drawString;
        }

        public virtual void Record(double value)
        {
            for (int i = 1; i < track.Length; i++)
            {
                this.track[i - 1] = track[i];
            }
            this.track[track.Length - 1] = value;
            this.r_count++;

            this.HistoryMin = Math.Min(HistoryMin, value);
            this.HistoryMax = Math.Max(HistoryMax, value);
            this.max = Double.MinValue;
            this.min = Double.MaxValue;
            double sum = 0;
            for (int i = 0; i < track.Length; i++)
            {
                sum += track[i];
                if (max < track[i])
                {
                    max = track[i];
                }
                if (min > track[i])
                {
                    min = track[i];
                }
            }
            this.avg = sum / r_count;
        }

        public double[] GetGrapTrack()
        {
            return (double[])track.Clone();
        }

        protected virtual void DrawMap(Graphics g, float x, float y, float w, float h)
        {
            double pw = w / (double)points.Length;

            double vh = HistoryMax;

            if (vh != 0)
            {
                for (int i = 0; i < points.Length; i++)
                {
                    double px = x + i * pw;
                    double py = y + h - h * track[i] / vh;
                    points[i].X = (float)px;
                    points[i].Y = (float)py;
                }
                for (int i = 1; i < points.Length; i++)
                {
                    g.DrawLine(drawLine, points[i - 1], points[i]);
                }
            }
        }

        protected virtual void DrawInfo(Graphics g, string text, float x, float y)
        {
            g.DrawString(text, font, drawString, x, y);
        }

        public void DrawGrap(Graphics g, float x, float y, float w, float h)
        {
            float fh = Math.Max(g.MeasureString(status + "", font).Height + 2, 20);

            DrawMap(g, x + 1, y + 1, w - 2, h - 2 - fh);

            if (!string.IsNullOrEmpty(status_max))
            {
                DrawInfo(g, status_max, x + 2, y + 2);
            }
            if (!string.IsNullOrEmpty(status_min))
            {
                DrawInfo(g, status_min, x + 2, y + 2 + fh);
            }
            DrawInfo(g, status, x + 2, y + h - fh);

            g.DrawRectangle(drawLine, x, y, w - 1, h - 1);
        }
    }

    public class SecondRateTracker : Tracker
    {
        public double RecordValuePerSec { get; private set; }
        public double RecordTotalValue { get; private set; }

        private double prewTime = 0;
        private double prewTotal = 0;

        public SecondRateTracker(int tracklen, Font font, Pen drawLine, Brush drawString)
            : base(tracklen, font, drawLine, drawString)
        {

        }
        public override void Record(double total)
        {
            RecordTotalValue = total;
            var curTime = CUtils.TickTimeMS;
            if (prewTime == 0)
            {
                this.RecordValuePerSec = total;
                base.Record(RecordValuePerSec);
            }
            else
            {
                this.RecordValuePerSec = (total - prewTotal) / ((curTime - prewTime) / 1000d);
                base.Record(RecordValuePerSec);
            }
            prewTime = curTime;
            prewTotal = total;
        }
    }

    public class BytesSecondRateTracker : SecondRateTracker
    {
        private string title;
        public string Title
        {
            get { return title; }
            set { title = value; }
        }

        public BytesSecondRateTracker(int tracklen, Font font, Pen drawLine, Brush drawString)
            : base(tracklen, font, drawLine, drawString)
        {

        }
        public override void Record(double value)
        {
            base.Record(value);
            this.StatusInfo = string.Format("{0} : {1}/s  Avg={2}/s  Total={3}", Title,
                CUtils.ToBytesSizeString((long)this.RecordValuePerSec),
                CUtils.ToBytesSizeString((long)this.Average),
                CUtils.ToBytesSizeString((long)this.RecordTotalValue));
            this.StatusMaxInfo = "Max : " + CUtils.ToBytesSizeString((long)this.Max);
            this.StatusMinInfo = "Min : " + CUtils.ToBytesSizeString((long)this.Min);
        }

    }


}
