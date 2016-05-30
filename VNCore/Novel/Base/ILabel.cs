namespace VNCore.Novel.Base
{
    public interface ILabel
    {
        int Timeout { get; set; }
        string Text { get; set; }
        Position Position { get; set; }
    }
}
