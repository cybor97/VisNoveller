using System;
using System.IO;

namespace VNCore.Extensions
{
    public enum ZipValidatingResult
    {
        OK,
        ZipNull,
        FileNotExist,
        UnknownIOError
    }
    public partial class ZipStorer
    {
        public static Exception ErrorData { get; set; }
        public static ZipValidatingResult Validate(string _filename, FileAccess _access)
        {
            try
            {
                if (File.Exists(_filename))
                    return Open(_filename, _access) != null ? ZipValidatingResult.OK : ZipValidatingResult.ZipNull;
                else return ZipValidatingResult.FileNotExist;
            }
            catch (Exception e)
            {
                ErrorData = e;
                return ZipValidatingResult.UnknownIOError;
            }
        }
        public static string GetRelativeParent(string path)
        {
            if (string.IsNullOrWhiteSpace(path)) return string.Empty;
            var result = Directory.GetParent(path).FullName
                .Replace(Directory.GetCurrentDirectory(), string.Empty)
                .Replace(Directory.GetDirectoryRoot(path), string.Empty);
            if (path.Contains("/")) return result.Replace('\\', '/');
            else return result;
        }
    }
}
