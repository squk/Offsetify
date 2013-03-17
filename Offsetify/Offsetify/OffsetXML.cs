using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Offsetify
{
    using System;
    using System.Collections.Generic;
    using System.Xml;

    internal class OffsetXML
    {
        public List<Offset> OffsetList = new List<Offset>();
        private string xmlLocation = "";

        public OffsetXML(string location)
        {
            this.xmlLocation = location;
            this.readXML();
        }

        private void readXML()
        {
            XmlDocument offsetDoc = new XmlDocument();
            XmlTextReader reader = new XmlTextReader(this.xmlLocation);
            offsetDoc.Load(reader);
            foreach (XmlNode offsetEntry in offsetDoc.SelectSingleNode("OffsetifyXML").SelectNodes("offsetEntry"))
            {
                string name = offsetEntry.Attributes.GetNamedItem("name").Value;
                string offset = offsetEntry.SelectSingleNode("offset").InnerText;
                string type = offsetEntry.SelectSingleNode("type").InnerText;
                string assignedValue = offsetEntry.SelectSingleNode("assignedValue").InnerText;
                string defaultValue = offsetEntry.SelectSingleNode("defaultValue").InnerText;
                this.OffsetList.Add(new Offset(name, offset, type, assignedValue, defaultValue));
            }
            reader.Close();
            reader.Dispose();
        }
    }
}

