using System;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Linq;
using Microsoft.SqlServer.Dts.Runtime;
using Ssis = Microsoft.SqlServer.Management.IntegrationServices;
using Remotis.Contract;

namespace Remotis.Service
{
    public class PackageService : IPackageService
    {
        
        public PackageResponse Run(FilePackage packageInfo, LogOption logOption)
        {
            var LogGate
            var integrationServices = new Application();
            if (!string.IsNullOrEmpty(packageInfo.Password))
                integrationServices.PackagePassword = packageInfo.Password;

            var package = integrationServices.LoadPackage(packageInfo.Path + packageInfo.Name, null);

            var packageResult = package.Execute(null, null, null, null, null);
            return new PackageResponse(packageResult == DTSExecResult.Success);
        }

        public PackageResponse Run(SqlPackage packageInfo)
        {
            var integrationServices = new Application();
            if (!string.IsNullOrEmpty(packageInfo.Password))
                integrationServices.PackagePassword = packageInfo.Password;

            var package = integrationServices.LoadFromDtsServer(packageInfo.Path + packageInfo.Name, packageInfo.Server, null);

            var packageResult = package.Execute(null, null, null, null, null);
            return new PackageResponse(packageResult == DTSExecResult.Success);
        }

        public PackageResponse Run(SqlPackage packageInfo, SqlAuthentification authentification)
        {
            var integrationServices = new Application();
            if (!string.IsNullOrEmpty(packageInfo.Password))
                integrationServices.PackagePassword = packageInfo.Password;

            var package = integrationServices.LoadFromSqlServer(packageInfo.Path + packageInfo.Name
                , packageInfo.Server
                , authentification.UserName
                , authentification.Password
                , null);

            var packageResult = package.Execute(null, null, null, null, null);
            return new PackageResponse(packageResult == DTSExecResult.Success);
        }

        public PackageResponse Run(CatalogPackage packageInfo)
        {
            var connection = new SqlConnection(string.Format(@"Data Source={0};Initial Catalog=master;Integrated Security=SSPI;", packageInfo.Server));
            var integrationServices = new Ssis.IntegrationServices(connection);

            var catalog = integrationServices.Catalogs[packageInfo.Catalog];
            var folder = catalog.Folders[packageInfo.Folder];
            var project = folder.Projects[packageInfo.Project];
            var package = project.Packages[packageInfo.Name];

            var setValueParameters = new Collection<Ssis.PackageInfo.ExecutionValueParameterSet>();
            setValueParameters.Add(new Ssis.PackageInfo.ExecutionValueParameterSet
            {
                ObjectType = 50,
                ParameterName = "SYNCHRONIZED",
                ParameterValue = 1
            });

            long executionIdentifier = package.Execute(packageInfo.Is32Bits, null, setValueParameters);

            var execution = catalog.Executions[executionIdentifier];

            return new PackageResponse(execution.Status == Ssis.Operation.ServerOperationStatus.Success);
        }
    }
}
