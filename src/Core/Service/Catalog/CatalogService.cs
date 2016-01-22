#if ! SqlServer2008R2
using Cassis.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Ssis = Microsoft.SqlServer.Management.IntegrationServices;

namespace Cassis.Core.Service.Catalog
{
    class CatalogService : AbstractPackageService
    {
        public new ICatalogPackage PackageInfo
        {
            get { return base.PackageInfo as ICatalogPackage; }
        }

        public CatalogService(ICatalogPackage packageInfo)
            : base(packageInfo)
        { }

        public override PackageResponse Run()
        {
            return Run(PackageInfo);
        }

        protected PackageResponse Run(ICatalogPackage etl)
        {
            var connection = new SqlConnection(string.Format(@"Data Source={0};Initial Catalog=master;Integrated Security=SSPI;", etl.Server));
            var integrationServices = new Ssis.IntegrationServices(connection);

            var package = GetPackage(integrationServices, etl);

            Ssis.EnvironmentReference environmentReference = null;
            if (etl is IEnvironment)
                environmentReference = GetEnvironmentReference(package.Parent, (etl as IEnvironment).Environment);

            var setValueParameters = new Collection<Ssis.PackageInfo.ExecutionValueParameterSet>();
            setValueParameters.Add(new Ssis.PackageInfo.ExecutionValueParameterSet
            {
                ObjectType = 50
                , ParameterName = "SYNCHRONIZED"
                , ParameterValue = 1
            });

            if (etl is IParameters)
            {
                var parameters = Parameterize(package.Parameters, package.Name, (etl as IParameters).Parameters);
                parameters.ToList().ForEach(p => setValueParameters.Add(p));
            }
            

            long executionIdentifier = -1;

            if (etl.Timeout == 0)
                executionIdentifier = package.Execute(etl.Is32Bits, environmentReference, setValueParameters);
            else
                executionIdentifier = package.Execute(etl.Is32Bits, environmentReference, setValueParameters, etl.Timeout);

            var execution = package.Parent.Parent.Parent.Executions[executionIdentifier];

            return new PackageResponse(execution.Status == Ssis.Operation.ServerOperationStatus.Success);
        }

        private Ssis.PackageInfo GetPackage(Ssis.IntegrationServices integrationServices, ICatalogPackage etl)
        {
            if (!integrationServices.Catalogs.Contains(etl.Catalog))
            {
                var names = String.Join(", ", integrationServices.Catalogs.Select(c => c.Name));
                throw new ArgumentOutOfRangeException("Catalog", String.Format("The catalog named '{0}' hasn't been found on the server '{1}'. List of existing catalogs: {2}.", etl.Catalog, etl.Server, names));
            }

            var catalog = integrationServices.Catalogs[etl.Catalog];

            if (!catalog.Folders.Contains(etl.Folder))
            {
                var names = String.Join(", ", catalog.Folders.Select(f => f.Name));
                throw new ArgumentOutOfRangeException("Folder", String.Format("The folder named '{0}' hasn't been found on the catalog '{1}'. List of existing folders: {2}.", etl.Folder, etl.Catalog, names));
            }
            var folder = catalog.Folders[etl.Folder];

            if (!folder.Projects.Contains(etl.Project))
            {
                var names = String.Join(", ", folder.Projects.Select(p => p.Name));
                throw new ArgumentOutOfRangeException("Project", String.Format("The project named '{0}' hasn't been found on the catalog '{1}'. List of existing projects: {2}.", etl.Project, etl.Folder, names));
            }
            var project = folder.Projects[etl.Project];

            if (project.Packages.Contains(etl.Name))
            {
                var names = String.Join(", ", project.Packages.Select(p => p.Name));
                throw new ArgumentOutOfRangeException("Name", String.Format("The package named '{0}' hasn't been found on the project '{1}'. List of existing packages: {2}.", etl.Name, etl.Project, names));
            }
            var package = project.Packages[etl.Name];

            return package;
        }

        private Ssis.EnvironmentReference GetEnvironmentReference(Ssis.ProjectInfo project, string environmentName)
        {
            var folder = project.Parent;

            if (string.IsNullOrEmpty(environmentName))
                return null;

            if (!folder.Environments.Contains(environmentName))
            {
                var names = String.Join(", ", folder.Environments.Select(e => e.Name));
                throw new ArgumentOutOfRangeException("Environment", String.Format("The environment named '{0}' hasn't been found on the folder '{1}'. List of existing catalogs: {2}.", environmentName, folder.Name, names));
            }

            if (!project.References.Contains(folder.Environments[environmentName].Name, folder.Name))
            {
                var names = String.Join(", ", project.References.Select(r => r.Name));
                throw new ArgumentOutOfRangeException("Environment", String.Format("The environment named '{0}' exists but is not referenced in the project '{1}'. List of existing references: {2}.", environmentName, project.Name, names));
            }

            return project.References[folder.Environments[environmentName].Name, folder.Name];
        }

        protected virtual IEnumerable<Ssis.PackageInfo.ExecutionValueParameterSet> Parameterize(Ssis.ParameterCollection existingParameters, string packageName, IEnumerable<PackageParameter> overridenParameters)
        {
            foreach (var param in overridenParameters)
            {
                if (!existingParameters.Contains(param.Name))
                    throw new ArgumentOutOfRangeException("overridenParameters", string.Format("No parameter named '{0}' found in the package {1}, can't override its value for execution.", param.Name, packageName));

                var existingParam = existingParameters[param.Name];
                var execParam = new Ssis.PackageInfo.ExecutionValueParameterSet()
                {
                    ObjectType = existingParam.ObjectType,
                    ParameterName = param.Name,
                    ParameterValue = DefineValue(param.Value.ToString(), existingParam.DataType)
                };
                yield return execParam;
            }
        }
    }
}
#endif