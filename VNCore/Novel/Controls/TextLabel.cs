﻿using System.IO;
using System.Text;
using System.Xml;
using VNCore.Novel.Animations;
using VNCore.Novel.Base;

namespace VNCore.Novel.Controls
{
    public class TextLabel : ILabel
    {
        public int Timeout { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public Position Position { get; set; }
        public Fork Fork { get; set; }
        public Storyboard Storyboard { get; set; }
        public override string ToString()
        {
            var stream = new MemoryStream();
            using (var writer = XmlWriter.Create(stream, new XmlWriterSettings { Indent = true, OmitXmlDeclaration = true }))
            {
                writer.WriteStartElement("Label");
                writer.WriteAttributeString("Type", "TextLabel");
                writer.WriteAttributeString("Title", Title);
                writer.WriteAttributeString("Timeout", Timeout.ToString());
                writer.WriteAttributeString("Position", Position.ToString());
                if (Storyboard != null)
                    writer.WriteRaw(Storyboard.ToString());
                if (Fork != null)
                    writer.WriteRaw(Fork.ToString());
                writer.WriteString(Text);
                writer.WriteEndElement();
            }
            return Encoding.UTF8.GetString(stream.GetBuffer()).Trim((char)0);
        }
        public static TextLabel Parse(string xml)
        {
            var result = new TextLabel();
            using (var reader = XmlReader.Create(new MemoryStream(Encoding.UTF8.GetBytes(xml))))
                while (!reader.EOF)
                {
                    if (reader.IsStartElement("Label"))
                    {
                        int timeout;
                        result.Timeout = int.TryParse(reader.GetAttribute("Timeout"), out timeout) ? timeout : -1;
                        Position position;
                        result.Position = Position.TryParse(reader.GetAttribute("Position"), out position) ? position : new Position();
                        result.Title = reader.GetAttribute("Title");
                        result.Text = reader.ReadElementContentAsString();
                    }
                    else if (reader.IsStartElement("Storyboard"))
                        result.Storyboard = Storyboard.Parse(reader.ReadOuterXml());
                    else if (reader.IsStartElement("Fork"))
                        result.Fork = Fork.Parse(reader.ReadOuterXml());
                    else reader.Read();
                }
            return result;
        }
    }
}
