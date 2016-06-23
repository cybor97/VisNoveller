using System.Collections.Generic;

namespace VNCore.Novel.Base
{
    public interface ICharacter
    {
        List<Image> Images { get; set; }
        Position Position { get; set; }
        IList<string> GetResources();
    }
}
