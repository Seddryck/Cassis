using Cassis.Contract;
using Cassis.Service;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cassis.Testing.Service.Unit
{
    public class PackageServiceFactoryTest
    {
        [Test]
        [TestCase(typeof(FilePackage), typeof(FileService))]
        [TestCase(typeof(SqlPackage), typeof(SqlService))]
        [TestCase(typeof(SqlAuthenticationPackage), typeof(SqlAuthenticationService))]
        [TestCase(typeof(CatalogPackage), typeof(CatalogService))]
        public void Get_FilePackage_FileService(Type packageType, Type serviceType)
        {
            var packageInfo = packageType.GetConstructor(new Type[] { }) as IPackageInfo;
            var factory = new PackageServiceFactory();

            var service = factory.Get(packageInfo);
            Assert.That(service, Is.TypeOf(serviceType));
        }
    }
}
