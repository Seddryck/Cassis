using Cassis.Core;
using Cassis.Core.Service.SqlHosted;
using Microsoft.SqlServer.Dts.Runtime;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Cassis.Core.Service.SqlAuthentication
{
    class SqlAuthenticationService : SqlHostedService
    {
        public new ISqlAuthenticationPackage PackageInfo
        {
            get { return base.PackageInfo as ISqlAuthenticationPackage; }
        }

        public SqlAuthenticationService(ISqlAuthenticationPackage packageInfo)
            : base(packageInfo)
        { }

        public override PackageResponse Run()
        {
            return Run(PackageInfo);
        }

        public PackageResponse Run(ISqlAuthenticationPackage etl)
        {
            var integrationServices = new Application();
            if (!string.IsNullOrEmpty(etl.Password))
                integrationServices.PackagePassword = etl.Password;

            var package = integrationServices.LoadFromSqlServer(etl.Path + etl.Name
                , etl.Server
                , etl.UserName
                , etl.Password
                , null);

            if ((etl as IParameters)?.Parameters != null)
                Parameterize((etl as IParameters).Parameters, ref package);

            var packageResult = package.Execute(null, null, null, null, null);
            return new PackageResponse(packageResult == DTSExecResult.Success);
        }


    }
}
