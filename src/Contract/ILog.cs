using System;
using System.Linq;
using System.ServiceModel;

namespace Remotis.Contract
{
    public interface ILog
    {
        [OperationContract(IsOneWay = true)]
        void OnLog(Message msg);
    }
}
