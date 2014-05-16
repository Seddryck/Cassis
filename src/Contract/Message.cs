using System;
using System.Linq;
using System.Xml.Serialization;

namespace Remotis.Contract
{
    public class Message
    {
        [XmlText]
        public string Description {get;set;}    
        [XmlAttribute("timestamp")]
        public DateTime TimeStamp {get;set;}

    }
}
