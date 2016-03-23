using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.IO;
using System.Text.RegularExpressions;

namespace DAnTE.Tools
{
	/// <summary>
	/// Summary description for clsRconnect.
	/// </summary>
	public class clsRconnect
	{
        public STATCONNECTORSRVLib.StatConnectorClass rdcom = null;
        //public RCOMServerLib.InternalConnectorClass rdcom = null; // Use 'rcom' for debugging
		public string rcmd;
		public clsRarray mRarray; // R data.frame 
        public DataTable mDataTable; // DataTable to store the R data.frame
        public string[] RowNames;
        public string[] Vector;


		public clsRconnect()
		{
			//
			// TODO: Add constructor logic here
			//
			rcmd = "";
			mRarray = new clsRarray();
            mDataTable = new DataTable();
		}

		public bool initR()
		{
			try
			{
				rdcom = new STATCONNECTORSRVLib.StatConnectorClass();
                //rdcom = new RCOMServerLib.InternalConnectorClass(); // Use 'rcom' for debugging
				rdcom.Init("R");
				return true;
			}
			catch(Exception e)
			{
				string errmsg = "R Init failed: " + rdcom.GetErrorText() + " Other: " + e.Message.ToString();
				Console.WriteLine(errmsg);
				return false;
			}
		}


		public bool loadR(string table, string filename, bool stripwhite, bool header, string separator)
		{
			try
			{
				rcmd = table.ToString() + "<-loadfile('" + filename.ToString() + 
					"',stripwhite=" + stripwhite.ToString().ToUpper() + ",header=" + 
					header.ToString().ToUpper() + ",separator='" + separator.ToString() + "')";
				rdcom.EvaluateNoReturn(rcmd);
				return true;
			}
			catch(Exception e)
			{
				string errmsg = rcmd.ToString() + " " + rdcom.GetErrorText() + " Other:	" + e.Message.ToString() ;
				Console.WriteLine(errmsg) ;
				return false;
			}
		}

		public bool GetTableFromRmatrix(string varName)
		{
            clsRarray mRarr;
			try
			{
                mDataTable.Clear();
				rcmd = "X<-sendmatrix(" + varName + ")" ;
				rdcom.EvaluateNoReturn(rcmd) ;
				rcmd = "Headers<-colnames(" + varName + ")" ;
				rdcom.EvaluateNoReturn(rcmd) ;
				rcmd = "Rows<-rownames(" + varName + ")" ;
				rdcom.EvaluateNoReturn(rcmd) ;
                
				object matrix = rdcom.GetSymbol("X") ;
				object colheaders = rdcom.GetSymbol("Headers") ;
				object rownames = rdcom.GetSymbol("Rows") ;

                string[] rowH = rownames as string[];
                string[] colH = colheaders as string[];
                mRarr = new clsRarray(((double[,])matrix), rowH, colH);
                mDataTable = RDoubleArray2DataTable(mRarr);

				return true ;
			}
			catch(Exception e)
			{
				string errmsg = rcmd.ToString() + " " + rdcom.GetErrorText() + " Other:	" + e.Message.ToString() ;
				Console.WriteLine(errmsg) ;
				return false ;
			}
		}

        public bool GetTableFromRvector(string varName)
        {
            clsRarray mRarr;
            try
            {
                rcmd = "X<-sendmatrix(" + varName + ")";
                rdcom.EvaluateNoReturn(rcmd);
                rcmd = "Headers<-colnames(" + varName + ")";
                rdcom.EvaluateNoReturn(rcmd);
                rcmd = "Rows<-rownames(" + varName + ")";
                rdcom.EvaluateNoReturn(rcmd);

                object matrix = rdcom.GetSymbol("X");
                object colheaders = rdcom.GetSymbol("Headers");
                object rownames = rdcom.GetSymbol("Rows");

                string[] rowH = rownames as string[];
                string colH = colheaders as string;
                mRarr = new clsRarray(((double[,])matrix), rowH, colH);
                mDataTable.Clear();
                mDataTable = RDoubleVector2DataTable(mRarr);

                return true;
            }
            catch (Exception e)
            {
                string errmsg = rcmd.ToString() + " " + rdcom.GetErrorText() + " Other:	" + e.Message.ToString();
                Console.WriteLine(errmsg);
                return false;
            }
        }

        public bool GetTableFromRproteinMatrix(string varName)
        {
            clsRarray mRarr;
            try
            {
                rcmd = "X<-sendmatrix(" + varName + ")";
                rdcom.EvaluateNoReturn(rcmd);
                rcmd = "Headers<-colnames(" + varName + ")";
                rdcom.EvaluateNoReturn(rcmd);
                rcmd = "Rows<-rownames(" + varName + ")";
                rdcom.EvaluateNoReturn(rcmd);

                object matrix = rdcom.GetSymbol("X");
                object colheaders = rdcom.GetSymbol("Headers");
                object rownames = rdcom.GetSymbol("Rows");

                mRarr = new clsRarray(((double[,])matrix), (string[])rownames, (string[])colheaders);

                mDataTable.Clear();
                mDataTable = RProteinArray2DataTable(mRarr);

                return true;
            }
            catch (Exception e)
            {
                string errmsg = rcmd.ToString() + " " + rdcom.GetErrorText() + " Other:	" + e.Message.ToString();
                Console.WriteLine(errmsg);
                return false;
            }
        }

        public bool GetTableFromRmatrixNonNumeric(string varName)
        {
            clsRarray mRarr;
            try
            {
                rcmd = "X<-sendmatrix(" + varName + ")";
                rdcom.EvaluateNoReturn(rcmd);
                rcmd = "Headers<-colnames(" + varName + ")";
                rdcom.EvaluateNoReturn(rcmd);
                rcmd = "Rows<-rownames(" + varName + ")";
                rdcom.EvaluateNoReturn(rcmd);

                object matrix = rdcom.GetSymbol("X");
                object colheaders = rdcom.GetSymbol("Headers");
                object rownames = rdcom.GetSymbol("Rows");

                mRarr = new clsRarray(((string[,])matrix), (string[])rownames, (string[])colheaders);

                mDataTable = RstrArray2DataTable(mRarr);

                return true;
            }
            catch (Exception e)
            {
                string errmsg = rcmd.ToString() + " " + rdcom.GetErrorText() + " Other:	" + e.Message.ToString();
                Console.WriteLine(errmsg);
                return false;
            }
        }

        public bool GetTableFromRProtInfoMatrix(string varName)
        {
            clsRarray mRarr;
            try
            {
                rcmd = "X<-sendmatrix(" + varName + ")";
                rdcom.EvaluateNoReturn(rcmd);
                rcmd = "Headers<-colnames(" + varName + ")";
                rdcom.EvaluateNoReturn(rcmd);
                rcmd = "Rows<-rownames(" + varName + ")";
                rdcom.EvaluateNoReturn(rcmd);

                object matrix = rdcom.GetSymbol("X");
                object colheaders = rdcom.GetSymbol("Headers");
                object rownames = rdcom.GetSymbol("Rows");

                mRarr = new clsRarray(((string[,])matrix), (string[])rownames, (string[])colheaders);

                mDataTable = RproteinInfoArray2DataTable(mRarr);

                return true;
            }
            catch (Exception e)
            {
                string errmsg = rcmd.ToString() + " " + rdcom.GetErrorText() + " Other:	" + e.Message.ToString();
                Console.WriteLine(errmsg);
                return false;
            }
        }

        public bool GetRowNamesFromRmatrix(string varName)
        {
            try
            {
                rcmd = "Rows<-rownames(" + varName + ")";
                rdcom.EvaluateNoReturn(rcmd);
                object rownames = rdcom.GetSymbol("Rows");
                RowNames = (string[])rownames;
                return true;
            }
            catch (Exception e)
            {
                string errmsg = rcmd.ToString() + " " + rdcom.GetErrorText() + " Other:	" + e.Message.ToString();
                Console.WriteLine(errmsg);
                return false;
            }
        }

        public bool GetRstringVector(string varName)
        {
            try
            {
                object vectr = rdcom.GetSymbol(varName);
                Vector = (string[])vectr;
                return true;
            }
            catch (Exception e)
            {
                string errmsg = rcmd.ToString() + " " + rdcom.GetErrorText() + " Other:	" + e.Message.ToString();
                Console.WriteLine(errmsg);
                return false;
            }
        }

        public bool SendTable2RmatrixNumeric(string varName, DataTable mDtable)
        {
            clsRarray mRarrySend = DataTable2Rarray(mDtable);
            if (mRarrySend.matrix != null)
            {
                try
                {
                    rdcom.SetSymbol("X", mRarrySend.matrix);
                    rcmd = varName + "<- getmatrix(X)";
                    rdcom.EvaluateNoReturn(rcmd);
                    rdcom.SetSymbol("colH", mRarrySend.colHeaders);
                    rdcom.SetSymbol("rowN", mRarrySend.rowNames);
                    rcmd = "colnames(" + varName + ") <- colH";
                    rdcom.EvaluateNoReturn(rcmd);
                    rcmd = "rownames(" + varName + ") <- rowN";
                    rdcom.EvaluateNoReturn(rcmd);
                    //rcmd = varName + "<- cleanmatrix(" + varName + ")"; // here's where the duplicates are removed
                    //rdcom.EvaluateNoReturn(rcmd);

                    return true;
                }
                catch (Exception e)
                {
                    string errmsg = rcmd.ToString() + " " + rdcom.GetErrorText() + " Other:	" + e.Message.ToString();
                    Console.WriteLine(errmsg);
                    return false;
                }
            }
            else
                return false;
        }

        public bool SendTable2RmatrixNonNumeric(string varName, DataTable mDtable)
        {
            clsRarray mRarrySend = DataTable2RstrArray(mDtable);
            if (mRarrySend.mstrMatrix != null)
            {
                try
                {
                    rdcom.SetSymbol("X", mRarrySend.mstrMatrix);
                    //rcmd = varName + "<- getmatrix(X)";
                    rcmd = varName + "<- X";
                    rdcom.EvaluateNoReturn(rcmd);
                    rdcom.SetSymbol("colH", mRarrySend.colHeaders);
                    rdcom.SetSymbol("rowN", mRarrySend.rowNames);
                    rcmd = "colnames(" + varName + ") <- colH";
                    rdcom.EvaluateNoReturn(rcmd);
                    rcmd = "rownames(" + varName + ") <- rowN";
                    rdcom.EvaluateNoReturn(rcmd);

                    return true;
                }
                catch (Exception e)
                {
                    string errmsg = rcmd.ToString() + " " + rdcom.GetErrorText() + " Other:	" + e.Message.ToString();
                    Console.WriteLine(errmsg);
                    return false;
                }
            }
            else
                return false;
        }
        
		public bool sourceRcmds(string filename)
		{
			try
			{
				rcmd = "source(\"" + filename.ToString() + "\")" ;
				rdcom.EvaluateNoReturn(rcmd);
				return true;
			}
			catch(Exception e)
			{
				string errmsg = rcmd.ToString() + " " + rdcom.GetErrorText() + " Other:	" + e.Message.ToString();
				Console.WriteLine(errmsg);
				return false;
			}
		}

        public bool loadRData(string filename)
        {
            try
            {
                rcmd = "load(\"" + filename.ToString() + "\")";
                rdcom.EvaluateNoReturn(rcmd);
                return true;
            }
            catch (Exception e)
            {
                string errmsg = rcmd.ToString() + " " + rdcom.GetErrorText() + " Other:	" + e.Message.ToString();
                Console.WriteLine(errmsg);
                return false;
            }
        }

		public bool closeR()
		{
			try
			{
				rcmd = "graphics.off()";
				rdcom.EvaluateNoReturn(rcmd);
				rcmd = "rm(list=ls(all=TRUE))";
				rdcom.EvaluateNoReturn(rcmd);
				rdcom.Close();
				return true;
			}
			catch(Exception e)
			{
				string errmsg = "R Close failed: " + rdcom.GetErrorText() + " Other: " + e.Message.ToString();
				Console.WriteLine(errmsg);
				return false;
			}
		}

        

        private DataTable RDoubleArray2DataTable(clsRarray mRary)
        {
            double[,] matrix = mRary.matrix;
            string[] rowNames = mRary.rowNames;
            string[] colHeaders = mRary.colHeaders;
            DataTable mdatatable = new DataTable();
            DataColumn mDataColumn;
            DataRow mDataRow;

            mDataColumn = new DataColumn();
            mDataColumn.DataType = System.Type.GetType("System.String");
            mDataColumn.ColumnName = clsRarray.rowNamesID;
            //mDataColumn.ReadOnly = true ;
            mdatatable.Columns.Add(mDataColumn);

            for (int i = 0; i < colHeaders.Length; i++)
            {
                mDataColumn = new DataColumn();
                mDataColumn.DataType = System.Type.GetType("System.Double");
                mDataColumn.ColumnName = colHeaders[i];
                //mDataColumn.ReadOnly = true ;
                mdatatable.Columns.Add(mDataColumn);
            }
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                mDataRow = mdatatable.NewRow();
                mDataRow[0] = rowNames[i];
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    mDataRow[j + 1] = matrix[i, j];
                }
                mdatatable.Rows.Add(mDataRow);
            }
            //return clsDataTable.ClearZeros(mdatatable);
            return clsDataTable.ReplaceMissing(mdatatable);
        }

        private DataTable RDoubleVector2DataTable(clsRarray mRary)
        {
            double[,] matrix = mRary.matrix;
            string[] rowNames = mRary.rowNames;
            string colHeaders = mRary.colHs;
            DataTable mdatatable = new DataTable();
            DataColumn mDataColumn;
            DataRow mDataRow;

            mDataColumn = new DataColumn();
            mDataColumn.DataType = System.Type.GetType("System.String");
            mDataColumn.ColumnName = clsRarray.rowNamesID;
            //mDataColumn.ReadOnly = true ;
            mdatatable.Columns.Add(mDataColumn);

            mDataColumn = new DataColumn();
            mDataColumn.DataType = System.Type.GetType("System.Double");
            mDataColumn.ColumnName = colHeaders;
            //mDataColumn.ReadOnly = true ;
            mdatatable.Columns.Add(mDataColumn);

            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                mDataRow = mdatatable.NewRow();
                mDataRow[0] = rowNames[i];
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    mDataRow[j + 1] = matrix[i, j];
                }
                mdatatable.Rows.Add(mDataRow);
            }
            //return clsDataTable.ClearZeros(mdatatable);
            return clsDataTable.ReplaceMissing(mdatatable);
        }

        private DataTable RstrArray2DataTable(clsRarray mRary)
        {
            string[,] matrix = mRary.mstrMatrix;
            string[] rowNames = mRary.rowNames;
            string[] colHeaders = mRary.colHeaders;
            DataTable mdatatable = new DataTable();
            DataColumn mDataColumn;
            DataRow mDataRow;

            mDataColumn = new DataColumn();
            mDataColumn.DataType = System.Type.GetType("System.String");
            mDataColumn.ColumnName = clsRarray.rowNamesID;
            //mDataColumn.ReadOnly = true ;
            mdatatable.Columns.Add(mDataColumn);

            for (int i = 0; i < colHeaders.Length; i++)
            {
                mDataColumn = new DataColumn();
                mDataColumn.DataType = System.Type.GetType("System.String");
                mDataColumn.ColumnName = colHeaders[i];
                //mDataColumn.ReadOnly = true ;
                mdatatable.Columns.Add(mDataColumn);
            }
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                mDataRow = mdatatable.NewRow();
                mDataRow[0] = rowNames[i];
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    mDataRow[j + 1] = matrix[i, j].ToString();
                }
                mdatatable.Rows.Add(mDataRow);
            }
            //return clsDataTable.ClearZeros(mdatatable);
            return mdatatable;
        }

        private DataTable RproteinInfoArray2DataTable(clsRarray mRary)
        {
            string[,] matrix = mRary.mstrMatrix;
            string[] rowNames = mRary.rowNames;
            string[] colHeaders = mRary.colHeaders;
            DataTable mdatatable = new DataTable();
            DataColumn mDataColumn;
            DataRow mDataRow;

            mDataColumn = new DataColumn();
            mDataColumn.DataType = System.Type.GetType("System.String");
            mDataColumn.ColumnName = clsRarray.rowNamesID;
            //mDataColumn.ReadOnly = true ;
            mdatatable.Columns.Add(mDataColumn);

            for (int i = 0; i < colHeaders.Length; i++)
            {
                mDataColumn = new DataColumn();
                mDataColumn.DataType = System.Type.GetType("System.String");
                //if (i == 0)
                //    mDataColumn.DataType = System.Type.GetType("System.Double");
                //else
                //    mDataColumn.DataType = System.Type.GetType("System.String");
                mDataColumn.ColumnName = colHeaders[i];
                //mDataColumn.ReadOnly = true ;
                mdatatable.Columns.Add(mDataColumn);
            }
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                mDataRow = mdatatable.NewRow();
                mDataRow[0] = rowNames[i];
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    mDataRow[j + 1] = matrix[i, j].ToString();
                }
                mdatatable.Rows.Add(mDataRow);
            }
            //return clsDataTable.ClearZeros(mdatatable);
            return mdatatable;
        }

        private DataTable RProteinArray2DataTable(clsRarray mRary)
        {
            double[,] matrix = mRary.matrix;
            string[] rowNames = mRary.rowNames;
            string[] colHeaders = mRary.colHeaders;
            DataTable mdatatable = new DataTable();
            DataColumn mDataColumn;
            DataRow mDataRow;

            mDataColumn = new DataColumn();
            mDataColumn.DataType = System.Type.GetType("System.String");
            mDataColumn.ColumnName = clsRarray.rowNamesID;
            //mDataColumn.ReadOnly = true ;
            mdatatable.Columns.Add(mDataColumn);

            for (int i = 0; i < colHeaders.Length; i++)
            {
                mDataColumn = new DataColumn();
                mDataColumn.DataType = System.Type.GetType("System.Double");
                mDataColumn.ColumnName = colHeaders[i];
                //mDataColumn.ReadOnly = true ;
                mdatatable.Columns.Add(mDataColumn);
            }
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                mDataRow = mdatatable.NewRow();
                mDataRow[0] = rowNames[i];
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    mDataRow[j + 1] = matrix[i, j];
                }
                mdatatable.Rows.Add(mDataRow);
            }
            //return clsDataTable.ClearZeros(mdatatable);
            return clsDataTable.ReplaceMissing(mdatatable);
            //return mdatatable;
        }

        private clsRarray DataTable2Rarray(DataTable mTable)
        {
            double[,] matrix = new double[mTable.Rows.Count, mTable.Columns.Count - 1];
            string[] rowNames = new string[mTable.Rows.Count];
            string[] colHeaders = new string[mTable.Columns.Count - 1];
            string cellValue = null;
            clsRarray mRary = new clsRarray();
            bool typerror = false;

            //DataColumnCollection columnHeaders = mTable.Columns ;
            for (int col = 0; col < mTable.Columns.Count; col++)
            {
                if (typerror)
                    break;
                if (col > 0) //Start from 2nd column
                    colHeaders[col - 1] = mTable.Columns[col].ToString();
                for (int row = 0; row < mTable.Rows.Count; row++)
                {
                    if (typerror)
                        break;
                    if (col == 0) //Mass Tags
                        rowNames[row] = mTable.Rows[row].ItemArray[0].ToString();
                    else // numeric data
                    {
                        cellValue = mTable.Rows[row].ItemArray[col].ToString();
                        if (cellValue.Length > 0) //not an empty cell
                            try
                            {
                                matrix[row, col - 1] = Convert.ToDouble(cellValue);
                            }
                            catch (FormatException ex)
                            {
                                MessageBox.Show("Invalid data type. Check for example, " + 
                                    "if you have text strings mixed with numerical data.\n\nError:" + 
                                    ex.Message,"File type error",MessageBoxButtons.OK,
                                    MessageBoxIcon.Error);
                                typerror = true;
                            }
                        else
                            matrix[row, col - 1] = 0;
                    }
                }
            }
            if (!typerror)
            {
                mRary.rowNames = rowNames;
                mRary.colHeaders = colHeaders;
                mRary.matrix = matrix;
            }
            else
            {
                mRarray = null;
            }
            return mRary;
        }

        private clsRarray DataTable2RstrArray(DataTable mTable)
        {
            string[,] matrix = new string[mTable.Rows.Count, mTable.Columns.Count - 1];
            string[] rowNames = new string[mTable.Rows.Count];
            string[] colHeaders = new string[mTable.Columns.Count - 1];
            string cellValue = null;
            clsRarray mRary = new clsRarray();
            bool typerror = false;

            //DataColumnCollection columnHeaders = mTable.Columns ;
            for (int col = 0; col < mTable.Columns.Count; col++)
            {
                if (typerror)
                    break;
                if (col > 0) //Start from 2nd column
                    colHeaders[col - 1] = mTable.Columns[col].ToString();
                for (int row = 0; row < mTable.Rows.Count; row++)
                {
                    if (typerror)
                        break;
                    if (col == 0) //Mass Tags
                        rowNames[row] = mTable.Rows[row].ItemArray[0].ToString();
                    else // string data
                    {
                        cellValue = mTable.Rows[row].ItemArray[col].ToString();
                        if (cellValue.Length > 0) //not an empty cell
                            matrix[row, col - 1] = cellValue;
                        else
                            matrix[row, col - 1] = "NA";
                    }
                }
            }
            if (!typerror)
            {
                mRary.rowNames = rowNames;
                mRary.colHeaders = colHeaders;
                mRary.mstrMatrix = matrix;
            }
            else
            {
                mRarray = null;
            }
            return mRary;
        }
	}

    /// <summary>
    /// R data matrix
    /// </summary>
    public class clsRarray
    {
        public double[,] matrix;
        public string[,] mstrMatrix;
        public string[] rowNames;
        public string[] colHeaders;
        public string colHs = null;
        public static string rowNamesID = "Row_ID";

        public clsRarray()
        {
            matrix = null;
            mstrMatrix = null;
            rowNames = null;
            colHeaders = null;
        }

        public clsRarray(double[,] mat, string[] rows, string[] cols)
        {
            matrix = mat;
            mstrMatrix = null;
            rowNames = rows;
            colHeaders = cols;
        }

        public clsRarray(string[,] mat, string[] rows, string[] cols)
        {
            matrix = null;
            mstrMatrix = mat;
            rowNames = rows;
            colHeaders = cols;
        }

        public clsRarray(double[,] mat, string[] rows, string col)
        {
            matrix = mat;
            mstrMatrix = null;
            rowNames = rows;
            colHs = col;
        }
    }
}
