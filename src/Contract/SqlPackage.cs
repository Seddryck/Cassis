using System;
using System.Linq;
using System.Xml.Serialization;

namespace Cassis.Contract
{
    [XmlRootAttribute("package-sql")]
    public class SqlPackage : IPackageInfo
    {
        [XmlAttribute("name")]
        public string Name { get; set; }
        [XmlAttribute("password")]
        public string Password { get; set; }
        [XmlAttribute("path")]
        public string Path { get; set; }
        [XmlAttribute("server")]
        public string Server { get; set; }
    }
}
