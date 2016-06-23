using System.Collections.Generic;

namespace VNCore.Novel.Base
{
    public interface ISlide
    {
        int ID { get; set; }
        string Title { get; set; }
        object Background { get; set; }
        IList<ILabel> Labels { get; set; }
        IList<string> GetResources();
    }
}