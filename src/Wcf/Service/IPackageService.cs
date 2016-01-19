using System;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;

namespace Cassis.Core
{
    [ServiceContract(CallbackContract = typeof (ILog))]
	public interface IPackageService
	{
        [OperationContract(Name="Run")]
        PackageResponse Run();
	}
}
