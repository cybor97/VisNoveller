using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Xml;
using System.IO;
using VNCore.Novel.Base;
using System;
using VNCore.Extensions;
using System.Linq;

namespace VNCore.Novel
{
    public class Novel : List<ISlide>
    {
        public int Version { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string KonamiCode { get; set; }
        public Icon Icon { get; set; }
        public Image Logo { get; set; }
        public List<string> Tags { get; set; }
        public Novel()
        {
            Tags = new List<string>();
        }
        public void WriteFile(string filename)
        {
            File.WriteAllBytes(filename, WriteStream().GetBuffer());
        }
        public string WriteString()
        {
            return Encoding.UTF8.GetString(WriteStream().GetBuffer());
        }
        public MemoryStream WriteStream()
        {
            var result = new MemoryStream();
            using (var writer = XmlWriter.Create(result, new XmlWriterSettings { Indent = true }))
            {
                writer.WriteStartElement("Novel");
                writer.WriteAttributeString("Version", Version.ToString());
                writer.WriteAttributeString("Title", Title);
                writer.WriteAttributeString("KonamiCode", KonamiCode);
                writer.WriteElementString("Description", Description);
                if (Icon != null) writer.WriteElementString("Icon", Convert.ToBase64String(Icon.ToByteArray()));
                if (Logo != null) writer.WriteElementString("Logo", Convert.ToBase64String(Logo.ToByteArray()));
                var tags = "";
                foreach (var current in Tags.Where(c => !string.IsNullOrWhiteSpace(c)))
                    tags += "#" + current;
                if (!string.IsNullOrWhiteSpace(tags)) writer.WriteElementString("Tags", tags);
                foreach (var current in this)
                    writer.WriteRaw(current.ToString());
                writer.WriteEndElement();
            }
            return result;
        }
        public static Novel ParseFile(string filename)
        {
            return Parse(new FileStream(filename, FileMode.Open));
        }
        public static Novel ParseText(string xml)
        {
            return Parse(new MemoryStream(Encoding.UTF8.GetBytes(xml)));
        }
        public static Novel Parse(Stream stream)
        {
            var result = new Novel();
            using (var reader = XmlReader.Create(stream))
                while (!reader.EOF)
                {
                    if (reader.IsStartElement("Novel"))
                    {
                        result.Title = reader.GetAttribute("Title");
                        result.KonamiCode = reader.GetAttribute("KonamiCode");
                        int version;
                        result.Version = int.TryParse(reader.GetAttribute("Version"), out version) ? version : 0;
                        reader.Read();
                    }
                    else if (reader.IsStartElement("Description"))
                        result.Description = reader.ReadElementContentAsString();
                    else if (reader.IsStartElement("Icon"))
                        result.Icon = Convert.FromBase64String(reader.ReadElementContentAsString()).ToIcon();
                    else if (reader.IsStartElement("Logo"))
                        result.Logo = Convert.FromBase64String(reader.ReadElementContentAsString()).ToBitmap();
                    else if (reader.IsStartElement("Tags"))
                        foreach (var current in reader.ReadElementContentAsString().Split(new[] { '#' }, StringSplitOptions.RemoveEmptyEntries))
                            result.Tags.Add(current);
                    else if (reader.IsStartElement("Slide"))
                        switch (reader.GetAttribute("Type"))
                        {
                            default:
                                result.Add(Slide.Parse(reader.ReadOuterXml()));
                                break;
                        }
                    else reader.Read();
                }
            return result;
        }
    }
}
