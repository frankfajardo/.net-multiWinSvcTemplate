using System.ComponentModel;
using System.Configuration.Install;
using System.ServiceProcess;
using System.Configuration.Install;
using System.Configuration;

namespace Abcm.WindowsServices
{
    [RunInstaller(true)]
    public partial class ProjectInstaller : System.Configuration.Install.Installer
    {
        public ProjectInstaller()
        {
            InitializeComponent();

            this.svcInstaller1.ServiceName = ConfigurationManager.AppSettings["FirstWindowsService.ServiceName"];
            this.svcInstaller1.Description = ConfigurationManager.AppSettings["FirstWindowsService.ServiceDescription"];
            this.svcInstaller1.StartType = System.ServiceProcess.ServiceStartMode.Automatic;
            this.svcpInstaller1.Account = ServiceAccount.LocalSystem;

            this.svcInstaller2.ServiceName = ConfigurationManager.AppSettings["SecondWindowsService.ServiceName"];
            this.svcInstaller2.Description = ConfigurationManager.AppSettings["SecondWindowsService.ServiceDescription"];
            this.svcInstaller2.StartType = System.ServiceProcess.ServiceStartMode.Automatic;
            this.svcpInstaller2.Account = ServiceAccount.LocalSystem;

            // 
            // ProjectInstaller
            // 
            this.Installers.AddRange(
                new Installer[] {
                this.svcpInstaller1,
                this.svcInstaller1,
                this.svcpInstaller2,
                this.svcInstaller2
            });


        }
    }
}
