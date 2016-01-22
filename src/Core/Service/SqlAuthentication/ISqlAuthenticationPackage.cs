using Cassis.Core.Service.SqlHosted;
using System;
using System.Linq;
using System.Xml.Serialization;

namespace Cassis.Core.Service.SqlAuthentication
{
    [XmlRootAttribute("package-sql-authentification")]
    public interface ISqlAuthenticationPackage : ISqlHostedPackage
    {
        [XmlAttribute("username")]
        string UserName { get; set; }
    }
}
