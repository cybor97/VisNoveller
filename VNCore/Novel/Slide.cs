using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using System.Xml;
using VNCore.Novel.Base;
using VNCore.Novel.Controls;

namespace VNCore.Novel
{
    public class Slide : ISlide
    {
        public int ID { get; set; }
        public int KonamiReplaceID { get; set; }
        public bool KonamiLocked { get; set; }
        public string Title { get; set; }
        public object Background { get; set; }
        public object BackgroundSound { get; set; }
        public IList<ILabel> Labels { get; set; }
        public IList<ICharacter> Characters { get; set; }
        public Slide()
        {
            Labels = new List<ILabel>();
            Characters = new List<ICharacter>();
        }
        public IList<string> GetResources()
        {
            var result = new List<string>();
            if (Background != null &&
                Background is Image &&
                ((Image)Background).Mode == ImageStoreMode.Path &&
                !string.IsNullOrWhiteSpace(((Image)Background).Path))
                result.Add(((Image)Background).Path);
            if (BackgroundSound != null && BackgroundSound is string && !string.IsNullOrWhiteSpace((string)BackgroundSound))
                result.Add((string)BackgroundSound);
            foreach (var current in Characters)
                result.AddRange(current.GetResources());
            return result;
        }
        public override string ToString()
        {
            var stream = new MemoryStream();
            using (var writer = XmlWriter.Create(stream, new XmlWriterSettings { Indent = true, OmitXmlDeclaration = true }))
            {
                writer.WriteStartElement("Slide");
                writer.WriteAttributeString("Type", "Slide");
                writer.WriteAttributeString("Title", Title);
                writer.WriteAttributeString("ID", ID.ToString());
                writer.WriteAttributeString("KonamiLocked", KonamiLocked.ToString());
                writer.WriteAttributeString("KonamiReplaceID", KonamiReplaceID.ToString());

                if (Background != null)
                {
                    writer.WriteStartElement("Background");
                    if (Background is Color)
                    {
                        writer.WriteAttributeString("Type", "Color");
                        writer.WriteString(ColorTranslator.ToHtml((Color)Background));
                    }
                    else if (Background is Image)
                    {
                        writer.WriteAttributeString("Type", "Image");
                        writer.WriteRaw(Background.ToString());
                    }
                    else if (Background is string)
                    {
                        writer.WriteAttributeString("Type", "File");
                        writer.WriteString((string)Background);
                    }
                    writer.WriteEndElement();
                }
                if (BackgroundSound != null)
                {
                    writer.WriteStartElement("BackgroundSound");
                    if (BackgroundSound is byte[])
                    {
                        writer.WriteAttributeString("Type", "Data");
                        writer.WriteString(Convert.ToBase64String((byte[])BackgroundSound));
                    }
                    else if (BackgroundSound is string)
                    {
                        writer.WriteAttributeString("Type", "File");
                        writer.WriteString((string)BackgroundSound);
                    }
                    writer.WriteEndElement();
                }

                foreach (var current in Labels)
                    writer.WriteRaw(current.ToString());
                foreach (var current in Characters)
                    writer.WriteRaw(current.ToString());
                writer.WriteEndElement();
            }
            return Encoding.UTF8.GetString(stream.GetBuffer()).Trim((char)0);
        }
        public static Slide Parse(string xml)
        {
            var result = new Slide();
            using (var reader = XmlReader.Create(new MemoryStream(Encoding.UTF8.GetBytes(xml))))
                while (!reader.EOF)
                    if (reader.IsStartElement("Slide"))
                    {
                        int id;
                        result.ID = int.TryParse(reader.GetAttribute("ID"), out id) ? id : 0;
                        result.Title = reader.GetAttribute("Title");
                        bool konamiLocked;
                        result.KonamiLocked = bool.TryParse(reader.GetAttribute("KonamiLocked"), out konamiLocked) && konamiLocked;
                        int konamiReplaceID;
                        result.KonamiReplaceID = int.TryParse(reader.GetAttribute("KonamiReplaceID"), out konamiReplaceID) ? konamiReplaceID : 0;
                        reader.Read();
                    }
                    else if (reader.IsStartElement("Background"))
                        switch (reader.GetAttribute("Type"))
                        {
                            case "Color":
                                result.Background = ColorTranslator.FromHtml(reader.ReadElementContentAsString());
                                break;
                            case "Image":
                                result.Background = Image.Parse(reader.ReadInnerXml());
                                break;
                            case "File":
                                result.Background = reader.ReadElementContentAsString();
                                break;
                        }
                    else if (reader.IsStartElement("BackgroundSound"))
                        switch (reader.GetAttribute("Type"))
                        {
                            case "Data":
                                result.BackgroundSound = Convert.FromBase64String(reader.ReadElementContentAsString());
                                break;
                            case "File":
                                result.BackgroundSound = reader.ReadElementContentAsString();
                                break;
                        }
                    else if (reader.IsStartElement("Label"))
                        switch (reader.GetAttribute("Type"))
                        {
                            default:
                                result.Labels.Add(TextLabel.Parse(reader.ReadOuterXml()));
                                break;
                        }
                    else if (reader.IsStartElement("Character"))
                        switch (reader.GetAttribute("Type"))
                        {
                            default:
                                result.Characters.Add(Character.Parse(reader.ReadOuterXml()));
                                break;
                        }
                    else reader.Read();
            return result;
        }
    }
}