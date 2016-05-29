using System.Collections.Generic;
using System.IO;
using VNCore.Novel;

namespace VNCore.Extensions
{
    public static class NovelPacker
    {
        public static int CurrentState = -1;
        public delegate void PackCompletedDelegate(object result);
        public static event PackCompletedDelegate PackCompleted;
        public static void Pack(string novelFilename, string packedNovelFilename)
        {
            if (File.Exists(novelFilename))
            {
                var validatingResult = Novel.Novel.Validate(novelFilename);
                if (validatingResult == NovelValidatingResult.OK)
                {
                    var novel = Novel.Novel.ParseFile(novelFilename);
                    var workingDirectory = Directory.GetParent(novelFilename).FullName;
                    var storage = ZipStorer.Create(packedNovelFilename, string.Format("{0} installer", novel.Title));
                    var resources = novel.GetResources();
                    storage.AddFile(ZipStorer.Compression.Deflate, novelFilename, Path.GetFileName(novelFilename), novel.Title);
                    for (int i = 0; i < resources.Count; i++)
                    {
                        storage.AddFile(ZipStorer.Compression.Deflate, Path.Combine(workingDirectory, resources[i]), resources[i], "");
                        CurrentState = (int)(((double)i / resources.Count) * 100);
                    }
                    storage.Close();
                    PackCompleted?.Invoke("OK");
                }
                else PackCompleted?.Invoke("FAIL|INCORRECT_FORMAT");
            }
            else PackCompleted?.Invoke("FAIL|FILE_NOT_FOUND");
        }
        public static void UnPack(string packedNovelFilename, string unpackDirectory)
        {
            if (!Directory.Exists(unpackDirectory)) Directory.CreateDirectory(unpackDirectory);
            if (ZipStorer.Validate(packedNovelFilename, FileAccess.Read) == ZipValidatingResult.OK)
            {
                var zip = ZipStorer.Open(packedNovelFilename, FileAccess.Read);
                var files = zip.ReadCentralDir();
                for (int i = 0; i < files.Count; i++)
                {
                    var dir = ZipStorer.GetRelativeParent(files[i].FilenameInZip);
                    if (!Directory.Exists(Path.Combine(unpackDirectory, dir)))
                    {
                        Directory.CreateDirectory(Path.Combine(unpackDirectory, dir));
                        CurrentState = (int)(((double)i / files.Count) * 100);
                    }
                    zip.ExtractFile(files[i], Path.Combine(dir, files[i].FilenameInZip));
                    PackCompleted?.Invoke("OK");
                }
            }
            else PackCompleted?.Invoke("FAIL|INCORRECT_FORMAT");
        }
    }
}
