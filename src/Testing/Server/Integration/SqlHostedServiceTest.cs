using Cassis.Core;
using Cassis.Core.Logging;
using Cassis.Core.Service;
using Cassis.Core.Service.SqlHosted;
using Microsoft.SqlServer.Dts.Runtime;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;



namespace Cassis.Testing.Core.Integration
{
    [Category ("Integration")]
    public class SqlHostedServiceTest
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
            if (File.Exists(@"..\..\Toto.txt"))
                File.Delete(@"..\..\Toto.txt");

            if (File.Exists(@"..\..\Toto2.txt"))
                File.Delete(@"..\..\Toto2.txt");
        }
        private void DeployPackage()
        {
            //Build the fullpath for the file to read
            if (!Directory.Exists("Etl"))
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
            var packageInfo = Mock.Of<ISqlHostedPackage>
            (
                p =>
                p.Password=="p@ssw0rd" &&
                p.Path == @"File System\CassisTesting\" &&
                p.Name =="Sample"
            );
            
            var packageService = new SqlHostedService(packageInfo);
            var result = packageService.Run();

            Assert.That(result.Success, Is.True);
            Assert.That(result.Errors, Has.Count.EqualTo(0));
            Assert.That(File.Exists(@"..\..\Toto.txt"));
        }

        [Test]
        public void Run_PackageWithParameter_Sucessful()
        {
            var packageInfo = Mock.Of<ISqlHostedPackage>
            (
                p =>
                p.Password == "p@ssw0rd" &&
                p.Path == @"File System\CassisTesting\" &&
                p.Name == "Sample"
            );

            var parameters = new List<IPackageParameter>();
            var parameter = Mock.Of<IPackageParameter>
            (
                p =>
                p.Name == "DestinationPath" &&
                p.Value == (object)@"C:\Users\Cedri\Projects\Cassis\src\Testing\Server\Toto2.txt"
            );
            parameters.Add(parameter);

            var factory = new PackageServiceFactory();
            var packageService = factory.Get(packageInfo, parameters);
            packageService.Run();

            Assert.That(File.Exists(@"..\..\Toto2.txt"));
        }

        [Test]
        public void Run_PackageWithLogger_LogCalled()
        {
            var packageInfo = Mock.Of<ISqlHostedPackage>
            (
                p =>
                p.Password == "p@ssw0rd" &&
                p.Path == @"File System\CassisTesting\" &&
                p.Name == "Sample"
            );

            var count = 0;
            LogAction log = (string a, string b, string c, string d, string e, string f, string g, DateTime h, DateTime i, int j, ref byte[] dataBytes) => count++;

            var factory = new PackageServiceFactory();
            var packageService = factory.Get(packageInfo, null, log);
            var result = packageService.Run();

            Assert.That(count, Is.GreaterThan(0));
        }

        [Test]
        public void Run_PackageWithLogger_PackageEndIsReceived()
        {
            var packageInfo = Mock.Of<ISqlHostedPackage>
            (
                p =>
                p.Password == "p@ssw0rd" &&
                p.Path == @"File System\CassisTesting\" &&
                p.Name == "Sample"
            );

            var packageEnd = false;
            LogAction log = (string eventName, string b, string c, string d, string e, string f, string g, DateTime h, DateTime i, int j, ref byte[] dataBytes) => packageEnd = eventName == "PackageEnd";

            var factory = new PackageServiceFactory();
            var packageService = factory.Get(packageInfo, null, log);
            var result = packageService.Run();

            Assert.That(packageEnd, Is.True);
        }


    }
}
