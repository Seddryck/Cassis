using Microsoft.SqlServer.Dts.Runtime;
using Cassis.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cassis.Core
{
    class PackageEvents : DefaultEvents, IPackageEvents
    {
        public IList<string> Errors { get; set; }
        private DateTime StartTime { get; set; }
        public TimeSpan ExecutionTime { get; set; }

        public PackageEvents()
        {
            Errors = new List<string>();
        }

        public override bool OnError(DtsObject source, int errorCode, string subComponent, string description, string helpFile, int helpContext, string idofInterfaceWithError)
        {
            var error = string.Empty;
            if (string.IsNullOrEmpty(subComponent))
                error = string.Format("[{1}] - {0}", description, errorCode);
            else
                error = string.Format("[{2}] in {0} - {1}", subComponent, description, errorCode);
            Errors.Add(error);

            return base.OnError(source, errorCode, subComponent, description, helpFile, helpContext, idofInterfaceWithError);
        }

        public override void OnPreExecute(Executable exec, ref bool fireAgain)
        {
            StartTime = DateTime.Now;
            base.OnPreExecute(exec, ref fireAgain);
        }

        public override void OnPostExecute(Executable exec, ref bool fireAgain)
        {
            ExecutionTime = DateTime.Now.Subtract(StartTime);
            base.OnPreExecute(exec, ref fireAgain);
        }
    }
}
