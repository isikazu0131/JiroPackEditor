namespace JiroPackEditor {
    partial class SelectTJAIndex {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SelectTJAIndex));
            label1 = new Label();
            CbTJANum = new ComboBox();
            BtOK = new Button();
            SuspendLayout();
            // 
            // label1
            // 
            label1.Location = new Point(14, 11);
            label1.Margin = new Padding(4, 0, 4, 0);
            label1.Name = "label1";
            label1.Size = new Size(303, 44);
            label1.TabIndex = 0;
            label1.Text = "ドラッグアンドドロップしたTJAファイルをコース内の何番目に入れるか選択してください";
            // 
            // CbTJANum
            // 
            CbTJANum.BackColor = Color.White;
            CbTJANum.DropDownStyle = ComboBoxStyle.DropDownList;
            CbTJANum.FormattingEnabled = true;
            CbTJANum.Items.AddRange(new object[] { "1曲目", "2曲目", "3曲目", "4曲目", "5曲目" });
            CbTJANum.Location = new Point(16, 59);
            CbTJANum.Margin = new Padding(4, 4, 4, 4);
            CbTJANum.Name = "CbTJANum";
            CbTJANum.Size = new Size(206, 23);
            CbTJANum.TabIndex = 1;
            // 
            // BtOK
            // 
            BtOK.Location = new Point(230, 59);
            BtOK.Margin = new Padding(4, 4, 4, 4);
            BtOK.Name = "BtOK";
            BtOK.Size = new Size(88, 29);
            BtOK.TabIndex = 2;
            BtOK.Text = "OK";
            BtOK.UseVisualStyleBackColor = true;
            BtOK.Click += BtOK_Click;
            // 
            // SelectTJAIndex
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(331, 102);
            Controls.Add(BtOK);
            Controls.Add(CbTJANum);
            Controls.Add(label1);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Margin = new Padding(4, 4, 4, 4);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "SelectTJAIndex";
            TopMost = true;
            Load += SelectTJAIndex_Load;
            ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox CbTJANum;
        private System.Windows.Forms.Button BtOK;
    }
}