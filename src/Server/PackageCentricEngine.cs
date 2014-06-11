using Microsoft.SqlServer.Dts.Runtime;
using Remotis.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Remotis.Service
{
    abstract class PackageCentricEngine : IPackageEngine
    {
        public LogOption LogOption { get; set; }
        protected FilePackage PackageInfo { get; set; }
        protected IEnumerable<PackageParameter> Parameters { get; set; }
        protected Application IntegrationService { get; set; }

        public PackageCentricEngine(FilePackage packageInfo, IEnumerable<PackageParameter> parameters)
        {
            PackageInfo = packageInfo;
            Parameters = parameters ?? new List<PackageParameter>();
            IntegrationService = new Application();
        }


        public PackageResponse Run()
        {
            SetPassword(PackageInfo.Password);
            var packagePath = GetPackagePath(PackageInfo);
            var package = LoadPackage(packagePath);
            Parameterize(Parameters, ref package);

            var events = new PackageEvents();
            var packageResult = package.Execute(null, null, events, null, null);
            return new PackageResponse(packageResult == DTSExecResult.Success, events);
        }

        protected virtual void SetPassword(string password)
        {
            if (!string.IsNullOrEmpty(password))
                IntegrationService.PackagePassword = password;
        }

        protected virtual string GetPackagePath(FilePackage packageInfo)
        {
            return packageInfo.Path
                    + packageInfo.Name
                    + (packageInfo.Name.EndsWith(".dtsx") ? "" : ".dtsx");
        }

        protected abstract Package LoadPackage(string packagePath);

        protected virtual void Parameterize(IEnumerable<PackageParameter> parameters, ref Package package)
        {
            foreach (var param in parameters)
            {
                if (!package.Parameters.Contains(param.Name))
                    throw new ArgumentException(string.Format("Unable to set the value for a package parameter. The parameter '{0}' doesn't exist in the package '{1}'.", param.Name, package.Name));
                package.Parameters[param.Name].Value = param.Value;
            }
        }
    }
}
