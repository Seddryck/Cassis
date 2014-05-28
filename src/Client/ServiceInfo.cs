using System;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;
using Remotis.Contract;
using System.ServiceModel.Channels;

namespace Remotis.Client
{
    public class ServiceInfo
    {
        public Binding Binding { get; set; }
        public EndpointAddress Address { get; set; }

        public ServiceInfo(string address, Binding binding)
        {
            Binding = binding;
            Address = new EndpointAddress(address);
        }

        public ServiceInfo(string address)
        {
            Binding = new NetTcpBinding();
            Address = new EndpointAddress(address);
        }

    }
}
