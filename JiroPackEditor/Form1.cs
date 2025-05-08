using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JiroPackEditor
{
    public partial class Form1 : Form
    {
        /// <summary>
        /// �ݒ���
        /// </summary>
        private Setting setting;

        /// <summary>
        /// TJA��D&D�����ۂɂǂ��Ɋi�[���邩�����₷��_�C�A���O
        /// </summary>
        private SelectTJAIndex selectTJAIndexDialog;

        /// <summary>
        /// ���݊J���Ă���p�b�N�t�@�C�����i���S�p�X�j
        /// </summary>
        private string NowOpeningFilePath = "";

        /// <summary>
        /// �N�����Ɉ����R�[�X�p�b�N
        /// </summary>
        private TJP nowTJP;

        /// <summary>
        /// �N�����Ɉ���TJC�i�ǂݎ��E�S�̂̏��X�V�̂݁j
        /// </summary>
        private TJC nowTJC;

        /// <summary>
        /// TJC����tja(5�Ȃ܂�)
        /// </summary>
        private TJA[] TJAs = new TJA[5];

        /// <summary>
        /// ��ʓ��̏���UI�����������Ă��邩�ǂ���
        /// </summary>
        private bool FlagChangeByUI = false;

        /// <summary>
        /// 臒l�ύX�p�t���O
        /// </summary>
        private bool FlagThresholdEdit = false;

        /// <summary>
        /// �䗦�ύX�p�t���O
        /// </summary>
        private bool FlagRatioEdit = false;

        /// <summary>
        /// ���݂̃t�@�C�����ۑ�����Ă��邩
        /// </summary>
        private bool isSaved = true;

        /// <summary>
        /// ���ݑI�����Ă���TJC�����Ԗڂ�
        /// </summary>
        private int nowTJCindex = 0;

        /// <summary>
        /// ���ݐԍ��i��I�����Ă��邩
        /// </summary>
        private bool isRed = true;

        /// <summary>
        /// �e�X�g�v���C���ɐ������ꂽTJC
        /// </summary>
        private List<FileInfo> TestTJCs = new List<FileInfo>();

        public Form1(string[] args)
        {
            InitializeComponent();
            if (args.Length != 0)
            {
                NowOpeningFilePath = args[0];
            }
        }

        /// <summary>
        /// �t�H�[���ǂݍ���
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_Load(object sender, EventArgs e)
        {
            // �A�v���P�[�V�����̎��s���ɃJ�����g�f�B���N�g����ύX����
            string appDirectory = Path.GetDirectoryName(Application.ExecutablePath);
            Directory.SetCurrentDirectory(appDirectory);

            setting = Setting.Read();
            CbIsTestTJCDelete.Checked = setting.IsTestTJCDelete;
            selectTJAIndexDialog = new SelectTJAIndex();
            if (String.IsNullOrEmpty(NowOpeningFilePath))
            {
                nowTJP = new TJP();
                nowTJP.Name = Constants.TJPDefault.Name;

                nowTJP.PackFolderBackColor = ColorInfo.GetColorCode(Constants.TJPDefault.PackBackColor);
                nowTJP.PackFolderForeColor = ColorInfo.GetColorCode(Constants.TJPDefault.PackForeColor);
                nowTJP.CourseFolderBackColor = ColorInfo.GetColorCode(Constants.TJPDefault.CourseBackColor);
                nowTJP.CourseFolderForeColor = ColorInfo.GetColorCode(Constants.TJPDefault.CourseForeColor);
                nowTJP.SongFolderBackColor = ColorInfo.GetColorCode(Constants.TJPDefault.SongBackColor);
                nowTJP.SongFolderForeColor = ColorInfo.GetColorCode(Constants.TJPDefault.SongForeColor);
            }
            else
            {
                OpenPackFile();
            }

            if (nowTJP == null)
            {
                this.Close();
            }

            LbTitle.Text = $"{nowTJP.Name}{Constants.Extention.TJP} - {Constants.AppInfo.Name} {Constants.AppInfo.Version}";
            this.AllowDrop = true;
            this.MaximumSize = this.Size;
            this.MinimumSize = this.Size;

            // ���ꂾ�������l��ݒ肷��
            CbRedOrGold.SelectedIndex = 0;

            PanelPack.Visible = true;
            PanelPack.Enabled = true;
            PanelCourse.Visible = false;
            PanelCourse.Enabled = false;
        }

        /// <summary>
        /// ���݂̃t�H�[���̃^�C�g����ύX���܂�
        /// </summary>
        private void FormTitleChange()
        {
            // �ύX���e�̕ۑ���Ԃ������Ă�������
            string strIsSaved = isSaved ? "" : "(�ύX���e���ۑ�)";
            // �J���Ă���p�X�̃t�@�C������������
            string path = Path.GetFileName(NowOpeningFilePath);
            // �V�K�쐬���őI�����Ă��Ȃ��ꍇ�̓p�b�N�m�[�h��I����Ԃɂ���
            if (TrPack.SelectedNode == null)
            {
                TrPack.SelectedNode = TrPack.Nodes[0];
            }
            if (TrPack.SelectedNode.Level == 0)
            {
                // ���A�v�����N���������̏������
                if (String.IsNullOrEmpty(NowOpeningFilePath))
                {
                    LbTitle.Text = $"{nowTJP.Name}{Constants.Extention.TJP}{strIsSaved} - {Constants.AppInfo.Name} {Constants.AppInfo.Version}";
                }
                else
                {
                    LbTitle.Text = $"{path}{strIsSaved} - {Constants.AppInfo.Name} {Constants.AppInfo.Version}";
                }
            }
            else if (TrPack.SelectedNode.Level == 1)
            {
                // ���A�v�����N���������̏������
                if (String.IsNullOrEmpty(NowOpeningFilePath))
                {
                    LbTitle.Text = $"{nowTJP.Name}{Constants.Extention.TJP}{strIsSaved} - {nowTJC.Name} - {Constants.AppInfo.Name} {Constants.AppInfo.Version}";
                }
                else
                {
                    LbTitle.Text = $"{path}{strIsSaved} - {nowTJC.Name} - {Constants.AppInfo.Name} {Constants.AppInfo.Version}";
                }
            }
            else
            {
                //�������Ȃ�
            }
        }

        /// <summary>
        /// �I������Ă���m�[�h�����Ƃɕ\�������ʂ�ύX����
        /// </summary>
        private void ChangeMenuByTrTJPSelectedNode(TreeNode selectedNode)
        {
            switch (selectedNode.Level)
            {
                // �p�b�N���I����
                case 0:
                    TrPack.SelectedNode = selectedNode;

                    // �E�N���b�N���j���[���[�h�ύX
                    TrPack.ContextMenuStrip = CmsPack;
                    PanelPack.Enabled = true;
                    PanelPack.Visible = true;
                    PanelCourse.Enabled = false;
                    PanelCourse.Visible = false;

                    LbTJCCount.Text = nowTJP.TJCs.Count().ToString();
                    LbTJPMapsCount.Text = nowTJP.TJCs.Sum(x => x.TJAs.Count(y => y != null)).ToString();
                    LbTotalPlayTime.Text = ToMinSec(nowTJP.TJCs.Sum(x => x.TotalOggTime()));
                    LbPackSize.Text = (nowTJP.TJCs.Sum(x => (double)x.GetTJAsSize()) / 1024 / 1024).ToString("0.00") + " MB";

                    // ��������ύX���邽��TbPackName�ύX���\�b�h�͒ʂ�Ȃ�
                    FlagChangeByUI = true;
                    TbPackName.Text = nowTJP.Name;
                    FlagChangeByUI = false;

                    FormTitleChange();
                    break;
                // �R�[�X���I����
                case 1:
                    TrPack.SelectedNode = selectedNode;

                    // �ꎞ�I�Ƀe�L�X�g�{�b�N�X�̕ύX���[�h��I���m�[�h�ύX�ɂ�郂�[�h�Ɉڂ�
                    FlagChangeByUI = true;
                    nowTJCindex = TrPack.SelectedNode.Index;
                    nowTJC = nowTJP.TJCs[nowTJCindex];

                    // �E�N���b�N���j���[���[�h�ύX
                    TrPack.ContextMenuStrip = CmsCourse;
                    TbCourseName.Text = nowTJP.TJCs[nowTJCindex].Name;
                    TJAs = nowTJP.TJCs[nowTJCindex].TJAs.ToArray();
                    SetTJCToView(false);
                    PanelPack.Enabled = false;
                    PanelPack.Visible = false;
                    PanelCourse.Enabled = true;
                    PanelCourse.Visible = true;

                    // �e�L�X�g�{�b�N�X�̕ύX���[�h�����ɖ߂�
                    FlagChangeByUI = false;

                    FormTitleChange();
                    break;
                // TJA���I����
                case 2:
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// �p�b�N�c���[�̑I���m�[�h���ύX���ꂽ�ꍇ�̏���
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TrPack_AfterSelect(object sender, TreeViewEventArgs e)
        {
            ChangeMenuByTrTJPSelectedNode(e.Node);
        }

        /// <summary>
        /// �p�b�N���ύX
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TbPackName_TextChanged(object sender, EventArgs e)
        {
            if (FlagChangeByUI == true) return;
            var PackNode = TrPack.SelectedNode;
            if (PackNode != null)
            {
                PackNode.Text = TbPackName.Text;
                nowTJP.Name = TbPackName.Text;
                ChangeSaveStatus();
            }
        }

        /// <summary>
        /// �R�[�X���ύX
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TbCourseName_TextChanged(object sender, EventArgs e)
        {
            if (FlagChangeByUI) return;
            if (TrPack.SelectedNode.Level == 0) return;
            var PackNode = TrPack.SelectedNode;
            if (PackNode != null)
            {

                nowTJP.TJCs[nowTJCindex].Name = TbCourseName.Text;
                nowTJP.TJCs[nowTJCindex].Number = TJC.GetNumByTitle(nowTJP.TJCs[nowTJCindex].Name);

                PackNode.Text = nowTJP.TJCs[nowTJCindex].Name;
                NmNumbering.Value = nowTJP.TJCs[nowTJCindex].Number;

            }
            ChangeSaveStatus();
        }

        /// <summary>
        /// �V�����R�[�X��ǉ�����ۂɎw�肳�ꂽ�R�[�X�����i�ʓ��ɂ������邩���J�E���g���A
        /// �R�[�X���Ɋi�[�ł���悤�ɉ��H���܂�
        /// </summary>
        /// <returns></returns>
        private string GetNewCourseNum(string name)
        {
            List<TreeNode> nodes = new List<TreeNode>();
            foreach (TreeNode node in TrPack.SelectedNode.Nodes)
            {
                nodes.Add(node);
            }
            int num = nodes.Count(x => x.Text.Contains(name)) + 1;

            return $"({num})";
        }

        /// <summary>
        /// �p�b�N��TJC��ǉ����܂�
        /// </summary>
        private void AddNewTJC(string TITLE)
        {
            TJC tjc = new TJC();
            tjc.Name = TITLE;
            tjc.Number = TJC.GetNumByTitle(TITLE);
            nowTJP.TJCs.Add(tjc);
            nowTJC = tjc;
            ChangeSaveStatus();
            // �R�[�X�������ς���
            LbTJCCount.Text = nowTJP.TJCs.Count().ToString();
        }

        /// <summary>
        /// �R�[�X���폜����
        /// </summary>
        private void DeleteTJC()
        {
            if (MessageBox.Show("�I�������R�[�X���폜���܂����H", "�z���}�ɏ����񂩁H", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                TrPack.SelectedNode.Remove();
                nowTJP.TJCs.RemoveAt(nowTJCindex);
                if (nowTJP.TJCs.Count == 0)
                {
                    PanelPack.Enabled = true;
                    PanelPack.Visible = true;
                    PanelCourse.Enabled = false;
                    PanelCourse.Visible = false;
                }
                else
                {
                    nowTJCindex = TrPack.SelectedNode.Index;
                    nowTJC = nowTJP.TJCs[nowTJCindex];
                    TJAs = nowTJC.TJAs.ToArray();
                    SetTJCToView(false);
                }

                LbTJCCount.Text = nowTJP.TJCs.Count().ToString();
                LbTJPMapsCount.Text = nowTJP.TJCs.Sum(x => x.TJAs.Count(y => y != null)).ToString();
                LbTotalPlayTime.Text = ToMinSec(nowTJP.TJCs.Sum(x => x.TotalOggTime()));
                LbPackSize.Text = (nowTJP.TJCs.Sum(x => (double)x.GetTJAsSize()) / 1024 / 1024).ToString("0.00") + " MB";

                ChangeSaveStatus();
            }
        }

        /// <summary>
        /// ���݂�TJP��TreeView�ɍĕ`�悵�܂�
        /// </summary>
        private void DrawTreeViewNowTJP()
        {
            TrPack.Nodes.Clear();
            // ��������͓ǂݍ���TJP�̓��e��TreeView�ɕ`�ʂ�����
            TrPack.Nodes.Add(nowTJP.Name);
            foreach (var tjc in nowTJP.TJCs)
            {
                TrPack.Nodes[0].Nodes.Add(tjc.Name);
            }
            FormTitleChange();
        }

        /// <summary>
        /// �I�𒆂�TJC����TJA���X�g�Ɋi�[����
        /// </summary>
        /// <param name="TJAFInfo"></param>
        private void SetTJAtoTJAListView(FileInfo TJAFInfo, int index)
        {
            TJA tja = new TJA(TJAFInfo);
            TJAs[index] = tja;
            //// ���݂�TJC��TJA���X�g�ň�ԍŏ��ɋ�ɂȂ��Ă���ӏ��ɓ����
            //foreach(var t in TJAs.Select((v, i) => (v, i))) {
            //    if (t.v == null) {
            //        TJAs[t.i] = tja;
            //        break;
            //    }
            //    if (String.IsNullOrEmpty(t.v.TJAPath.Name)) {
            //        TJAs[t.i] = tja;
            //        break;
            //    }
            //}
            nowTJP.TJCs[TrPack.SelectedNode.Index].TJAs = TJAs.ToList();
            nowTJC = nowTJP.TJCs[TrPack.SelectedNode.Index];
            SetTJCToView(false);
            FormTitleChange();
        }

        /// <summary>
        /// ���݂�TJP�i�p�b�N���j����ʏ�ɕ\�����܂�
        /// </summary>
        private void SetTJPInfoToView()
        {
            TbPackName.Text = nowTJP.Name;
            LbPackColorView.BackColor = ColorInfo.GetColor(nowTJP.PackFolderBackColor);
            LbPackColorView.ForeColor = ColorInfo.GetColor(nowTJP.PackFolderForeColor);
            LbCourseColorView.BackColor = ColorInfo.GetColor(nowTJP.CourseFolderBackColor);
            LbCourseColorView.ForeColor = ColorInfo.GetColor(nowTJP.CourseFolderForeColor);
            LbSongColorView.BackColor = ColorInfo.GetColor(nowTJP.SongFolderBackColor);
            LbSongColorView.ForeColor = ColorInfo.GetColor(nowTJP.SongFolderForeColor);

            // TJP���
            LbTJPMapsCount.Text = nowTJP.TJCs.Sum(x => x.TJAs.Count(y => y != null)).ToString();
            LbTotalPlayTime.Text = nowTJP.TJCs.Sum(x => x.TJAs.Where(y => y != null).Sum(y => y.MusicPlayTime)).ToString();
        }

        /// <summary>
        /// ���݂�TJC����ʏ�ɕ\�����܂�
        /// </summary>
        private void SetTJCToView(bool isCreatedNewTJC)
        {

            // �����ł�UI����ʏ�̃p�����[�^��������t���O�������Ă���

            TbTJA1.Text = "";
            LbNotesCount1.Text = "0";
            LbLevel1.Text = "0.0";
            TbTJA2.Text = "";
            LbNotesCount2.Text = "0";
            LbLevel2.Text = "0.0";
            TbTJA3.Text = "";
            LbNotesCount3.Text = "0";
            LbLevel3.Text = "0.0";
            TbTJA4.Text = "";
            LbNotesCount4.Text = "0";
            LbLevel4.Text = "0.0";
            TbTJA5.Text = "";
            LbNotesCount5.Text = "0";
            LbLevel5.Text = "0.0";
            foreach (var t in TJAs.Select((v, i) => (v, i)))
            {
                // null�������Ă��邱�Ƃ����邪�A���̂��Ƃɂ�tja���i�[����Ă��邱�Ƃ�
                // �Ȃ��͂Ȃ��̂ň�U��΂�
                if (t.v == null) continue;
                switch (t.i)
                {
                    case 0:
                        TbTJA1.Text = t.v.TITLE;
                        LbNotesCount1.Text = t.v.NotesCount.ToString();
                        LbLevel1.Text = t.v.LEVEL.ToString("0.0");
                        break;
                    case 1:
                        TbTJA2.Text = t.v.TITLE;
                        LbNotesCount2.Text = t.v.NotesCount.ToString();
                        LbLevel2.Text = t.v.LEVEL.ToString("0.0");
                        break;
                    case 2:
                        TbTJA3.Text = t.v.TITLE;
                        LbNotesCount3.Text = t.v.NotesCount.ToString();
                        LbLevel3.Text = t.v.LEVEL.ToString("0.0");
                        break;
                    case 3:
                        TbTJA4.Text = t.v.TITLE;
                        LbNotesCount4.Text = t.v.NotesCount.ToString();
                        LbLevel4.Text = t.v.LEVEL.ToString("0.0");
                        break;
                    case 4:
                        TbTJA5.Text = t.v.TITLE;
                        LbNotesCount5.Text = t.v.NotesCount.ToString();
                        LbLevel5.Text = t.v.LEVEL.ToString("0.0");
                        break;
                    default:
                        break;
                }
            }

            // ��ʏ��UI���X�V����
            // �i���o�����O�X�V
            if (FlagChangeByUI == false)
            {
                NmNumbering.Value = TJC.GetNumByTitle(nowTJC.Name);
            }
            else
            {
                NmNumbering.Value = nowTJC.Number;
            }

            // �t�H���_�J���[
            LbLevelColorView.BackColor = ColorInfo.GetColor(nowTJC.LevelBackColor);
            LbLevelColorView.ForeColor = ColorInfo.GetColor(nowTJC.LevelForeColor);

            // �V�K�쐬����TJC�Ȃ珉���z�u�ɂ���
            if (isCreatedNewTJC)
            {
                // �ԍ��i�֐؂�ւ�
                CbRedOrGold.SelectedIndex = 0;
                CbCondition1.SelectedIndex = 7;
                CbCondition2.SelectedIndex = 7;
                CbCondition3.SelectedIndex = 7;
            }
            // ���i�������̕\���؂�ւ�
            // �\���O��-1
            if (CbRedOrGold.SelectedIndex == 0)
            {
                TbConditionName.Text = nowTJC.TJDRed.Name;
                if (nowTJC.TJDRed != null)
                {
                    CbCondition1.SelectedIndex = (int)nowTJC.TJDRed.PassingConditions[0].passingType;
                    NmThreshold1.Value = nowTJC.TJDRed.PassingConditions[0].Threshold;
                    NmRatio1.Value = (decimal)nowTJC.TJDRed.PassingConditions[0].Ratio * 100;
                    CbCondition2.SelectedIndex = (int)nowTJC.TJDRed.PassingConditions[1].passingType;
                    NmThreshold2.Value = nowTJC.TJDRed.PassingConditions[1].Threshold;
                    NmRatio2.Value = (decimal)nowTJC.TJDRed.PassingConditions[1].Ratio * 100;
                    CbCondition3.SelectedIndex = (int)nowTJC.TJDRed.PassingConditions[2].passingType;
                    NmThreshold3.Value = nowTJC.TJDRed.PassingConditions[2].Threshold;
                    NmRatio3.Value = (decimal)nowTJC.TJDRed.PassingConditions[2].Ratio * 100;
                }
            }
            else if (CbRedOrGold.SelectedIndex == 1)
            {
                TbConditionName.Text = nowTJC.TJDGold.Name;
                if (nowTJC.TJDGold != null)
                {
                    CbCondition1.SelectedIndex = (int)nowTJC.TJDGold.PassingConditions[0].passingType;
                    NmThreshold1.Value = nowTJC.TJDGold.PassingConditions[0].Threshold;
                    NmRatio1.Value = (decimal)nowTJC.TJDGold.PassingConditions[0].Ratio * 100;
                    CbCondition2.SelectedIndex = (int)nowTJC.TJDGold.PassingConditions[1].passingType;
                    NmThreshold2.Value = nowTJC.TJDGold.PassingConditions[1].Threshold;
                    NmRatio2.Value = (decimal)nowTJC.TJDGold.PassingConditions[1].Ratio * 100;
                    CbCondition3.SelectedIndex = (int)nowTJC.TJDGold.PassingConditions[2].passingType;
                    NmThreshold3.Value = nowTJC.TJDGold.PassingConditions[2].Threshold;
                    NmRatio3.Value = (decimal)nowTJC.TJDGold.PassingConditions[2].Ratio * 100;
                }
            }
            NmLife.Value = nowTJC.Life;

            // UpdateThresholdByRatio();
            CbUseCondition.Checked = nowTJC.isTJDEnabled;
            CbTitleInvisible.Checked = nowTJC.IsTitleHide;
            TbCourseName.Text = nowTJC.Name;
            LbCourseNotes.Text = nowTJC.TotalNoteCount().ToString();
            LbCourseTime.Text = ToMinSec(nowTJC.TotalOggTime());
            LbAvgLEVEL.Text = nowTJC.AvgLevel().ToString("0.0");

        }

        /// <summary>
        /// �b�𕪕b�ɂ��܂�
        /// </summary>
        /// <param name="seconds"></param>
        /// <returns></returns>
        private string ToMinSec(double seconds)
        {
            if (seconds < 0) { return "�擾�G���["; }
            seconds = Math.Round(seconds, 3);
            int minutes = (int)(seconds / 60);
            double remainingSeconds = seconds - minutes * 60.0;
            return $"{minutes}:{remainingSeconds:00.000}";
        }

        /// <summary>
        /// ���A�v���փh���b�O�A���h�h���b�v���ꂽ�ۂ̏���
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_DragDrop(object sender, DragEventArgs e)
        {
            string[] Files = null;
            // �t�@�C���̏ꍇ�̂ݏ������s��
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                Files = (string[])e.Data.GetData(DataFormats.FileDrop);
                if (Files.Length >= 2)
                {
                    ErrorDialog.Show("�t�@�C���͂P�����h���b�O�A���h�h���b�v���Ă�������");
                    return;
                }
            }
            if (TrPack.SelectedNode.Level == 0)
            {
                ErrorDialog.Show("�ǉ�����R�[�X��I��������ԂŃh���b�O�A���h�h���b�v���Ă�������");
                return;
            }

            // D&D�����t�@�C���̏����擾���Ă���(*.tja�O��)
            FileInfo tjaInfo = new FileInfo(Files[0]);
            if (tjaInfo.Extension != Constants.Extention.tja && tjaInfo.Extension != Constants.Extention.TJA)
            {
                ErrorDialog.Show("�h���b�O�A���h�h���b�v����t�@�C���͕K��\".tja\"�t�@�C�����w�肵�Ă�������");
                return;
            }

            // D&D���ꂽtja��TJC���ɓ˂�����
            if (selectTJAIndexDialog.ShowDialog() == DialogResult.OK)
            {
                // ���Ԗڂɓ˂����ނ�
                int inputIndex = selectTJAIndexDialog.SelectedIndex;
                SetTJAtoTJAListView(tjaInfo, inputIndex);
                ChangeSaveStatus();
            }
        }

        #region IO�֌W
        /// <summary>
        /// �Z�[�u�^���Z�[�u�̐؂�ւ�
        /// </summary>
        private void ChangeSaveStatus()
        {
            isSaved = false;
            FormTitleChange();
        }

        /// <summary>
        /// �V�K�쐬
        /// </summary>
        private void CreateNewFile()
        {
            nowTJP = new TJP();
            nowTJP.Name = Constants.TJPDefault.Name;
            isSaved = true;
            DrawTreeViewNowTJP();
            TrPack.SelectedNode = TrPack.Nodes[0];

            ChangeMenuByTrTJPSelectedNode(TrPack.Nodes[0]);
        }

        /// <summary>
        /// �p�b�N�t�@�C�����J��
        /// </summary>
        private void OpenPackFile()
        {
            var openedTJP = TJP.Read(NowOpeningFilePath);
            if (openedTJP == null)
            {
                MessageBox.Show("�t�@�C���̓ǂݍ��݂Ɏ��s���܂����B");
                return;
            }
            nowTJP = openedTJP;
            DrawTreeViewNowTJP();
            TrPack.SelectedNode = TrPack.Nodes[0];
            ChangeMenuByTrTJPSelectedNode(TrPack.Nodes[0]);
            TrPack.ExpandAll();
            isSaved = true;
        }

        /// <summary>
        /// TJP���㏑�����V�K�ۑ��̂ǂ������ŕۑ�����
        /// </summary>
        /// <returns>true: �ۑ��ɐ��� false: �ۑ��Ɏ��s</returns>
        private bool SaveTJP()
        {
            bool isSaveSuccess = false;
            // �V�K�쐬�������̂Ȃ�V�K�ɖ��O��t���ĕۑ�����
            if (String.IsNullOrEmpty(NowOpeningFilePath))
            {
                isSaveSuccess = SaveNewTJPFile();
            }
            else
            {
                isSaveSuccess = SaveTJPFile();
            }

            // �㏑���ۑ��ɐ��������ꍇ�A���ۑ��\��������
            if (isSaveSuccess)
            {
                isSaved = true;
                FormTitleChange();
            }
            return isSaveSuccess;
        }

        /// <summary>
        /// TJP�t�@�C����ۑ�����i�㏑���j
        /// </summary>
        /// <returns></returns>
        private bool SaveTJPFile()
        {
            var isSaveSuccess = TJP.Write(nowTJP, NowOpeningFilePath);
            if (isSaveSuccess)
            {
                isSaved = true;
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// TJP�t�@�C����ۑ�����i�V�K�j
        /// </summary>
        private bool SaveNewTJPFile()
        {
            CommonSaveFileDialog sFileDialog = new CommonSaveFileDialog();
            sFileDialog.Title = "���O��t���ĕۑ�";
            sFileDialog.Filters.Add(new CommonFileDialogFilter("���ۂ��񎟘Y�����p�b�N�ҏW�p�t�@�C��", "*.tjp"));
            sFileDialog.DefaultFileName = $"{nowTJP.Name}{Constants.Extention.TJP}";
            if (sFileDialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                // �ۑ����̃t�@�C���p�X
                string sFileName = sFileDialog.FileName;
                // .tjp���g���q�Ƃ��Đݒ肵�Ă��Ȃ������ꍇ�͕t������
                if (sFileName.EndsWith(Constants.Extention.TJP) == false)
                {
                    sFileName += Constants.Extention.TJP;
                }
                var isSaveSuccess = TJP.Write(nowTJP, sFileName);
                if (isSaveSuccess)
                {
                    isSaved = true;
                    NowOpeningFilePath = sFileDialog.FileName;
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// �ύX�����ۑ��������ꍇ�̊m�F����
        /// </summary>
        /// <returns>true : ���ɕۑ��ς� or ���[�U�����ۑ����� / false : ���[�U�����L�����Z������</returns>
        private bool SaveCheck()
        {
            if (isSaved == false)
            {
                var saveAnswer = MessageBox.Show($"�p�b�N�t�@�C�����e�̕ύX���ۑ�����Ă��܂���B\r\n�ۑ����܂����H",
                                                 "�ҏW���e���p�[�ɂȂ�Ƃ���������I�I�I�I���Ԃˁ[",
                                                 MessageBoxButtons.YesNoCancel,
                                                 MessageBoxIcon.Warning);
                DialogResult moreAnswer = DialogResult.No;
                if (saveAnswer == DialogResult.No)
                {
                    moreAnswer = MessageBox.Show("�{���ɕۑ����Ȃ��đ��v�H",
                                                     "�{���ɑ��v�H",
                                                     MessageBoxButtons.YesNo,
                                                     MessageBoxIcon.Warning);
                }
                if (saveAnswer == DialogResult.Yes || moreAnswer == DialogResult.No)
                {
                    // �ۑ�����
                    bool isSaveSuccess = SaveTJP();
                    // �ۑ������ꍇ
                    if (isSaveSuccess) { return true; }
                    // �ۑ��ł��Ȃ��A���[�U�����L�����Z��
                    else return false;
                }
                else
                {
                    return true;
                }
            }
            return true;
        }
        #endregion

        /// <summary>
        /// �h���b�O���̃X�e�[�^�X�ύX
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_DragEnter(object sender, DragEventArgs e)
        {
            // �t�@�C���̏ꍇ�̂ݏ������s��
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        #region ��ʏ㕔���j���[

        /// <summary>
        /// �V�K�쐬
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void �V�K�쐬ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // �t�@�C���̕ύX�̖��ۑ����m�F����
            if (SaveCheck() == true)
            {
                CreateNewFile();
            }
        }

        /// <summary>
        /// �p�b�N�t�@�C�����J��
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void �J��ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // �t�@�C���̕ύX�̖��ۑ����m�F����
            if (SaveCheck() == true)
            {
                // �J���t�@�C�����擾����
                CommonOpenFileDialog fDialog = new CommonOpenFileDialog();
                fDialog.Title = "�J���p�b�N�t�@�C����I��";
                fDialog.Multiselect = false;
                fDialog.Filters.Add(new CommonFileDialogFilter("�p�b�N�G�f�B�b�g�p�t�@�C��", "*.tjp"));
                if (fDialog.ShowDialog() == CommonFileDialogResult.Ok)
                {
                    NowOpeningFilePath = fDialog.FileName;
                    OpenPackFile();
                }
            }
        }

        /// <summary>
        /// �㏑���ۑ�����
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void �㏑���ۑ�ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveTJP();
        }

        /// <summary>
        /// ���O��t���ĕۑ�����
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ���O��t���ĕۑ�ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveNewTJPFile();
        }

        /// <summary>
        /// �p�b�N�o�̓t�H���_�ݒ�
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void �p�b�N���o�͂���t�H���_�̐ݒ�ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var folderDialog = new CommonOpenFileDialog();
            folderDialog.IsFolderPicker = true;

            if (folderDialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                setting.PackOutputFolderName = folderDialog.FileName;
                Setting.Write(setting);
            }
            else
            {
                return;
            }
        }

        #endregion

        #region �ۑ�ȑI���{�^��
        /// <summary>
        /// �I������tja��tjc�ɃZ�b�g���܂�
        /// </summary>
        /// <param name="tjaindex"></param>
        private void SetTJA(int tjaindex)
        {
            var openFDialog = new CommonOpenFileDialog();
            openFDialog.Title = "���ʂ�I�����Ă�������";
            openFDialog.Filters.Add(new CommonFileDialogFilter("tja�t�@�C��", "*.tja"));
            if (openFDialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                TJA tja = TJA.SetTJAbyPath(openFDialog.FileName);
                if (tja == null)
                {
                    MessageBox.Show("tja�t�@�C����o�^�ł��܂���ł����B");
                    return;
                }
                TJAs[tjaindex] = tja;
                nowTJP.TJCs[nowTJCindex].TJAs = TJAs.ToList();
                SetTJCToView(false);
            }
        }

        private void BtSongSelect1_Click(object sender, EventArgs e)
        {
            SetTJA(0);
        }

        private void BtSongSelect2_Click(object sender, EventArgs e)
        {
            SetTJA(1);
        }

        private void BtSongSelect3_Click(object sender, EventArgs e)
        {
            SetTJA(2);
        }

        private void BtSongSelect4_Click(object sender, EventArgs e)
        {
            SetTJA(3);
        }

        private void BtSongSelect5_Click(object sender, EventArgs e)
        {
            SetTJA(4);
        }
        #endregion

        #region �N���A�{�^��
        private void BtClear1_Click(object sender, EventArgs e)
        {
            nowTJP.TJCs[TrPack.SelectedNode.Index].TJAs[0] = null;
            TJAs = nowTJP.TJCs[TrPack.SelectedNode.Index].TJAs.ToArray();
            SetTJCToView(false);
        }

        private void BtClear2_Click(object sender, EventArgs e)
        {
            nowTJP.TJCs[TrPack.SelectedNode.Index].TJAs[1] = null;
            TJAs = nowTJP.TJCs[TrPack.SelectedNode.Index].TJAs.ToArray();
            SetTJCToView(false);
        }

        private void BtClear3_Click(object sender, EventArgs e)
        {
            nowTJP.TJCs[TrPack.SelectedNode.Index].TJAs[2] = null;
            TJAs = nowTJP.TJCs[TrPack.SelectedNode.Index].TJAs.ToArray();
            SetTJCToView(false);
        }

        private void BtClear4_Click(object sender, EventArgs e)
        {
            nowTJP.TJCs[TrPack.SelectedNode.Index].TJAs[3] = null;
            TJAs = nowTJP.TJCs[TrPack.SelectedNode.Index].TJAs.ToArray();
            SetTJCToView(false);
        }

        private void BtClear5_Click(object sender, EventArgs e)
        {
            nowTJP.TJCs[TrPack.SelectedNode.Index].TJAs[4] = null;
            TJAs = nowTJP.TJCs[TrPack.SelectedNode.Index].TJAs.ToArray();
            SetTJCToView(false);
        }

        #endregion

        /// <summary>
        /// �E�N���b�N�����ۂɉE�N���b�N�����m�[�h��I����Ԃɂ���
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TrPack_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                var clickNode = TrPack.GetNodeAt(e.X, e.Y);
                if (clickNode != null)
                {
                    TrPack.SelectedNode = clickNode;
                }
            }
        }

        #region Genre.ini�J���[�ύX
        /// <summary>
        /// �p�b�N�̃t�H���_�J���[�ύX
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtChangePackColor_Click(object sender, EventArgs e)
        {
            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                LbPackColorView.BackColor = colorDialog.Color;
            }
            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                LbPackColorView.ForeColor = colorDialog.Color;
            }
            nowTJP.PackFolderBackColor = ColorInfo.GetColorCode(LbPackColorView.BackColor);
            nowTJP.PackFolderForeColor = ColorInfo.GetColorCode(LbPackColorView.ForeColor);
            ChangeSaveStatus();
        }
        /// <summary>
        /// TJC�t�H���_�J���[�ύX
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtChangeCourseColor_Click(object sender, EventArgs e)
        {
            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                LbCourseColorView.BackColor = colorDialog.Color;
            }
            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                LbCourseColorView.ForeColor = colorDialog.Color;
            }
            nowTJP.CourseFolderBackColor = ColorInfo.GetColorCode(LbCourseColorView.BackColor);
            nowTJP.CourseFolderForeColor = ColorInfo.GetColorCode(LbCourseColorView.ForeColor);
            ChangeSaveStatus();
        }
        /// <summary>
        /// �ۑ�ȃt�H���_�J���[�ύX
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtChangeSongColor_Click(object sender, EventArgs e)
        {
            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                LbSongColorView.BackColor = colorDialog.Color;
            }
            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                LbSongColorView.ForeColor = colorDialog.Color;
            }
            nowTJP.SongFolderBackColor = ColorInfo.GetColorCode(LbSongColorView.BackColor);
            nowTJP.SongFolderForeColor = ColorInfo.GetColorCode(LbSongColorView.ForeColor);
            ChangeSaveStatus();
        }
        /// <summary>
        /// �ۑ�ȃt�H���_�����x���ʃt�H���_�J���[�ύX
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtChangeCourseSongColor_Click(object sender, EventArgs e)
        {
            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                LbLevelColorView.BackColor = colorDialog.Color;
            }
            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                LbLevelColorView.ForeColor = colorDialog.Color;
            }
            nowTJP.TJCs[nowTJCindex].LevelBackColor = ColorInfo.GetColorCode(LbLevelColorView.BackColor);
            nowTJP.TJCs[nowTJCindex].LevelForeColor = ColorInfo.GetColorCode(LbLevelColorView.ForeColor);
            ChangeSaveStatus();
        }

        #endregion

        #region �E�N���b�N���j���[
        /// <summary>
        /// �R�[�X�̍폜�{�^������
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RClickMenuTJCDelete_Click(object sender, EventArgs e)
        {
            DeleteTJC();
        }

        /// <summary>
        /// �V�����R�[�X�̒ǉ��{�^������
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RClickMenuAddNewTJC_Click(object sender, EventArgs e)
        {
            string NewCourseNum = GetNewCourseNum("�V�����R�[�X");
            TrPack.Nodes[0].Nodes.Add($"�V�����R�[�X {NewCourseNum}");
            AddNewTJC($"�V�����R�[�X {NewCourseNum}");
            TrPack.ExpandAll();
        }

        #endregion

        #region �t�H�[���֌W
        /// <summary>
        /// �t�H�[������悤�Ƃ����^�C�~���O�̏���
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            // �t�@�C���̕ύX�̖��ۑ���h��
            // ���[�U�����L�����Z�������ꍇ
            if (SaveCheck() == false)
            {
                e.Cancel = true;
            }

            // �e�X�g�p�ɍ����tjc�̍폜
            if (setting.IsTestTJCDelete)
            {
                foreach (var tjc in TestTJCs)
                {
                    if (tjc == null) continue;
                    if (tjc.Exists == false) continue;
                    tjc.Delete();
                }
            }

        }

        /// <summary>
        /// �㏑���ۑ����悤�Ƃ������ǂ������m���ď�������
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.S)
            {
                SaveTJP();
            }
            else if (e.KeyCode == Keys.Up || e.KeyCode == Keys.Down)
            {

            }

        }
        #endregion

        #region TJC�̕��ёւ�
        /// <summary>
        /// �R�[�X���i���o�����O���ŕ��בւ�
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RClickSortByNum_Click(object sender, EventArgs e)
        {
            nowTJP.TJCs.Sort((x, y) => x.Number.CompareTo(y.Number));
            ChangeMenuByTrTJPSelectedNode(TrPack.Nodes[0]);
            DrawTreeViewNowTJP();
            TrPack.ExpandAll();
        }

        /// <summary>
        /// �R�[�X�𖼑O���ŕ��בւ�
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RClickSortByTJCName_Click(object sender, EventArgs e)
        {
            nowTJP.TJCs.Sort((x, y) => x.Name.CompareTo(y.Name));
            ChangeMenuByTrTJPSelectedNode(TrPack.Nodes[0]);
            DrawTreeViewNowTJP();
            TrPack.ExpandAll();
        }

        #endregion

        private void �w���vToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Random random = new Random();
            var rand = random.Next(1, 1000);
            if (rand != 1)
            {
                MessageBox.Show("��{�I�ȑ�������͓��A�v���t���́u�戵������.pdf�v�����ǂ݂��������B\r\n�q���g�F�R�[�X�̐V�K�쐬�͍������E�N���b�N����Ƃł��܂�");
            }
            else
            {
                MessageBox.Show("���̃E�B���h�E��0.1%�̊m���ŕ\������Ă��܂�\r\n" +
                                "���b�L�[�ł��ˁ`",
                                "���b�L�[�E�B���h�E");
            }
        }

        /// <summary>
        /// �݂����Ȏ��i�ʕ��e���v���[�g�����{�^������
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtMJECreate_Click(object sender, EventArgs e)
        {
            if (SaveCheck() == true)
            {
                QFormSPDP qFormSPDP = new QFormSPDP();
                if (qFormSPDP.ShowDialog() == DialogResult.OK)
                {
                    TJP MDEtjp = TJP.CreateMJEtemp(qFormSPDP.isSP);
                    if (MDEtjp != null)
                    {
                        nowTJP = MDEtjp;
                        DrawTreeViewNowTJP();
                        SetTJPInfoToView();
                        ChangeMenuByTrTJPSelectedNode(TrPack.Nodes[0]);
                        TrPack.ExpandAll();
                        MessageBox.Show("�e���v���[�g���쐬���܂����B");
                    }
                }
                else
                {
                    MessageBox.Show("�e���v���[�g�̍쐬���L�����Z�����܂����B");
                }
            }
        }

        private void BtTJDforMJE_Click(object sender, EventArgs e)
        {

            // �ԍ��i�̏ꍇ
            if (CbRedOrGold.SelectedIndex == 0)
            {
                // �e���x���ɂ���ď�����ς���
                if (NmNumbering.Value >= 1 && NmNumbering.Value <= 5)
                {
                    NmRatio2.Value = 20;
                    NmRatio3.Value = 5;
                }
                else if (NmNumbering.Value >= 6 && NmNumbering.Value <= 10)
                {
                    NmRatio2.Value = 15;
                    NmRatio3.Value = 3;
                }
                else if (NmNumbering.Value >= 11)
                {
                    NmRatio2.Value = 10;
                    NmRatio3.Value = 2;
                }
            }
            // �����i�̏ꍇ
            else
            {
                // �e���x���ɂ���ď�����ς���
                if (NmNumbering.Value >= 1 && NmNumbering.Value <= 5)
                {
                    NmRatio2.Value = 5;
                    NmRatio3.Value = (decimal)1.5;
                }
                else if (NmNumbering.Value >= 6 && NmNumbering.Value <= 10)
                {
                    NmRatio2.Value = 3;
                    NmRatio3.Value = 1;
                }
                else if (NmNumbering.Value >= 11)
                {
                    NmRatio2.Value = 2;
                    NmRatio3.Value = (decimal)0.5;
                }
            }
            UpdateThresholdByRatio();
        }

        #region �����ݒ�
        /// <summary>
        /// �I�����������ɂ����UI��ύX����
        /// </summary>
        /// <param name="cbCondition"></param>
        /// <param name="NmThreshold"></param>
        /// <param name="LbMoreLess"></param>
        /// <param name="NmRatio"></param>
        /// <param name="LbPer"></param>
        private void ConditionSetting(ComboBox cbCondition, NumericUpDown NmThreshold, Label LbMoreLess, NumericUpDown NmRatio, Label LbPer, int index)
        {
            int selectTJCindex = TrPack.SelectedNode.Index;

            switch (cbCondition.SelectedItem)
            {
                case "�Ȃ�":
                    NmThreshold.Enabled = false;
                    NmThreshold.Value = 0;
                    LbMoreLess.Text = "";
                    NmRatio.Value = 0;
                    NmRatio.Visible = false;
                    LbPer.Visible = false;
                    if (isRed) nowTJP.TJCs[selectTJCindex].TJDRed.PassingConditions[index].passingType = PassingType.None;
                    else nowTJP.TJCs[selectTJCindex].TJDGold.PassingConditions[index].passingType = PassingType.None;
                    break;
                case "�X�R�A":
                    NmThreshold.Enabled = true;
                    NmThreshold.Maximum = 99999999;
                    NmThreshold.Value = 1;
                    LbMoreLess.Text = "�ȏ�";
                    NmRatio.Value = 0;
                    NmRatio.Visible = false;
                    LbPer.Visible = false;
                    if (isRed) nowTJP.TJCs[selectTJCindex].TJDRed.PassingConditions[index].passingType = PassingType.Score;
                    else nowTJP.TJCs[selectTJCindex].TJDGold.PassingConditions[index].passingType = PassingType.Score;
                    break;
                case "�ǂ̐�":
                    NmThreshold.Enabled = true;
                    NmThreshold.Maximum = 65535 * 5;
                    NmThreshold.Value = 0;
                    LbMoreLess.Text = "�ȏ�";
                    NmRatio.Value = 0;
                    NmRatio.Visible = true;
                    LbPer.Visible = true;
                    if (isRed) nowTJP.TJCs[selectTJCindex].TJDRed.PassingConditions[index].passingType = PassingType.GreatCount;
                    else nowTJP.TJCs[selectTJCindex].TJDGold.PassingConditions[index].passingType = PassingType.GreatCount;
                    break;
                case "�̐�":
                    NmThreshold.Enabled = true;
                    NmThreshold.Maximum = 65535 * 5;
                    NmThreshold.Value = 0;
                    LbMoreLess.Text = "����";
                    NmRatio.Value = 0;
                    NmRatio.Visible = true;
                    LbPer.Visible = true;
                    if (isRed) nowTJP.TJCs[selectTJCindex].TJDRed.PassingConditions[index].passingType = PassingType.GoodCount;
                    else nowTJP.TJCs[selectTJCindex].TJDGold.PassingConditions[index].passingType = PassingType.GoodCount;
                    break;
                case "�s�̐�":
                    NmThreshold.Enabled = true;
                    NmThreshold.Maximum = 65535 * 5;
                    NmThreshold.Value = 0;
                    LbMoreLess.Text = "����";
                    NmRatio.Value = 0;
                    NmRatio.Visible = true;
                    LbPer.Visible = true;
                    if (isRed) nowTJP.TJCs[selectTJCindex].TJDRed.PassingConditions[index].passingType = PassingType.BadCount;
                    else nowTJP.TJCs[selectTJCindex].TJDGold.PassingConditions[index].passingType = PassingType.BadCount;
                    break;
                case "�A�Ő�":
                    NmThreshold.Enabled = true;
                    NmThreshold.Maximum = 99999;
                    NmThreshold.Value = 0;
                    LbMoreLess.Text = "�ȏ�";
                    NmRatio.Value = 0;
                    NmRatio.Visible = false;
                    LbPer.Visible = false;
                    if (isRed) nowTJP.TJCs[selectTJCindex].TJDRed.PassingConditions[index].passingType = PassingType.RoleCount;
                    else nowTJP.TJCs[selectTJCindex].TJDGold.PassingConditions[index].passingType = PassingType.RoleCount;
                    break;
                case "�ő�R���{��":
                    NmThreshold.Enabled = true;
                    NmThreshold.Maximum = 65535 * 5;
                    NmThreshold.Value = 0;
                    LbMoreLess.Text = "�ȏ�";
                    NmRatio.Value = 0;
                    NmRatio.Visible = false;
                    LbPer.Visible = false;
                    if (isRed) nowTJP.TJCs[selectTJCindex].TJDRed.PassingConditions[index].passingType = PassingType.MaxCombo;
                    else nowTJP.TJCs[selectTJCindex].TJDGold.PassingConditions[index].passingType = PassingType.MaxCombo;
                    break;
                case "�@������":
                    NmThreshold.Enabled = true;
                    NmThreshold.Maximum = 65535 * 5;
                    NmThreshold.Value = 0;
                    LbMoreLess.Text = "�ȏ�";
                    NmRatio.Value = 0;
                    NmRatio.Visible = false;
                    LbPer.Visible = false;
                    if (isRed) nowTJP.TJCs[selectTJCindex].TJDRed.PassingConditions[index].passingType = PassingType.HitCount;
                    else nowTJP.TJCs[selectTJCindex].TJDGold.PassingConditions[index].passingType = PassingType.HitCount;
                    break;
                default:
                    break;
            }
        }
        private void CbCondition1_SelectedIndexChanged(object sender, EventArgs e)
        {
            ConditionSetting(CbCondition1,
                             NmThreshold1,
                             LbMoreLess1,
                             NmRatio1,
                             LbPer1,
                             0);
        }

        private void CbCondition2_SelectedIndexChanged(object sender, EventArgs e)
        {
            ConditionSetting(CbCondition2,
                             NmThreshold2,
                             LbMoreLess2,
                             NmRatio2,
                             LbPer2,
                             1);
        }

        private void CbCondition3_SelectedIndexChanged(object sender, EventArgs e)
        {
            ConditionSetting(CbCondition3,
                             NmThreshold3,
                             LbMoreLess3,
                             NmRatio3,
                             LbPer3,
                             2);
        }
        #endregion

        /// <summary>
        /// ���i�����؂�ւ�
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CbRedOrGold_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (nowTJC == null) return;
            // �ԍ��i
            if (CbRedOrGold.SelectedIndex == 0)
            {
                isRed = true;
                FlagChangeByUI = true;
                TbConditionName.Text = nowTJC.TJDRed.Name;
                CbCondition1.SelectedIndex = (int)nowTJC.TJDRed.PassingConditions[0].passingType;
                CbCondition2.SelectedIndex = (int)nowTJC.TJDRed.PassingConditions[1].passingType;
                CbCondition3.SelectedIndex = (int)nowTJC.TJDRed.PassingConditions[2].passingType;

                FlagThresholdEdit = true;
                NmThreshold1.Value = (decimal)(nowTJC.TJDRed.PassingConditions[0].Threshold);
                NmThreshold2.Value = (decimal)(nowTJC.TJDRed.PassingConditions[1].Threshold);
                NmThreshold3.Value = (decimal)(nowTJC.TJDRed.PassingConditions[2].Threshold);
                FlagThresholdEdit = false;

                FlagRatioEdit = true;
                NmRatio1.Value = (decimal)(nowTJC.TJDRed.PassingConditions[0].Ratio * 100);
                NmRatio2.Value = (decimal)(nowTJC.TJDRed.PassingConditions[1].Ratio * 100);
                NmRatio3.Value = (decimal)(nowTJC.TJDRed.PassingConditions[2].Ratio * 100);
                FlagRatioEdit = false;
                FlagChangeByUI = false;
            }
            // �����i
            else
            {
                isRed = false;
                FlagChangeByUI = true;
                TbConditionName.Text = nowTJC.TJDGold.Name;
                CbCondition1.SelectedIndex = (int)nowTJC.TJDGold.PassingConditions[0].passingType;
                CbCondition2.SelectedIndex = (int)nowTJC.TJDGold.PassingConditions[1].passingType;
                CbCondition3.SelectedIndex = (int)nowTJC.TJDGold.PassingConditions[2].passingType;

                FlagThresholdEdit = true;
                NmThreshold1.Value = (decimal)(nowTJC.TJDGold.PassingConditions[0].Threshold);
                NmThreshold2.Value = (decimal)(nowTJC.TJDGold.PassingConditions[1].Threshold);
                NmThreshold3.Value = (decimal)(nowTJC.TJDGold.PassingConditions[2].Threshold);
                FlagThresholdEdit = false;

                FlagRatioEdit = true;
                NmRatio1.Value = (decimal)(nowTJC.TJDGold.PassingConditions[0].Ratio * 100);
                NmRatio2.Value = (decimal)(nowTJC.TJDGold.PassingConditions[1].Ratio * 100);
                NmRatio3.Value = (decimal)(nowTJC.TJDGold.PassingConditions[2].Ratio * 100);
                FlagRatioEdit = false;
                FlagChangeByUI = false;
            }
        }

        /// <summary>
        /// �����̗L�������ύX
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CbUseCondition_CheckedChanged(object sender, EventArgs e)
        {
            if (FlagChangeByUI == true) return;
            if (nowTJC.isTJDEnabled) nowTJC.isTJDEnabled = false;
            else nowTJC.isTJDEnabled = true;
        }

        /// <summary>
        /// �e�X�g�v���C�{�^��
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtTestPlay_Click(object sender, EventArgs e)
        {
            var testTJCFInfo = nowTJC.TestPlay();
            if (testTJCFInfo == null) return;
            TestTJCs.Add(testTJCFInfo);
        }

        /// <summary>
        /// �G�N�X�|�[�g
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtCreate_Click(object sender, EventArgs e)
        {
            var isZip = MessageBox.Show("�p�b�N�����k���܂����H", "���k�m�F", MessageBoxButtons.YesNoCancel);
            if (isZip == DialogResult.Cancel)
            {
                return;
            }
            // �G�N�X�|�[�g
            var isExportSuccess = TJP.Export(nowTJP, setting, isZip == DialogResult.Yes);
            if (isExportSuccess)
            {
                MessageBox.Show("�G�N�X�|�[�g�ɐ������܂���");
            }
            else
            {
                MessageBox.Show("�G�N�X�|�[�g�Ɏ��s���܂���");
            }
        }

        #region ���i�����ύX
        /// <summary>
        /// ���i��������ύX������A���ׂẴR�[�X�̍��i��������ύX����
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TbConditionName_TextChanged(object sender, EventArgs e)
        {
            if (FlagChangeByUI) return;
            // �ԍ��i�̖��̕ύX
            if (CbRedOrGold.SelectedIndex == 0)
            {
                foreach (var tjc in nowTJP.TJCs)
                {
                    tjc.TJDRed.Name = TbConditionName.Text;
                }
            }
            // �����i�̖��̕ύX
            else if (CbRedOrGold.SelectedIndex == 1)
            {
                foreach (var tjc in nowTJP.TJCs)
                {
                    tjc.TJDGold.Name = TbConditionName.Text;
                }
            }
            ChangeSaveStatus();
        }

        /// <summary>
        /// ���i������臒l���X�V���܂�
        /// </summary>
        private void UpdateThresholdByRatio()
        {
            if (FlagChangeByUI == true) return;
            int selectTJCindex = TrPack.SelectedNode.Index;
            if (FlagRatioEdit == true)
            {
                return;
            }
            FlagThresholdEdit = true;
            var notesCount = nowTJP.TJCs[selectTJCindex].TotalNoteCount();
            if (notesCount == 0) return;
            if ((string)CbCondition1.SelectedItem != "�X�R�A" &&
                (string)CbCondition1.SelectedItem != "�A�Ő�" &&
                (string)CbCondition1.SelectedItem != "����������")
            {
                NmThreshold1.Value = Math.Round(nowTJP.TJCs[selectTJCindex].TotalNoteCount() * (NmRatio1.Value / 100), 0);
                if (isRed)
                {
                    nowTJP.TJCs[selectTJCindex].TJDRed.PassingConditions[0].Threshold = (int)NmThreshold1.Value;
                    nowTJP.TJCs[selectTJCindex].TJDRed.PassingConditions[0].Ratio = (double)Math.Round(NmThreshold1.Value / notesCount, 3);
                }
                else
                {
                    nowTJP.TJCs[selectTJCindex].TJDGold.PassingConditions[0].Threshold = (int)NmThreshold1.Value;
                    nowTJP.TJCs[selectTJCindex].TJDGold.PassingConditions[0].Ratio = (double)Math.Round(NmThreshold1.Value / notesCount, 3);
                }
            }
            if ((string)CbCondition2.SelectedItem != "�X�R�A" &&
                (string)CbCondition2.SelectedItem != "�A�Ő�" &&
                (string)CbCondition2.SelectedItem != "����������")
            {
                NmThreshold2.Value = Math.Round(nowTJP.TJCs[selectTJCindex].TotalNoteCount() * (NmRatio2.Value / 100), 0);
                if (isRed)
                {
                    nowTJP.TJCs[selectTJCindex].TJDRed.PassingConditions[1].Threshold = (int)NmThreshold2.Value;
                    nowTJP.TJCs[selectTJCindex].TJDRed.PassingConditions[1].Ratio = (double)Math.Round(NmThreshold2.Value / notesCount, 3);
                }
                else
                {
                    nowTJP.TJCs[selectTJCindex].TJDGold.PassingConditions[1].Threshold = (int)NmThreshold2.Value;
                    nowTJP.TJCs[selectTJCindex].TJDGold.PassingConditions[1].Ratio = (double)Math.Round(NmThreshold2.Value / notesCount, 3);
                }
            }
            if ((string)CbCondition3.SelectedItem != "�X�R�A" &&
                (string)CbCondition3.SelectedItem != "�A�Ő�" &&
                (string)CbCondition3.SelectedItem != "����������")
            {
                NmThreshold3.Value = Math.Round(nowTJP.TJCs[selectTJCindex].TotalNoteCount() * (NmRatio3.Value / 100), 0);
                if (isRed)
                {
                    nowTJP.TJCs[selectTJCindex].TJDRed.PassingConditions[2].Threshold = (int)NmThreshold3.Value;
                    nowTJP.TJCs[selectTJCindex].TJDRed.PassingConditions[2].Ratio = (double)Math.Round(NmThreshold3.Value / notesCount, 3);
                }
                else
                {
                    nowTJP.TJCs[selectTJCindex].TJDGold.PassingConditions[2].Threshold = (int)NmThreshold3.Value;
                    nowTJP.TJCs[selectTJCindex].TJDGold.PassingConditions[2].Ratio = (double)Math.Round(NmThreshold3.Value / notesCount, 3);
                }
            }
            FlagThresholdEdit = false;
            ChangeSaveStatus();
        }

        /// <summary>
        /// ���i�����̊����l���X�V���܂�
        /// </summary>
        private void UpdateRatioByThreshold()
        {
            if (FlagChangeByUI == true) return;
            int selectTJCindex = TrPack.SelectedNode.Index;
            if (FlagThresholdEdit == true) return;
            FlagRatioEdit = true;
            var notesCount = nowTJC.TotalNoteCount();
            if (notesCount == 0) return;

            if ((string)CbCondition1.SelectedItem != "�X�R�A" &&
                (string)CbCondition1.SelectedItem != "�A�Ő�" &&
                (string)CbCondition1.SelectedItem != "����������")
            {
                NmRatio1.Value = Math.Round(NmThreshold1.Value / notesCount * 100, 1);
                if (isRed)
                {
                    nowTJP.TJCs[selectTJCindex].TJDRed.PassingConditions[0].Ratio = (double)Math.Round(NmThreshold1.Value / notesCount, 3);
                    nowTJP.TJCs[selectTJCindex].TJDRed.PassingConditions[0].Threshold = (int)NmThreshold1.Value;
                }
                else
                {
                    nowTJP.TJCs[selectTJCindex].TJDGold.PassingConditions[0].Ratio = (double)Math.Round(NmThreshold1.Value / notesCount, 3);
                    nowTJP.TJCs[selectTJCindex].TJDGold.PassingConditions[0].Threshold = (int)NmThreshold1.Value;
                }
            }
            if ((string)CbCondition2.SelectedItem != "�X�R�A" &&
                (string)CbCondition2.SelectedItem != "�A�Ő�" &&
                (string)CbCondition2.SelectedItem != "����������")
            {
                NmRatio2.Value = Math.Round(NmThreshold2.Value / nowTJP.TJCs[selectTJCindex].TotalNoteCount() * 100, 1);
                if (isRed)
                {
                    nowTJP.TJCs[selectTJCindex].TJDRed.PassingConditions[1].Ratio = (double)Math.Round(NmThreshold2.Value / notesCount, 3);
                    nowTJP.TJCs[selectTJCindex].TJDRed.PassingConditions[1].Threshold = (int)NmThreshold2.Value;
                }
                else
                {
                    nowTJP.TJCs[selectTJCindex].TJDGold.PassingConditions[1].Ratio = (double)Math.Round(NmThreshold2.Value / notesCount, 3);
                    nowTJP.TJCs[selectTJCindex].TJDGold.PassingConditions[1].Threshold = (int)NmThreshold2.Value;
                }
            }
            if ((string)CbCondition3.SelectedItem != "�X�R�A" &&
                (string)CbCondition3.SelectedItem != "�A�Ő�" &&
                (string)CbCondition3.SelectedItem != "����������")
            {
                NmRatio3.Value = Math.Round(NmThreshold3.Value / nowTJP.TJCs[selectTJCindex].TotalNoteCount() * 100, 1);
                if (isRed)
                {
                    nowTJP.TJCs[selectTJCindex].TJDRed.PassingConditions[2].Ratio = (double)Math.Round(NmThreshold3.Value / notesCount, 3);
                    nowTJP.TJCs[selectTJCindex].TJDRed.PassingConditions[2].Threshold = (int)NmThreshold3.Value;
                }
                else
                {
                    nowTJP.TJCs[selectTJCindex].TJDGold.PassingConditions[2].Ratio = (double)Math.Round(NmThreshold3.Value / notesCount, 3);
                    nowTJP.TJCs[selectTJCindex].TJDGold.PassingConditions[2].Threshold = (int)NmThreshold3.Value;
                }
            }
            FlagRatioEdit = false;
            ChangeSaveStatus();
        }

        private void NmThreshold1_ValueChanged(object sender, EventArgs e)
        {
            UpdateRatioByThreshold();
        }

        private void NmThreshold2_ValueChanged(object sender, EventArgs e)
        {
            UpdateRatioByThreshold();
        }

        private void NmThreshold3_ValueChanged(object sender, EventArgs e)
        {
            UpdateRatioByThreshold();
        }

        private void NmRatio1_ValueChanged(object sender, EventArgs e)
        {
            UpdateThresholdByRatio();
        }

        private void NmRatio2_ValueChanged(object sender, EventArgs e)
        {
            UpdateThresholdByRatio();
        }

        private void NmRatio3_ValueChanged(object sender, EventArgs e)
        {
            UpdateThresholdByRatio();
        }
        #endregion

        private void CbTitleInvisible_CheckedChanged(object sender, EventArgs e)
        {
            if (FlagChangeByUI == true) return;
            nowTJP.TJCs[nowTJCindex].IsTitleHide = CbTitleInvisible.Checked;
        }

        private void CbNumbering_CheckedChanged(object sender, EventArgs e)
        {
            nowTJP.TJCs[nowTJCindex].IsNumberingEnable = CbNumbering.Checked;
        }

        private void NmNumbering_ValueChanged(object sender, EventArgs e)
        {
            if (FlagChangeByUI == true) return;
            nowTJP.TJCs[nowTJCindex].Number = (int)NmNumbering.Value;
        }

        private void CbIsTestTJCDelete_CheckedChanged(object sender, EventArgs e)
        {
            setting.IsTestTJCDelete = CbIsTestTJCDelete.Checked;
        }

        private void NmLife_ValueChanged(object sender, EventArgs e)
        {
            if (FlagChangeByUI == true) return;
            nowTJP.TJCs[nowTJCindex].Life = (int)NmNumbering.Value;
        }

        private void ���̃A�v���ɂ��Ă̏��ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AppInfoDialog aInfoDialog = new AppInfoDialog();
            if (aInfoDialog.ShowDialog() == DialogResult.OK) { }
        }

        /// <summary>
        /// �R�[�X�̕������j���[�I��
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RClickMenuTJCDuplicate_Click(object sender, EventArgs e)
        {
            TJCDuplicate();

        }

        /// <summary>
        /// TJC�̕���
        /// </summary>
        private void TJCDuplicate()
        {
            // �I�𒆂̃m�[�h
            int TJCindex = TrPack.SelectedNode.Index;
            // ��������R�s�[
            TJC copiedTJC = new TJC()
            {
                Name = nowTJC.Name,
                Life = nowTJC.Life,
                TJAs = nowTJC.TJAs,
                IsNumberingEnable = nowTJC.IsNumberingEnable,
                IsTitleHide = nowTJC.IsTitleHide,
                LevelBackColor = nowTJC.LevelBackColor,
                LevelForeColor = nowTJC.LevelForeColor,
                isTJDEnabled = nowTJC.isTJDEnabled,
                TJDRed = nowTJC.TJDRed,
                TJDGold = nowTJC.TJDGold,
                isTJDCombine = nowTJC.isTJDCombine
            };
            if (copiedTJC == null) return;
            string num = GetNewCourseNum(copiedTJC.Name);
            copiedTJC.Name = copiedTJC.Name + num;
            // �R�s�[����tjc�̑}��
            nowTJP.TJCs.Insert(TJCindex + 1, copiedTJC);
            TrPack.Nodes[0].Nodes.Insert(TJCindex + 1, copiedTJC.Name);

            ChangeSaveStatus();
        }

        private void PanelCourse_Paint(object sender, PaintEventArgs e)
        {

        }

        /// <summary>
        /// ����{�^��
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PbClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// �ŏ����{�^��
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PbMinimum_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private bool isDragging = false;
        private Point startPoint = new Point(0, 0);
        private void Form_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                isDragging = true;
                startPoint = new Point(e.X, e.Y); // �}�E�X�̃N���b�N�ʒu��ۑ�
            }
        }

        private void Form_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDragging)
            {
                Point p = PointToScreen(e.Location); // �}�E�X�̉�ʏ�̈ʒu���擾
                this.Location = new Point(p.X - startPoint.X, p.Y - startPoint.Y); // �t�H�[���̈ʒu���X�V
            }
        }

        private void Form_MouseUp(object sender, MouseEventArgs e)
        {
            isDragging = false; // �h���b�O��Ԃ�����

        }
    }
}
