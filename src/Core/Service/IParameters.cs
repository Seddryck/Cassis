using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Cassis.Core.Service
{
    public interface IParameters
    {
        [XmlElement("parameters")]
        PackageParameter[] Parameters { get; set; }
    }
}
