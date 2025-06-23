using System.Diagnostics;
using System.Numerics;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrackBar;

namespace Boid
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            DoubleBuffered = true;
            BackColor = Color.Black;
            GameWorld.Instance.Width = this.ClientSize.Width;
            GameWorld.Instance.Height = this.ClientSize.Height;

            timer1.Interval = 16;
            timer1.Tick += (s, e) => this.Invalidate();
            timer1.Start();
            stopwatch.Start();
            Application.Idle += GameLoop;
        } 

        Stopwatch stopwatch = new Stopwatch();
        float accumulator = 0f;
        const float fixedDeltaTime = 1f / 60f; // 固定时间步长: 60FPS

        private void GameLoop(object? sender, EventArgs e)
        {
            while (AppStillIdle)
            {
                float deltaTime = (float)stopwatch.Elapsed.TotalSeconds;
                stopwatch.Restart();

                accumulator += deltaTime;
                while (accumulator >= fixedDeltaTime)
                {
                    GameWorld.Instance.Update(fixedDeltaTime);
                    accumulator -= fixedDeltaTime;
                }
            }
        }
        private bool AppStillIdle
        {
            get
            {
                NativeMethods.PeekMessage(out var msg, IntPtr.Zero, 0, 0, 0);
                return msg.message == 0;
            }
        }

        private static class NativeMethods
        {
            [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
            public struct Message { public IntPtr hWnd; public uint message; public IntPtr wParam; public IntPtr lParam; public uint time; public System.Drawing.Point p; }

            [System.Runtime.InteropServices.DllImport("user32.dll")]
            public static extern bool PeekMessage(out Message msg, IntPtr hWnd, uint messageFilterMin, uint messageFilterMax, uint flags);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            GameWorld.Instance.Render(e.Graphics);

        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            base.OnMouseClick(e);

            Vector2 pos = new Vector2(e.X, e.Y);
            if (e.Button == MouseButtons.Left)
            {

                GameWorld.Instance.SetTargetPos(pos);
                GameWorld.Instance.SelectAgent(pos);
            }
            else if (e.Button == MouseButtons.Right)
            {
                GameWorld.Instance.AddAgent(pos);
            }
        }

        private void tbSeek_Click(object sender, EventArgs e)
        {
            GameWorld.Instance.SetVehiclesBehavior(EBehavior.Seek);
        }

        private void tbFlee_Click(object sender, EventArgs e)
        {
            GameWorld.Instance.SetVehiclesBehavior(EBehavior.Flee);
        }

        private void tbPusuit_Click(object sender, EventArgs e)
        {
            GameWorld.Instance.SetVehiclesBehavior(EBehavior.Pursuit);
        }

        private void tbEvade_Click(object sender, EventArgs e)
        {
            GameWorld.Instance.SetVehiclesBehavior(EBehavior.Evade);
        }

        private void tbWander_Click(object sender, EventArgs e)
        {
            GameWorld.Instance.SetVehiclesBehavior(EBehavior.Wander);
        }

        private void tbBoids_Click(object sender, EventArgs e)
        {
            GameWorld.Instance.SetVehiclesBehavior(EBehavior.Boids);
        }

        private void tbArrive_Click(object sender, EventArgs e)
        {
            GameWorld.Instance.SetVehiclesBehavior(EBehavior.Arrive);
        }
    }


}
