#if ! SqlServer2008R2
using System;
using System.Linq;
using System.Xml.Serialization;

namespace Cassis.Core.Service.Catalog
{
    [XmlRootAttribute("package-catalog")]
    public class CatalogPackage : IPackageInfo
    {
        [XmlAttribute("name")]
        public string Name { get; set; }
        [XmlAttribute("password")]
        public string Password { get; set; }
        [XmlAttribute("server")]
        public string Server { get; set; }
        [XmlAttribute("catalog")]
        public string Catalog { get; set; }
        [XmlAttribute("environment")]
        public string Environment { get; set; }
        [XmlAttribute("folder")]
        public string Folder { get; set; }
        [XmlAttribute("project")]
        public string Project { get; set; }
        [XmlAttribute("bits-32")]
        public bool Is32Bits { get; set; }
        [XmlAttribute("timeout")]
        public int Timeout { get; set; }
        [XmlElement("parameters")]
        public PackageParameter[] Parameters { get; set; }
    }
}
#endif