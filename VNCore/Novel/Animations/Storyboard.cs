using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using VNCore.Novel.Base;

namespace VNCore.Novel.Animations
{
    public class Storyboard : List<IAnimation<object>>
    {
        /// <summary>
        /// In milliseconds
        /// </summary>
        public int Duration { get; set; }
        public TimeSpan DurationTimeSpan
        {
            get
            {
                return TimeSpan.FromMilliseconds(Duration);
            }
            set
            {
                Duration = (int)value.TotalMilliseconds;
            }
        }
        public bool AutoReverse { get; set; }
        public bool RepeatForever { get; set; }
        public override string ToString()
        {
            var stream = new MemoryStream();
            using (var writer = XmlWriter.Create(stream, new XmlWriterSettings { Indent = true, OmitXmlDeclaration = true }))
            {
                writer.WriteStartElement("Storyboard");
                writer.WriteAttributeString("Duration", Duration.ToString());
                writer.WriteAttributeString("AutoReverse", AutoReverse.ToString());
                writer.WriteAttributeString("RepeatForever", RepeatForever.ToString());
                foreach (var current in this)
                    writer.WriteRaw(current.ToString());
                writer.WriteEndElement();
            }
            return Encoding.UTF8.GetString(stream.GetBuffer()).Trim((char)0);
        }
        public static Storyboard Parse(string xml)
        {
            var result = new Storyboard();
            using (var reader = XmlReader.Create(new MemoryStream(Encoding.UTF8.GetBytes(xml))))
                while (!reader.EOF)
                    if (reader.IsStartElement("Storyboard"))
                    {
                        int duration;
                        result.Duration = int.TryParse(reader.GetAttribute("Duration"), out duration) ? duration : 0;
                        bool autoReverse, repeatForever;
                        result.AutoReverse = bool.TryParse(reader.GetAttribute("AutoReverse"), out autoReverse) && autoReverse;
                        result.RepeatForever = bool.TryParse(reader.GetAttribute("RepeatForever"), out repeatForever) && repeatForever;
                        reader.Read();
                    }
                    else if (reader.IsStartElement("Animation"))
                        switch (reader.GetAttribute("Type"))
                        {
                            case "PositionAnimation":
                                result.Add((IAnimation<object>)PositionAnimation.Parse(reader.ReadOuterXml()));
                                break;
                            default:
                                reader.Read();
                                break;
                        }
                    else reader.Read();
            return result;
        }
    }
}
