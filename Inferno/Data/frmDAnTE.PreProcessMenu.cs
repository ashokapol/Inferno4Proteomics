using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.IO;
using System.Text.RegularExpressions;
using DAnTE.Tools;
using DAnTE.ExtraControls;

namespace DAnTE.Inferno
{
    partial class frmDAnTE
	{
        DAnTE.Purgatorio.clsLogTransformPar mclsLogPar;
        DAnTE.Purgatorio.clsLinRegrnPar mclsLinRegPar;
        DAnTE.Purgatorio.clsLoessPar mclsLoessPar;
        DAnTE.Purgatorio.clsCentralTendencyPar mclsMeanCPar;
        DAnTE.Purgatorio.clsQnormPar mclsQnormPar;
        DAnTE.Purgatorio.clsMADPar mclsMADPar;
        DAnTE.Purgatorio.clsImputePar mclsImputePar;

        #region Pre-processing Menu items

        /// <summary>
        /// Calculate log Expressions
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuItemLogEset_Click(object sender, System.EventArgs e)
        {
            if (ctltreeView.SelectedNode.Tag != null)
            {
                string rcmd = null;
                clsDatasetTreeNode mclsSelected = (clsDatasetTreeNode)ctltreeView.SelectedNode.Tag;

                if (mclsSelected.mDTable != null)
                {
                    string Rdataset = mclsSelected.mstrRdatasetName;
                    if (mclsSelected.mblIsNumeric)
                    {
                        #region Hook Threading events
                        m_BackgroundWorker.DoWork += new DoWorkEventHandler(m_BackgroundWorker_Log2);
                        m_BackgroundWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(
                            m_BackgroundWorker_Log2Completed);
                        #endregion

                        mclsLogPar = new DAnTE.Purgatorio.clsLogTransformPar();
                        mclsLogPar.Rdataset = mclsSelected.mstrRdatasetName;
                        mclsLogPar.DatasetName = mclsSelected.mstrDataText;

                        frmLogPar mfrmLog = new frmLogPar(mclsLogPar);

                        DialogResult res = mfrmLog.ShowDialog();
                        if (res == DialogResult.OK)
                        {
                            mclsLogPar = mfrmLog.clsLogPar;
                            Add2AnalysisHTable(mclsLogPar, "LogTransform");
                            
                            rcmd = mclsLogPar.Rcmd;

                            m_BackgroundWorker.RunWorkerAsync(rcmd);
                            mfrmShowProgress.Message = "Calculating Log Expressions ...";
                            mfrmShowProgress.ShowDialog();
                        }
                        #region Unhook Threading events
                        m_BackgroundWorker.DoWork -= new DoWorkEventHandler(m_BackgroundWorker_Log2);
                        m_BackgroundWorker.RunWorkerCompleted -= new RunWorkerCompletedEventHandler(
                            m_BackgroundWorker_Log2Completed);
                        #endregion
                    }
                }
            }
        }

        private void menuItemLOESSNorm_Click(object sender, EventArgs e) //LOESS
        {
            if (ctltreeView.SelectedNode.Tag != null)
            {
                clsDatasetTreeNode mclsSelected = (clsDatasetTreeNode)ctltreeView.SelectedNode.Tag;

                if (!mhtDatasets.ContainsKey("Expressions") || !mhtDatasets.ContainsKey("Factors"))
                    MessageBox.Show("Data and factors are needed for LOESS. Data should be in log scale.",
                        "No Data Found!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else if (mclsSelected.mblIsNumeric)
                {
                    #region Hook Threading events
                    m_BackgroundWorker.DoWork += new DoWorkEventHandler(m_BackgroundWorker_Lowess);
                    m_BackgroundWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(
                        m_BackgroundWorker_LowessCompleted);
                    #endregion

                    mclsLoessPar = new DAnTE.Purgatorio.clsLoessPar();
                    mclsLoessPar.Rdataset = mclsSelected.mstrRdatasetName;
                    mclsLoessPar.DataSetName = mclsSelected.mstrDataText;
                                        
                    frmLOESSPar mfrmLoessPar = new frmLOESSPar(mclsLoessPar);
                    mfrmLoessPar.DataSetName = mclsSelected.mstrDataText;

                    clsDatasetTreeNode mclsFactors = (clsDatasetTreeNode)mhtDatasets["Factors"];
                    mfrmLoessPar.PopulateFactorComboBox = clsDataTable.DataTableRows(mclsFactors.mDTable);

                    if (mfrmLoessPar.ShowDialog() == DialogResult.OK)
                    {
                        mclsLoessPar = mfrmLoessPar.clsLoessPar;

                        Add2AnalysisHTable(mclsLoessPar, "LOESS");

                        string rcmd = mclsLoessPar.Rcmd;

                        m_BackgroundWorker.RunWorkerAsync(rcmd);
                        mfrmShowProgress.Message = "LOESS Normalizing Data ...";
                        mfrmShowProgress.ShowDialog();
                    }

                    #region Unhook Threading events
                    m_BackgroundWorker.DoWork -= new DoWorkEventHandler(m_BackgroundWorker_Lowess);
                    m_BackgroundWorker.RunWorkerCompleted -= new RunWorkerCompletedEventHandler(
                        m_BackgroundWorker_LowessCompleted);
                    #endregion
                }
            }
        }

        private void mnuItemQnorm_Click(object sender, EventArgs e)
        {
            if (ctltreeView.SelectedNode.Tag != null)
            {
                clsDatasetTreeNode mclsSelected = (clsDatasetTreeNode)ctltreeView.SelectedNode.Tag;
                string rcmd = null;
                bool qnOK = false;

                if (!mclsSelected.mblIsNumeric || !mhtDatasets.ContainsKey("Expressions"))
                    MessageBox.Show("Load some data or select a proper dataset first.", "Check again...!",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                else
                {
                    // check if there's enough complete data to do Quantile normalization
                    rcmd = "qnOK <- IsCompleteData(" + mclsSelected.mstrRdatasetName + ")";
                    try
                    {
                        rConnector.rdcom.EvaluateNoReturn(rcmd);
                        object rout = rConnector.rdcom.GetSymbol("qnOK");
                        qnOK = (bool)rout;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("RDCOM failed: " + ex.Message, "Error!");
                    }
                    if (qnOK)
                    {
                        #region Hook Threading events
                        m_BackgroundWorker.DoWork += new DoWorkEventHandler(m_BackgroundWorker_Quantile);
                        m_BackgroundWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(
                            m_BackgroundWorker_QuantileCompleted);
                        #endregion

                        mclsQnormPar = new DAnTE.Purgatorio.clsQnormPar();
                        mclsQnormPar.Rdataset = mclsSelected.mstrRdatasetName;
                        mclsQnormPar.DataSetName = mclsSelected.mstrDataText;

                        Add2AnalysisHTable(mclsQnormPar, "Quantile_Normalization");
                        //rcmd = "quaNormEset <- quantileN(" + mclsSelected.mstrRdatasetName + ")";
                        rcmd = mclsQnormPar.Rcmd;

                        m_BackgroundWorker.RunWorkerAsync(rcmd);
                        mfrmShowProgress.Message = "Quantile Normalizing ...";
                        mfrmShowProgress.ShowDialog();

                        #region Unhook Threading events
                        m_BackgroundWorker.DoWork -= new DoWorkEventHandler(m_BackgroundWorker_Quantile);
                        m_BackgroundWorker.RunWorkerCompleted -= new RunWorkerCompletedEventHandler(
                            m_BackgroundWorker_QuantileCompleted);
                        #endregion
                    }
                    else
                        MessageBox.Show("Not enough complete data for Quantile Normalization", "Too many missing",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void menuItemMeanC_Click(object sender, EventArgs e)
        {
            if (ctltreeView.SelectedNode.Tag != null)
            {
                clsDatasetTreeNode mclsSelected = (clsDatasetTreeNode)ctltreeView.SelectedNode.Tag;

                if (!mclsSelected.mblIsNumeric || !mhtDatasets.ContainsKey("Expressions"))
                    MessageBox.Show("Load some data or select a proper dataset first.", "Check again...!",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                else
                {
                    #region Hook Threading events
                    m_BackgroundWorker.DoWork += new DoWorkEventHandler(m_BackgroundWorker_MeanC);
                    m_BackgroundWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(
                        m_BackgroundWorker_MeanCCompleted);
                    #endregion

                    mclsMeanCPar = new DAnTE.Purgatorio.clsCentralTendencyPar();
                    mclsMeanCPar.Rdataset = mclsSelected.mstrRdatasetName;
                    mclsMeanCPar.DataSetName = mclsSelected.mstrDataText;

                    frmMeanCenterPar mfrmMeanC = new frmMeanCenterPar(mclsMeanCPar);
                    mfrmMeanC.DataSetName = mclsSelected.mstrDataText;

                    if (mfrmMeanC.ShowDialog() == DialogResult.OK)
                    {
                        mclsMeanCPar = mfrmMeanC.clsCentrTendPar;

                        Add2AnalysisHTable(mclsMeanCPar, "CentralTendency");

                        string rcmd = mclsMeanCPar.Rcmd;

                        m_BackgroundWorker.RunWorkerAsync(rcmd);
                        mfrmShowProgress.Message = "Mean Centering Data ...";
                        mfrmShowProgress.ShowDialog();
                    }

                    #region Unhook Threading events
                    m_BackgroundWorker.DoWork -= new DoWorkEventHandler(m_BackgroundWorker_MeanC);
                    m_BackgroundWorker.RunWorkerCompleted -= new RunWorkerCompletedEventHandler(
                        m_BackgroundWorker_MeanCCompleted);
                    #endregion
                }
            }
        }

        private void menuItemMAD_Click(object sender, EventArgs e) // MAD Adjustment
        {
            if (ctltreeView.SelectedNode.Tag != null)
            {
                if (ctltreeView.SelectedNode.Tag != null)
                {
                    clsDatasetTreeNode mclsSelected = (clsDatasetTreeNode)ctltreeView.SelectedNode.Tag;

                    if (!mclsSelected.mblIsNumeric || !mhtDatasets.ContainsKey("Expressions"))
                        MessageBox.Show("Load some data or select a proper dataset first.", "Check again...!",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    else
                    {
                        #region Hook Threading events
                        m_BackgroundWorker.DoWork += new DoWorkEventHandler(m_BackgroundWorker_MAD);
                        m_BackgroundWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(
                            m_BackgroundWorker_MADCompleted);
                        #endregion

                        mclsMADPar = new DAnTE.Purgatorio.clsMADPar();
                        mclsMADPar.Rdataset = mclsSelected.mstrRdatasetName;
                        mclsMADPar.DataSetName = mclsSelected.mstrDataText;
                                                
                        frmMADPar mfrmMad = new frmMADPar(mclsMADPar);
                        mfrmMad.DataSetName = mclsSelected.mstrDataText;
                        if (mhtDatasets.ContainsKey("Factors"))
                        {
                            clsDatasetTreeNode mclsFactors = (clsDatasetTreeNode)mhtDatasets["Factors"];
                            mfrmMad.PopulateFactorComboBox = clsDataTable.DataTableRows(mclsFactors.mDTable);
                            mclsMADPar.marrFactors = clsDataTable.DataTableRows(mclsFactors.mDTable);
                        }
                        else
                            mfrmMad.PopulateFactorComboBox = null;

                        if (mfrmMad.ShowDialog() == DialogResult.OK)
                        {
                            mclsMADPar = mfrmMad.clsMADPar;

                            Add2AnalysisHTable(mclsMADPar, "MedianAbsoluteDeviation_Adjustment");

                            string rcmd = mclsMADPar.Rcmd;                            

                            m_BackgroundWorker.RunWorkerAsync(rcmd);
                            mfrmShowProgress.Message = "MAD Adjusting Data ...";
                            mfrmShowProgress.ShowDialog();
                        }
                        #region Unhook Threading events
                        m_BackgroundWorker.DoWork -= new DoWorkEventHandler(m_BackgroundWorker_MAD);
                        m_BackgroundWorker.RunWorkerCompleted -= new RunWorkerCompletedEventHandler(
                            m_BackgroundWorker_MADCompleted);
                        #endregion
                    }
                }
            }
        }

        private void menuItemLinReg_Click(object sender, EventArgs e)
        {
            if (ctltreeView.SelectedNode.Tag != null)
            {
                clsDatasetTreeNode mclsSelected = (clsDatasetTreeNode)ctltreeView.SelectedNode.Tag;

                if (!mhtDatasets.ContainsKey("Expressions") || !mhtDatasets.ContainsKey("Factors"))
                    MessageBox.Show("Data and factors are needed for Linear Regression.",
                        "No Data Found!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else if (mclsSelected.mblIsNumeric)
                {
                    #region Hook Threading events
                    m_BackgroundWorker.DoWork += new DoWorkEventHandler(m_BackgroundWorker_LinReg);
                    m_BackgroundWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(
                        m_BackgroundWorker_LinRegCompleted);
                    #endregion

                    mclsLinRegPar = new DAnTE.Purgatorio.clsLinRegrnPar();
                    mclsLinRegPar.Rdataset = mclsSelected.mstrRdatasetName;
                    mclsLinRegPar.DataSetName = mclsSelected.mstrDataText;

                    clsDatasetTreeNode mclsFactors = (clsDatasetTreeNode)mhtDatasets["Factors"];
                    mclsLinRegPar.marrFactors = clsDataTable.DataTableRows(mclsFactors.mDTable);

                    frmLinRegPar mfrmLinReg = new frmLinRegPar(mclsLinRegPar);
                    if (mfrmLinReg.ShowDialog() == DialogResult.OK)
                    {
                        mclsLinRegPar = mfrmLinReg.clsLinRegPar;

                        Add2AnalysisHTable(mclsLinRegPar, "LinearRegression");

                        string rcmd = mclsLinRegPar.Rcmd;

                        m_BackgroundWorker.RunWorkerAsync(rcmd);
                        mfrmShowProgress.Message = "Linear Regressing Data ...";
                        mfrmShowProgress.ShowDialog();
                    }
                    #region Unhook Threading events
                    m_BackgroundWorker.DoWork -= new DoWorkEventHandler(m_BackgroundWorker_LinReg);
                    m_BackgroundWorker.RunWorkerCompleted -= new RunWorkerCompletedEventHandler(
                        m_BackgroundWorker_LinRegCompleted);
                    #endregion
                }
            }
        }

        private void menuItemImpute_Click(object sender, EventArgs e)
        {
            if (ctltreeView.SelectedNode.Tag != null)
            {
                clsDatasetTreeNode mclsSelected = (clsDatasetTreeNode)ctltreeView.SelectedNode.Tag;

                if (mhtDatasets.ContainsKey("Expressions") && mclsSelected.mblIsNumeric)
                {
                    #region Hook Threading events
                    m_BackgroundWorker.DoWork += new DoWorkEventHandler(m_BackgroundWorker_Impute);
                    m_BackgroundWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(
                        m_BackgroundWorker_ImputeCompleted);
                    #endregion

                    mclsImputePar = new DAnTE.Purgatorio.clsImputePar();
                    mclsImputePar.Rdataset = mclsSelected.mstrRdatasetName;
                    mclsImputePar.DataSetName = mclsSelected.mstrDataText;

                    frmImputePar mfrmImpute = new frmImputePar(mclsImputePar);
                    mfrmImpute.DataSetName = mclsSelected.mstrDataText;
                    if (mhtDatasets.ContainsKey("Factors"))
                    {
                        clsDatasetTreeNode mclsFactors = (clsDatasetTreeNode)mhtDatasets["Factors"];
                        mfrmImpute.PopulateFactorComboBox = clsDataTable.DataTableRows(mclsFactors.mDTable);
                        mclsImputePar.marrFactors = clsDataTable.DataTableRows(mclsFactors.mDTable);
                    }
                    else
                        mfrmImpute.PopulateFactorComboBox = null;

                    if (mfrmImpute.ShowDialog() == DialogResult.OK)
                    {
                        mclsImputePar = mfrmImpute.clsImputePar;

                        Add2AnalysisHTable(mclsImputePar, "Imputation");

                        string rcmd = mclsImputePar.Rcmd;

                        m_BackgroundWorker.RunWorkerAsync(rcmd);
                        mfrmShowProgress.Message = "Imputing Data ...";
                        mfrmShowProgress.ShowDialog();
                    }
                    #region Unhook Threading events
                    m_BackgroundWorker.DoWork -= new DoWorkEventHandler(m_BackgroundWorker_Impute);
                    m_BackgroundWorker.RunWorkerCompleted -= new RunWorkerCompletedEventHandler(
                        m_BackgroundWorker_ImputeCompleted);
                    #endregion
                }
            }
        }

        #endregion

        #region Private Methods
        private bool DoMADAdjustment(string rcmd)
        {
            DataTable mDTMAD = new DataTable();
            bool success = true;
            try
            {
                rConnector.rdcom.EvaluateNoReturn(rcmd);
                if (rConnector.GetTableFromRmatrix("madEset"))
                {
                    mDTMAD = rConnector.mDataTable.Copy();
                    mDTMAD.TableName = "madEset";
                    rConnector.rdcom.EvaluateNoReturn("cat(\"Data MAD Adjusted.\n\")");
                    AddDataset2HashTable(mDTMAD);
                }
                else
                    success = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("RDCOM failed: " + ex.Message, "Error!");
                success = false;
            }
            return success;
        }

        private bool DoMeanCenter(string rcmd)
        {
            DataTable mDTMeanC = new DataTable();
            bool success = true;

            try
            {
                rConnector.rdcom.EvaluateNoReturn(rcmd);
                if (rConnector.GetTableFromRmatrix("meanCEset"))
                {
                    mDTMeanC = rConnector.mDataTable.Copy();
                    mDTMeanC.TableName = "meanCEset";
                    rConnector.rdcom.EvaluateNoReturn("cat(\"Data mean centered.\n\")");
                    AddDataset2HashTable(mDTMeanC);
                }
                else
                    success = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("RDCOM failed: " + ex.Message, "Error!");
                success = false;
            }

            return success;
        }

        private bool DoQuantileN(string rcmd)
        {
            DataTable mDTQuantile = new DataTable();
            bool success = true;

            try
            {
                rConnector.rdcom.EvaluateNoReturn(rcmd);
                if (rConnector.GetTableFromRmatrix("quaNormEset"))
                {
                    mDTQuantile = rConnector.mDataTable.Copy();
                    mDTQuantile.TableName = "quaNormEset";
                    rConnector.rdcom.EvaluateNoReturn("cat(\"Data Quantile Normalized (Only complete data).\n\")");
                    AddDataset2HashTable(mDTQuantile);
                }
                else
                    success = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("RDCOM failed: " + ex.Message, "Error!");
                success = false;
            }

            return success;
        }

        private bool DoLowess(string rcmd)
        {
            DataTable mDTLoess = new DataTable();
            bool success = true;
            try
            {
                rConnector.rdcom.EvaluateNoReturn(rcmd);
                if (rConnector.GetTableFromRmatrix("loessData"))
                {
                    mDTLoess = rConnector.mDataTable.Copy();
                    mDTLoess.TableName = "loessData";
                    rConnector.rdcom.EvaluateNoReturn("cat(\"LOESS normalization done.\n\")");
                    AddDataset2HashTable(mDTLoess);
                }
                else
                    success = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("RDCOM failed: " + ex.Message, "Error!");
                success = false;
            }

            return success;
        }

        private bool DoLinReg(string rcmd)
        {
            DataTable mDTLinReg = new DataTable();
            bool success = true;
            try
            {
                rConnector.rdcom.EvaluateNoReturn(rcmd);
                if (rConnector.GetTableFromRmatrix("linregData"))
                {
                    mDTLinReg = rConnector.mDataTable.Copy();
                    mDTLinReg.TableName = "linregData";
                    rConnector.rdcom.EvaluateNoReturn("cat(\"Linear Regression done.\n\")");
                    AddDataset2HashTable(mDTLinReg);
                }
                else
                    success = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("RDCOM failed: " + ex.Message, "Error!");
                success = false;
            }

            return success;
        }

        private bool DoImpute(string rcmd)
        {
            DataTable mDTImpute = new DataTable();
            bool success = true;
            try
            {
                rConnector.rdcom.EvaluateNoReturn(rcmd);
                if (rConnector.GetTableFromRmatrix("imputedData"))
                {
                    mDTImpute = rConnector.mDataTable.Copy();
                    mDTImpute.TableName = "imputedData";
                    rConnector.rdcom.EvaluateNoReturn("cat(\"Imputing done.\n\")");
                    AddDataset2HashTable(mDTImpute);
                }
                else
                    success = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("RDCOM failed: " + ex.Message, "Error!");
                success = false;
            }

            return success;
        }

        private bool DoMergeColumns(string rcmd)
        {
            DataTable mDTmerged = new DataTable();
            bool success = true;
            try
            {
                rConnector.rdcom.EvaluateNoReturn(rcmd);
                if (rConnector.GetTableFromRmatrix("mergedData"))
                {
                    mDTmerged = rConnector.mDataTable.Copy();
                    mDTmerged.TableName = "mergedData";
                    rConnector.rdcom.EvaluateNoReturn("cat(\"Merging done.\n\")");
                    AddDataset2HashTable(mDTmerged);
                }
                else
                    success = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("RDCOM failed: " + ex.Message, "Error!");
                success = false;
            }

            return success;
        }
        #endregion
    }
}