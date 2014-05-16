using System;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;

namespace Remotis.Contract
{
    [ServiceContract(CallbackContract = typeof (ILog))]
	public interface IRemotisService
	{

        [OperationContract]
        void Run(PackageRequest request);
	}
}
