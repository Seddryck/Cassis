#if ! SqlServer2008R2
using System;
using System.Linq;
using System.Xml.Serialization;

namespace Cassis.Core.Service.Catalog
{
    [XmlRootAttribute("package-catalog")]
    public interface ICatalogPackage : IPackageInfo
    {
        [XmlAttribute("password")]
        string Password { get; set; }
        [XmlAttribute("server")]
        string Server { get; set; }
        [XmlAttribute("catalog")]
        string Catalog { get; set; }
        [XmlAttribute("folder")]
        string Folder { get; set; }
        [XmlAttribute("project")]
        string Project { get; set; }
        [XmlAttribute("bits-32")]
        bool Is32Bits { get; set; }
        [XmlAttribute("timeout")]
        int Timeout { get; set; }
    }
}
#endif