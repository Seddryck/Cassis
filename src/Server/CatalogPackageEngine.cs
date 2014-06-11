using Microsoft.SqlServer.Management.IntegrationServices;
using Remotis.Contract;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Remotis.Service
{
    class CatalogPackageEngine : IPackageEngine
    {
        public LogOption LogOption { get; set; }
        protected CatalogPackage PackageInfo { get; set; }
        protected IEnumerable<PackageParameter> Parameters { get; set; }
        public CatalogPackageEngine(CatalogPackage packageInfo, IEnumerable<PackageParameter> parameters)
        {
            PackageInfo = packageInfo;
            Parameters = parameters;
        }

        protected virtual IntegrationServices GetIntegrationServices(string serverName)
        {
            var connection = new SqlConnection(string.Format(@"Data Source={0};Initial Catalog=master;Integrated Security=SSPI;", serverName));
            var integrationServices = new IntegrationServices(connection);
            return integrationServices;
        }


        public PackageResponse Run()
        {
            var integrationServices = GetIntegrationServices(PackageInfo.Server);

            var catalog = integrationServices.Catalogs[PackageInfo.Catalog];
            var folder = catalog.Folders[PackageInfo.Folder];
            var project = folder.Projects[PackageInfo.Project];
            var package = project.Packages[PackageInfo.Name + (PackageInfo.Name.EndsWith(".dtsx") ? "" : ".dtsx")];

            foreach (var parameter in Parameters)
                package.Parameters[parameter.Name].Set(ParameterInfo.ParameterValueType.Literal, parameter.Value);

            package.Alter();

            var setValueParameters = new Collection<PackageInfo.ExecutionValueParameterSet>();
            setValueParameters.Add(new PackageInfo.ExecutionValueParameterSet
            {
                ObjectType = 50,
                ParameterName = "SYNCHRONIZED",
                ParameterValue = 1
            });

            long executionIdentifier = package.Execute(PackageInfo.Is32Bits, null, setValueParameters);

            var execution = catalog.Executions[executionIdentifier];
            var errors = execution.Messages.Where(m => m.MessageType == (short)MessageType.Error).Select(m => m.Message);

            return new PackageResponse(execution.Status == Operation.ServerOperationStatus.Success, errors);
        }
    }
}
