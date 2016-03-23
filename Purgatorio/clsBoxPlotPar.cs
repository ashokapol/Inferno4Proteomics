using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using DAnTE.Properties;

namespace DAnTE.Purgatorio
{
    public class clsBoxPlotPar
    {
        private string rcmd;
        public string datasubset;
        public string color;
        public bool trBkground;
        public string Rdataset;
        public string mstrDatasetName;
        public string factor;
        public int factorIdx;
        public decimal boxWidth;
        public decimal fontScale;
        public bool outliers;
        public bool showcount;
        public bool stamp;
        public string tempFile;
        public ArrayList Factors = new ArrayList();
        public ArrayList Datasets = new ArrayList();
        public ArrayList CheckedDatasets = new ArrayList();

        public clsBoxPlotPar()
        {
            datasubset = null;
            mstrDatasetName = "";
            color = Settings.Default.boxplotCol;
            if (color == "")
            {
                color = "#CAFF70";
                Settings.Default.boxplotCol = color;
                Settings.Default.Save();
            }
            trBkground = false;
            factor = "1";
            factorIdx = 0;
            boxWidth = 0.8M;
            fontScale = 0.8M;
            outliers = true;
            showcount = false;
            stamp = false;
        }

        [DAnTE.Tools.clsAnalysisAttribute("RBoxPlotCmd", "Plots")]
        public string Rcmd
        {
            get
            {
                rcmd = "dataBoxPlots(" + Rdataset + "[," + datasubset + "]," + this.FontScale + ",";
                rcmd = rcmd + @"color=""" + color + @"""," + this.BoxWidth + "," + this.Background +
                        ",Factor=" + factor + "," + this.ShowCount + "," + this.Stamp + ",";
                if (!outliers)
                    rcmd = rcmd + "outliers=FALSE,";
                rcmd = rcmd + @"file=""" + tempFile + @""")";
                return rcmd;
            }
        }

        public string FontScale
        {
            get
            {
                return ("labelscale=" + fontScale.ToString());
            }
        }

        public string BoxWidth
        {
            get
            {
                return ("boxwidth=" + boxWidth);
            }
        }

        public string Background
        {
            get
            {
                if (trBkground)
                    return "bkground=\"transparent\"";
                else
                    return "bkground=\"white\"";
            }
        }

        public string Stamp
        {
            get
            {
                if (stamp)
                    return @"stamp=""" + Settings.Default.DataFileName.Replace("\\", "\\\\") + @"""";
                else
                    return "stamp=NULL";
            }
        }

        public string ShowCount
        {
            get
            {
                if (showcount)
                    return "showcount=TRUE";
                else
                    return "showcount=FALSE";
            }
        }
    }
}
