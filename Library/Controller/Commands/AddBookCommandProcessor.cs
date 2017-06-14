using Library.App;

namespace Library.Controller.Commands
{
    public class AddBookCommandProcessor : ICommandProcessor
    {
        private readonly Model.Library _library;
        private readonly IRepository _repository;

        public AddBookCommandProcessor(Model.Library library, IRepository repository)
        {
            _library = library;
            _repository = repository;
        }

        public bool CanProcess(string command)
        {
            return command.StartsWith("add book");
        }

        public string Description => "add book <book name>";

        public void Process(string command)
        {
            var bookName = command.Substring(9);
            var newBook = _library.AddBook(bookName);
            _repository.AddBook(newBook);
        }
    }
}