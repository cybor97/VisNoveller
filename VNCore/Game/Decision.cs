using System.IO;
using System.Text;
using System.Xml;

namespace VNCore.Game
{
    public class Decision
    {
        public int SlideID { get; set; }
        public int DecisionID { get; set; }
        public override string ToString()
        {
            var stream = new MemoryStream();
            using (var writer = XmlWriter.Create(stream))
            {
                writer.WriteStartElement("Decision");
                writer.WriteAttributeString("SlideID", SlideID.ToString());
                writer.WriteAttributeString("DecisionID", DecisionID.ToString());
                writer.WriteEndElement();
            }
            return Encoding.UTF8.GetString(stream.GetBuffer());
        }
        public static Decision Parse(string xml)
        {
            var result = new Decision();
            using (var reader = XmlReader.Create(new MemoryStream(Encoding.UTF8.GetBytes(xml))))
                while (reader.Read())
                    if (reader.IsStartElement("Decision"))
                    {
                        result.SlideID = int.Parse(reader.GetAttribute("SlideID"));
                        result.DecisionID = int.Parse(reader.GetAttribute("DecisionID"));
                    }
            return result;
        }
    }
}