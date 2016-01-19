using Cassis.Core.Service.File;
using System;
using System.Linq;
using System.Xml.Serialization;

namespace Cassis.Core.Service.SqlHosted
{
    [XmlRootAttribute("package-sql")]
    public class SqlHostedPackage : FilePackage
    {
        [XmlAttribute("server")]
        public string Server { get; set; }
        [XmlElement("parameters")]
        public PackageParameter[] Parameters { get; set; }
    }
}
