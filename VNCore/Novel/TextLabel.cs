namespace VNCore.Novel
{
    public class TextLabel : ILabel
    {
        public int Timeout { get; set; }
        public int ClickRedirect { get; set; }
        public string Text { get; set; }
        public Position Position { get; set; }
    }
}
