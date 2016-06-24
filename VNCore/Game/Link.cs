using System;
using System.IO;
using System.Text;
using System.Xml;

namespace VNCore.Game
{
    public enum LinkMode
    {
        None,
        Image,
        Label,
        Slide
    }
    public class Link
    {
        public int LinkID { get; set; }
        public int DestinationID { get; set; }
        public LinkMode Mode { get; set; }
        public override string ToString()
        {
            var stream = new MemoryStream();
            using (var writer = XmlWriter.Create(stream))
            {
                writer.WriteStartElement("Link");
                writer.WriteAttributeString("DestinationID", DestinationID.ToString());
                writer.WriteAttributeString("LinkID", LinkID.ToString());
                writer.WriteAttributeString("Mode", Mode.ToString());
                writer.WriteEndElement();
            }
            return Encoding.UTF8.GetString(stream.GetBuffer());
        }
        public static Link Parse(string xml)
        {
            var result = new Link();
            using (var reader = XmlReader.Create(new MemoryStream(Encoding.UTF8.GetBytes(xml))))
                while (reader.Read())
                    if (reader.IsStartElement("Link"))
                    {
                        LinkMode mode;
                        result.DestinationID = int.Parse(reader.GetAttribute("DestinationID"));
                        result.LinkID = int.Parse(reader.GetAttribute("LinkID"));
                        result.Mode = Enum.TryParse(reader.GetAttribute("Mode"), out mode) ? mode : LinkMode.None;
                    }
            return result;
        }
    }
}