using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;
using VNCore.Novel.Base;
using System;
using System.Linq;

namespace VNCore.Novel
{
    public enum NovelValidatingResult : byte
    {
        OK,
        Empty,
        IncorrectFormat,
        NovelFileNotExists,
        ResourceFileNotExists,
        InnerNavigationProblems
    }
    public enum ReservedIDs : sbyte
    {
        StartSlide = -1,
        MenuSlide = -2,
        WaitingSlide = -3,
        EndingSlide = -4,
        ExitConfirmSlide = -5
    }
    public class Novel : List<ISlide>
    {
        public uint Version { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string KonamiCode { get; set; }
        public Image Icon { get; set; }
        public Image Logo { get; set; }
        public IList<string> Tags { get; set; }
        public Novel()
        {
            Tags = new List<string>();
        }
        public void WriteFile(string filename)
        {
            File.WriteAllText(filename, WriteString());
        }
        public string WriteString()
        {
            return Encoding.UTF8.GetString(WriteStream().GetBuffer()).Trim((char)0);
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
                if (Icon != null)
                {
                    writer.WriteStartElement("Icon");
                    writer.WriteRaw(Icon.ToString());
                    writer.WriteEndElement();
                }
                if (Logo != null)
                {
                    writer.WriteStartElement("Logo");
                    writer.WriteRaw(Icon.ToString());
                    writer.WriteEndElement();
                }
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
        public static NovelValidatingResult Validate(string novelFilename)
        {
            var workingDirectory = Directory.GetParent(novelFilename).FullName;
            if (File.Exists(novelFilename))
                if (!string.IsNullOrWhiteSpace(File.ReadAllText(novelFilename)))
                    try
                    {
                        var novel = ParseFile(novelFilename);
                        if (novel != null)
                        {
                            bool allFilesExists = true;
                            foreach (var current in novel.GetResources())
                                allFilesExists = allFilesExists && File.Exists(Path.Combine(Directory.GetParent(novelFilename).FullName, current));
                            if (allFilesExists)
                            {
                                bool innerNavigationOK = true;
                                var ids = new List<int>();
                                foreach (var currentSlide in novel)
                                    ids.Add(currentSlide.ID);
                                for (int i = 0; i < ids.Count; i++)
                                    for (int n = 0; n < ids.Count; n++)
                                        if (i != n)
                                            innerNavigationOK = innerNavigationOK &&
                                                ids[i] != ids[n] &&
                                                (ids[i] > 0 || Enum.GetValues(typeof(ReservedIDs)).OfType<int>().ToArray().Contains(ids[i]));
                                return innerNavigationOK ? NovelValidatingResult.OK : NovelValidatingResult.InnerNavigationProblems;
                            }
                            else return NovelValidatingResult.ResourceFileNotExists;
                        }
                        else return NovelValidatingResult.IncorrectFormat;
                    }
                    catch (XmlException)
                    {
                        return NovelValidatingResult.IncorrectFormat;
                    }
                else return NovelValidatingResult.Empty;
            else return NovelValidatingResult.NovelFileNotExists;
        }
        public static Novel ParseFile(string filename)
        {
            return ParseText(File.ReadAllText(filename));
        }
        public static Novel ParseText(string xml)
        {
            return Parse(new MemoryStream(Encoding.UTF8.GetBytes(xml.Trim((char)0))));
        }
        public static Novel Parse(Stream stream)
        {
            var result = new Novel();
            using (var reader = XmlReader.Create(stream, new XmlReaderSettings { CheckCharacters = false }))
                while (!reader.EOF)
                {
                    if (reader.IsStartElement("Novel"))
                    {
                        result.Title = reader.GetAttribute("Title");
                        result.KonamiCode = reader.GetAttribute("KonamiCode");
                        uint version;
                        result.Version = uint.TryParse(reader.GetAttribute("Version"), out version) ? version : 0;
                        reader.Read();
                    }
                    else if (reader.IsStartElement("Description"))
                        result.Description = reader.ReadElementContentAsString();
                    else if (reader.IsStartElement("Icon"))
                        result.Icon = Image.Parse(reader.ReadInnerXml());
                    else if (reader.IsStartElement("Logo"))
                        result.Logo = Image.Parse(reader.ReadInnerXml());
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
        public List<string> GetResources()
        {
            var result = new List<string>();
            if (Icon != null && Icon.Mode == ImageStoreMode.Path && !string.IsNullOrWhiteSpace(Icon.Path)) result.Add(Icon.Path);
            if (Logo != null && Logo.Mode == ImageStoreMode.Fact && !string.IsNullOrWhiteSpace(Logo.Path)) result.Add(Logo.Path);
            foreach (var currentISlide in this)
                if (currentISlide != null)
                    result.AddRange(currentISlide.GetResources());
            return result;
        }
    }
}
