using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace SimpleMVCAuthentication.Settings
{
    public class SettingsManager
    {
        public static AuthenticationSettings AuthenticationSettings
        {
            get
            {
                return (AuthenticationSettings)ConfigurationManager.GetSection("ftpSettings");
            }
        }

        public object Create(object parent, object configContext, XmlNode section)
        {
            if (section.Attributes["type"] == null)
            {
                return null;
            }
            XmlSerializer ser = new XmlSerializer(typeof(AuthenticationSettings));
            StringReader reader = new StringReader(section.InnerXml);
            XmlReader xmlRead = XmlReader.Create(reader);

            object result = ser.Deserialize(xmlRead);

            return result;
        }
    }
}
