using System;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;

namespace Remotis.Contract
{
    [ServiceContract(CallbackContract = typeof (ILog))]
	public interface IPackageService
	{

        [OperationContract]
        PackageResponse Run(PackageRequest request);
	}
}
