namespace RandPointTool
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
            txtLeftBottom = new TextBox();
            label1 = new Label();
            btnGen = new Button();
            btnExport = new Button();
            label2 = new Label();
            txtRightUp = new TextBox();
            label3 = new Label();
            txtCircle = new TextBox();
            label4 = new Label();
            txtRadius = new TextBox();
            txtNum = new TextBox();
            label5 = new Label();
            SuspendLayout();
            // 
            // txtLeftBottom
            // 
            txtLeftBottom.Location = new Point(51, 12);
            txtLeftBottom.Name = "txtLeftBottom";
            txtLeftBottom.Size = new Size(146, 23);
            txtLeftBottom.TabIndex = 0;
            txtLeftBottom.Text = "0,0,0";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(12, 15);
            label1.Name = "label1";
            label1.Size = new Size(33, 15);
            label1.TabIndex = 1;
            label1.Text = "左下";
            // 
            // btnGen
            // 
            btnGen.Location = new Point(600, 11);
            btnGen.Name = "btnGen";
            btnGen.Size = new Size(87, 23);
            btnGen.TabIndex = 2;
            btnGen.Text = "随机种怪";
            btnGen.UseVisualStyleBackColor = true;
            btnGen.Click += btnGen_Click;
            // 
            // btnExport
            // 
            btnExport.Location = new Point(600, 39);
            btnExport.Name = "btnExport";
            btnExport.Size = new Size(87, 23);
            btnExport.TabIndex = 3;
            btnExport.Text = "导出配置";
            btnExport.UseVisualStyleBackColor = true;
            btnExport.Click += btnExport_Click;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(12, 44);
            label2.Name = "label2";
            label2.Size = new Size(33, 15);
            label2.TabIndex = 5;
            label2.Text = "右上";
            // 
            // txtRightUp
            // 
            txtRightUp.Location = new Point(51, 41);
            txtRightUp.Name = "txtRightUp";
            txtRightUp.Size = new Size(146, 23);
            txtRightUp.TabIndex = 4;
            txtRightUp.Text = "0,0,0";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(205, 15);
            label3.Name = "label3";
            label3.Size = new Size(61, 15);
            label3.TabIndex = 7;
            label3.Text = "BOSS圆心";
            // 
            // txtCircle
            // 
            txtCircle.Location = new Point(272, 11);
            txtCircle.Name = "txtCircle";
            txtCircle.Size = new Size(146, 23);
            txtCircle.TabIndex = 6;
            txtCircle.Text = "0,0,0";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(205, 44);
            label4.Name = "label4";
            label4.Size = new Size(61, 15);
            label4.TabIndex = 9;
            label4.Text = "BOSS半径";
            // 
            // txtRadius
            // 
            txtRadius.Location = new Point(272, 40);
            txtRadius.Name = "txtRadius";
            txtRadius.Size = new Size(146, 23);
            txtRadius.TabIndex = 8;
            txtRadius.Text = "0";
            // 
            // txtNum
            // 
            txtNum.Location = new Point(491, 11);
            txtNum.Name = "txtNum";
            txtNum.Size = new Size(88, 23);
            txtNum.TabIndex = 10;
            txtNum.Text = "100";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(439, 15);
            label5.Name = "label5";
            label5.Size = new Size(46, 15);
            label5.TabIndex = 11;
            label5.Text = "种怪数";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(label5);
            Controls.Add(txtNum);
            Controls.Add(label4);
            Controls.Add(txtRadius);
            Controls.Add(label3);
            Controls.Add(txtCircle);
            Controls.Add(label2);
            Controls.Add(txtRightUp);
            Controls.Add(btnExport);
            Controls.Add(btnGen);
            Controls.Add(label1);
            Controls.Add(txtLeftBottom);
            Name = "Form1";
            Text = "Form1";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox txtLeftBottom;
        private Label label1;
        private Button btnGen;
        private Button btnExport;
        private Label label2;
        private TextBox txtRightUp;
        private Label label3;
        private TextBox txtCircle;
        private Label label4;
        private TextBox txtRadius;
        private TextBox txtNum;
        private Label label5;
    }
}
