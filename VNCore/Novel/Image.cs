using System;
using System.IO;
using System.Text;
using System.Xml;

namespace VNCore.Novel.Base
{
    public enum ImageStoreMode
    {
        Fact,
        Path
    }
    public class Image
    {
        public int ID { get; set; }
        public string Path { get; set; }
        public byte[] Data { get; set; }
        public ImageStoreMode Mode { get; set; }
        public override string ToString()
        {
            var stream = new MemoryStream();
            using (var writer = XmlWriter.Create(stream, new XmlWriterSettings { Indent = true, OmitXmlDeclaration = true }))
            {
                writer.WriteStartElement("Image");
                writer.WriteAttributeString("ID", ID.ToString());
                writer.WriteAttributeString("Mode", Mode.ToString());
                writer.WriteAttributeString("Path", Path);
                if (Mode == ImageStoreMode.Fact && Data != null && Data.Length > 0)
                    writer.WriteString(Convert.ToBase64String(Data));
                writer.WriteEndElement();
            }
            return Encoding.UTF8.GetString(stream.GetBuffer()).Trim((char)0);
        }
        public static Image Parse(string xml)
        {
            var result = new Image();
            using (var reader = XmlReader.Create(new MemoryStream(Encoding.UTF8.GetBytes(xml))))
                while (!reader.EOF)
                {
                    if (reader.IsStartElement("Image"))
                    {
                        int id;
                        ImageStoreMode mode;
                        result.ID = int.TryParse(reader.GetAttribute("ID"), out id) ? id : 0;
                        result.Mode = Enum.TryParse<ImageStoreMode>(reader.GetAttribute("Mode"), out mode) ? mode : 0;
                        result.Path = reader.GetAttribute("Path");
                        result.Data = Convert.FromBase64String(reader.ReadElementContentAsString());
                    }
                    else reader.Read();
                }
            return result;

        }
    }
}
