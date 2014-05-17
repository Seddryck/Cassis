using System;
using System.Linq;
using Microsoft.SqlServer.Dts.Runtime;
using Remotis.Contract;

namespace Remotis.Service
{
    public class PackageService : IPackageService
    {
        public PackageResponse Run(PackageRequest request)
        {
            var integrationServices = new Application();
            if (!string.IsNullOrEmpty(request.Password))
                integrationServices.PackagePassword = request.Password;

            var package = integrationServices.LoadPackage(request.Path + request.Name, null);

            var packageResult = package.Execute(null, null, null, null, null);
            return new PackageResponse(packageResult == DTSExecResult.Success);
        }
    }
}
