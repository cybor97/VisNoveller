using System.Collections.Generic;
using System.Drawing;

namespace VNCore.Novel
{
    public class Novel : List<ISlide>
    {
        public int Version { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public Icon Icon { get; set; }
        public Image Logo { get; set; }
        public List<string> Tags { get; set; }
        public Novel()
        {
            Tags = new List<string>();
        }
    }
}
