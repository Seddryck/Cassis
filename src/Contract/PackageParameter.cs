using System;
using System.Linq;
using System.Xml.Serialization;

namespace Cassis.Contract
{
    [XmlRootAttribute("parameter")]
    class PackageParameter
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlText]
        public object Value { get; set; }
    }
}
