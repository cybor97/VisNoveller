﻿using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using VNCore.Novel.Animations;
using VNCore.Novel.Base;

namespace VNCore.Novel.Controls
{
    public class Character : ICharacter
    {
        public int ID { get; set; }
        public List<object> Images { get; set; }
        public Position Position { get; set; }
        public Storyboard Storyboard { get; set; }
        public override string ToString()
        {
            var stream = new MemoryStream();
            using (var writer = XmlWriter.Create(stream, new XmlWriterSettings { Indent = true, OmitXmlDeclaration = true }))
            {
                writer.WriteStartElement("Character");
                writer.WriteAttributeString("Type", "Character");
                writer.WriteAttributeString("ID", ID.ToString());
                writer.WriteAttributeString("Position", Position.ToString());
                if (Storyboard != null)
                    writer.WriteRaw(Storyboard.ToString());
                if (Images != null)
                    foreach (var current in Images)
                        if (current is Image)
                            writer.WriteRaw(((Image)current).ToString());
                writer.WriteEndElement();
            }
            return Encoding.UTF8.GetString(stream.GetBuffer()).Trim((char)0);
        }
        public static Character Parse(string xml)
        {
            var result = new Character();
            using (var reader = XmlReader.Create(new MemoryStream(Encoding.UTF8.GetBytes(xml))))
                while (!reader.EOF)
                    if (reader.IsStartElement("Character"))
                    {
                        int id;
                        result.ID = int.TryParse(reader.GetAttribute("ID"), out id) ? id : 0;
                        Position position;
                        result.Position = Position.TryParse(reader.GetAttribute("Position"), out position) ? position : new Position();
                        reader.Read();
                    }
                    else if (reader.IsStartElement("Storyboard"))
                        result.Storyboard = Storyboard.Parse(reader.ReadOuterXml());
                    else if (reader.IsStartElement("Image"))
                    {
                        if (result.Images == null) result.Images = new List<object>();
                        result.Images.Add(Image.Parse(reader.ReadOuterXml()));
                    }
                    else reader.Read();
            return result;
        }

    }
}
