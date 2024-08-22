namespace RandSample
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        Action<Graphics> mOnPaint;
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            if (mOnPaint != null)
            {
                mOnPaint.Invoke(e.Graphics);
            }
        }
        private void MichellSample_Click(object sender, EventArgs e)
        {
            Sample sample = new Sample();
            var ret = sample.MichellSample(ClientRectangle.Width, ClientRectangle.Height, 1000);

            mOnPaint = g =>
            {
                foreach (var p in ret)
                {
                    g.FillRectangle(Brushes.Black, new Rectangle(p, new Size(2, 2)));
                }
            };

            Invalidate();
        }

        private void RandomSample_Click(object sender, EventArgs e)
        {
            Sample sample = new Sample();
            var ret = sample.RandomSample(ClientRectangle.Width, ClientRectangle.Height, 1000);

            mOnPaint = g =>
            {
                foreach (var p in ret)
                {
                    g.FillRectangle(Brushes.Black, new Rectangle(p, new Size(2, 2)));
                }
            };

            Invalidate();

        }

        private void PoissonDiscSample_Click(object sender, EventArgs e)
        {
            Sample sample = new Sample();
            var ret = sample.PoissonDiscSample(ClientRectangle.Width, ClientRectangle.Height, 1000);

            mOnPaint = g =>
            {
                foreach (var p in ret)
                {
                    g.FillRectangle(Brushes.Black, new Rectangle(p, new Size(2, 2)));
                }
            };

            Invalidate();
        }

        private void gridSampleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Sample sample = new Sample();
            var ret = sample.GridSample(ClientRectangle.Width, ClientRectangle.Height, 100);

            mOnPaint = g =>
            {
                foreach (var p in ret)
                {
                    g.FillRectangle(Brushes.Black, new Rectangle(p, new Size(2, 2)));
                }
            };

            Invalidate();
        }
    }
}