using System;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;

namespace Cassis.Contract
{
    [ServiceContract(CallbackContract = typeof (ILog))]
	public interface IPackageService
	{
        [OperationContract(Name="Run")]
        PackageResponse Run();
	}
}
