using Microsoft.SqlServer.Dts.Runtime;
using Remotis.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Remotis.Service
{
    class SqlHostedWithAuthPackageEngine : SqlHostedPackageEngine
    {
        protected SqlAuthentification Authentification {get; set;}

        public SqlHostedWithAuthPackageEngine(SqlHostedPackage packageInfo, SqlAuthentification authentification, IEnumerable<PackageParameter> parameters)
            : base(packageInfo, parameters) 
        {
            Authentification = authentification;
        }


    }
}
