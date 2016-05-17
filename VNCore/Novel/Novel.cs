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
    }
}
