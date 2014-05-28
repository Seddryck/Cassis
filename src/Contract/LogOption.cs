using System;
using System.Linq;
using System.Xml.Serialization;

namespace Remotis.Contract
{
    [XmlRootAttribute("log-option")]
    public class LogOption
    {
        [XmlAttribute("verbose")]
        public bool IsVerbose { get; set; }

        public LogOption()
        {

        }

        public static LogOption Verbose
        {
            get
            {
                return new LogOption() { IsVerbose = true };
            }
        }

        public static LogOption None
        {
            get
            {
                return new LogOption() { IsVerbose = false };
            }
        }
    }
}
