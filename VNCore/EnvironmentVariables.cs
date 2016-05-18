using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace VNCore
{
    public static class EnvironmentVariables
    {
        static string DataDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "VisNoveller");
        public static string ConfigurationFile = Path.Combine(DataDirectory, "Config.xml");
    }
}
