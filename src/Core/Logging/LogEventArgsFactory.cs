using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cassis.Core.Logging
{
    class LogEventArgsFactory
    {
       public LogEventArgs Build
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
           )
        {
            return new LogEventArgs
            (
                eventName,
                computerName,
                operatorName,
                sourceName,
                sourceGuid,
                executionGuid,
                messageText,
                startTime,
                endTime,
                dataCode,
                ref dataBytes
           );
        }
    }
}
