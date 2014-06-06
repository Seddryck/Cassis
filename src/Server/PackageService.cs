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

        public PackageResponse Run(FilePackage packageInfo, LogOption logOption, IEnumerable<Remotis.Contract.PackageParameter> parameters)
        {
            var integrationServices = new Application();
            if (!string.IsNullOrEmpty(packageInfo.Password))
                integrationServices.PackagePassword = packageInfo.Password;

            var packagePath = packageInfo.Path 
                + packageInfo.Name 
                + (packageInfo.Name.EndsWith(".dtsx") ? "" : ".dtsx");
            var package = integrationServices.LoadPackage(packagePath, null);

            Parameterize(parameters, ref package);

            var events = new PackageEvents();
            var packageResult = package.Execute(null, null, events, null, null);
            return new PackageResponse(packageResult == DTSExecResult.Success, events);
        }

        public PackageResponse Run(SqlPackage packageInfo, LogOption logOption)
        {
            var integrationServices = new Application();
            if (!string.IsNullOrEmpty(packageInfo.Password))
                integrationServices.PackagePassword = packageInfo.Password;

            var package = integrationServices.LoadFromDtsServer(packageInfo.Path + packageInfo.Name, packageInfo.Server, null);

            var events = new PackageEvents();
            var packageResult = package.Execute(null, null, events, null, null);
            return new PackageResponse(packageResult == DTSExecResult.Success, events);
        }

        public PackageResponse Run(SqlPackage packageInfo, SqlAuthentification authentification, LogOption logOption)
        {
            var integrationServices = new Application();
            if (!string.IsNullOrEmpty(packageInfo.Password))
                integrationServices.PackagePassword = packageInfo.Password;

            var package = integrationServices.LoadFromSqlServer(packageInfo.Path + packageInfo.Name
                , packageInfo.Server
                , authentification.UserName
                , authentification.Password
                , null);

            var events = new PackageEvents();
            var packageResult = package.Execute(null, null, events, null, null);
            return new PackageResponse(packageResult == DTSExecResult.Success);
        }

        public PackageResponse Run(CatalogPackage packageInfo, LogOption logOption, IEnumerable<Remotis.Contract.PackageParameter> parameters)
        {
            var connection = new SqlConnection(string.Format(@"Data Source={0};Initial Catalog=master;Integrated Security=SSPI;", packageInfo.Server));
            var integrationServices = new Ssis.IntegrationServices(connection);

            var catalog = integrationServices.Catalogs[packageInfo.Catalog];
            var folder = catalog.Folders[packageInfo.Folder];
            var project = folder.Projects[packageInfo.Project];
            var package = project.Packages[packageInfo.Name + (packageInfo.Name.EndsWith(".dtsx") ? "" : ".dtsx")];

            foreach (var parameter in parameters)
                package.Parameters[parameter.Name].Set(Ssis.ParameterInfo.ParameterValueType.Literal, parameter.Value);

            package.Alter();

            var setValueParameters = new Collection<Ssis.PackageInfo.ExecutionValueParameterSet>();
            setValueParameters.Add(new Ssis.PackageInfo.ExecutionValueParameterSet
            {
                ObjectType = 50,
                ParameterName = "SYNCHRONIZED",
                ParameterValue = 1
            });

            long executionIdentifier = package.Execute(packageInfo.Is32Bits, null, setValueParameters);

            var execution = catalog.Executions[executionIdentifier];
            var errors = execution.Messages.Where(m => m.MessageType == (short)MessageType.Error).Select(m=>m.Message);

            return new PackageResponse(execution.Status == Ssis.Operation.ServerOperationStatus.Success, errors);
        }

        protected virtual void Parameterize(IEnumerable<Remotis.Contract.PackageParameter> parameters, ref Package package)
        {
            foreach (var param in parameters)
            {
                package.Parameters[param.Name].Value = param.Value;
            }
        }
    }
}
