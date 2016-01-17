using System;
using System.Linq;
using System.ServiceModel;

namespace Cassis.Contract
{
    [CallbackBehavior(ConcurrencyMode = ConcurrencyMode.Reentrant, UseSynchronizationContext = false)]
    public class Logger : ILog
    {
        private readonly Action<Message> callbackDelegate;

        public Logger(Action<Message> callbackDelegate)
        {
            this.callbackDelegate = callbackDelegate;
        }
        
        public void OnLog(Message msg)
        {
            if (callbackDelegate != null)
            {
                callbackDelegate.Invoke(msg);
            }
        }
    }
}
