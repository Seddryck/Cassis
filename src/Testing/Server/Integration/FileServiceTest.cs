using NUnit.Framework;
using Cassis.Contract;
using Cassis.Service;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Cassis.Testing.Service
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
            if (File.Exists("Toto2.txt"))
                File.Delete("Toto2.txt");
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
            var packageInfo = new FilePackage()
            {
                Password="p@ssw0rd",
                Path=@"Etl\",
                Name="Sample"
            };
            
            var packageService = new FileService(packageInfo);
            var result = packageService.Run();

            Assert.That(result.Success, Is.True);
            Assert.That(result.Errors, Has.Count.EqualTo(0));
            Assert.That(File.Exists("Toto2.txt"));
        }




    }
}
