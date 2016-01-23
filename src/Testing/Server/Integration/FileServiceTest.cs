using Cassis.Core;
using Cassis.Core.Logging;
using Cassis.Core.Service.File;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Cassis.Testing.Core.Integration
{
    [Category ("Integration")]
    public class FileServiceTest
    {
        public string PackageFullPath { get; set; }

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
        }
        private void DeployPackage()
        {
            //Build the fullpath for the file to read
            Directory.CreateDirectory("Etl");
            var @namespace = this.GetType().Assembly.GetName().Name;
            PackageFullPath = FileOnDisk.CreatePhysicalFile(@"Etl\Sample.dtsx", string.Format("{0}.Resources.Sample.dtsx", @namespace));
        }

        [TearDown]
        public void CleanupTestEnvironement()
        {
            CleanPackage();
            CleanOutputFile();
        }

        private void CleanPackage()
        {
            if (!string.IsNullOrEmpty(PackageFullPath) && File.Exists(PackageFullPath))
                File.Delete(PackageFullPath);
        } 
        #endregion

        [Test]
        public void Run_Package_Sucessful()
        {
            var packageInfo = Mock.Of<IFilePackage>
            (
                p =>
                p.Password=="p@ssw0rd" &&
                p.Path ==@"Etl\" &&
                p.Name =="Sample"
            );
            
            var packageService = new FileService(packageInfo);
            var result = packageService.Run();

            Assert.That(result.Success, Is.True);
            Assert.That(result.Errors, Has.Count.EqualTo(0));
            Assert.That(File.Exists(@"..\..\Toto.txt"));
        }

        [Test]
        public void Run_PackageWithLogger_LogCalled()
        {
            var packageInfo = Mock.Of<IFilePackage>
            (
                p =>
                p.Password == "p@ssw0rd" &&
                p.Path == @"Etl\" &&
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
            var packageInfo = Mock.Of<IFilePackage>
            (
                p =>
                p.Password == "p@ssw0rd" &&
                p.Path == @"Etl\" &&
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
