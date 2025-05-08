﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JiroPackEditor {
    public partial class SelectTJAIndex : Form {
        /// <summary>
        /// 選ばれた番号
        /// </summary>
        public int SelectedIndex = 0;

        /// <summary>
        /// 
        /// </summary>
        public ErrorDialog errorDialog; 

        public SelectTJAIndex() {
            InitializeComponent();
        }

        private void SelectTJAIndex_Load(object sender, EventArgs e) {
            errorDialog = new ErrorDialog();
            CbTJANum.SelectedIndex = 0;
            this.MaximumSize = this.Size;
            this.MinimumSize = this.Size;
        }

        private void BtOK_Click(object sender, EventArgs e) {
            if (CbTJANum.Text == "") {
                ErrorDialog.Show("楽曲を何番目に入れたいかを選んでください");
                return;
            }
            DialogResult = DialogResult.OK;
            SelectedIndex = CbTJANum.SelectedIndex;
        }
    }
}
