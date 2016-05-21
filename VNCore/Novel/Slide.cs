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
    }
}