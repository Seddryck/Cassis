using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace Cassis.Contract
{
    [XmlRootAttribute("response")]
    public class PackageResponse
    {
        [XmlAttribute("success")]
        public bool Success {get; set;}

        public List<string> Errors { get; set; }

        public PackageResponse()
        {
            Errors = new List<string>();
        }

        public PackageResponse(bool isSuccess) : this()
        {
            Success = isSuccess;
        }

        public PackageResponse(bool isSuccess, IPackageEvents events)
        {
            Success = isSuccess;
            Errors = events.Errors.ToList();
        }
        public PackageResponse(bool isSuccess, IEnumerable<string> errors)
        {
            Success = isSuccess;
            Errors = errors.ToList();
        }
    }

}
