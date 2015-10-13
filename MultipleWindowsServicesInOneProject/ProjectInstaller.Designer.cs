using System.ServiceProcess;
using System.Configuration.Install;
using System.Configuration;

namespace MultipleWindowsServicesInOneProject
{

    partial class ProjectInstaller
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.svcInstaller1 = new ServiceInstaller();
            this.svcpInstaller1 = new ServiceProcessInstaller();

            this.svcInstaller2 = new ServiceInstaller();
            this.svcpInstaller2 = new ServiceProcessInstaller();

        }

        #endregion

        private ServiceProcessInstaller svcpInstaller1;
        private ServiceInstaller svcInstaller1;

        private ServiceProcessInstaller svcpInstaller2;
        private ServiceInstaller svcInstaller2;

    }
}