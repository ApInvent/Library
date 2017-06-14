using System.Collections.Generic;
using Library.Model;

namespace Library.App
{
    public interface IRepository
    {
        void AddBook(Book book);

        void AddUser(User user);

        void UpdateBook(Book book);

        void UpdateUser(User user);

        List<Book> GetBooks();

        List<User> GetUsers();
    }
}