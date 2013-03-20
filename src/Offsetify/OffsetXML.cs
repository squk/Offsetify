using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Windows;

namespace Offsetify
{
    using System;
    using System.Collections.Generic;
    using System.Xml;

    internal class OffsetXML
    {
        public static List<Offset> ReadOffsetListFromXML(string location)
        {
            List<Offset> OffsetList = new List<Offset>();
            XmlDocument offsetDoc = new XmlDocument();
            XmlTextReader reader = new XmlTextReader(location);
            offsetDoc.Load(reader);

            int applicationVersion = Properties.Settings.Default.applicationVersion;
            int builtWithVersion = Convert.ToInt32(offsetDoc.SelectSingleNode("OffsetifyXML").Attributes.GetNamedItem("version").Value);
            
            if (builtWithVersion < applicationVersion)
            {
                MessageBox.Show("WARNING : This Offsetify XML that you are opening was made with an older version of Offsetify!");
            }
            else if (builtWithVersion > applicationVersion)
            {
                MessageBox.Show("WARNING : This Offsetify XML that you are opening was made with a newer version of Offsetify!");
            }

            foreach (XmlNode offsetEntry in offsetDoc.SelectSingleNode("OffsetifyXML").SelectNodes("offsetEntry"))
            {
                string name = offsetEntry.Attributes.GetNamedItem("name").Value;
                string offset = offsetEntry.SelectSingleNode("offset").InnerText;
                string type = offsetEntry.SelectSingleNode("type").InnerText;
                string assignedValue = offsetEntry.SelectSingleNode("assignedValue").InnerText;
                string defaultValue = offsetEntry.SelectSingleNode("defaultValue").InnerText;
                string notes = offsetEntry.SelectSingleNode("notes").InnerText;
                OffsetList.Add(new Offset(name, offset, type, assignedValue, defaultValue, notes));
            }
            reader.Close();
            reader.Dispose();

            return OffsetList;
        }

        public static bool WriteOffsetListToXML(string location, List<Offset> offsets)
        {
            try
            {
                using (XmlWriter writer = XmlWriter.Create(location))
                {
                    writer.WriteStartDocument();
                    writer.WriteStartElement("OffsetifyXML");
                    writer.WriteAttributeString("version", Properties.Settings.Default.applicationVersion.ToString());
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
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}

