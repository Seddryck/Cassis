using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Remotis.Testing.Service
{
    public class ConfigurationReader
    {
        private static string Get(string name)
        {
            var xmldoc = new XmlDocument();
            xmldoc.Load(GetFilename());
            XmlNodeList nodes = xmldoc.GetElementsByTagName("add");
            foreach (XmlNode node in nodes)
                if (node.Attributes["name"].Value == name)
                    return node.Attributes["connectionString"].Value;
            throw new Exception();
        }

        private static string GetFilename()
        {
            //If available use the user file
            if (System.IO.File.Exists("ConnectionString.user.config"))
            {
                return "ConnectionString.user.config";
            }
            else if (System.IO.File.Exists("ConnectionString.config"))
            {
                return "ConnectionString.config";
            }
            return "";
        }

        public static string GetServerName()
        {
            return Get("ServerName");
        }


        internal static string GetSqlServerName()
        {
            return Get("SqlServerName");
        }
        internal static string GetDestinationPath()
        {
            return Get("DestinationPath");
        }
    }
}
