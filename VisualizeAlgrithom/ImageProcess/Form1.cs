using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Security.Permissions;
using System.Windows.Forms;
namespace ImageProcessDemo
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        //加载
        private void btnOpenImage_Click(object sender, EventArgs e)
        {
            var bmp = new Bitmap("1.jpg");
            if (bmp == null)
            {
                MessageBox.Show("加载图片失败!", "错误");
                return;
            }
            pictureBox1.Image = bmp;
        }


        //向右旋转图像90°代码如下:
        private void Rotate90(object sender, EventArgs e)
        {

            Graphics g = pictureBox2.CreateGraphics();
            Bitmap bmp = new Bitmap("1.jpg");//加载图像
            g.FillRectangle(Brushes.White, pictureBox2.DisplayRectangle);//填充窗体背景为白色
            Point[] destinationPoints = {
                new Point(200, 0), // destination for upper-left point of original
                new Point(200, 200),// destination for upper-right point of original
                new Point(0, 0)}; // destination for lower-left point of original
            g.DrawImage(bmp, destinationPoints);

        }


        //旋转图像180°代码如下:
        private void Rotate180(object sender, EventArgs e)
        {

            Graphics g = pictureBox2.CreateGraphics();
            Bitmap bmp = new Bitmap("1.jpg");
            g.FillRectangle(Brushes.White, pictureBox2.DisplayRectangle);
            Point[] destinationPoints = {
                new Point(0, 200), // destination for upper-left point of original
                new Point(200, 200),// destination for upper-right point of original
                new Point(0, 0)}; // destination for lower-left point of original
            g.DrawImage(bmp, destinationPoints);

        }
        //翻转90度
        private void btn_RotateFlip_Click(object sender, EventArgs e)
        {
            Image tmp = Image.FromFile("1.jpg");
            tmp.RotateFlip(RotateFlipType.Rotate90FlipNone);
            this.pictureBox2.Image = tmp;
        }

        //图像切变代码:
        private void QieBian(object sender, EventArgs e)
        {

            Graphics g = pictureBox2.CreateGraphics();
            Bitmap bmp = new Bitmap("1.jpg");
            g.FillRectangle(Brushes.White, pictureBox2.DisplayRectangle);
            Point[] destinationPoints = {
                new Point(0, 0), // destination for upper-left point of original
                new Point(200, 0), // destination for upper-right point of original
                new Point(100, 200)};// destination for lower-left point of original
            g.DrawImage(bmp, destinationPoints);

        }



        //图像截取:
        private void CutImage(object sender, EventArgs e)
        {
            Bitmap bmp = new Bitmap("1.jpg");
            Graphics g = pictureBox2.CreateGraphics();
            g.FillRectangle(Brushes.White, pictureBox2.DisplayRectangle);
            Rectangle sr = new Rectangle(50, 50, 100, 100);//要截取的矩形区域
            Rectangle dr = new Rectangle(0, 0, 200, 200);//要显示到Form的矩形区域
            g.DrawImage(bmp, dr, sr, GraphicsUnit.Pixel);

        }


        //改变图像大小:
        private void Scale(object sender, EventArgs e)
        {
            Graphics g = pictureBox2.CreateGraphics();
            Bitmap bmp = new Bitmap("1.jpg");
            g.FillRectangle(Brushes.White, pictureBox2.DisplayRectangle);
            int width = bmp.Width;
            int height = bmp.Height;
            // 改变图像大小使用低质量的模式
            g.InterpolationMode = InterpolationMode.NearestNeighbor;
            g.DrawImage(bmp,
                new Rectangle(10, 10, 120, 120), // source 
                new Rectangle(0, 0, width, height), // destination rectangle
                GraphicsUnit.Pixel);
            // 使用高质量模式
            //g.CompositingQuality = CompositingQuality.HighSpeed;
            //g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            //g.DrawImage(bmp,
            //    new Rectangle(130, 10, 120, 120),
            //    new Rectangle(0, 0, width, height),
            //    GraphicsUnit.Pixel);

        }


        //设置图像的分辩率:
        private void Resolution(object sender, EventArgs e)
        {
            Bitmap bmp = new Bitmap("1.jpg");
            Graphics g = pictureBox2.CreateGraphics();
            g.FillRectangle(Brushes.White, pictureBox2.DisplayRectangle);
            bmp.SetResolution(100f, 100f);
            g.DrawImage(bmp, 0, 0);
            //bmp.SetResolution(1200f, 1200f);
            //g.DrawImage(bmp, 0, 0);

        }

        //底片效果
        //原理: GetPixel方法获得每一点像素的值, 然后再使用SetPixel方法将取反后的颜色值设置到对应的点.
        private void DiPian(object sender, EventArgs e)
        {

            int Height = this.pictureBox2.Image.Height;
            int Width = this.pictureBox2.Image.Width;
            Bitmap newbitmap = new Bitmap(Width, Height);
            Bitmap oldbitmap = new Bitmap("1.jpg");
            Color pixel;
            for (int x = 1; x < Width; x++)
            {
                for (int y = 1; y < Height; y++)
                {
                    int r, g, b;
                    pixel = oldbitmap.GetPixel(x, y);
                    r = 255 - pixel.R;
                    g = 255 - pixel.G;
                    b = 255 - pixel.B;
                    newbitmap.SetPixel(x, y, Color.FromArgb(r, g, b));
                }
            }
            this.pictureBox2.Image = newbitmap;

        }

        //浮雕效果
        //原理: 对图像像素点的像素值分别与相邻像素点的像素值相减后加上128, 然后将其作为新的像素点的值.
        private void FuDiao(object sender, EventArgs e)
        {

            int Height = this.pictureBox2.Image.Height;
            int Width = this.pictureBox2.Image.Width;
            Bitmap newBitmap = new Bitmap(Width, Height);
            Bitmap oldBitmap = new Bitmap("1.jpg");
            Color pixel1, pixel2;
            for (int x = 0; x < Width - 1; x++)
            {
                for (int y = 0; y < Height - 1; y++)
                {
                    int r = 0, g = 0, b = 0;
                    pixel1 = oldBitmap.GetPixel(x, y);
                    pixel2 = oldBitmap.GetPixel(x + 1, y + 1);
                    r = Math.Abs(pixel1.R - pixel2.R + 128);
                    g = Math.Abs(pixel1.G - pixel2.G + 128);
                    b = Math.Abs(pixel1.B - pixel2.B + 128);
                    if (r > 255)
                        r = 255;
                    if (r < 0)
                        r = 0;
                    if (g > 255)
                        g = 255;
                    if (g < 0)
                        g = 0;
                    if (b > 255)
                        b = 255;
                    if (b < 0)
                        b = 0;
                    newBitmap.SetPixel(x, y, Color.FromArgb(r, g, b));
                }
            }
            this.pictureBox2.Image = newBitmap;

        }

        //三.黑白效果
        //原理: 彩色图像处理成黑白效果通常有3种算法；
        //(1).最大值法: 使每个像素点的 R, G, B 值等于原像素点的 RGB(颜色值) 中最大的一个；
        //(2).平均值法: 使用每个像素点的 R, G, B值等于原像素点的RGB值的平均值；
        //(3).加权平均值法: 对每个像素点的 R, G, B值进行加权
        // 自认为第三种方法做出来的黑白效果图像最 "真实".
        private void BlackWhite(object sender, EventArgs e)
        {
            Bitmap oldBitmap = new Bitmap("1.jpg");
            int Height = oldBitmap.Height;
            int Width = oldBitmap.Width;
            Bitmap newBitmap = new Bitmap(Width, Height);
            Color pixel;
            for (int x = 0; x < Width; x++)
                for (int y = 0; y < Height; y++)
                {
                    pixel = oldBitmap.GetPixel(x, y);
                    int r, g, b, Result = 0;
                    r = pixel.R;
                    g = pixel.G;
                    b = pixel.B;
                    //实例程序以加权平均值法产生黑白图像
                    int iType = 2;
                    switch (iType)
                    {
                        case 0://平均值法
                            Result = ((r + g + b) / 3);
                            break;
                        case 1://最大值法
                            Result = r > g ? r : g;
                            Result = Result > b ? Result : b;
                            break;
                        case 2://加权平均值法
                            Result = ((int)(0.7 * r) + (int)(0.2 * g) + (int)(0.1 * b));
                            break;
                    }
                    newBitmap.SetPixel(x, y, Color.FromArgb(Result, Result, Result));
                }
            this.pictureBox2.Image = newBitmap;

        }
        //灰度图
        private void btn_GetGray_Click(object sender, EventArgs e)
        {

            Bitmap currentBitmap = new Bitmap("1.jpg");
            Graphics g = Graphics.FromImage(currentBitmap);
            ImageAttributes ia = new ImageAttributes();

            float[][] colorMatrix =   {
                new   float[]   {0.299f,   0.299f,   0.299f,   0,   0},
                new   float[]   {0.587f,   0.587f,   0.587f,   0,   0},
                new   float[]   {0.114f,   0.114f,   0.114f,   0,   0},
                new   float[]   {0,   0,   0,   1,   0},
                new   float[]   {0,   0,   0,   0,   1}
            };

            ColorMatrix cm = new ColorMatrix(colorMatrix);
            ia.SetColorMatrix(cm, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
            g.DrawImage(currentBitmap, new Rectangle(0, 0, currentBitmap.Width, currentBitmap.Height), 0, 0, currentBitmap.Width, currentBitmap.Height, GraphicsUnit.Pixel, ia);
            this.pictureBox2.Image = (Image)(currentBitmap.Clone());
            g.Dispose();
        }



        //柔化效果
        //原理: 当前像素点与周围像素点的颜色差距较大时取其平均值.
        private void RouHua(object sender, EventArgs e)
        {
            Bitmap MyBitmap = new Bitmap("1.jpg");
            int Height = MyBitmap.Height;
            int Width = MyBitmap.Width;
            Bitmap bitmap = new Bitmap(Width, Height);
            Color pixel;
            //高斯模板
            int[] Gauss = {
                1, 2, 1,
                2, 4, 2,
                1, 2, 1 };
            for (int x = 1; x < Width - 1; x++)
                for (int y = 1; y < Height - 1; y++)
                {
                    int r = 0, g = 0, b = 0;
                    int Index = 0;
                    for (int col = -1; col <= 1; col++)
                        for (int row = -1; row <= 1; row++)
                        {
                            pixel = MyBitmap.GetPixel(x + row, y + col);
                            r += pixel.R * Gauss[Index];
                            g += pixel.G * Gauss[Index];
                            b += pixel.B * Gauss[Index];
                            Index++;
                        }
                    r /= 16;
                    g /= 16;
                    b /= 16;
                    //处理颜色值溢出
                    r = r > 255 ? 255 : r;
                    r = r < 0 ? 0 : r;
                    g = g > 255 ? 255 : g;
                    g = g < 0 ? 0 : g;
                    b = b > 255 ? 255 : b;
                    b = b < 0 ? 0 : b;
                    bitmap.SetPixel(x - 1, y - 1, Color.FromArgb(r, g, b));
                }
            this.pictureBox2.Image = bitmap;

        }

        //锐化效果
        //原理:突出显示颜色值大(即形成形体边缘)的像素点.
        private void RuiHua(object sender, EventArgs e)
        {
            Bitmap oldBitmap = new Bitmap("1.jpg");
            int Height = oldBitmap.Height;
            int Width = oldBitmap.Width;
            Bitmap newBitmap = new Bitmap(Width, Height);
            Color pixel;
            //拉普拉斯模板
            int[] Laplacian = { -1, -1, -1, -1, 9, -1, -1, -1, -1 };
            for (int x = 1; x < Width - 1; x++)
                for (int y = 1; y < Height - 1; y++)
                {
                    int r = 0, g = 0, b = 0;
                    int Index = 0;
                    for (int col = -1; col <= 1; col++)
                        for (int row = -1; row <= 1; row++)
                        {
                            pixel = oldBitmap.GetPixel(x + row, y + col); 
                            r += pixel.R * Laplacian[Index];
                            g += pixel.G * Laplacian[Index];
                            b += pixel.B * Laplacian[Index];
                            Index++;
                        }
                    //处理颜色值溢出
                    r = r > 255 ? 255 : r;
                    r = r < 0 ? 0 : r;
                    g = g > 255 ? 255 : g;
                    g = g < 0 ? 0 : g;
                    b = b > 255 ? 255 : b;
                    b = b < 0 ? 0 : b;
                    newBitmap.SetPixel(x - 1, y - 1, Color.FromArgb(r, g, b));
                }
            this.pictureBox2.Image = newBitmap;

        }

        //雾化效果
        //原理: 在图像中引入一定的随机值, 打乱图像中的像素值
        private void WuHua(object sender, EventArgs e)
        {
            System.Random MyRandom = new Random();

            Bitmap oldBitmap = new Bitmap("1.jpg");
            int Height = oldBitmap.Height;
            int Width = oldBitmap.Width;
            Bitmap newBitmap = new Bitmap(Width, Height);
            Color pixel;
            for (int x = 1; x < Width - 1; x++)
                for (int y = 1; y < Height - 1; y++)
                {
                    int k = MyRandom.Next(123456);
                    //像素块大小
                    int dx = x + k % 19;
                    int dy = y + k % 19;
                    if (dx >= Width)
                        dx = Width - 1;
                    if (dy >= Height)
                        dy = Height - 1;
                    pixel = oldBitmap.GetPixel(dx, dy);
                    newBitmap.SetPixel(x, y, pixel);
                }
            this.pictureBox2.Image = newBitmap;
        }


        //均值滤波
        private void btnAvgFilter_Click(object sender, EventArgs e)
        {
            var bmp = new Bitmap("1.jpg");
            GrayBitmapData gbmp = new GrayBitmapData(bmp);
            gbmp.AverageFilter(3);
            gbmp.ShowImage(pictureBox2);
        }

        //转换为灰度图
        private void btnToGray_Click(object sender, EventArgs e)
        {
            var bmp = new Bitmap("1.jpg");
            GrayBitmapData gbmp = new GrayBitmapData(bmp);
            gbmp.ShowImage(pictureBox2);
        }

        //图片亮度处理
        private void btn_Grap_Click(object sender, EventArgs e)
        {
            //亮度百分比
            int percent = 50;
            float v = 0.006F * percent;

            float[][] matrix = {
                new float[] { 1, 0, 0, 0, 0 },
                new float[] { 0, 1, 0, 0, 0 },
                new float[] { 0, 0, 1, 0, 0 },
                new float[] { 0, 0, 0, 1, 0 },
                new float[] { v, v, v, 0, 1 }
            };
            ColorMatrix cm = new ColorMatrix(matrix);
            ImageAttributes attr = new ImageAttributes();
            attr.SetColorMatrix(cm);

            Image tmp = Image.FromFile("1.jpg");
            Graphics g = Graphics.FromImage(tmp);

            try
            {
                Rectangle destRect = new Rectangle(0, 0, tmp.Width, tmp.Height);
                g.DrawImage(tmp, destRect, 0, 0, tmp.Width, tmp.Height, GraphicsUnit.Pixel, attr);
            }
            finally
            {
                g.Dispose();
            }

            this.pictureBox2.Image = (Image)tmp.Clone();
        }


        //抓屏将生成的图片显示在pictureBox2
        private void btn_Screen_Click(object sender, EventArgs e)
        {
            Image myImage = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
            Graphics g = Graphics.FromImage(myImage);
            g.CopyFromScreen(new Point(0, 0), new Point(0, 0), new Size(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height));
            g.Dispose();
            this.pictureBox2.SizeMode = PictureBoxSizeMode.StretchImage;
            this.pictureBox2.Image = myImage;
            myImage.Save("Screen", ImageFormat.Png);
        }


    }
}
