using Cassis.Core;
using Cassis.Core.Logging;
using Cassis.Core.Service.File;
using Microsoft.SqlServer.Dts.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cassis.Core.Service
{
    abstract class AbstractDtsPackageService : AbstractPackageService
    {
        public AbstractDtsPackageService(IPackageInfo packageInfo)
            : base(packageInfo)
        { }

        public abstract Package Load(ref Application integrationServices);

        protected virtual PackageResponse Run(IFilePackage etl)
        {
            var integrationServices = new Application();
            if (!string.IsNullOrEmpty(etl.Password))
                integrationServices.PackagePassword = etl.Password;

            var package = Load(ref integrationServices);

            Parameterize(ref package);

            var events = new PackageEvents();

            LogGateway logger = null;
            if (Log != null)
                logger = new LogGateway(Log);

            var packageResult = package.Execute(null, null, events, logger, null);
            return new PackageResponse(packageResult == DTSExecResult.Success, events);
        }
    }
}
