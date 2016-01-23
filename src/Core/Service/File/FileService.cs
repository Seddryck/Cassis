using Cassis.Core;
using Microsoft.SqlServer.Dts.Runtime;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Cassis.Core.Service.File
{
    class FileService : AbstractDtsPackageService
    {
        public new IFilePackage PackageInfo
        {
            get { return base.PackageInfo as IFilePackage; }
        }

        public FileService(IFilePackage packageInfo)
            : base(packageInfo)
        { }

        public override PackageResponse Run()
        {
            return Run(PackageInfo);
        }

        public override Package Load(ref Application integrationServices)
        {
            var packagePath = PackageInfo.Path
               + PackageInfo.Name
               + (PackageInfo.Name.EndsWith(".dtsx") ? "" : ".dtsx");
            return integrationServices.LoadPackage(packagePath, null);
        }
    }
}
