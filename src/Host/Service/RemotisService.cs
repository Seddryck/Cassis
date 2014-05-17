using System;
using System.Configuration;
using System.Linq;
using System.ServiceModel;
using Remotis.Contract;

namespace Remotis.Host.Service
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public class RemotisService : IPackageService
    {

        public PackageResponse Run(PackageRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
