using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace MyControlLibrary
{
    /// <summary>
    /// 单词连线控件
    /// </summary>
    public partial class LigatureControl: UserControl
    {
        private PointF startP;
        private bool startDraw=false;
        private Graphics DrawTools_Graphics;//目标绘图板       
        private Image orginalImg;//原始画布，用来保存已完成的绘图过程       
        private Graphics newgraphics;//中间画板
        private Image finishingImg;//中间画布，用来保存绘图过程中的痕迹
        public LigatureControl()
        {
            InitializeComponent();
        }

        private void LigatureControl_MouseDown(object sender, MouseEventArgs e)
        {
            startDraw = true;
            startP = new PointF(e.X, e.Y);
        }

        private void LigatureControl_MouseUp(object sender, MouseEventArgs e)
        {
            startDraw = false;
            //为了让完成后的绘图过程保留下来，要将中间图片绘制到原始画布上
            newgraphics = Graphics.FromImage(orginalImg);
            newgraphics.DrawImage(finishingImg, 0, 0);
            newgraphics.Dispose();
        }

        private void LigatureControl_MouseMove(object sender, MouseEventArgs e)
        {
            if (startDraw)
            {
                var img = (Image)orginalImg.Clone();
                newgraphics = Graphics.FromImage(img);
                newgraphics.DrawLine(new Pen(Brushes.Black, 3), startP, new PointF(e.X, e.Y));
                newgraphics.Dispose();//绘图完毕释放中间画板所占资源
                newgraphics = Graphics.FromImage(finishingImg);//另建一个中间画板,画布为中间图片
                newgraphics.DrawImage(img, 0, 0);//将图片画到中间图片
                newgraphics.Dispose();
                DrawTools_Graphics.DrawImage(img, 0, 0);//将图片画到目标画板上
                img.Dispose();
            }
          
        }

        private void LigatureControl_Load(object sender, EventArgs e)
        {
            var bmp = new Bitmap(this.Width, this.Height);
            var g = Graphics.FromImage(bmp);
            g.FillRectangle(new SolidBrush(this.BackColor), new Rectangle(0, 0,this.Width, this.Height));
            g.Dispose();
            DrawTools_Graphics = CreateGraphics();
            finishingImg = (Image)bmp.Clone();
            orginalImg = (Image)bmp.Clone();
        }
    }
}
