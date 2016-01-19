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
    class FileService : AbstractPackageService
    {
        public new FilePackage PackageInfo
        {
            get { return base.PackageInfo as FilePackage; }
        }

        public FileService(FilePackage packageInfo)
            : base(packageInfo)
        { }

        public override PackageResponse Run()
        {
            return Run(PackageInfo);
        }

        protected PackageResponse Run(FilePackage packageInfo)
        {
            var integrationServices = new Application();
            if (!string.IsNullOrEmpty(packageInfo.Password))
                integrationServices.PackagePassword = packageInfo.Password;

            var packagePath = packageInfo.Path
                + packageInfo.Name
                + (packageInfo.Name.EndsWith(".dtsx") ? "" : ".dtsx");
            var package = integrationServices.LoadPackage(packagePath, null);

            var events = new PackageEvents();
            var packageResult = package.Execute(null, null, events, null, null);
            return new PackageResponse(packageResult == DTSExecResult.Success, events);
        }
    }
}
