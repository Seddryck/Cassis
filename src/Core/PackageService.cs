using System;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Linq;
using Microsoft.SqlServer.Dts.Runtime;
using Ssis = Microsoft.SqlServer.Management.IntegrationServices;
using Remotis.Contract;
using System.Collections.Generic;

namespace Remotis.Service
{
    public class PackageService : IPackageService
    {
        private IPackageEngine engine { get; set; }

        public PackageResponse Run(FilePackage packageInfo, LogOption logOption, IEnumerable<Remotis.Contract.PackageParameter> parameters)
        {
            engine = new FilePackageEngine(packageInfo, parameters);
            return Execute(logOption);
        }

        public PackageResponse Run(SqlHostedPackage packageInfo, LogOption logOption, IEnumerable<Remotis.Contract.PackageParameter> parameters)
        {
            engine = new SqlHostedPackageEngine(packageInfo, parameters);
            return Execute(logOption);
        }

        public PackageResponse Run(SqlHostedPackage packageInfo, SqlAuthentification authentification, LogOption logOption, IEnumerable<Remotis.Contract.PackageParameter> parameters)
        {
            engine = new SqlHostedWithAuthPackageEngine(packageInfo, authentification, parameters);
            return Execute(logOption);
        }

        public PackageResponse Run(CatalogPackage packageInfo, LogOption logOption, IEnumerable<Remotis.Contract.PackageParameter> parameters)
        {
            engine = new CatalogPackageEngine(packageInfo, parameters);
            return Execute(logOption);
        }

        protected PackageResponse Execute(LogOption logOption)
        {
            engine.LogOption = logOption;
            return engine.Run();
        }

    }
}
