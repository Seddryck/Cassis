using Cassis.Contract;
using Microsoft.SqlServer.Dts.Runtime;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Cassis.Service
{
    class SqlAuthenticationService : AbstractPackageService
    {
        public new SqlAuthenticationPackage PackageInfo
        {
            get { return base.PackageInfo as SqlAuthenticationPackage; }
        }

        public SqlAuthenticationService(SqlAuthenticationPackage packageInfo)
            : base(packageInfo)
        { }

        public override PackageResponse Run()
        {
            return Run(PackageInfo);
        }

        public PackageResponse Run(SqlAuthenticationPackage packageInfo)
        {
            var integrationServices = new Application();
            if (!string.IsNullOrEmpty(packageInfo.Password))
                integrationServices.PackagePassword = packageInfo.Password;

            var package = integrationServices.LoadFromSqlServer(packageInfo.Path + packageInfo.Name
                , packageInfo.Server
                , packageInfo.UserName
                , packageInfo.Password
                , null);

            var packageResult = package.Execute(null, null, null, null, null);
            return new PackageResponse(packageResult == DTSExecResult.Success);
        }


    }
}
