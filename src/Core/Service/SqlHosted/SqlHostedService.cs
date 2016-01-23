using Cassis.Core;
using Cassis.Core.Logging;
using Cassis.Core.Service;
using Microsoft.SqlServer.Dts.Runtime;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Cassis.Core.Service.File;

namespace Cassis.Core.Service.SqlHosted
{
    class SqlHostedService : AbstractDtsPackageService
    {
        public new ISqlHostedPackage PackageInfo
        {
            get { return base.PackageInfo as ISqlHostedPackage; }
        }

        public SqlHostedService(ISqlHostedPackage packageInfo)
            : base(packageInfo)
        { }

        public override PackageResponse Run()
        {
            return Run(PackageInfo);
        }

        public override Package Load(ref Application integrationServices)
        {
            return integrationServices.LoadFromDtsServer(PackageInfo.Path + PackageInfo.Name, PackageInfo.Server, null);
        }
    }
}
