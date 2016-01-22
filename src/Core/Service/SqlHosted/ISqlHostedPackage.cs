using Cassis.Core.Service.File;
using System;
using System.Linq;
using System.Xml.Serialization;

namespace Cassis.Core.Service.SqlHosted
{
    [XmlRootAttribute("package-sql")]
    public interface ISqlHostedPackage : IFilePackage
    {
        [XmlAttribute("server")]
        string Server { get; set; }
        
    }
}
