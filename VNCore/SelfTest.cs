using System;
using System.IO;
using VNCore.Extensions;
using VNCore.Novel;
using VNCore.Novel.Base;
using VNCore.Novel.Controls;

namespace VNCore
{
    class SelfTest
    {
        public static void Main()
        {
            var novel = new Novel.Novel
            {
                Version = 0,
                Title = "TestNovel",
                Description = "Just another visual novel. Test one.",
                KonamiCode = "1324",
                Tags = { "ByDeveloper", "Test", "InDevelopment" }
            };
            novel.Add(new Slide
            {
                Title = "First slide",
                Background=new Image { Mode = ImageStoreMode.Path, Path= "TestNovelResources/Images/1.png"},
                Labels = new[] { new TextLabel { Title = "Unknown voice", Text = "TestNovel. The beginning." } },
                Characters = new[] { new Character { ID = 0, Position = new Position { X = 20, Y = 1, Height = 20, Width = 20 } } }
            });
            var directory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            var filename = Path.Combine(directory, string.Format("TestNovel.{0}", EnvironmentVariables.NovelExtension));
            novel.WriteFile(filename);
            NovelPacker.Pack(filename, Path.Combine(directory, string.Format("TestNovel.Packed.{0}", EnvironmentVariables.PackedNovelExtension)));
            //var novel = Novel.Novel.ParseFile(filename);
            //Console.WriteLine(novel);
        }
    }
}
