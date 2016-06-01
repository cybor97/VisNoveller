using VNCore.Novel.Base;

namespace VNCore.Novel.Animations
{
    public class PositionAnimation : IAnimation<Position>
    {
        public Position From { get; set; }

        public Position To { get; set; }

        public int Duration { get; set; }

        public bool AutoReverse { get; set; }

        public bool RepeatForever { get; set; }

    }
}
