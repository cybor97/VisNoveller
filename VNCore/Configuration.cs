using System;
using System.IO;
using System.Xml;

namespace VNCore
{
    public static class Configuration
    {
        public static string UserName { get; set; }
        public static string Language { get; set; }
        public static bool Fullscreen { get; set; }
        public static bool AutoSave { get; set; }
        public static int SoundVolume { get; set; }
        public static void Init()
        {
            if (File.Exists(EnvironmentVariables.ConfigurationFile))
                Load();
            else
            {
                Fullscreen = true;
                Save();
            }
        }
        public static void Save()
        {
            using (var writer = XmlWriter.Create(EnvironmentVariables.ConfigurationFile))
            {
                writer.WriteStartElement("Config");
                writer.WriteAttributeString("UserName", UserName ?? (UserName = Environment.UserName));
                writer.WriteAttributeString("Language", Language ?? (Language = System.Globalization.CultureInfo.CurrentCulture.TwoLetterISOLanguageName));
                writer.WriteAttributeString("Fullscreen", Fullscreen.ToString());
                writer.WriteAttributeString("AutoSave", AutoSave.ToString());
                writer.WriteAttributeString("SoundVolume", SoundVolume.ToString());
                writer.WriteEndElement();
            }
        }
        public static void Load()
        {
            if (File.Exists(EnvironmentVariables.ConfigurationFile))
                using (var reader = XmlReader.Create(EnvironmentVariables.ConfigurationFile))
                    while (reader.Read())
                        if (reader.IsStartElement("Config"))
                        {
                            UserName = reader.GetAttribute("UserName");
                            Language = reader.GetAttribute("Language");
                            Fullscreen = bool.Parse(reader.GetAttribute("Fullscreen"));
                            AutoSave = bool.Parse(reader.GetAttribute("AutoSave"));
                            SoundVolume = int.Parse(reader.GetAttribute("SoundVolume"));
                        }
        }
    }
}
