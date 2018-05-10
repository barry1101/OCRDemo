using System;
using System.Drawing;
using System.Windows.Forms;

namespace MyControlLibrary
{
    /// <summary>
    /// 单词连线控件
    /// </summary>
    public partial class LigatureControl : UserControl
    {
        private PointF _startP;
        private bool _startDraw = false;
        private Graphics _drawToolsGraphics;    //目标绘图板       
        private Image _orginalImg;  //原始画布，用来保存已完成的绘图过程       
        private Graphics _newgraphics;  //中间画板
        private Image _finishingImg;    //中间画布，用来保存绘图过程中的痕迹

        public LigatureControl()
        {
            InitializeComponent();
        }

        private void LigatureControl_MouseDown(object sender, MouseEventArgs e)
        {
            _startDraw = true;
            _startP = new PointF(e.X, e.Y);
        }

        private void LigatureControl_MouseUp(object sender, MouseEventArgs e)
        {
            _startDraw = false;
            //为了让完成后的绘图过程保留下来，要将中间图片绘制到原始画布上
            _newgraphics = Graphics.FromImage(_orginalImg);
            _newgraphics.DrawImage(_finishingImg, 0, 0);
            _newgraphics.Dispose();
        }

        private void LigatureControl_MouseMove(object sender, MouseEventArgs e)
        {
            if (_startDraw)
            {
                var img = (Image)_orginalImg.Clone();
                _newgraphics = Graphics.FromImage(img);
                _newgraphics.DrawLine(new Pen(Brushes.Black, 3), _startP, new PointF(e.X, e.Y));
                _newgraphics.Dispose(); //绘图完毕释放中间画板所占资源
                _newgraphics = Graphics.FromImage(_finishingImg);   //另建一个中间画板,画布为中间图片
                _newgraphics.DrawImage(img, 0, 0);  //将图片画到中间图片
                _newgraphics.Dispose();
                _drawToolsGraphics.DrawImage(img, 0, 0);    //将图片画到目标画板上
                img.Dispose();
            }
        }

        private void LigatureControl_Load(object sender, EventArgs e)
        {
            var bmp = new Bitmap(Width, Height);
            var g = Graphics.FromImage(bmp);
            g.FillRectangle(new SolidBrush(BackColor), new Rectangle(0, 0, Width, Height));
            g.Dispose();
            _drawToolsGraphics = CreateGraphics();
            _finishingImg = (Image)bmp.Clone();
            _orginalImg = (Image)bmp.Clone();
        }
    }
}
