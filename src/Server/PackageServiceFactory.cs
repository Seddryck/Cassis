using Cassis.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cassis.Service
{
    public class PackageServiceFactory
    {
        public virtual IPackageService Get(IPackageInfo packageInfo)
        {
            #if !SqlServer2008R2
            if (packageInfo is CatalogPackage)
                return new CatalogService(packageInfo as CatalogPackage);
            #endif
            if (packageInfo is SqlAuthenticationPackage)
                return new SqlAuthenticationService(packageInfo as SqlAuthenticationPackage);
            if (packageInfo is SqlHostedPackage)
                return new SqlHostedService(packageInfo as SqlHostedPackage);
            if (packageInfo is FilePackage)
                return new FileService(packageInfo as FilePackage);
            throw new ArgumentException();
        }

        public virtual IPackageService Get(IPackageInfo packageInfo, IEnumerable<PackageParameter> parameters)
        {
            var service = Get(packageInfo) as AbstractPackageService;
            service.Parameters = parameters;

            return service;
        }
    }
}
