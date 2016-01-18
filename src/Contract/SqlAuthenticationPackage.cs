﻿using System;
using System.Linq;
using System.Xml.Serialization;

namespace Cassis.Contract
{
    [XmlRootAttribute("package-sql-authentification")]
    public class SqlAuthenticationPackage : SqlHostedPackage
    {
        [XmlAttribute("username")]
        public string UserName { get; set; }
    }
}
