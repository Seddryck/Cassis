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

        public override Package Load(ref Application integrationServices)
        {
            return integrationServices.LoadFromSqlServer(PackageInfo.Path + PackageInfo.Name
                , PackageInfo.Server
                , PackageInfo.UserName
                , PackageInfo.Password
                , null);
        }


    }
}
