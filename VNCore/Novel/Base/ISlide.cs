using System.Collections.Generic;

namespace VNCore.Novel
{
    public interface ISlide
    {
        int ID { get; set; }
        string Title { get; set; }
        object Background { get; set; }
        List<ILabel> Labels { get; set; }
    }
}