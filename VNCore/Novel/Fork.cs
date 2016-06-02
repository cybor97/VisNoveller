using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using VNCore.Game;

namespace VNCore.Novel
{
    public class Fork
    {
        public Link MainLink { get; set; }
        public Link AlternativeLink { get; set; }
        public List<Link> RequiredDecisions { get; set; }
        public Link Navigate(GameState state)
        {
            return RequiredDecisions == null ||
                RequiredDecisions.Count == 0 ||
                AlternativeLink == null ||
                RequiredDecisions.TrueForAll(c => state.Links.Contains(c)) ?
                MainLink : AlternativeLink;
        }
        public override string ToString()
        {
            var stream = new MemoryStream();
            using (var writer = XmlWriter.Create(stream))
            {
                writer.WriteStartElement("Fork");

                writer.WriteStartElement("MainLink");
                writer.WriteRaw(MainLink.ToString());
                writer.WriteEndElement();

                writer.WriteStartElement("AlternativeLink");
                writer.WriteRaw(MainLink.ToString());
                writer.WriteEndElement();

                if (RequiredDecisions != null)
                {
                    writer.WriteStartElement("RequiredDecisions");
                    foreach (var current in RequiredDecisions)
                        if (current != null)
                            writer.WriteRaw(current.ToString());
                    writer.WriteEndElement();
                }
                writer.WriteEndElement();
            }
            return Encoding.UTF8.GetString(stream.GetBuffer());
        }
        public static Fork Parse(string xml)
        {
            var result = new Fork();
            using (var reader = XmlReader.Create(new MemoryStream(Encoding.UTF8.GetBytes(xml))))
                while (!reader.EOF)
                    if (reader.IsStartElement("Fork"))
                        reader.Read();
                    else if (reader.IsStartElement("MainLink"))
                        result.MainLink = Link.Parse(reader.ReadInnerXml());
                    else if (reader.IsStartElement("AlternativeLink"))
                        result.AlternativeLink = Link.Parse(reader.ReadInnerXml());
                    else if (reader.IsStartElement("RequiredDecisions"))
                    {
                        if (result.RequiredDecisions == null) result.RequiredDecisions = new List<Link>();
                        using (var RequiredDecisionsReader = XmlReader.Create(new MemoryStream(Encoding.UTF8.GetBytes(reader.ReadOuterXml()))))
                            while (!RequiredDecisionsReader.EOF)
                                if (RequiredDecisionsReader.IsStartElement("Link"))
                                    result.RequiredDecisions.Add(Link.Parse(RequiredDecisionsReader.ReadOuterXml()));
                                else RequiredDecisionsReader.Read();
                    }
                    else reader.Read();
            return result;
        }
    }
}
