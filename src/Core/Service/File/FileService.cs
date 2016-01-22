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

        protected PackageResponse Run(IFilePackage etl)
        {
            var integrationServices = new Application();
            if (!string.IsNullOrEmpty(etl.Password))
                integrationServices.PackagePassword = etl.Password;

            var packagePath = etl.Path
                + etl.Name
                + (etl.Name.EndsWith(".dtsx") ? "" : ".dtsx");
            var package = integrationServices.LoadPackage(packagePath, null);

            if ((etl as IParameters)?.Parameters != null)
                Parameterize((etl as IParameters).Parameters, ref package);

            var events = new PackageEvents();
            var packageResult = package.Execute(null, null, events, null, null);
            return new PackageResponse(packageResult == DTSExecResult.Success, events);
        }
    }
}
