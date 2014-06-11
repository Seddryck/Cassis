using Microsoft.SqlServer.Dts.Runtime;
using Remotis.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Remotis.Service
{
    class FilePackageEngine : PackageCentricEngine
    {

        public FilePackageEngine(FilePackage packageInfo, IEnumerable<PackageParameter> parameters)
            : base(packageInfo,parameters)
        {

        }
        protected override Package LoadPackage(string packagePath)
        {
            return IntegrationService.LoadPackage(packagePath, null);
        }
    }
}
