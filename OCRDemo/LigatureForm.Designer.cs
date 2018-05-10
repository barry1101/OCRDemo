namespace OCRDemo
{
    partial class LigatureForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.ligatureControl1 = new MyControlLibrary.LigatureControl();
            this.SuspendLayout();
            // 
            // ligatureControl1
            // 
            this.ligatureControl1.Location = new System.Drawing.Point(13, 13);
            this.ligatureControl1.Name = "ligatureControl1";
            this.ligatureControl1.Size = new System.Drawing.Size(679, 466);
            this.ligatureControl1.TabIndex = 0;
            // 
            // LigatureForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(120F, 120F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(706, 494);
            this.Controls.Add(this.ligatureControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "LigatureForm";
            this.ShowIcon = false;
            this.Text = "单词连线";
            this.ResumeLayout(false);

        }

        #endregion

        private MyControlLibrary.LigatureControl ligatureControl1;
    }
}