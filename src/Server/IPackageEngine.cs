using Remotis.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Remotis.Service
{
    interface IPackageEngine
    {
        LogOption LogOption { get; set; }
        PackageResponse Run();
    }
}
