using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace Remotis.Contract
{
    [XmlRootAttribute("response")]
    public class PackageResponse
    {
        [XmlAttribute("success")]
        public bool Success {get; set;}

        public PackageResponse()
        { }

        public PackageResponse(bool isSuccess)
        {
            Success = isSuccess;
        }
    }

}
