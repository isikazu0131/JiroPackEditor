using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JiroPackEditor {
    /// <summary>
    /// エラーダイアログ表示用
    /// </summary>
    public class ErrorDialog {
        public static void Show(string msg) {
            MessageBox.Show(msg, "ざんねん", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
