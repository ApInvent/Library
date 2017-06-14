using System;
using System.Linq;
using System.Text.RegularExpressions;
using Library.App;

namespace Library.Controller.Commands
{
    public class ReturnBookCommandProcessor : ICommandProcessor
    {
        private readonly Model.Library _library;
        private readonly IRepository _repository;
        private string _pattern = "return book uid:(\\d) bid:(\\d)";

        public ReturnBookCommandProcessor(Model.Library library, IRepository repository)
        {
            _library = library;
            _repository = repository;
        }

        public bool CanProcess(string command)
        {
            return Regex.IsMatch(command, _pattern);
        }

        public string Description => "return book uid:<user id> bid:<book id>";

        public void Process(string command)
        {
            var match = Regex.Match(command, _pattern);

            var userId = int.Parse(match.Groups[1].Value);
            var user = _library.Users.FirstOrDefault(u => u.Id == userId);
            if (user == null)
            {
                throw new ArgumentException("Invalid user id");
            }

            var bookId = int.Parse(match.Groups[2].Value);
            var book = _library.Books.SingleOrDefault(b => b.Id == bookId);
            if (book == null)
            {
                throw new ArgumentException("Invalid book id");
            }

            _library.ReturnBook(user, book);
            _repository.UpdateBook(book);
            _repository.UpdateUser(user);
        }
    }
}