using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.IO;
using System.Windows.Forms;
using DAnTE.Properties;
using DAnTE.Tools;
using DAnTE.Purgatorio;

namespace DAnTE.Inferno
{
    public partial class frmHistogramPar : Form
    {
        private int MAX = frmDAnTE.MAX;
        private int numCol, bins;
        private ArrayList marrDatasets = new ArrayList();
        string foreC = "#FFC38A", borderC = "#5FAE27";
        private clsHistogramPar mclsHistPar = new clsHistogramPar();
        
        public frmHistogramPar(clsHistogramPar clsHistPar)
        {
            mclsHistPar = clsHistPar;
            InitializeComponent();
        }

        private void mbtnOK_Click(object sender, EventArgs e)
        {
            bool valid = true;

            try
            {
                numCol = Convert.ToInt32(mtxtPlotCols.Text);
                bins = Convert.ToInt32(mtxtBoxBins.Text) + 1;
            }
            catch (Exception ex)
            {
                valid = false;
                MessageBox.Show("Error:" + ex.Message, "Wrong data type");
            }
            if (mlstViewDataSets.CheckedIndices.Count == 0)
            {
                MessageBox.Show("No datasets selected.", "Select datasets");
                this.DialogResult = DialogResult.None;
            }
            else if (valid)
                this.DialogResult = DialogResult.OK;
        }

        private void mbtnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void buttonToggleAll_Click(object sender, System.EventArgs e)
        {
            int N = mlstViewDataSets.Items.Count > MAX ? N = 20 : N = mlstViewDataSets.Items.Count;
            for (int i = 0; i < N; i++)
            {
                if (mlstViewDataSets.Items[i].Checked == true)
                {
                    mlstViewDataSets.Items[i].Checked = false;
                }
                else
                {
                    mlstViewDataSets.Items[i].Checked = true;
                }
            }
            if (mlstViewDataSets.Items.Count > MAX)
            {
                mtxtPlotCols.Text = "5";
                MessageBox.Show("This will select too many datasets to be plotted on one page." +
                    Environment.NewLine + "Therefore, total selected set to " + MAX.ToString() + ".",
                    "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void mbtnForeC_Click(object sender, EventArgs e)
        {
            if (hexColorDialog.ShowDialog() == DialogResult.OK)
            {
                foreC = clsHexColorUtil.ColorToHex(hexColorDialog.Color);
                this.mlblFC.BackColor = hexColorDialog.Color;
                this.mlblFC.ForeColor = hexColorDialog.Color;
                Settings.Default.histFore = foreC;
                Settings.Default.Save();
                mclsHistPar.Fcol = foreC;
            }
        }

        private void mbtnBorderC_Click(object sender, EventArgs e)
        {
            if (hexColorDialog.ShowDialog() == DialogResult.OK)
            {
                borderC = clsHexColorUtil.ColorToHex(hexColorDialog.Color);
                this.mlblBC.BackColor = hexColorDialog.Color;
                this.mlblBC.ForeColor = hexColorDialog.Color;
                Settings.Default.histBrdr = borderC;
                Settings.Default.Save();
                mclsHistPar.Bcol = borderC;
            }
        }

        private void mlstViewDataSets_ItemChecked(object sender, ItemCheckEventArgs e)
        {
            try
            {
                if (mlstViewDataSets.CheckedIndices.Count > MAX)
                    MessageBox.Show("You are selecting too many datasets to be plotted on one page." +
                        Environment.NewLine + "Maximum suggested is " + MAX.ToString() + ".",
                        "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                double colN = Math.Ceiling(Math.Sqrt(mlstViewDataSets.CheckedIndices.Count));
                if (colN < 1)
                    colN = 1;
                mtxtPlotCols.Text = colN.ToString();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception:" + ex);
            }
        }

        private void mbtnDefaults_Click(object sender, EventArgs e)
        {
            foreC = "#FFC38A";
            borderC = "#5FAE27";
            Settings.Default.histFore = foreC;
            Settings.Default.histBrdr = borderC;
            Settings.Default.Save();
            this.mlblFC.BackColor = clsHexColorUtil.HexToColor(foreC);
            this.mlblFC.ForeColor = clsHexColorUtil.HexToColor(foreC);
            this.mlblBC.BackColor = clsHexColorUtil.HexToColor(borderC);
            this.mlblBC.ForeColor = clsHexColorUtil.HexToColor(borderC);
            mtxtPlotCols.Text = "2";
            mchkBoxRug.Checked = true;
            mchkBoxTransparent.Checked = false;
            mtxtBoxBins.Text = "10";
            mchkBoxAutoBin.Checked = true;
        }

        private void mchkBoxAutoBin_CheckedChanged(object sender, EventArgs e)
        {
            mtxtBoxBins.Enabled = !mchkBoxAutoBin.Checked;
        }
        
        private void FormLoad_event(object sender, EventArgs e)
        {
            int bins = mclsHistPar.numBins - 1;
            foreC = mclsHistPar.Fcol;
            borderC = mclsHistPar.Bcol;
            mtxtBoxBins.Text = bins.ToString();
            mtxtPlotCols.Text = mclsHistPar.ncolumns.ToString();
            this.mlblFC.BackColor = clsHexColorUtil.HexToColor(foreC);
            this.mlblFC.ForeColor = clsHexColorUtil.HexToColor(foreC);
            this.mlblBC.BackColor = clsHexColorUtil.HexToColor(borderC);
            this.mlblBC.ForeColor = clsHexColorUtil.HexToColor(borderC);
            this.PopulateListView = mclsHistPar.Datasets;
            this.DataSetName = mclsHistPar.mstrDatasetName;
            this.SelectedDatasets = mclsHistPar.CheckedDatasets;
            mchkBoxAutoBin.Checked = mclsHistPar.autoBins;
            mchkBoxStamp.Checked = mclsHistPar.stamp;
        }

        #region Properties

        public clsHistogramPar clsHistPar
        {
            get
            {
                mclsHistPar.datasubset = "c(" + Selected + ")";
                mclsHistPar.ncolumns = NumPlotColumns;
                mclsHistPar.Fcol = ForeGColor;
                mclsHistPar.Bcol = BorderColor;
                mclsHistPar.bkground = Background;
                mclsHistPar.addrug = AddRug;
                mclsHistPar.CheckedDatasets = SelectedDatasets;
                mclsHistPar.Bins = strBins;
                mclsHistPar.numBins = numBins;
                mclsHistPar.autoBins = mchkBoxAutoBin.Checked;
                mclsHistPar.stamp = mchkBoxStamp.Checked;
                mclsHistPar.ncolumns = Convert.ToInt16(mtxtPlotCols.Text);

                return mclsHistPar;
            }
        }

        public string ForeGColor
        {
            get { return foreC; }
        }

        public string BorderColor
        {
            get { return borderC; }
        }

        public int NumPlotColumns
        {
            get
            {
                int Ncols = 2;
                try
                {
                    Ncols = Convert.ToInt16(mtxtPlotCols.Text);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Invalid type. Reverting to default. \nError: " + ex.Message, "Type error");
                    Ncols = 2;
                }
                return Ncols;
            }
        }

        public ArrayList PopulateListView
        {
            set
            {
                marrDatasets = value;
                ListViewItem[] lstVcolln = new ListViewItem[marrDatasets.Count];
                
                for (int i = 0; i < marrDatasets.Count; i++)
                {
                    ListViewItem lstVItem = new ListViewItem(marrDatasets[i].ToString());
                    lstVItem.Tag = i;
                    lstVcolln[i] = lstVItem;
                }
                mlstViewDataSets.Items.AddRange(lstVcolln);
            }
        }

        public string Selected
        {
            get
            {
                string selected = null;
                ListView.CheckedIndexCollection indexes = mlstViewDataSets.CheckedIndices;
                if (indexes.Count != 0)
                {
                    int k = 0;
                    foreach (int i in indexes)
                    {
                        if (k == 0)
                            selected = Convert.ToString(Convert.ToInt16(mlstViewDataSets.
                                Items[i].Tag) + 1);
                        else
                            selected = selected + "," + Convert.ToString(Convert.ToInt16(
                                mlstViewDataSets.Items[i].Tag) + 1);
                        k++;
                        if (k == MAX)
                            break;
                    }
                }
                return selected;
            }
        }

        public ArrayList SelectedDatasets
        {
            get
            {
                ArrayList selectedDS = new ArrayList();
                ListView.CheckedIndexCollection indexes = mlstViewDataSets.CheckedIndices;
                if (indexes.Count != 0)
                {
                    foreach (int i in indexes)
                    {
                        selectedDS.Add(mlstViewDataSets.Items[i].ToString());
                    }
                }
                return selectedDS;
            }
            set
            {
                ArrayList selectedDS = value;
                for (int i = 0; i < mlstViewDataSets.Items.Count; i++)
                    for (int j = 0; j < selectedDS.Count; j++)
                    {
                        if (selectedDS[j].ToString().Equals(mlstViewDataSets.Items[i].ToString()))
                            mlstViewDataSets.Items[i].Checked = true;
                    }
            }
        }


        public string Background
        {
            get
            {
                if (mchkBoxTransparent.Checked)
                    return "bkground=\"transparent\"";
                else
                    return "bkground=\"white\"";
            }
        }

        public string AddRug
        {
            get
            {
                if (mchkBoxRug.Checked)
                    return "addRug=TRUE";
                else
                    return "addRug=FALSE";
            }
        }

        public string DataSetName
        {
            set
            {
                mlblDataName.Text = value;
            }
        }
        
        public string strBins
        {
            get
            {
                if (mchkBoxAutoBin.Checked)
                    return @"cells=""Sturges""";
                else
                    return "cells=" + bins.ToString();
            }
        }

        public int numBins
        {
            get
            {
                return bins;
            }
        }

        public bool Stamp
        {
            get
            {
                return mchkBoxStamp.Checked;
            }
        }
        #endregion

        
    }
}