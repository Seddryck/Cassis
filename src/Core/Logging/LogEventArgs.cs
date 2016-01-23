using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cassis.Core.Logging
{
    public class LogEventArgs : EventArgs
    {
        public string EventName { get; }
        public string ComputerName { get; }
        public string OperatorName { get; }
        public string SourceName { get; }
        public string SourceGuid { get; }
        public string ExecutionGuid { get; }
        public string MessageText { get; }
        public DateTime StartTime { get; }
        public DateTime EndTime { get; }
        public int DataCode { get; }
        public byte[] DataBytes { get; }

        public LogEventArgs
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
            EventName = eventName;
            ComputerName = computerName;
            OperatorName = operatorName;
            SourceName = sourceName;
            SourceGuid = sourceGuid;
            ExecutionGuid = executionGuid;
            MessageText = messageText;
            StartTime = startTime;
            EndTime = endTime;
            DataCode = dataCode;
            DataBytes = dataBytes;
        }
    }
}
