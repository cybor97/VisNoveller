using System;
using System.Drawing;
using System.IO;
using System.Text;
using System.Xml;
using VNCore.Extensions;
using VNCore.Novel.Base;

namespace VNCore.Novel
{
    public static class NovelParser
    {
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
                                result.Add(ParseSlide(reader.ReadOuterXml()));
                                break;
                        }
                    else reader.Read();
                }
            return result;
        }
        public static Slide ParseSlide(string xml)
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
                    else if (reader.IsStartElement("Label"))
                    {
                        switch (reader.GetAttribute("Type"))
                        {
                            default:
                                result.Labels.Add(ParseTextLabel(reader.ReadOuterXml()));
                                break;
                        }
                    }
                    else reader.Read();
                }
            return result;
        }
        public static TextLabel ParseTextLabel(string xml)
        {
            var result = new TextLabel();
            using (var reader = XmlReader.Create(new MemoryStream(Encoding.UTF8.GetBytes(xml))))
                while (!reader.EOF)
                {
                    if (reader.IsStartElement("Label"))
                    {
                        int timeout, clickRedirect;
                        byte x, y;
                        result.Timeout = int.TryParse(reader.GetAttribute("Timeout"), out timeout) ? timeout : -1;
                        result.ClickRedirect = int.TryParse(reader.GetAttribute("ClickRedirect"), out clickRedirect) ? clickRedirect : -1;
                        result.Position = new Position
                        {
                            X = byte.TryParse(reader.GetAttribute("PositionX"), out x) ? x : (byte)0,
                            Y = byte.TryParse(reader.GetAttribute("PositionY"), out y) ? y : (byte)0
                        };
                        result.Text = reader.ReadElementContentAsString();
                    }
                    else reader.Read();
                }
            return result;
        }
    }
}
