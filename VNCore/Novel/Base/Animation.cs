using System.Collections.Generic;

namespace VNCore.Novel.Base
{
    public interface IAnimation<AnimatedType>
    {
        AnimatedType From { get; set; }
        AnimatedType To { get; set; }
        /// <summary>
        /// In milliseconds
        /// </summary>
        int Duration { get; set; }
        bool AutoReverse { get; set; }
        bool RepeatForever { get; set; }
    }
}
