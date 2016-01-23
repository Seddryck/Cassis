using Microsoft.SqlServer.Dts.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cassis.Core.Logging
{
    class LogGateway : IDTSLogging
    {
        protected readonly LogAction log;

        public LogGateway(LogAction log)
        {
            this.log = log;
        }

        public bool Enabled
        {
            get
            {
                return true;
            }
        }

        public bool[] GetFilterStatus(ref string[] eventNames)
        {
            throw new NotImplementedException();
        }

        public void Log(string eventName, string computerName, string operatorName, string sourceName, string sourceGuid, string executionGuid, string messageText, DateTime startTime, DateTime endTime, int dataCode, ref byte[] dataBytes)
        {
            log.Invoke(eventName, computerName, operatorName, sourceName, sourceGuid, executionGuid, messageText, startTime, endTime, dataCode, ref dataBytes);
        }
    }
}
