using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cassis.Core.Logging
{
    public class LogEventService
    {
        public LogEventService()
        {
            Log = Write;
        }

        public event EventHandler<LogEventArgs> Sent;

        public readonly LogAction Log;

        public void Write
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
            var factory = new LogEventArgsFactory();
            var eventArgs = factory.Build
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

            OnSent(eventArgs);
        }

        protected virtual void OnSent(LogEventArgs e)
        {
            if (Sent != null)
                Sent(this, e);
        }
    }
}
