using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace DAnTE.Paradiso
{
	/// <summary>
	/// Summary description for frmRmessages. 
	/// </summary>
	public class frmRmsg : System.Windows.Forms.Form
    {
        //public AxSTATCONNECTORCLNTLib.AxStatConnectorCharacterDevice axStatConnectorCharacterDevice1;
        //public STATCONNECTORCLNTLib.StatConnectorCharacterDevice axStatConnectorCharacterDevice1;
        private Panel panel1;
        private Button mbtnOK;
        public AxSTATCONNECTORCLNTLib.AxStatConnectorCharacterDevice axStatConnectorCharacterDevice1;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		//private static frmRmessages _instance = null ;

		public frmRmsg()
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

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmRmsg));
            this.panel1 = new System.Windows.Forms.Panel();
            this.mbtnOK = new System.Windows.Forms.Button();
            this.axStatConnectorCharacterDevice1 = new AxSTATCONNECTORCLNTLib.AxStatConnectorCharacterDevice();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.axStatConnectorCharacterDevice1)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.mbtnOK);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 528);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(693, 51);
            this.panel1.TabIndex = 4;
            // 
            // mbtnOK
            // 
            this.mbtnOK.Location = new System.Drawing.Point(314, 15);
            this.mbtnOK.Name = "mbtnOK";
            this.mbtnOK.Size = new System.Drawing.Size(75, 23);
            this.mbtnOK.TabIndex = 0;
            this.mbtnOK.Text = "OK";
            this.mbtnOK.UseVisualStyleBackColor = true;
            this.mbtnOK.Click += new System.EventHandler(this.btnOKClick_event);
            // 
            // axStatConnectorCharacterDevice1
            // 
            this.axStatConnectorCharacterDevice1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.axStatConnectorCharacterDevice1.Enabled = true;
            this.axStatConnectorCharacterDevice1.Location = new System.Drawing.Point(0, 0);
            this.axStatConnectorCharacterDevice1.Name = "axStatConnectorCharacterDevice1";
            this.axStatConnectorCharacterDevice1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axStatConnectorCharacterDevice1.OcxState")));
            this.axStatConnectorCharacterDevice1.Size = new System.Drawing.Size(0, 0);
            this.axStatConnectorCharacterDevice1.TabIndex = 5;
            // 
            // frmRmsg
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(693, 579);
            this.Controls.Add(this.axStatConnectorCharacterDevice1);
            this.Controls.Add(this.panel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmRmsg";
            this.Text = "R Messages";
            this.Load += new System.EventHandler(this.frmRmsg_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmClosing_event);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.axStatConnectorCharacterDevice1)).EndInit();
            this.ResumeLayout(false);

		}
		#endregion

		private void btnOKClick_event(object sender, System.EventArgs e)
		{
			this.DialogResult = DialogResult.OK ;
			this.Hide() ;
		}

        private void FrmClosing_event(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }

        private void frmRmsg_Load(object sender, EventArgs e)
        {
            
        }

        //public static frmRmessages Instance
        //{
        //    get
        //    {
        //        if (frmRmessages._instance == null || frmRmessages._instance.IsDisposed)
        //        {
        //            frmRmessages._instance = new frmRmessages();
        //        }
        //        return frmRmessages._instance;
        //    }
        //}
	}
}
