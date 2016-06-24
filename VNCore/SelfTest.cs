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
            //var novel = new Novel.Novel
            //{
            //    Version = 0,
            //    Title = "TestNovel",
            //    Description = "Just another visual novel. Test one.",
            //    KonamiCode = "1324",
            //    Tags = { "ByDeveloper", "Test", "InDevelopment" },
            //    Icon = new Image { Mode = ImageStoreMode.Path, Path = "Resources/Images/Icon.png" },
            //    Logo = new Image { Mode = ImageStoreMode.Path, Path = "Resources/Images/Logo.png" }
            //};
            //novel.Add(new Slide
            //{
            //    Title = "First slide",
            //    Background = new Image { Mode = ImageStoreMode.Path, Path = "Resources/Images/1.png" },
            //    BackgroundSound = "Resources/Sounds/DW-OP_test.mp3",
            //    Labels = new[] { new TextLabel { Title = "Unknown voice", Text = "TestNovel. The beginning." } },
            //    Characters = new[] { new Character { ID = 0, Position = new Position { X = 20, Y = 1, Height = 20, Width = 20 } } }
            //});
            var directory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "Tested Novel");
            var filename = Path.Combine(directory, string.Format("TestNovel.{0}", EnvironmentVariables.NovelExtension));
            //novel.WriteFile(filename);
            //NovelPacker.Pack(filename, Path.Combine(directory, string.Format("TestNovel.Packed.{0}", EnvironmentVariables.PackedNovelExtension)));

            NovelPacker.UnPack(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), string.Format("TestNovel.Packed.{0}", EnvironmentVariables.PackedNovelExtension)), directory);
            var novel = Novel.Novel.ParseFile(filename);
            Console.WriteLine(novel);
        }
    }
}
