using System;
using System.Collections.Generic;

namespace VNCore.Extensions
{
    public static class XmlStringProcessor
    {
        public static string FormatXmlString(this string xml, bool removeEmptyLines = true, bool removeNullSymbols = true)
        {
            var result = xml.Replace("<", "\n<").Replace(">", ">\n");
            if (removeNullSymbols) result = result.Replace(((char)0).ToString(), "");
            if (removeEmptyLines)
            {
                var lines = new List<string>(result.Split('\n'));
                lines.RemoveAll(c => string.IsNullOrWhiteSpace(c));
                result = "";
                for (int i = 0; i < lines.Count; i++)
                    result += lines[i] + (i < lines.Count - 1 ? "\n" : "");
            }
            return result.Trim(Environment.NewLine.ToCharArray()).Trim(null);
        }
    }
}
