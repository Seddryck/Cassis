using System;
using System.Linq;
using System.ServiceModel;

namespace Cassis.Core.Logging
{
    public interface ILog
    {
        [OperationContract(IsOneWay = true)]
        void OnLog(Message msg);
    }
}
