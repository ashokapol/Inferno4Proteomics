using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Wizard.UI;
using DAnTE.Properties;

namespace DAnTE.Inferno
{
    public partial class ctlMSMSWelcomeWizPage : Wizard.UI.ExternalWizardPage
    {
        FolderBrowserDialog folderBrwseDlg;
        string foldername = null;

        public ctlMSMSWelcomeWizPage()
        {
            InitializeComponent();
            folderBrwseDlg = new FolderBrowserDialog();
        }

        private void ctlAnovaWelcomeWizPage_SetActive(object sender, CancelEventArgs e)
        {
            SetWizardButtons(WizardButtons.Next);
            mtxtBoxSeqOutFolder.Text = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            mtxtBoxSeqOutFolder.Enabled = mrBtnSeqOut.Checked;
            mBtnOutBrowse.Enabled = mrBtnSeqOut.Checked;
        }

        private void mrBtnSeqOut_CheckedChanged(object sender, EventArgs e)
        {
            mtxtBoxSeqOutFolder.Enabled = mrBtnSeqOut.Checked;
            mBtnOutBrowse.Enabled = mrBtnSeqOut.Checked;
        }

        private void mBtnOutBrowse_Click(object sender, EventArgs e)
        {
            folderBrwseDlg.Description = "Select the directory you want to use during this analysis.";
            folderBrwseDlg.SelectedPath = foldername;
            DialogResult dresult = folderBrwseDlg.ShowDialog();
            if (dresult == DialogResult.OK)
            {
                foldername = folderBrwseDlg.SelectedPath;
                mtxtBoxSeqOutFolder.Text = foldername;
                Settings.Default.msmsFolder = foldername;
                Settings.Default.Save();
            }
        }

        public enmMSMSreadType MSMSreadtype
        {
            get
            {
                if (mrBtnSynOut.Checked)
                    return enmMSMSreadType.SYNOUT;
                else if (mrBtnSeqOut.Checked)
                    return enmMSMSreadType.SEQOUT;
                else
                    return enmMSMSreadType.LABKEY;
            }
        }

        public string SeqOutFolder
        {
            get
            {
                return mtxtBoxSeqOutFolder.Text;
            }
            set
            {
                foldername = value;
                mtxtBoxSeqOutFolder.Text = foldername;
                Settings.Default.msmsFolder = foldername;
                Settings.Default.Save();
            }
        }

        
    }
}