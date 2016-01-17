using System;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;

namespace Cassis.Contract
{
    [ServiceContract(CallbackContract = typeof (ILog))]
	public interface IPackageService
	{
        [OperationContract(Name="RunFilePackage")]
        PackageResponse Run(FilePackage request, LogOption logOption);

        [OperationContract(Name = "RunSqlPackage")]
        PackageResponse Run(SqlPackage request);

        [OperationContract(Name = "RunSqlPackageWithSqlAuthentification")]
        PackageResponse Run(SqlPackage request, SqlAuthentification authentification);

        [OperationContract(Name = "RunCatalogPackage")]
        PackageResponse Run(CatalogPackage request);
	}
}
