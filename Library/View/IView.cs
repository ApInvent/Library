namespace Library.View
{
    public interface IView
    {
        string Command { get; }
        void Draw();
    }
}