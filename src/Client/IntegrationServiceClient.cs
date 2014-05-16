using System;
using System.Linq;
using System.ServiceModel;
using Remotis.Contract;

namespace Remotis.Client
{
    public class IntegrationServiceClient
    {
        private readonly ServiceInfo serviceInfo;

        public IntegrationServiceClient(ServiceInfo serviceInfo)
        {
            this.serviceInfo = serviceInfo;
        }

        /// <summary>
        /// Execute a package on a remote server
        /// </summary>
        /// <param name="package">Information about the package to execute</param>
        /// <param name="log">Function called when a log message is received</param>
        public void Execute(PackageRequest package, Action<Message> log)
        {
            var logger = new Logger(log);
            var context = new InstanceContext(logger);
            using (var factory = new DuplexChannelFactory<IRemotisService>(context, serviceInfo.EndPoint))
            {
                var channel = factory.CreateChannel();
                channel.Run(package);
            }
        }
    }
}
