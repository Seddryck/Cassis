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
    class SqlHostedService : AbstractPackageService
    {
        public new SqlHostedPackage PackageInfo
        {
            get { return base.PackageInfo as SqlHostedPackage; }
        }

        public SqlHostedService(SqlHostedPackage packageInfo)
            : base(packageInfo)
        { }

        public override PackageResponse Run()
        {
            return Run(PackageInfo);
        }

        protected PackageResponse Run(SqlHostedPackage etl)
        {
            var integrationServices = new Application();
            if (!string.IsNullOrEmpty(etl.Password))
                integrationServices.PackagePassword = etl.Password;

            var package = integrationServices.LoadFromDtsServer(etl.Path + etl.Name, etl.Server, null);

            Parameterize(etl.Parameters, ref package);

            var events = new PackageEvents();
            var packageResult = package.Execute(null, null, events, null, null);
            return new PackageResponse(packageResult == DTSExecResult.Success, events);
        }

        protected virtual void Parameterize(IEnumerable<PackageParameter> parameters, ref Package package)
        {
            foreach (var param in parameters)
            {
#if !SqlServer2008R2
                if (package.Parameters.Contains(param.Name))
                    package.Parameters[param.Name].Value = param.Value.ToString();
                else
                {
#endif
                    if (package.Variables.Contains(param.Name))
                        package.Variables[param.Name].Value = DefineValue(param.Value.ToString(), package.Variables[param.Name].DataType);
                    else
                        throw new ArgumentOutOfRangeException("param.Name", string.Format("No parameter or variable named '{0}' found in the package {1}, can't override its value for execution.", param.Name, package.Name));
#if !SqlServer2008R2
                }
#endif
            }
        }
    }
}
