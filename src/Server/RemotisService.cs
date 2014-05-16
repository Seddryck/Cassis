using System;
using System.Linq;
using Microsoft.SqlServer.Dts.Runtime;
using Remotis.Contract;

namespace Remotis.Server
{
    public class RemotisService : IRemotisService
    {
        public void Run(PackageRequest request)
        {
            var integrationServices = new Application();
            if (!string.IsNullOrEmpty(request.Password))
                integrationServices.PackagePassword = request.Password;

            var package = integrationServices.LoadPackage(request.Path, null);

            var packageResult = package.Execute(null, null, null, null, null);
        }
    }
}
