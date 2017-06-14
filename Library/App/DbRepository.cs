using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using Library.Model;

namespace Library.App
{
    public class DbRepository : IRepository
    {
        private readonly SQLiteConnection _connection;
        private readonly Dictionary<int, Book> _bookCache = new Dictionary<int, Book>();
        public DbRepository(string fileName)
        {
            _connection = new SQLiteConnection("Data Source=.\\" + fileName);
            Init(fileName);
        }

        private void Init(string fileName)
        {
            if (File.Exists(fileName))
            {
                _connection.Open();
                return;
            }

            SQLiteConnection.CreateFile(fileName);
            _connection.Open();
            using (var cmd = _connection.CreateCommand())
            {
                cmd.CommandText = @"create table if not exists [Book](
                                [Id] integer not null primary key,
                                [Name] nvarchar(2048) not null,
                                [Borrowed] bit not null default 0 
                            );";
                cmd.ExecuteNonQuery();

                cmd.CommandText = @"create table if not exists [User](
                                [Id] integer not null primary key,
                                [Name] nvarchar(2048) not null
                            );";
                cmd.ExecuteNonQuery();

                cmd.CommandText = @"create table if not exists [BorrowedBook](
                                [Id] integer not null primary key,
                                [UserId] integer not null,
                                [BookId] integer not null,
                                [BorrowingDate] datetime not null
                            );";
                cmd.ExecuteNonQuery();
            }
        }

        public void AddBook(Book book)
        {
            var cmd = _connection.CreateCommand();
            cmd.CommandText = "insert into Book (id, name, borrowed) values(:id, :name, :borrowed)";
            cmd.Parameters.AddWithValue("id", book.Id);
            cmd.Parameters.AddWithValue("name", book.Name);
            cmd.Parameters.AddWithValue("borrowed", book.Borrowed);
            cmd.ExecuteNonQuery();
        }

        public void AddUser(User user)
        {
            var cmd = _connection.CreateCommand();
            cmd.CommandText = "insert into User (id, name) values(:id, :name)";
            cmd.Parameters.AddWithValue("id", user.Id);
            cmd.Parameters.AddWithValue("name", user.Name);
            cmd.ExecuteNonQuery();
        }

        public void UpdateBook(Book book)
        {
            var cmd = _connection.CreateCommand();
            cmd.CommandText = "update Book set borrowed = :borrowed where id = :id";
            cmd.Parameters.AddWithValue("id", book.Id);
            cmd.Parameters.AddWithValue("borrowed", book.Borrowed);
            cmd.ExecuteNonQuery();
        }

        public void UpdateUser(User user)
        {
            var cmd = _connection.CreateCommand();
            cmd.CommandText = "delete from BorrowedBook where UserId = :userId";
            cmd.Parameters.AddWithValue("userId", user.Id);
            cmd.ExecuteNonQuery();

            foreach (var borrowedBook in user.BorrowedBooks)
            {
                cmd.CommandText = "insert into BorrowedBook (UserId, BookId, BorrowingDate) values(:userId, :bookId, :borrowingDate)";
                cmd.Parameters.AddWithValue("userId", user.Id);
                cmd.Parameters.AddWithValue("bookId", borrowedBook.Book.Id);
                cmd.Parameters.AddWithValue("borrowingDate", borrowedBook.BorrowingDate);
                cmd.ExecuteNonQuery();
            }
        }

        public List<Book> GetBooks()
        {
            var bookList = new List<Book>();

            var cmd = _connection.CreateCommand();
            cmd.CommandText = "select id, name, borrowed from Book";
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    var bookId = int.Parse(reader["id"].ToString());
                    if (_bookCache.ContainsKey(bookId))
                    {
                        bookList.Add(_bookCache[bookId]);
                        continue;
                    }

                    var book = new Book
                    {
                        Id = bookId,
                        Name = (string) reader["name"],
                        Borrowed = (bool) reader["borrowed"]
                    };
                    _bookCache.Add(bookId, book);
                    bookList.Add(book);
                }
            }

            return bookList;
        }

        public List<User> GetUsers()
        {
            var userList = new List<User>();

            var cmd = _connection.CreateCommand();
            cmd.CommandText = "select id, name from User";
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    var userId = int.Parse(reader["id"].ToString());
                    var user = new User
                    {
                        Id = userId,
                        Name = (string) reader["name"],
                        BorrowedBooks = new List<BorrowedBook>()
                    };
                    userList.Add(user);
                }
            }
            
            foreach (var user in userList)
            {
                cmd.CommandText = "select bb.UserId, bb.BookId, bb.BorrowingDate, b.Name, b.Borrowed " +
                                  "from BorrowedBook bb " +
                                  "join Book b on b.Id = bb.BookId " +
                                  "where UserId = :userId";
                cmd.Parameters.AddWithValue("userId", user.Id);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Book book;
                        var bookId = int.Parse(reader["bookId"].ToString());
                        if (_bookCache.ContainsKey(bookId))
                        {
                            book = _bookCache[bookId];
                        }
                        else
                        {
                            book = new Book
                            {
                                Id = bookId,
                                Name = (string)reader["name"],
                                Borrowed = bool.Parse(reader["borrowed"].ToString())
                            };
                            _bookCache.Add(bookId, book);
                        }
                        var borrowedBook = new BorrowedBook(book, DateTime.Parse(reader["borrowingDate"].ToString()));
                        user.BorrowedBooks.Add(borrowedBook);
                    }
                }
            }

            return userList;
        }
    }
}