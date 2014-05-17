using System;
using System.Linq;
using System.ServiceModel;
using Remotis.Service;

namespace Remotis.Host.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            System.Console.WriteLine("Console host is opening...");
            var serviceHost = new ServiceHost(typeof(PackageService));
            serviceHost.Open();
            System.Console.WriteLine("Console host is listening... press ENTER to close");
            System.Console.Read();
            System.Console.WriteLine("Console host is closing...");
            if (serviceHost.State == CommunicationState.Opened)
                serviceHost.Close();
            System.Console.WriteLine("Console host is closed.");
        }
    }
}
