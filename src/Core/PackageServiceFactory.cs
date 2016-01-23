using Cassis.Core;
using Cassis.Core.Logging;
using Cassis.Core.Service;
#if ! SqlServer2008R2
using Cassis.Core.Service.Catalog;
#endif
using Cassis.Core.Service.File;
using Cassis.Core.Service.SqlAuthentication;
using Cassis.Core.Service.SqlHosted;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cassis.Core
{
    public class PackageServiceFactory
    {
        public virtual IPackageService Get(IPackageInfo packageInfo)
        {
            #if !SqlServer2008R2
            if (packageInfo is ICatalogPackage)
                return new CatalogService(packageInfo as ICatalogPackage);
            #endif
            if (packageInfo is ISqlAuthenticationPackage)
                return new SqlAuthenticationService(packageInfo as ISqlAuthenticationPackage);
            if (packageInfo is ISqlHostedPackage)
                return new SqlHostedService(packageInfo as ISqlHostedPackage);
            if (packageInfo is IFilePackage)
                return new FileService(packageInfo as IFilePackage);
            throw new ArgumentException();
        }

        public virtual IPackageService Get(IPackageInfo packageInfo, IEnumerable<IPackageParameter> parameters)
        {
            var service = Get(packageInfo) as AbstractPackageService;
            service.Parameters = parameters;

            return service;
        }

        public virtual IPackageService Get
        (
            IPackageInfo packageInfo
            , IEnumerable<IPackageParameter> parameters
            , LogAction log
        )
        {
            var service = Get(packageInfo) as AbstractPackageService;
            service.Parameters = parameters;
            service.Log = log;

            return service;
        }
    }
}
