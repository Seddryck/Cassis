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
            if (packageInfo is CatalogPackage)
                return new CatalogService(packageInfo as CatalogPackage);
            if (packageInfo is SqlAuthenticationPackage)
                return new SqlAuthenticationService(packageInfo as SqlAuthenticationPackage);
            if (packageInfo is SqlPackage)
                return new SqlService(packageInfo as SqlPackage);
            if (packageInfo is FilePackage)
                return new FileService(packageInfo as FilePackage);
            throw new ArgumentException();
        }
    }
}
