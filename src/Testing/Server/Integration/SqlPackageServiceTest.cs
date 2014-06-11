using NUnit.Framework;
using Remotis.Contract;
using Remotis.Service;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.SqlServer.Dts.Runtime;

namespace Remotis.Testing.Service
{
    [Category ("Integration")]
    public class SqlPackageServiceTest
    {
        public const string IntegrationFolder = @"File System\RemotisTesting\";

        #region Setup & Cleanup
        [SetUp]
        public void InitializeTestEnvironement()
        {
            CleanOutputFile();
            DeployPackage();
        }

        private void CleanOutputFile()
        {
            if (File.Exists("Toto2.txt"))
                File.Delete("Toto2.txt");
        }
        private void DeployPackage()
        {
            //Build the fullpath for the file to read
            Directory.CreateDirectory("Etl");
            var packageFullPath = FileOnDisk.CreatePhysicalFile(@"Etl\Sample.dtsx", string.Format("{0}.Resources.Sample.dtsx", this.GetType().Namespace));

            CleanPackage();
            var integrationServer = new Application();
            integrationServer.PackagePassword = "p@ssw0rd";
            var package = integrationServer.LoadPackage(packageFullPath, null);

            if (!integrationServer.FolderExistsOnDtsServer(IntegrationFolder, ConfigurationReader.GetServerName()))
                integrationServer.CreateFolderOnDtsServer(IntegrationFolder.Split('\\')[0], IntegrationFolder.Split('\\')[1], ConfigurationReader.GetServerName());
            
            // Save the package under myFolder which is found under the 
            // File System folder on the Integration Services service.
            integrationServer.SaveToDtsServer(package, null, IntegrationFolder + "Sample", ConfigurationReader.GetServerName());
        }

        [TearDown]
        public void CleanupTestEnvironement()
        {
            CleanPackage();
            CleanOutputFile();
        }

        private void CleanPackage()
        {
            var integrationServer = new Application();
            if (integrationServer.ExistsOnDtsServer(IntegrationFolder + "Sample", ConfigurationReader.GetServerName()))
                integrationServer.RemoveFromDtsServer(IntegrationFolder + "Sample", ConfigurationReader.GetServerName());
        } 
        #endregion

        [Test]
        public void Run_SqlPackage_Sucessful()
        {
            var packageInfo = new SqlHostedPackage()
            {
                Password="p@ssw0rd",
                Path = IntegrationFolder,
                Name="Sample",
                Server="."
            };
            
            var packageService = new PackageService();
            var result = packageService.Run(packageInfo, null, null);

            Assert.That(result.Success, Is.True);
            Assert.That(result.Errors, Has.Count.EqualTo(0));
            Assert.That(File.Exists("Toto2.txt"));
        }


    }
}
