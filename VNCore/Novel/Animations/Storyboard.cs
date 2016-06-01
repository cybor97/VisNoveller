using System.Collections.Generic;
using VNCore.Novel.Base;

namespace VNCore.Novel.Animations
{
    public class Storyboard<AnimatedType> : List<IAnimation<AnimatedType>>
    {
        /// <summary>
        /// In milliseconds
        /// </summary>
        int Duration { get; set; }
        bool AutoReverse { get; set; }
        bool RepeatForever { get; set; }
    }
}
