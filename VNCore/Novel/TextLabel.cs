using System.IO;
using System.Text;
using System.Xml;
using VNCore.Novel.Base;

namespace VNCore.Novel
{
    public class TextLabel : ILabel
    {
        public int Timeout { get; set; }
        public int ClickRedirect { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public Position Position { get; set; }
        public override string ToString()
        {
            var stream = new MemoryStream();
            using (var writer = XmlWriter.Create(stream, new XmlWriterSettings { Indent = true }))
            {
                writer.WriteStartElement("Label");
                writer.WriteAttributeString("Type", "TextLabel");
                writer.WriteAttributeString("Title", Title);
                writer.WriteAttributeString("Timeout", Timeout.ToString());
                writer.WriteAttributeString("ClickRedirect", ClickRedirect.ToString());
                writer.WriteAttributeString("PositionX", Position.X.ToString());
                writer.WriteAttributeString("PositionY", Position.Y.ToString());
                writer.WriteString(Text);
                writer.WriteEndElement();
            }
            return Encoding.UTF8.GetString(stream.GetBuffer());
        }
    }
}
