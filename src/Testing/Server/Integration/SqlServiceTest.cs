using Cassis.Core.Service.SqlHosted;
using Microsoft.SqlServer.Dts.Runtime;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;



namespace Cassis.Testing.Core.Integration
{
    [Category ("Integration")]
    public class SqlServiceTest
    {
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
            var @namespace = this.GetType().Assembly.GetName().Name;
            var packageFullPath = FileOnDisk.CreatePhysicalFile(@"Etl\Sample.dtsx", string.Format("{0}.Resources.Sample.dtsx", @namespace));

            CleanPackage();
            var integrationServer = new Application();
            integrationServer.PackagePassword = "p@ssw0rd";
            var package = integrationServer.LoadPackage(packageFullPath, null);

            if (!integrationServer.FolderExistsOnDtsServer(@"File System\CassisTesting", ConfigurationReader.GetServerName()))
                integrationServer.CreateFolderOnDtsServer(@"File System", "CassisTesting", ConfigurationReader.GetServerName());
            
            // Save the package under myFolder which is found under the 
            // File System folder on the Integration Services service.
            integrationServer.SaveToDtsServer(package, null, @"File System\CassisTesting\Sample", ConfigurationReader.GetServerName());
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
            if (integrationServer.ExistsOnDtsServer(@"File System\CassisTesting\Sample", ConfigurationReader.GetServerName()))
                integrationServer.RemoveFromDtsServer(@"File System\CassisTesting\Sample", ConfigurationReader.GetServerName());
        } 
        #endregion

        [Test]
        public void Run_Package_Sucessful()
        {
            var packageInfo = new SqlHostedPackage()
            {
                Password="p@ssw0rd",
                Path = @"File System\CassisTesting\",
                Name="Sample"
            };
            
            var packageService = new SqlHostedService(packageInfo);
            var result = packageService.Run();

            Assert.That(result.Success, Is.True);
            Assert.That(result.Errors, Has.Count.EqualTo(0));
            Assert.That(File.Exists("Toto2.txt"));
        }


    }
}
