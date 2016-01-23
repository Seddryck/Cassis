﻿using System;
using System.Linq;
using System.Xml.Serialization;

namespace Cassis.Core
{
    [XmlRootAttribute("parameter")]
    public class PackageParameter
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlText]
        public object Value { get; set; }
    }
}