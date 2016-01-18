using Cassis.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cassis.Service
{
    abstract class AbstractPackageService : IPackageService
    {
        private readonly IPackageInfo packageInfo;

        protected IPackageInfo PackageInfo
        {
            get { return packageInfo; }
        }

        public AbstractPackageService(IPackageInfo packageInfo)
        {
            this.packageInfo = packageInfo;
        }

        public abstract PackageResponse Run();
    }
}
