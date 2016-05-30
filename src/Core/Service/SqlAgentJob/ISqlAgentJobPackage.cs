#if ! SqlServer2008R2
using System;
using System.Linq;
using System.Xml.Serialization;
using System.ComponentModel;

namespace Cassis.Core.Service.SqlAgentJob
{
    [XmlRootAttribute("package-sql-agent-job")]
    public interface ISqlAgentJobPackage : IPackageInfo
    {
        [XmlAttribute("server")]
        string Server { get; set; }
        [XmlAttribute("job")]
        string JobName { get; set; }
    }
}
#endif