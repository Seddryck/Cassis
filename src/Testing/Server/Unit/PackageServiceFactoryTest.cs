using Cassis.Core;
using Cassis.Core.Logging;
using Cassis.Core.Service;
using Cassis.Core.Service.Catalog;
using Cassis.Core.Service.File;
using Cassis.Core.Service.SqlAuthentication;
using Cassis.Core.Service.SqlHosted;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cassis.Testing.Core.Unit
{
    public class PackageServiceFactoryTest
    {
        public static object DynamicMock(Type type)
        {
            var mock = typeof(Mock<>).MakeGenericType(type).GetConstructor(Type.EmptyTypes).Invoke(new object[] { });
            return mock.GetType().GetProperties().Single(f => f.Name == "Object" && f.PropertyType == type).GetValue(mock, new object[] { });
        }

        [Test]
        [TestCase(typeof(IFilePackage), typeof(FileService))]
        [TestCase(typeof(ISqlHostedPackage), typeof(SqlHostedService))]
        [TestCase(typeof(ISqlAuthenticationPackage), typeof(SqlAuthenticationService))]
        [TestCase(typeof(ICatalogPackage), typeof(CatalogService))]
        public void Get_Package_CorrespondingService(Type packageType, Type serviceType)
        {
            
            var packageInfo = DynamicMock(packageType) as IPackageInfo;
            var factory = new PackageServiceFactory();

            var service = factory.Get(packageInfo);
            Assert.That(service, Is.TypeOf(serviceType));
        }

        [Test]
        [TestCase(typeof(IFilePackage), typeof(FileService))]
        [TestCase(typeof(ISqlHostedPackage), typeof(SqlHostedService))]
        [TestCase(typeof(ISqlAuthenticationPackage), typeof(SqlAuthenticationService))]
        [TestCase(typeof(ICatalogPackage), typeof(CatalogService))]
        public void Get_PackageParameters_CorrespondingServiceAndParameters(Type packageType, Type serviceType)
        {
            var packageInfo = DynamicMock(packageType) as IPackageInfo;

            var parameters = new List<IPackageParameter>()
            {
                Mock.Of<IPackageParameter>(p => p.Name=="first" && p.Value == (object)16)
                , Mock.Of<IPackageParameter>(p => p.Name == "second" && p.Value == (object)"seventeen")
            };
            var factory = new PackageServiceFactory();

            var service = factory.Get(packageInfo, parameters);
            Assert.That(service, Is.TypeOf(serviceType));

            var serviceClass = service as AbstractPackageService;
            Assert.That(serviceClass.Parameters, Has.Count.EqualTo(2));
        }

        [Test]
        [TestCase(typeof(IFilePackage), typeof(FileService))]
        [TestCase(typeof(ISqlHostedPackage), typeof(SqlHostedService))]
        [TestCase(typeof(ISqlAuthenticationPackage), typeof(SqlAuthenticationService))]
        [TestCase(typeof(ICatalogPackage), typeof(CatalogService))]
        public void Get_PackageParameters_CorrespondingServiceAndLog(Type packageType, Type serviceType)
        {
            var packageInfo = DynamicMock(packageType) as IPackageInfo;
            var logger = new LogEventService();
            var factory = new PackageServiceFactory();

            var service = factory.Get(packageInfo, null, logger.Log);
            Assert.That(service, Is.TypeOf(serviceType));

            var serviceClass = service as AbstractPackageService;
        }


    }
}
