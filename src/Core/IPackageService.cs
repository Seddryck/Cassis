using System;
using System.Linq;
using System.Runtime.Serialization;

namespace Cassis.Core
{
	public interface IPackageService
	{
        PackageResponse Run();
	}
}
