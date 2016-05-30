using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using VNCore.Game;

namespace VNCore.Novel
{
    public enum ClickProcessMode
    {
        None,
        SwitchImage,
        SwitchLabel,
        SwitchSlide
    }
    public class ClickProcess
    {
        public Decision CurrentDecision { get; set; }
        public List<Decision> NeededDecisions { get; set; }
        public ClickProcessMode Mode { get; set; }
        public override string ToString()
        {
            var stream = new MemoryStream();
            using (var writer = XmlWriter.Create(stream))
            {
                writer.WriteStartElement("ClickProcess");
                //TODO: Implement
                writer.WriteEndElement();
            }
            return Encoding.UTF8.GetString(stream.GetBuffer());
        }
        public static Decision Parse(string xml)
        {
            var result = new Decision();
            using (var reader = XmlReader.Create(new MemoryStream(Encoding.UTF8.GetBytes(xml))))
                while (reader.Read())
                    if (reader.IsStartElement("ClickProcess"))
                    {
                        //TODO: Implement
                    }
            return result;
        }
    }
}
