using System.Numerics;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrackBar;

namespace Boid
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            world.CreateWorld();

            timer1.Interval = 100;
            timer1.Tick += Tick;
            timer1.Start();
        }

        GameWorld world = new GameWorld();
        private void Tick(object? sender, EventArgs e)
        {
            world.Update(timer1.Interval);
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            world.Render(e.Graphics);

        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            base.OnMouseClick(e);
        }
    }


}
