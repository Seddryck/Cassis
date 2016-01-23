using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cassis.Core.Logging
{
    public delegate void LogAction
            (
            string eventName,
            string computerName,
            string operatorName,
            string sourceName,
            string sourceGuid,
            string executionGuid,
            string messageText,
            DateTime startTime,
            DateTime endTime,
            int dataCode,
            ref byte[] dataBytes
           );
}
