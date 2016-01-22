using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Cassis.Core.Service.Catalog
{
    public interface IEnvironment
    {
        [XmlAttribute("environment")]
        string Environment { get; set; }
    }
}
