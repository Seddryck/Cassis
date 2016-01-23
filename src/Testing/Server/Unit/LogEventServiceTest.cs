using Cassis.Core.Logging;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cassis.Testing.Core.Unit
{
    public class LogEventServiceTest
    {
        [Test]
        public void Write_LogMessage_EventSent()
        {
            var wasCalled = false;
            var logger = new LogEventService();
            logger.Sent += (sender, e) => wasCalled=true;

            byte[] dataBytes = new byte[256];
            logger.Write
                (
                    "eventName",
                    "computerName",
                    "operatorName",
                    "sourceName",
                    "sourceGuid",
                    "executionGuid",
                    "messageText",
                    new DateTime(2015, 1, 1),
                    new DateTime(2015, 2, 1),
                    10,
                    ref dataBytes
                );

            Assert.That(wasCalled, Is.True);
        }

        [Test]
        public void Write_LogMessage_CorrectLogMessage()
        {
            LogEventArgs eventLog = null;
            var logger = new LogEventService();
            logger.Sent += (sender, e) => eventLog = e;

            byte[] dataBytes = new byte[256];
            logger.Write
                (
                    "eventName",
                    "computerName",
                    "operatorName",
                    "sourceName",
                    "sourceGuid",
                    "executionGuid",
                    "messageText",
                    new DateTime(2015, 1, 1, 10, 15, 16, 125),
                    new DateTime(2015, 2, 1),
                    10,
                    ref dataBytes
                );

            Assert.That(eventLog, Is.Not.Null);
            Assert.That(eventLog, Is.TypeOf<LogEventArgs>());

            Assert.That(eventLog.EventName, Is.EqualTo("eventName"));
            Assert.That(eventLog.StartTime, Is.EqualTo(new DateTime(2015, 1, 1, 10, 15, 16, 125)));
            Assert.That(eventLog.DataCode, Is.EqualTo(10));

        }
    }
}
