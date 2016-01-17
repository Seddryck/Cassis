using System;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceModel.Configuration;
using NUnit.Framework;
using Cassis.Client;
using Cassis.Contract;

namespace Cassis.Testing.Acceptance
{
    [Category ("Acceptance")]
    public class PackageTest
    {
        private const string ServiceHostExeFile = "Cassis.Host.Console.exe";
        public ServiceInfo ServiceInfo { get; private set; }
        public Process ConsoleHost { get; private set; }
        public string PackageFullPath { get; set; }

        [SetUp]
        public void InitializeTestEnvironement()
        {
            GetServiceAddress();
            StartService();
            DeployPackage();
        }
  
        private void GetServiceAddress()
        {
            var config = ConfigurationManager.OpenExeConfiguration(ServiceHostExeFile);
            var servicesSection = (ServicesSection)config.GetSection("system.serviceModel/services");  
            var address = servicesSection.Services[0].Host.BaseAddresses[0].BaseAddress;  
            ServiceInfo = new ServiceInfo(address);
        }

        private void StartService()
        {
            try
            {
                // Prepare the process to run
                var start = new ProcessStartInfo();
                // Enter the executable to run, including the complete path
                start.FileName = ServiceHostExeFile;
                // Do you want to show a console window?
                start.WindowStyle = ProcessWindowStyle.Normal;
                start.CreateNoWindow = false;

                // Run the external process & wait for it to finish
                ConsoleHost = Process.Start(start);
                Console.WriteLine("Service started with PID {0}", ConsoleHost.Id);
            }
            catch (Exception)
            {
                Assert.Fail("Unable to start console host for service");
            }
        }

        private void DeployPackage()
        {
            //Build the fullpath for the file to read
            Directory.CreateDirectory("Etl");
            PackageFullPath = FileOnDisk.CreatePhysicalFile(@"Etl\Sample.dtsx", string.Format("{0}.Resources.Sample.dtsx", this.GetType().Namespace));
        }

        [TearDown]
        public void CleanupTestEnvironement()
        {
            KillService();
            CleanPackage();
        }

        private void CleanPackage()
        {
            if (!string.IsNullOrEmpty(PackageFullPath) && File.Exists(PackageFullPath))
                File.Delete(PackageFullPath);
        }

        private void KillService()
        {
            if (ConsoleHost != null)
                ConsoleHost.Kill();
        }

        [Test]
        public void Run_PackageWithoutParameter_ExecutionSuccessful()
        {
            var package = new FilePackage();
            package.Name = Path.GetFileName(PackageFullPath);
            package.Path = Path.GetDirectoryName(PackageFullPath) + Path.DirectorySeparatorChar;
            package.Password = "p@ssw0rd";

            var client = new IntegrationServiceClient(ServiceInfo);
            var response = client.Execute(package, null);

            Assert.True(response.Success);
        }
    }
}
