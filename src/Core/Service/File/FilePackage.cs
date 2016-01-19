using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace Cassis.Core.Service.File
{
    [XmlRootAttribute("package-file")]
    public class FilePackage : IPackageInfo
    {
        [XmlAttribute("name")]
        public string Name {get; set;}
        [XmlAttribute("password")]
        public string Password {get; set;}
        [XmlAttribute("path")]
        public string Path { get; set; }
    }

}
