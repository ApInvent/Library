using Library.App;

namespace Library.Controller.Commands
{
    public class AddUserCommandProcessor : ICommandProcessor
    {
        private readonly Model.Library _library;
        private readonly IRepository _repository;

        public AddUserCommandProcessor(Model.Library library, IRepository repository)
        {
            _library = library;
            _repository = repository;
        }

        public bool CanProcess(string command)
        {
            return command.StartsWith("add user");
        }

        public string Description => "add user <user name>";
        public void Process(string command)
        {
            var userName = command.Substring(9);
            var newUser = _library.AddUser(userName);
            _repository.AddUser(newUser);
        }
    }
}