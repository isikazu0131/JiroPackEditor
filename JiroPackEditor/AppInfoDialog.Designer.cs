namespace JiroPackEditor {
    partial class AppInfoDialog {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AppInfoDialog));
            LbInfo = new Label();
            LinkGitHub = new LinkLabel();
            pictureBox1 = new PictureBox();
            label1 = new Label();
            button1 = new Button();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // LbInfo
            // 
            LbInfo.AutoSize = true;
            LbInfo.Font = new Font("ＭＳ ゴシック", 9F, FontStyle.Regular, GraphicsUnit.Point, 128);
            LbInfo.Location = new Point(14, 11);
            LbInfo.Margin = new Padding(4, 0, 4, 0);
            LbInfo.Name = "LbInfo";
            LbInfo.Size = new Size(29, 12);
            LbInfo.TabIndex = 0;
            LbInfo.Text = "Info";
            // 
            // LinkGitHub
            // 
            LinkGitHub.AutoSize = true;
            LinkGitHub.Location = new Point(14, 279);
            LinkGitHub.Margin = new Padding(4, 0, 4, 0);
            LinkGitHub.Name = "LinkGitHub";
            LinkGitHub.Size = new Size(235, 15);
            LinkGitHub.TabIndex = 1;
            LinkGitHub.TabStop = true;
            LinkGitHub.Text = "GitHubへ進む(当アプリのソースコードが見れます)";
            LinkGitHub.LinkClicked += LinkGitHub_LinkClicked;
            // 
            // pictureBox1
            // 
            pictureBox1.BackgroundImageLayout = ImageLayout.Stretch;
            pictureBox1.Image = Properties.Resources.miosunaicon;
            pictureBox1.Location = new Point(416, 152);
            pictureBox1.Margin = new Padding(4, 4, 4, 4);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(99, 105);
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.TabIndex = 2;
            pictureBox1.TabStop = false;
            pictureBox1.Click += pictureBox1_Click;
            // 
            // label1
            // 
            label1.Font = new Font("ＭＳ ゴシック", 9F, FontStyle.Regular, GraphicsUnit.Point, 128);
            label1.Location = new Point(398, 106);
            label1.Margin = new Padding(4, 0, 4, 0);
            label1.Name = "label1";
            label1.Size = new Size(141, 42);
            label1.TabIndex = 3;
            label1.Text = "　私が作りました\r\n＼　フハハハハ　／";
            label1.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // button1
            // 
            button1.Location = new Point(416, 265);
            button1.Margin = new Padding(4, 4, 4, 4);
            button1.Name = "button1";
            button1.Size = new Size(99, 29);
            button1.TabIndex = 4;
            button1.Text = "OK";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // AppInfoDialog
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(530, 309);
            Controls.Add(button1);
            Controls.Add(label1);
            Controls.Add(pictureBox1);
            Controls.Add(LinkGitHub);
            Controls.Add(LbInfo);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Margin = new Padding(4, 4, 4, 4);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "AppInfoDialog";
            Text = "このアプリについて";
            TopMost = true;
            Load += AppInfoDialog_Load;
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label LbInfo;
        private System.Windows.Forms.LinkLabel LinkGitHub;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button1;
    }
}