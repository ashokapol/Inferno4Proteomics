using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DAnTE.Tools;

namespace DAnTE.Inferno
{
    public partial class frmQRollupPar : Form
    {
        DAnTE.Purgatorio.clsQRollupPar mclsQRollup = new DAnTE.Purgatorio.clsQRollupPar();

        public frmQRollupPar(DAnTE.Purgatorio.clsQRollupPar mclsQRoll)
        {
            InitializeComponent();
            mclsQRollup = mclsQRoll;
        }

        private void mbtnOK_Click(object sender, EventArgs e)
        {
            //int minC;
            double minP, top;
            bool success = true;
            int topN;
            try
            {
                minP = Convert.ToDouble(mtxtBoxMinPresent.Text);
                top = Convert.ToDouble(mtxtBoxThres.Text);
                topN = Convert.ToInt32(mtxtBoxNum.Text);
                if ((minP > 100) || (minP < 0) || (top > 100) || (top < 0) || (topN < 0))
                    success = false;
            }
            catch (Exception ex)
            {
                success = false;
                //Console.WriteLine(ex.Message);
                MessageBox.Show("Invalid parameter. Check again!", "Error!", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            if (!success)
                MessageBox.Show("Invalid parameter. Check again!", "Error!", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            else
                DialogResult = DialogResult.OK;
        }

        private void mrBtnPerct_CheckedChanged(object sender, EventArgs e)
        {
            mtxtBoxThres.Enabled = mrBtnPerct.Checked;
            mtxtBoxNum.Enabled = mrBtnNum.Checked;
        }

        private void mrBtnNum_CheckedChanged(object sender, EventArgs e)
        {
            mtxtBoxThres.Enabled = mrBtnPerct.Checked;
            mtxtBoxNum.Enabled = mrBtnNum.Checked;
        }

        private void frmQRollupPar_Load(object sender, EventArgs e)
        {
            this.DataSetName = mclsQRollup.DataSetName;
            mrBtnPerct.Checked = true;
            mtxtBoxThres.Enabled = mrBtnPerct.Checked;
            mtxtBoxNum.Enabled = mrBtnNum.Checked;
        }

        #region Properties
        public DAnTE.Purgatorio.clsQRollupPar clsQRollupPar
        {
            get
            {
                mclsQRollup.mstrMinPresence = MinPresence;
                mclsQRollup.mblOneHits = OneHitWonders;
                mclsQRollup.mblModeMean = ModeMean;
                mclsQRollup.mstrTop = Threshold;
                mclsQRollup.mstrTopN = TopN;
                mclsQRollup.mblUseTopN = this.UseTopN;

                return mclsQRollup;
            }
        }

        public bool ModeMean
        {
            get
            {
                return mrbtnMean.Checked;
            }
        }

        public string MinPresence
        {
            get
            {
                return mtxtBoxMinPresent.Text;
            }
        }

        public string Threshold
        {
            get
            {
                return mtxtBoxThres.Text;
            }
        }

        public string TopN
        {
            get
            {
                return mtxtBoxNum.Text;
            }
        }

        public bool UseTopN
        {
            get
            {
                if (mrBtnNum.Checked)
                    return true;
                else
                    return false;
            }
        }

        public bool OneHitWonders
        {
            get
            {
                return mchkBoxOneHit.Checked;
            }
        }

        public string DataSetName
        {
            set
            {
                mlblDataName.Text = value;
            }
        }

        #endregion
    }
}