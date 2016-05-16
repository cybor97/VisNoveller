using System.Collections.Generic;

namespace VNCore.Novel
{
    public class Slide : ISlide
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public object Background { get; set; }
        public List<ILabel> Labels { get; set; }
        public Slide()
        {
            Labels = new List<ILabel>();
        }
    }
}