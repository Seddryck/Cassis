using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace Cassis.Core.Service
{
    [XmlRootAttribute("parameter")]
    public interface IPackageParameter
    {
        [XmlAttribute("name")]
        string Name { get; set; }
        [XmlText]
        object Value { get; set; }
        
    }
}
