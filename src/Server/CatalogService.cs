using Cassis.Contract;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Ssis = Microsoft.SqlServer.Management.IntegrationServices;

namespace Cassis.Service
{
    class CatalogService : AbstractPackageService
    {
        public new CatalogPackage PackageInfo
        {
            get { return base.PackageInfo as CatalogPackage; }
        }

        public CatalogService(CatalogPackage packageInfo)
            : base(packageInfo)
        { }

        public override PackageResponse Run()
        {
            return Run(PackageInfo);
        }

        protected PackageResponse Run(CatalogPackage packageInfo)
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
                ObjectType = 50
                , ParameterName = "SYNCHRONIZED"
                , ParameterValue = 1
            });

            long executionIdentifier = package.Execute(packageInfo.Is32Bits, null, setValueParameters);

            var execution = catalog.Executions[executionIdentifier];

            return new PackageResponse(execution.Status == Ssis.Operation.ServerOperationStatus.Success);
        }
    }
}
