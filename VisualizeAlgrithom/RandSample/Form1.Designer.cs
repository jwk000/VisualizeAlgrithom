namespace RandSample
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
            menuStrip1 = new MenuStrip();
            randomSampleToolStripMenuItem = new ToolStripMenuItem();
            michellSampleToolStripMenuItem = new ToolStripMenuItem();
            poissonDiscSampleToolStripMenuItem = new ToolStripMenuItem();
            gridSampleToolStripMenuItem = new ToolStripMenuItem();
            menuStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // menuStrip1
            // 
            menuStrip1.Items.AddRange(new ToolStripItem[] { randomSampleToolStripMenuItem, michellSampleToolStripMenuItem, poissonDiscSampleToolStripMenuItem, gridSampleToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(800, 24);
            menuStrip1.TabIndex = 0;
            menuStrip1.Text = "menuStrip1";
            // 
            // randomSampleToolStripMenuItem
            // 
            randomSampleToolStripMenuItem.Name = "randomSampleToolStripMenuItem";
            randomSampleToolStripMenuItem.Size = new Size(103, 20);
            randomSampleToolStripMenuItem.Text = "RandomSample";
            randomSampleToolStripMenuItem.Click += RandomSample_Click;
            // 
            // michellSampleToolStripMenuItem
            // 
            michellSampleToolStripMenuItem.Name = "michellSampleToolStripMenuItem";
            michellSampleToolStripMenuItem.Size = new Size(97, 20);
            michellSampleToolStripMenuItem.Text = "MichellSample";
            michellSampleToolStripMenuItem.Click += MichellSample_Click;
            // 
            // poissonDiscSampleToolStripMenuItem
            // 
            poissonDiscSampleToolStripMenuItem.Name = "poissonDiscSampleToolStripMenuItem";
            poissonDiscSampleToolStripMenuItem.Size = new Size(121, 20);
            poissonDiscSampleToolStripMenuItem.Text = "PoissonDiscSample";
            poissonDiscSampleToolStripMenuItem.Click += PoissonDiscSample_Click;
            // 
            // gridSampleToolStripMenuItem
            // 
            gridSampleToolStripMenuItem.Name = "gridSampleToolStripMenuItem";
            gridSampleToolStripMenuItem.Size = new Size(80, 20);
            gridSampleToolStripMenuItem.Text = "GridSample";
            gridSampleToolStripMenuItem.Click += gridSampleToolStripMenuItem_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 397);
            Controls.Add(menuStrip1);
            MainMenuStrip = menuStrip1;
            Name = "Form1";
            Text = "Form1";
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private MenuStrip menuStrip1;
        private ToolStripMenuItem michellSampleToolStripMenuItem;
        private ToolStripMenuItem randomSampleToolStripMenuItem;
        private ToolStripMenuItem poissonDiscSampleToolStripMenuItem;
        private ToolStripMenuItem gridSampleToolStripMenuItem;
    }
}