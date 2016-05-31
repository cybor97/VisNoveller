using System.Collections.Generic;

namespace VNCore.Novel.Base
{
    public interface ICharacter
    {
        List<object> Images { get; set; }
        Position Position { get; set; }
    }
}
