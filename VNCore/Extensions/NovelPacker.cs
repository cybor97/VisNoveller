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
                switch (Novel.Novel.Validate(novelFilename))
                {
                    case NovelValidatingResult.OK:
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
                        break;
                    case NovelValidatingResult.Empty:
                        PackCompleted?.Invoke("FAIL|FILE_IS_EMPTY");
                        break;
                    case NovelValidatingResult.IncorrectFormat:
                        PackCompleted?.Invoke("FAIL|INCORRECT_FORMAT");
                        break;
                    case NovelValidatingResult.InnerNavigationProblems:
                        PackCompleted?.Invoke("FAIL|INNER_NAVIGATION_PROBLEMS");
                        break;
                    case NovelValidatingResult.NovelFileNotExists:
                        PackCompleted?.Invoke("FAIL|FILE_NOT_FOUND");
                        break;
                    case NovelValidatingResult.ResourceFileNotExists:
                        PackCompleted?.Invoke("FAIL|RESOURCE_FILE_NOT_FOUND");
                        break;
                }
            else PackCompleted?.Invoke("FAIL|FILE_NOT_FOUND");
        }
        public static void UnPack(string packedNovelFilename, string unpackDirectory)
        {
            if (!Directory.Exists(unpackDirectory)) Directory.CreateDirectory(unpackDirectory);
            switch (ZipStorer.Validate(packedNovelFilename, FileAccess.Read))
            {
                case ZipValidatingResult.OK:
                    var zip = ZipStorer.Open(packedNovelFilename, FileAccess.Read);
                    var files = zip.ReadCentralDir();
                    for (int i = 0; i < files.Count; i++)
                    {
                            CurrentState = (int)(((double)i / files.Count) * 100);
                        zip.ExtractFile(files[i], Path.Combine(unpackDirectory,  files[i].FilenameInZip));
                    }
                    PackCompleted?.Invoke("OK");
                    break;
                case ZipValidatingResult.FileNotExist:
                    PackCompleted?.Invoke("FAIL|FILE_NOT_FOUND");
                    break;
                case ZipValidatingResult.ZipNull:
                    PackCompleted?.Invoke("FAIL|ZIP_NULL");
                    break;
                case ZipValidatingResult.UnknownIOError:
                    PackCompleted?.Invoke("FAIL|UNKNOWN_IO_ERROR");
                    break;
            }
        }
    }
}
