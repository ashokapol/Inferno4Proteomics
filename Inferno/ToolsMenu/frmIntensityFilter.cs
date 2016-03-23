using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace DAnTE.Inferno
{
    public partial class frmIntensityFilter : Form
    {
        public frmIntensityFilter()
        {
            InitializeComponent();
        }

        private string SelectedMode()
        {
            string mode = "mean";

            if (mrBtnMean.Checked)
                mode = "mean";
            if (mrBtnMedian.Checked)
                mode = "median";
            if (mrBtnMin.Checked)
                mode = "minimum";
            
            return mode;
        }

        public string CutOff
        {
            get
            {
                return "cutoff=" + mtxtBoxFthres.Text;
            }
        }

        public string Method
        {
            get
            {
                return "method=\"" + SelectedMode() + "\"";
            }
        }

        public string DataSetName
        {
            set
            {
                mlblDataName.Text = value;
            }
        }

        private void mbtnOK_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }

        private void mbtnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }
    }
}