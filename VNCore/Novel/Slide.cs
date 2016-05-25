using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using System.Xml;
using VNCore.Extensions;
using VNCore.Novel.Base;

namespace VNCore.Novel
{
    public class Slide : ISlide
    {
        public int ID { get; set; }
        public int KonamiReplaceID { get; set; }
        public bool KonamiLocked { get; set; }
        public string Title { get; set; }
        public object Background { get; set; }
        public byte[] BackgroundMusic { get; set; }
        public List<ILabel> Labels { get; set; }
        public Slide()
        {
            Labels = new List<ILabel>();
        }
        public override string ToString()
        {
            var stream = new MemoryStream();
            using (var writer = XmlWriter.Create(stream, new XmlWriterSettings { Indent = true }))
            {
                writer.WriteStartElement("Slide");
                writer.WriteAttributeString("Type", "Slide");
                writer.WriteAttributeString("Title", Title);
                writer.WriteAttributeString("ID", ID.ToString());
                writer.WriteAttributeString("KonamiLocked", KonamiLocked.ToString());
                writer.WriteAttributeString("KonamiReplaceID", KonamiReplaceID.ToString());

                writer.WriteStartElement("Background");
                if (Background is Color)
                {
                    writer.WriteAttributeString("Type", "Color");
                    writer.WriteString(ColorTranslator.ToHtml((Color)Background));
                }
                else if (Background is Image)
                {
                    writer.WriteAttributeString("Type", "Image");
                    writer.WriteString(Convert.ToBase64String(((Image)Background).ToByteArray()));
                }
                writer.WriteEndElement();

                if (BackgroundMusic != null && BackgroundMusic.Length > 0)
                    writer.WriteElementString("BackgroundMusic", Convert.ToBase64String(BackgroundMusic));

                foreach (var current in Labels)
                    writer.WriteRaw(current.ToString());
                writer.WriteEndElement();
            }
            return Encoding.UTF8.GetString(stream.GetBuffer());
        }
        public static Slide Parse(string xml)
        {
            var result = new Slide();
            using (var reader = XmlReader.Create(new MemoryStream(Encoding.UTF8.GetBytes(xml))))
                while (!reader.EOF)
                {
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
                    {
                        switch (reader.GetAttribute("Type"))
                        {
                            case "Color":
                                result.Background = ColorTranslator.FromHtml(reader.ReadElementContentAsString());
                                break;
                            case "Image":
                                result.Background = Convert.FromBase64String(reader.ReadElementContentAsString()).ToBitmap();
                                break;
                        }
                    }
                    else if (reader.IsStartElement("BackgroundMusic"))
                        result.BackgroundMusic = Convert.FromBase64String(reader.ReadElementContentAsString());
                    else if (reader.IsStartElement("Label"))
                    {
                        switch (reader.GetAttribute("Type"))
                        {
                            default:
                                result.Labels.Add(TextLabel.Parse(reader.ReadOuterXml()));
                                break;
                        }
                    }
                    else reader.Read();
                }
            return result;
        }
    }
}