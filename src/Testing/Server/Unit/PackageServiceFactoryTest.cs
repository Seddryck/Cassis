using Cassis.Core;
using Cassis.Core.Service;
using Cassis.Core.Service.Catalog;
using Cassis.Core.Service.File;
using Cassis.Core.Service.SqlAuthentication;
using Cassis.Core.Service.SqlHosted;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cassis.Testing.Core.Unit
{
    public class PackageServiceFactoryTest
    {
        [Test]
        [TestCase(typeof(FilePackage), typeof(FileService))]
        [TestCase(typeof(SqlHostedPackage), typeof(SqlHostedService))]
        [TestCase(typeof(SqlAuthenticationPackage), typeof(SqlAuthenticationService))]
        [TestCase(typeof(CatalogPackage), typeof(CatalogService))]
        public void Get_Package_CorrespondingService(Type packageType, Type serviceType)
        {
            var ctor = packageType.GetConstructor(new Type[] { });
            var packageInfo = ctor.Invoke(new object[] { }) as IPackageInfo;
            var factory = new PackageServiceFactory();

            var service = factory.Get(packageInfo);
            Assert.That(service, Is.TypeOf(serviceType));
        }

        [Test]
        [TestCase(typeof(FilePackage), typeof(FileService))]
        [TestCase(typeof(SqlHostedPackage), typeof(SqlHostedService))]
        [TestCase(typeof(SqlAuthenticationPackage), typeof(SqlAuthenticationService))]
        [TestCase(typeof(CatalogPackage), typeof(CatalogService))]
        public void Get_PackageParameters_CorrespondingServiceAndParameters(Type packageType, Type serviceType)
        {
            var ctor = packageType.GetConstructor(new Type[] { });
            var packageInfo = ctor.Invoke(new object[] { }) as IPackageInfo;

            var parameters = new List<PackageParameter>()
            {
                new PackageParameter() { Name="first", Value = 16}
                , new PackageParameter() { Name="second", Value = "seventeen"}
            };
            var factory = new PackageServiceFactory();

            var service = factory.Get(packageInfo, parameters);
            Assert.That(service, Is.TypeOf(serviceType));

            var serviceClass = service as AbstractPackageService;
            Assert.That(serviceClass.Parameters, Has.Count.EqualTo(2));
        }
    }
}
