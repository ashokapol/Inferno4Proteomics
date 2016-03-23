using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.IO;
using DAnTE.Paradiso;

namespace DAnTE.Inferno
{
	/// <summary>
	/// Summary description for frmMain.
	/// </summary>
	public class frmMain : System.Windows.Forms.Form
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public frmMain()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

        private static bool AppUpdatesAvailableInternal()
        {
            string filelocation = @"\\floyd\Software\Inferno\";
            string fname = null, currver = Application.ProductVersion;

            if (Directory.Exists(filelocation))
            {
                string[] folders = Directory.GetDirectories(filelocation);
                foreach (string f in folders)
                {
                    if (f.Contains("Ver_"))
                    {
                        fname = f.Substring(f.IndexOf("_") + 1).Trim();
                        if (fname.Equals(currver))
                            return false;
                        else
                            return true;
                    }
                }
            }
            return false;
        }

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this.Size = new System.Drawing.Size(300,300);
			this.Text = "frmMain";
		}
		#endregion

		[STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            //if (AppUpdatesAvailableInternal())
            //{
            //    MessageBox.Show("New version of Inferno available.",
            //        "New version!", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //}
            
            //Splasher.Show(typeof(frmSplash));

            string SelectedDNTFile = null;
            string logFilename = null;

            DAnTE.Tools.ProgramArguments pa = new DAnTE.Tools.ProgramArguments(args);

            if (args.Length > 0)
            {
                SelectedDNTFile = pa.DNTfilename;
                logFilename = pa.LOGfilename;
            }
            frmDAnTEmdi mfrmDAnTEmdi = new frmDAnTEmdi(SelectedDNTFile, logFilename);
            //mfrmDAnTEmdi.SessionFile = SelectedDNTFile;
            //mfrmDAnTEmdi.logFilename = logFilename;
            if (mfrmDAnTEmdi != null && !mfrmDAnTEmdi.IsDisposed)
                Application.Run(mfrmDAnTEmdi);
        }
	}
}
