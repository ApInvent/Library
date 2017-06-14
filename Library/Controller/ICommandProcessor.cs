namespace Library.Controller
{
    public interface ICommandProcessor
    {
        bool CanProcess(string command);
        string Description { get; }
        void Process(string command);
    }
}