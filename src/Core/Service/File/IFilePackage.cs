using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace Cassis.Core.Service.File
{
    [XmlRootAttribute("package-file")]
    public interface IFilePackage : IPackageInfo
    {
        [XmlAttribute("password")]
        string Password {get; set;}
        [XmlAttribute("path")]
        string Path { get; set; }
    }

}
