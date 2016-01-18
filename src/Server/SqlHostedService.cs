﻿using Cassis.Contract;
using Microsoft.SqlServer.Dts.Runtime;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Cassis.Service
{
    class SqlHostedService : AbstractPackageService
    {
        public new SqlHostedPackage PackageInfo
        {
            get { return base.PackageInfo as SqlHostedPackage; }
        }

        public SqlHostedService(SqlHostedPackage packageInfo)
            : base(packageInfo)
        { }

        public override PackageResponse Run()
        {
            return Run(PackageInfo);
        }

        protected PackageResponse Run(SqlHostedPackage packageInfo)
        {
            var integrationServices = new Application();
            if (!string.IsNullOrEmpty(packageInfo.Password))
                integrationServices.PackagePassword = packageInfo.Password;

            var package = integrationServices.LoadFromDtsServer(packageInfo.Path + packageInfo.Name, packageInfo.Server, null);

            var events = new PackageEvents();
            var packageResult = package.Execute(null, null, events, null, null);
            return new PackageResponse(packageResult == DTSExecResult.Success, events);
        }
        
    }
}