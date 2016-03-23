using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using DAnTE.Tools;
using DAnTE.Properties;

namespace DAnTE.Inferno
{
    public partial class frmLOESSPar : Form
    {
        private ArrayList marrColumns = new ArrayList();
        DAnTE.Purgatorio.clsLoessPar mclsLoess = new DAnTE.Purgatorio.clsLoessPar();

        public frmLOESSPar(DAnTE.Purgatorio.clsLoessPar clsLoess)
        {
            InitializeComponent();
            mclsLoess = clsLoess;
        }

        private void mbtnOK_Click(object sender, EventArgs e)
        {
            bool outfolderOK = Directory.Exists(mtxtBoxFolder.Text);
            if (mchkBoxPlot.Checked && !outfolderOK)
                MessageBox.Show("Invalid Folder name", "Error!", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            else if (Convert.ToDouble(mlblSpan.Text) < 0.1)
                MessageBox.Show("Span value too small.", "Warning!", MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            else
                DialogResult = DialogResult.OK;
        }

        private void mbtnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void mchkBoxPlot_CheckedChanged(object sender, EventArgs e)
        {
            if (mchkBoxPlot.Checked)
            {
                mtxtBoxFolder.Enabled = true;
                mbtnSelectFolder.Enabled = true;
                mtxtBoxFolder.Text = Settings.Default.WorkingFolder; 
            }
            else
            {
                mtxtBoxFolder.Enabled = false;
                mbtnSelectFolder.Enabled = false;
            }
        }

        private void mbtnSelectFolder_Click(object sender, EventArgs e)
        {
            string folderName = null;
            FolderBrowserDialog folderBrowserDialog1 = new FolderBrowserDialog();
            DialogResult result = folderBrowserDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                folderName = folderBrowserDialog1.SelectedPath;
                mtxtBoxFolder.Text = folderName;
            }
            else mtxtBoxFolder.Text = Settings.Default.WorkingFolder;
        }

        private void mtrackBarSpan_Scroll(object sender, EventArgs e)
        {
            Double span = (Double)mtrackBarSpan.Value / 10.0;
            mlblSpan.Text = span.ToString();
        }

        private void frmPickFactor_Load(object sender, EventArgs e)
        {
            mtxtBoxFolder.Text = Settings.Default.WorkingFolder;
        }

        #region Properties

        public DAnTE.Purgatorio.clsLoessPar clsLoessPar
        {
            get
            {
                mclsLoess.mblPlot = PlotFlag;
                mclsLoess.OutFolder_pub = OutFolder;
                mclsLoess.mintFactorIndex = FactorIndex;
                mclsLoess.FactorSelected = Factor;
                mclsLoess.Reference_pub = Reference;
                mclsLoess.span = Span;

                return mclsLoess;
            }
        }

        public string OutFolder
        {
            get
            {
                return mtxtBoxFolder.Text;
            }
            set
            {
                mtxtBoxFolder.Text = value;
            }
        }

        public bool DoPlotting
        {
            get
            {
                return mchkBoxPlot.Checked;
            }
        }

        public bool PlotFlag
        {
            get { return mchkBoxPlot.Checked; }
        }

        public string Span
        {
            get
            {
                return mlblSpan.Text;
            }
        }

        public ArrayList PopulateFactorComboBox
        {
            set
            {
                mcmbBoxFactors.DataSource = value;
            }
        }
                        
        public string DataSetName
        {
            set
            {
                mlblDataName.Text = value;
            }
        }

        public string Factor
        {
            get { return mcmbBoxFactors.SelectedItem.ToString(); }
        }

        public int FactorIndex
        {
            get { return mcmbBoxFactors.SelectedIndex + 1; }
        }

        public string Reference
        {
            get
            {
                if (mrBtnFirst.Checked)
                    return "FirstDataset";
                else if (mrBtnMedian.Checked)
                    return "MedianData";
                else if (mrBtnMinMissing.Checked)
                    return "LeastMissing";
                else
                    return "LeastMissing";
            }
        }

        #endregion
    }
}