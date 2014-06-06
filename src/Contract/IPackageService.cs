using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;

namespace Remotis.Contract
{
    [ServiceContract(CallbackContract = typeof (ILog))]
	public interface IPackageService
	{
        [OperationContract(Name="RunFilePackage")]
        PackageResponse Run(FilePackage request, LogOption logOption, IEnumerable<PackageParameter> parameters);

        [OperationContract(Name = "RunSqlPackage")]
        PackageResponse Run(SqlPackage request, LogOption logOption);

        [OperationContract(Name = "RunSqlPackageWithSqlAuthentification")]
        PackageResponse Run(SqlPackage request, SqlAuthentification authentification, LogOption logOption);

        [OperationContract(Name = "RunCatalogPackage")]
        PackageResponse Run(CatalogPackage request, LogOption logOption, IEnumerable<Remotis.Contract.PackageParameter> parameters);
	}
}
