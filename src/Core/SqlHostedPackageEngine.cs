using Microsoft.SqlServer.Dts.Runtime;
using Remotis.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Remotis.Service
{
    class SqlHostedPackageEngine : PackageCentricEngine
    {
        protected new SqlHostedPackage PackageInfo
        {
            get { return base.PackageInfo as SqlHostedPackage; }
            set { base.PackageInfo = value; }
        }

        public SqlHostedPackageEngine(SqlHostedPackage packageInfo, IEnumerable<PackageParameter> parameters)
            : base(packageInfo, parameters) {}

        protected override string GetPackagePath(FilePackage packageInfo)
        {
            return packageInfo.Path
                    + packageInfo.Name;
        }

        protected override Package LoadPackage(string packagePath)
        {
            return IntegrationService.LoadFromDtsServer(packagePath, PackageInfo.Server, null);
        }

    }
}
