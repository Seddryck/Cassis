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
    public class CatalogPackageServiceTest
    {
        #region Setup & Cleanup
        [SetUp]
        public void InitializeTestEnvironement()
        {
            CleanOutputFile();
        }

        private void CleanOutputFile()
        {
            if (File.Exists(ConfigurationReader.GetDestinationPath()))
                File.Delete(ConfigurationReader.GetDestinationPath());
        }
        
        [TearDown]
        public void CleanupTestEnvironement()
        {
            CleanOutputFile();
        }

        #endregion

        [Test]
        public void Run_CatalogPackage_Sucessful()
        {
            var packageInfo = new CatalogPackage()
            {
                Password="p@ssw0rd"
                , Server = ConfigurationReader.GetSqlServerName()
                , Catalog="SSISDB"
                , Folder = "RemotisTesting" 
                , Project="Remotis.Testing"
                , Name="Sample"
            };
            
            var packageService = new PackageService();
            var result = packageService.Run(packageInfo, null, null);

            Assert.That(result.Success, Is.True);
            Assert.That(result.Errors, Has.Count.EqualTo(0));
        }

        [Test]
        public void Run_Parameters_Sucessful()
        {
            var packageInfo = new CatalogPackage()
            {
                Password="p@ssw0rd"
                , Server = ConfigurationReader.GetSqlServerName()
                , Catalog="SSISDB"
                , Folder = "RemotisTesting" 
                , Project="Remotis.Testing"
                , Name="Sample"
            };

            var path = Path.GetDirectoryName(ConfigurationReader.GetDestinationPath()) + @"\Toto4.txt";
         
            var param = new PackageParameter()
            {
                Name = "DestinationPath",
                Value = path,
            };
            var parameters = new List<PackageParameter>() { param };

            var packageService = new PackageService();
            var result = packageService.Run(packageInfo, null, parameters);

            Assert.That(result.Success, Is.True);
            Assert.That(result.Errors, Has.Count.EqualTo(0));
            Assert.That(!File.Exists(ConfigurationReader.GetDestinationPath()));
            Assert.That(File.Exists(path));

            File.Delete(path);
        }


    }
}
