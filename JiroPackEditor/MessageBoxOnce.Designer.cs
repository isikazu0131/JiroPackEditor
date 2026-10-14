namespace JiroPackEditor {
    partial class MessageBoxOnce {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MessageBoxOnce));
            LbMsg = new Label();
            BtOk = new Button();
            CbNextView = new CheckBox();
            SuspendLayout();
            // 
            // LbMsg
            // 
            LbMsg.Location = new Point(14, 11);
            LbMsg.Margin = new Padding(4, 0, 4, 0);
            LbMsg.Name = "LbMsg";
            LbMsg.Size = new Size(255, 56);
            LbMsg.TabIndex = 0;
            LbMsg.Text = "label1";
            // 
            // BtOk
            // 
            BtOk.Location = new Point(169, 66);
            BtOk.Margin = new Padding(4, 4, 4, 4);
            BtOk.Name = "BtOk";
            BtOk.Size = new Size(88, 29);
            BtOk.TabIndex = 1;
            BtOk.Text = "OK";
            BtOk.UseVisualStyleBackColor = true;
            BtOk.Click += BtOk_Click;
            // 
            // CbNextView
            // 
            CbNextView.AutoSize = true;
            CbNextView.Location = new Point(16, 71);
            CbNextView.Margin = new Padding(4, 4, 4, 4);
            CbNextView.Name = "CbNextView";
            CbNextView.Size = new Size(126, 19);
            CbNextView.TabIndex = 2;
            CbNextView.Text = "次回以降表示しない";
            CbNextView.UseVisualStyleBackColor = true;
            // 
            // MessageBoxOnce
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(272, 109);
            Controls.Add(CbNextView);
            Controls.Add(BtOk);
            Controls.Add(LbMsg);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Margin = new Padding(4, 4, 4, 4);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "MessageBoxOnce";
            Text = "MessageBoxOnce";
            FormClosing += MessageBoxOnce_FormClosing;
            Load += MessageBoxOnce_Load;
            ResumeLayout(false);
            PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label LbMsg;
        private System.Windows.Forms.Button BtOk;
        private System.Windows.Forms.CheckBox CbNextView;
    }
}