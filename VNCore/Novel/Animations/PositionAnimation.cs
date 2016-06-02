using System.IO;
using System.Text;
using System.Xml;
using VNCore.Novel.Base;

namespace VNCore.Novel.Animations
{
    public class PositionAnimation : IAnimation<Position>
    {
        public Position From { get; set; }

        public Position To { get; set; }

        public int Duration { get; set; }

        public bool AutoPlay { get; set; }

        public bool AutoReverse { get; set; }

        public bool RepeatForever { get; set; }
        public override string ToString()
        {
            var stream = new MemoryStream();
            using (var writer = XmlWriter.Create(stream))
            {
                writer.WriteStartElement("Animation");
                writer.WriteAttributeString("Type", "PositionAnimation");
                writer.WriteAttributeString("From", From.ToString());
                writer.WriteAttributeString("To", To.ToString());
                writer.WriteAttributeString("Duration", Duration.ToString());
                writer.WriteAttributeString("AutoPlay", AutoPlay.ToString());
                writer.WriteAttributeString("AutoReverse", AutoReverse.ToString());
                writer.WriteAttributeString("RepeatForever", RepeatForever.ToString());
                writer.WriteEndElement();
            }
            return Encoding.UTF8.GetString(stream.GetBuffer());
        }
        public static PositionAnimation Parse(string xml)
        {
            var result = new PositionAnimation();
            using (var reader = XmlReader.Create(new MemoryStream(Encoding.UTF8.GetBytes(xml))))
                while (!reader.EOF)
                    if (reader.IsStartElement("PositionAnimation"))
                    {
                        Position from, to;
                        result.From = Position.TryParse(reader.GetAttribute("From"), out from) ? from : new Position();
                        result.To = Position.TryParse(reader.GetAttribute("To"), out to) ? from : new Position();
                        int duration;
                        result.Duration = int.TryParse(reader.GetAttribute("Duration"), out duration) ? duration : 0;
                        bool autoPlay, autoReverse, repeatForever;
                        result.AutoPlay = bool.TryParse(reader.GetAttribute("AutoPlay"), out autoPlay) && autoPlay;
                        result.AutoReverse = bool.TryParse(reader.GetAttribute("AutoReverse"), out autoReverse) && autoReverse;
                        result.RepeatForever = bool.TryParse(reader.GetAttribute("RepeatForever"), out repeatForever) && repeatForever;
                    }
                    else reader.Read();
            return result;

        }
    }
}
