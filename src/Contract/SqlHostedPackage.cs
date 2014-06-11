using System;
using System.Linq;
using System.Xml.Serialization;

namespace Remotis.Contract
{
    [XmlRootAttribute("package-sql")]
    public class SqlHostedPackage : FilePackage
    {
        [XmlAttribute("server")]
        public string Server { get; set; }
    }
}
