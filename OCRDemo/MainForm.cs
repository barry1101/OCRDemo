using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using IronOcr;
using SpeechLib;

namespace OCRDemo
{
    public partial class MainForm : Form
    {
        private bool _mouseIsDown = false;
        private Rectangle _selectArea = Rectangle.Empty;

        public MainForm()
        {
            InitializeComponent();
        }

        private void pictureBox_MouseDown(object sender, MouseEventArgs e)
        {
            if (_mouseIsDown) return;

            _mouseIsDown = true;
            DrawStart(e.Location);
        }

        private void pictureBox_MouseUp(object sender, MouseEventArgs e)
        {
            Cursor.Clip = Rectangle.Empty;
            _mouseIsDown = false;
            DrawRectangle();

            var text = SelectText();
            SpeakText(text);

            _selectArea = Rectangle.Empty;
        }

        private void pictureBox_MouseMove(object sender, MouseEventArgs e)
        {
            if (_mouseIsDown)
                ResizeToRectangle(e.Location);
        }

        /// <summary>  
        /// 初始化选择框  
        /// </summary>  
        /// <param name="startPoint"></param>  
        private void DrawStart(Point startPoint)
        {
            //指定工作区为整个控件  
            Cursor.Clip = RectangleToScreen(new Rectangle(pictureBox.Location.X, pictureBox.Location.Y,
                pictureBox.Width, pictureBox.Height));
            _selectArea = new Rectangle(startPoint.X + pictureBox.Location.X,
                startPoint.Y + pictureBox.Location.Y,
                pictureBox.Location.X, pictureBox.Location.Y);
        }
        /// <summary>  
        /// 在鼠标移动的时改变选择框的大小  
        /// </summary>  
        /// <param name="p">鼠标的位置</param>  
        private void ResizeToRectangle(Point p)
        {
            _selectArea.Width = p.X + pictureBox.Location.X - _selectArea.Left;
            _selectArea.Height = p.Y + pictureBox.Location.Y - _selectArea.Top;
            DrawRectangle();
        }
        /// <summary>  
        /// 绘制选择框  
        /// </summary>  
        private void DrawRectangle()
        {
            pictureBox.Refresh();
            var rect = RectangleToScreen(_selectArea);
            ControlPaint.DrawReversibleFrame(rect, Color.White, FrameStyle.Thick);
        }

        private string SelectText()
        {
            var rect = RectangleToScreen(_selectArea);
            var img = new Bitmap(rect.Width, rect.Height);

            var graphics = Graphics.FromImage(img);
            graphics.CompositingQuality = CompositingQuality.HighQuality;
            graphics.CopyFromScreen(rect.Left,rect.Top,0,0,
                new Size(rect.Width,rect.Height));

            var ocr = new AdvancedOcr()
            {
                CleanBackgroundNoise = true,
                EnhanceContrast = true,
                EnhanceResolution = true,
            };
            var result = ocr.Read(img);

            Debug.WriteLine(result.Text);

            return result.Text;
        }

        private void SpeakText(string text)
        {
            const SpeechVoiceSpeakFlags flag = SpeechVoiceSpeakFlags.SVSFlagsAsync;
            var voice = new SpVoice();
            voice.Voice = voice.GetVoices(string.Empty, string.Empty).Item(0);
            voice.Speak(text, flag);
        }
    }
}
