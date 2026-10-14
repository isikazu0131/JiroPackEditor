namespace JiroPackEditor {
    partial class QFormSPDP {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(QFormSPDP));
            label1 = new Label();
            BtSP = new Button();
            BtDP = new Button();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(18, 12);
            label1.Margin = new Padding(4, 0, 4, 0);
            label1.Name = "label1";
            label1.Size = new Size(171, 15);
            label1.TabIndex = 0;
            label1.Text = "どちらで作成するかを選んでください";
            // 
            // BtSP
            // 
            BtSP.Location = new Point(86, 59);
            BtSP.Margin = new Padding(4, 4, 4, 4);
            BtSP.Name = "BtSP";
            BtSP.Size = new Size(88, 29);
            BtSP.TabIndex = 1;
            BtSP.Text = "SP";
            BtSP.UseVisualStyleBackColor = true;
            BtSP.Click += BtSP_Click;
            // 
            // BtDP
            // 
            BtDP.Location = new Point(86, 95);
            BtDP.Margin = new Padding(4, 4, 4, 4);
            BtDP.Name = "BtDP";
            BtDP.Size = new Size(88, 29);
            BtDP.TabIndex = 2;
            BtDP.Text = "DP";
            BtDP.UseVisualStyleBackColor = true;
            BtDP.Click += BtDP_Click;
            // 
            // QFormSPDP
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(265, 164);
            Controls.Add(BtDP);
            Controls.Add(BtSP);
            Controls.Add(label1);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Margin = new Padding(4, 4, 4, 4);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "QFormSPDP";
            Text = "QFormSPDP";
            Load += QFormSPDP_Load;
            ResumeLayout(false);
            PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button BtSP;
        private System.Windows.Forms.Button BtDP;
    }
}