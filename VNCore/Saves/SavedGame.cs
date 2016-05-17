using System.IO;
using System.Text;
using System.Xml;

namespace VNCore.Saves
{
    class SavedGame
    {
        public string Name { get; set; }
        public string NovelPath { get; set; }
        public int SlideID { get; set; }
        public override string ToString()
        {
            var stream = new MemoryStream();
            using (var writer = XmlWriter.Create(stream, new XmlWriterSettings { Indent = true }))
            {
                writer.WriteStartElement("Game");
                writer.WriteAttributeString("Name", Name);
                writer.WriteAttributeString("NovelPath", NovelPath);
                writer.WriteAttributeString("SlideID", SlideID.ToString());
            }
            return Encoding.UTF8.GetString(stream.GetBuffer());
        }
        public static SavedGame Parse(string xml)
        {
            var result = new SavedGame();
            using (var reader = XmlReader.Create(new MemoryStream(Encoding.UTF8.GetBytes(xml))))
                while (!reader.EOF)
                    if (reader.IsStartElement("Game"))
                    {
                        result.Name = reader.GetAttribute("Name");
                        result.NovelPath = reader.GetAttribute("NovelPath");
                        int slideID;
                        slideID = int.TryParse(reader.GetAttribute("SlideID"), out slideID) ? slideID : 0;
                    }
            return result;
        }
    }

}
