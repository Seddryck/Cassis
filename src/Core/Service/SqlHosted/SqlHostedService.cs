using Cassis.Core;
using Cassis.Core.Service;
using Microsoft.SqlServer.Dts.Runtime;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Cassis.Core.Service.SqlHosted
{
    class SqlHostedService : AbstractPackageService
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

        protected PackageResponse Run(ISqlHostedPackage etl)
        {
            var integrationServices = new Application();
            if (!string.IsNullOrEmpty(etl.Password))
                integrationServices.PackagePassword = etl.Password;

            var package = integrationServices.LoadFromDtsServer(etl.Path + etl.Name, etl.Server, null);

            if ((etl as IParameters)?.Parameters!=null)
                Parameterize((etl as IParameters).Parameters, ref package);

            var events = new PackageEvents();
            var packageResult = package.Execute(null, null, events, null, null);
            return new PackageResponse(packageResult == DTSExecResult.Success, events);
        }
    }
}
