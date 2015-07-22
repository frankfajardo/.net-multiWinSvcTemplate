using System.ServiceProcess;
using System.Configuration.Install;
using System.Reflection;


namespace MultipleWindowsServicesInOneProject
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            if (args.Length > 0)
            {
                // Install service
                if (args[0].Trim().ToLower() == "/i")
                { 
                    ManagedInstallerClass.InstallHelper(new string[] { "/i", Assembly.GetExecutingAssembly().Location });
                    return;
                }

                // Uninstall service                 
                if (args[0].Trim().ToLower() == "/u")
                { 
                    ManagedInstallerClass.InstallHelper(new string[] { "/u", Assembly.GetExecutingAssembly().Location });
                    return;
                }
            }

            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[] 
            { 
                new FirstWindowsService(),
                new SecondWindowsService() 
            };
            ServiceBase.Run(ServicesToRun);
        }

    }
}
