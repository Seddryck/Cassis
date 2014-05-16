using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace Remotis.Contract
{
    [XmlRootAttribute("package")]
    public class PackageRequest
    {
        [XmlAttribute("name")]
        public string Name {get; set;}
        [XmlAttribute("password")]
        public string Password {get; set;}
        [XmlAttribute("path")]
        public string Path { get; set; }
    }

}
