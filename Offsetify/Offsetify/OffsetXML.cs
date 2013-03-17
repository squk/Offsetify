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
            this.ReadXML();
        }

        private void ReadXML()
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
                string notes = offsetEntry.SelectSingleNode("notes").InnerText;
                this.OffsetList.Add(new Offset(name, offset, type, assignedValue, defaultValue, notes));
            }
            reader.Close();
            reader.Dispose();
        }

        public static void WriteOffsetListToXML(string location, List<Offset> offsets)
        {
            using (XmlWriter writer = XmlWriter.Create(location))
            {
                writer.WriteStartDocument();
                    writer.WriteStartElement("OffsetifyXML");
                    foreach (Offset offset in offsets)
                    {
                        writer.WriteStartElement("offsetEntry");
                            writer.WriteAttributeString("name", offset.Name);
                            writer.WriteElementString("offset", offset.memOffset);
                            writer.WriteElementString("type", offset.Type);
                            writer.WriteElementString("assignedValue", offset.AssignedValue);
                            writer.WriteElementString("defaultValue", offset.DefaultValue);
                            writer.WriteElementString("notes", offset.Notes);
                        writer.WriteEndElement();
                    }   
                    writer.WriteEndElement();
                writer.WriteEndDocument();

                writer.Dispose();
            }
        }
    }
}

