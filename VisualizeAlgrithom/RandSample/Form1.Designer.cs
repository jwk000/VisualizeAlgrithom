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
            michellSampleToolStripMenuItem = new ToolStripMenuItem();
            randomSampleToolStripMenuItem = new ToolStripMenuItem();
            poissonDiscSampleToolStripMenuItem = new ToolStripMenuItem();
            menuStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // menuStrip1
            // 
            menuStrip1.Items.AddRange(new ToolStripItem[] { randomSampleToolStripMenuItem, michellSampleToolStripMenuItem, poissonDiscSampleToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(800, 25);
            menuStrip1.TabIndex = 0;
            menuStrip1.Text = "menuStrip1";
            // 
            // michellSampleToolStripMenuItem
            // 
            michellSampleToolStripMenuItem.Name = "michellSampleToolStripMenuItem";
            michellSampleToolStripMenuItem.Size = new Size(104, 21);
            michellSampleToolStripMenuItem.Text = "MichellSample";
            michellSampleToolStripMenuItem.Click += MichellSample_Click;
            // 
            // randomSampleToolStripMenuItem
            // 
            randomSampleToolStripMenuItem.Name = "randomSampleToolStripMenuItem";
            randomSampleToolStripMenuItem.Size = new Size(112, 21);
            randomSampleToolStripMenuItem.Text = "RandomSample";
            randomSampleToolStripMenuItem.Click += RandomSample_Click;
            // 
            // poissonDiscSampleToolStripMenuItem
            // 
            poissonDiscSampleToolStripMenuItem.Name = "poissonDiscSampleToolStripMenuItem";
            poissonDiscSampleToolStripMenuItem.Size = new Size(132, 21);
            poissonDiscSampleToolStripMenuItem.Text = "PoissonDiscSample";
            poissonDiscSampleToolStripMenuItem.Click += PoissonDiscSample_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
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
    }
}