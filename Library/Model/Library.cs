using System;
using System.Collections.Generic;
using System.Linq;

namespace Library.Model
{
    public class Library
    {
        public event EventHandler LibraryChange;

        public Library(List<User> users, List<Book> books)
        {
            Books = books;
            Users = users;
            BadUsers = users
                .Where(u => u.BorrowedBooks.Any(bb => bb.BorrowingDate.AddMonths(1) > DateTime.Now))
                .ToList();
        }

        public List<Book> Books { get; set; }

        public List<User> Users { get; set; }

        public List<User> BadUsers { get; set; }

        public User AddUser(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException("Ivalid user name", nameof(name));
            }
            var newUser = new User {Id = Users.Count, Name = name, BorrowedBooks = new List<BorrowedBook>()};
            Users.Add(newUser);
            OnLibraryChange();
            return newUser;
        }

        public Book AddBook(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException("Ivalid book name", nameof(name));
            }
            var newBook = new Book {Id = Books.Count, Name = name};
            Books.Add(newBook);
            OnLibraryChange();
            return newBook;
        }

        public void BorrowBook(User user, Book book)
        {
            if (book.Borrowed)
            {
                throw new ArgumentException("Book already borrowed.");
            }

            if (user.BorrowedBooks.Count == 3)
            {
                throw new ArgumentException("User have maximum books (3)");
            }

            book.Borrowed = true;
            user.BorrowedBooks.Add(new BorrowedBook(book, DateTime.Now));

            OnLibraryChange();
        }

        public void ReturnBook(User user, Book book)
        {
            book.Borrowed = false;
            var borrowedBook = user.BorrowedBooks.SingleOrDefault(bb => bb.Book == book);
            if (borrowedBook == null)
            {
                throw new ArgumentException("This user does't have this book");
            }
            user.BorrowedBooks.Remove(borrowedBook);

            if (BadUsers.Contains(user))
            {
                var haveOverduedBook = user.BorrowedBooks.Any(bb => bb.BorrowingDate.AddMonths(1) > DateTime.Now);
                if (!haveOverduedBook)
                {
                    BadUsers.Remove(user);
                }
            }

            OnLibraryChange();
        }

        protected virtual void OnLibraryChange()
        {
            LibraryChange?.Invoke(this, EventArgs.Empty);
        }
    }
}