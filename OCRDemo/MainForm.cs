using IronOcr;
using SpeechLib;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace OCRDemo
{
    public partial class MainForm : Form
    {
        private bool _mouseIsDown = false;
        private Rectangle _selectArea = Rectangle.Empty;
        private readonly AutoOcr _ocr;

        public MainForm()
        {
            InitializeComponent();
            _ocr = new AutoOcr();

            // 强制初始化OCR
            var img = new Bitmap(1,1);
            _ocr.Read(img);
        }

        private void pictureBox_MouseDown(object sender, MouseEventArgs e)
        {
            if (_mouseIsDown) return;

            _mouseIsDown = true;
            DrawStart(e.Location);
        }

        private void pictureBox_MouseUp(object sender, MouseEventArgs e)
        {
            // 修正Width和Height可能为负数的问题
            if (_selectArea.Width < 0)
            {
                _selectArea.Width = -_selectArea.Width;
                _selectArea.X -= _selectArea.Width;
            }

            if (_selectArea.Height < 0)
            {
                _selectArea.Height = -_selectArea.Height;
                _selectArea.Y -= _selectArea.Height;
            }

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
                startPoint.Y + pictureBox.Location.Y,1,1);
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

        /// <summary>
        /// 识别框选的单词/句子
        /// </summary>
        /// <returns>识别的结果</returns>
        private string SelectText()
        {
            var rect = RectangleToScreen(_selectArea);
            var img = new Bitmap(rect.Width, rect.Height);

            var graphics = Graphics.FromImage(img);
            graphics.CompositingQuality = CompositingQuality.HighQuality;
            graphics.CopyFromScreen(rect.Left, rect.Top, 0, 0,
                new Size(rect.Width, rect.Height));

            var result = _ocr.Read(img);

            Debug.WriteLine(result.Text);

            return result.Text;
        }

        /// <summary>
        /// 朗读单词/句子
        /// </summary>
        /// <param name="text">要朗读的单词/句子</param>
        private void SpeakText(string text)
        {
            const SpeechVoiceSpeakFlags flag = SpeechVoiceSpeakFlags.SVSFlagsAsync;
            var voice = new SpVoice();
            voice.Voice = voice.GetVoices(string.Empty, string.Empty).Item(0);
            voice.Speak(text, flag);
        }

        private void toolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            new LigatureForm { StartPosition = FormStartPosition.CenterParent }.ShowDialog();
        }
    }
}
