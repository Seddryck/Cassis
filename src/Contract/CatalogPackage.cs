using System;
using System.Linq;
using System.Xml.Serialization;

namespace Cassis.Contract
{
    [XmlRootAttribute("package-catalog")]
    public class CatalogPackage
    {
        [XmlAttribute("name")]
        public string Name { get; set; }
        [XmlAttribute("password")]
        public string Password { get; set; }
        [XmlAttribute("catalog")]
        public string Catalog { get; set; }
        [XmlAttribute("folder")]
        public string Folder { get; set; }
        [XmlAttribute("project")]
        public string Project { get; set; }
        [XmlAttribute("server")]
        public string Server { get; set; }
        [XmlAttribute("bits-32")]
        public bool Is32Bits { get; set; }
    }
}
