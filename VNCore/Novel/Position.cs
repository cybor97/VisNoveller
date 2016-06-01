using System;

namespace VNCore.Novel
{
    public struct Position
    {
        public sbyte X { get; set; }
        public sbyte Y { get; set; }
        public sbyte Width { get; set; }
        public sbyte Height { get; set; }
        public override string ToString()
        {
            return string.Format("{0} {1} {2} {3}", X, Y, Width, Height);
        }
        public static bool TryParse(string data, out Position result)
        {
            try
            {
                result = Parse(data);
                return true;
            }
            catch
            {
                result = new Position();
                return false;
            }
        }
        public static Position Parse(string data)
        {
            var dataArray = data.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            var result = new Position();
            result.X = sbyte.Parse(dataArray[0]);
            result.Y = sbyte.Parse(dataArray[1]);
            result.Width = sbyte.Parse(dataArray[2]);
            result.Height = sbyte.Parse(dataArray[3]);
            return result;
        }
    }
}
