namespace Boid
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            timer1 = new System.Windows.Forms.Timer(components);
            toolStrip1 = new ToolStrip();
            tbSeek = new ToolStripLabel();
            tbFlee = new ToolStripLabel();
            tbPursuit = new ToolStripLabel();
            tbEvade = new ToolStripLabel();
            tbArrive = new ToolStripLabel();
            tbWander = new ToolStripLabel();
            tbBoids = new ToolStripLabel();
            toolStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // toolStrip1
            // 
            toolStrip1.Items.AddRange(new ToolStripItem[] { tbSeek, tbFlee, tbPursuit, tbEvade, tbArrive, tbWander, tbBoids });
            toolStrip1.Location = new Point(0, 0);
            toolStrip1.Name = "toolStrip1";
            toolStrip1.Size = new Size(800, 28);
            toolStrip1.TabIndex = 0;
            toolStrip1.Text = "toolStrip1";
            // 
            // tbSeek
            // 
            tbSeek.DisplayStyle = ToolStripItemDisplayStyle.Text;
            tbSeek.Font = new Font("Segoe UI", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            tbSeek.Name = "tbSeek";
            tbSeek.Size = new Size(49, 25);
            tbSeek.Text = "seek";
            tbSeek.Click += tbSeek_Click;
            // 
            // tbFlee
            // 
            tbFlee.Font = new Font("Segoe UI", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            tbFlee.Name = "tbFlee";
            tbFlee.Size = new Size(43, 25);
            tbFlee.Text = "flee";
            tbFlee.Click += tbFlee_Click;
            // 
            // tbPursuit
            // 
            tbPursuit.Font = new Font("Segoe UI", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            tbPursuit.Name = "tbPursuit";
            tbPursuit.Size = new Size(71, 25);
            tbPursuit.Text = "pursuit";
            tbPursuit.Click += tbPusuit_Click;
            // 
            // tbEvade
            // 
            tbEvade.Font = new Font("Segoe UI", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            tbEvade.Name = "tbEvade";
            tbEvade.Size = new Size(62, 25);
            tbEvade.Text = "evade";
            tbEvade.Click += tbEvade_Click;
            // 
            // tbArrive
            // 
            tbArrive.Font = new Font("Segoe UI", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            tbArrive.Name = "tbArrive";
            tbArrive.Size = new Size(60, 25);
            tbArrive.Text = "arrive";
            tbArrive.Click += tbArrive_Click;
            // 
            // tbWander
            // 
            tbWander.Font = new Font("Segoe UI", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            tbWander.Name = "tbWander";
            tbWander.Size = new Size(75, 25);
            tbWander.Text = "wander";
            tbWander.Click += tbWander_Click;
            // 
            // tbBoids
            // 
            tbBoids.Font = new Font("Segoe UI", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            tbBoids.Name = "tbBoids";
            tbBoids.Size = new Size(58, 25);
            tbBoids.Text = "boids";
            tbBoids.Click += tbBoids_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(toolStrip1);
            Name = "Form1";
            Text = "Form1";
            toolStrip1.ResumeLayout(false);
            toolStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Timer timer1;
        private ToolStrip toolStrip1;
        private ToolStripLabel tbSeek;
        private ToolStripLabel tbFlee;
        private ToolStripLabel tbPursuit;
        private ToolStripLabel tbEvade;
        private ToolStripLabel tbWander;
        private ToolStripLabel tbBoids;
        private ToolStripLabel tbArrive;
    }
}
