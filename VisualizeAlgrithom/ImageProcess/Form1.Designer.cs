namespace ImageProcessDemo
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.加载ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.加载ToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.抓屏ToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.颜色处理ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.旋转90度ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.旋转180度ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.翻转ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.截取图片ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.切变ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.缩放ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.修改分辨率ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.底片ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.取灰度ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.浮雕ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.黑白ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.柔化ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.锐化ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.雾化ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.亮度处理ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.灰度图ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.滤波图ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(12, 28);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(200, 200);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // pictureBox2
            // 
            this.pictureBox2.Location = new System.Drawing.Point(239, 28);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(200, 200);
            this.pictureBox2.TabIndex = 1;
            this.pictureBox2.TabStop = false;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.加载ToolStripMenuItem,
            this.颜色处理ToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(450, 25);
            this.menuStrip1.TabIndex = 2;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // 加载ToolStripMenuItem
            // 
            this.加载ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.加载ToolStripMenuItem1,
            this.抓屏ToolStripMenuItem1});
            this.加载ToolStripMenuItem.Name = "加载ToolStripMenuItem";
            this.加载ToolStripMenuItem.Size = new System.Drawing.Size(44, 21);
            this.加载ToolStripMenuItem.Text = "文件";
            // 
            // 加载ToolStripMenuItem1
            // 
            this.加载ToolStripMenuItem1.Name = "加载ToolStripMenuItem1";
            this.加载ToolStripMenuItem1.Size = new System.Drawing.Size(100, 22);
            this.加载ToolStripMenuItem1.Text = "加载";
            this.加载ToolStripMenuItem1.Click += new System.EventHandler(this.btnOpenImage_Click);
            // 
            // 抓屏ToolStripMenuItem1
            // 
            this.抓屏ToolStripMenuItem1.Name = "抓屏ToolStripMenuItem1";
            this.抓屏ToolStripMenuItem1.Size = new System.Drawing.Size(100, 22);
            this.抓屏ToolStripMenuItem1.Text = "抓屏";
            this.抓屏ToolStripMenuItem1.Click += new System.EventHandler(this.btn_Screen_Click);
            // 
            // 颜色处理ToolStripMenuItem
            // 
            this.颜色处理ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.旋转90度ToolStripMenuItem,
            this.旋转180度ToolStripMenuItem,
            this.翻转ToolStripMenuItem,
            this.截取图片ToolStripMenuItem,
            this.切变ToolStripMenuItem,
            this.缩放ToolStripMenuItem,
            this.修改分辨率ToolStripMenuItem,
            this.底片ToolStripMenuItem,
            this.黑白ToolStripMenuItem,
            this.灰度图ToolStripMenuItem,
            this.取灰度ToolStripMenuItem,
            this.浮雕ToolStripMenuItem,
            this.柔化ToolStripMenuItem,
            this.锐化ToolStripMenuItem,
            this.雾化ToolStripMenuItem,
            this.亮度处理ToolStripMenuItem,
            this.滤波图ToolStripMenuItem});
            this.颜色处理ToolStripMenuItem.Name = "颜色处理ToolStripMenuItem";
            this.颜色处理ToolStripMenuItem.Size = new System.Drawing.Size(68, 21);
            this.颜色处理ToolStripMenuItem.Text = "图像处理";
            // 
            // 旋转90度ToolStripMenuItem
            // 
            this.旋转90度ToolStripMenuItem.Name = "旋转90度ToolStripMenuItem";
            this.旋转90度ToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.旋转90度ToolStripMenuItem.Text = "旋转90度";
            this.旋转90度ToolStripMenuItem.Click += new System.EventHandler(this.Rotate90);
            // 
            // 旋转180度ToolStripMenuItem
            // 
            this.旋转180度ToolStripMenuItem.Name = "旋转180度ToolStripMenuItem";
            this.旋转180度ToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.旋转180度ToolStripMenuItem.Text = "旋转180度";
            this.旋转180度ToolStripMenuItem.Click += new System.EventHandler(this.Rotate180);
            // 
            // 翻转ToolStripMenuItem
            // 
            this.翻转ToolStripMenuItem.Name = "翻转ToolStripMenuItem";
            this.翻转ToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.翻转ToolStripMenuItem.Text = "翻转";
            this.翻转ToolStripMenuItem.Click += new System.EventHandler(this.btn_RotateFlip_Click);
            // 
            // 截取图片ToolStripMenuItem
            // 
            this.截取图片ToolStripMenuItem.Name = "截取图片ToolStripMenuItem";
            this.截取图片ToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.截取图片ToolStripMenuItem.Text = "截取图片";
            this.截取图片ToolStripMenuItem.Click += new System.EventHandler(this.CutImage);
            // 
            // 切变ToolStripMenuItem
            // 
            this.切变ToolStripMenuItem.Name = "切变ToolStripMenuItem";
            this.切变ToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.切变ToolStripMenuItem.Text = "切变";
            this.切变ToolStripMenuItem.Click += new System.EventHandler(this.QieBian);
            // 
            // 缩放ToolStripMenuItem
            // 
            this.缩放ToolStripMenuItem.Name = "缩放ToolStripMenuItem";
            this.缩放ToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.缩放ToolStripMenuItem.Text = "缩放";
            this.缩放ToolStripMenuItem.Click += new System.EventHandler(this.Scale);
            // 
            // 修改分辨率ToolStripMenuItem
            // 
            this.修改分辨率ToolStripMenuItem.Name = "修改分辨率ToolStripMenuItem";
            this.修改分辨率ToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.修改分辨率ToolStripMenuItem.Text = "修改分辨率";
            this.修改分辨率ToolStripMenuItem.Click += new System.EventHandler(this.Resolution);
            // 
            // 底片ToolStripMenuItem
            // 
            this.底片ToolStripMenuItem.Name = "底片ToolStripMenuItem";
            this.底片ToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.底片ToolStripMenuItem.Text = "底片";
            this.底片ToolStripMenuItem.Click += new System.EventHandler(this.DiPian);
            // 
            // 取灰度ToolStripMenuItem
            // 
            this.取灰度ToolStripMenuItem.Name = "取灰度ToolStripMenuItem";
            this.取灰度ToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.取灰度ToolStripMenuItem.Text = "灰度图3";
            this.取灰度ToolStripMenuItem.Click += new System.EventHandler(this.btn_GetGray_Click);
            // 
            // 浮雕ToolStripMenuItem
            // 
            this.浮雕ToolStripMenuItem.Name = "浮雕ToolStripMenuItem";
            this.浮雕ToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.浮雕ToolStripMenuItem.Text = "浮雕";
            this.浮雕ToolStripMenuItem.Click += new System.EventHandler(this.FuDiao);
            // 
            // 黑白ToolStripMenuItem
            // 
            this.黑白ToolStripMenuItem.Name = "黑白ToolStripMenuItem";
            this.黑白ToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.黑白ToolStripMenuItem.Text = "灰度图1";
            this.黑白ToolStripMenuItem.Click += new System.EventHandler(this.BlackWhite);
            // 
            // 柔化ToolStripMenuItem
            // 
            this.柔化ToolStripMenuItem.Name = "柔化ToolStripMenuItem";
            this.柔化ToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.柔化ToolStripMenuItem.Text = "柔化";
            this.柔化ToolStripMenuItem.Click += new System.EventHandler(this.RouHua);
            // 
            // 锐化ToolStripMenuItem
            // 
            this.锐化ToolStripMenuItem.Name = "锐化ToolStripMenuItem";
            this.锐化ToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.锐化ToolStripMenuItem.Text = "锐化";
            this.锐化ToolStripMenuItem.Click += new System.EventHandler(this.RuiHua);
            // 
            // 雾化ToolStripMenuItem
            // 
            this.雾化ToolStripMenuItem.Name = "雾化ToolStripMenuItem";
            this.雾化ToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.雾化ToolStripMenuItem.Text = "雾化";
            this.雾化ToolStripMenuItem.Click += new System.EventHandler(this.WuHua);
            // 
            // 亮度处理ToolStripMenuItem
            // 
            this.亮度处理ToolStripMenuItem.Name = "亮度处理ToolStripMenuItem";
            this.亮度处理ToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.亮度处理ToolStripMenuItem.Text = "曝光";
            this.亮度处理ToolStripMenuItem.Click += new System.EventHandler(this.btn_Grap_Click);
            // 
            // 灰度图ToolStripMenuItem
            // 
            this.灰度图ToolStripMenuItem.Name = "灰度图ToolStripMenuItem";
            this.灰度图ToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.灰度图ToolStripMenuItem.Text = "灰度图2";
            this.灰度图ToolStripMenuItem.Click += new System.EventHandler(this.btnToGray_Click);
            // 
            // 滤波图ToolStripMenuItem
            // 
            this.滤波图ToolStripMenuItem.Name = "滤波图ToolStripMenuItem";
            this.滤波图ToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.滤波图ToolStripMenuItem.Text = "滤波图";
            this.滤波图ToolStripMenuItem.Click += new System.EventHandler(this.btnAvgFilter_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(450, 242);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 颜色处理ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 取灰度ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 翻转ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 亮度处理ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 灰度图ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 滤波图ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 雾化ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 锐化ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 柔化ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 黑白ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 浮雕ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 底片ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 加载ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 加载ToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem 抓屏ToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem 旋转90度ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 旋转180度ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 切变ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 截取图片ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 缩放ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 修改分辨率ToolStripMenuItem;
    }
}

