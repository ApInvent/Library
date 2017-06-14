using System;
using System.Collections.Generic;
using Library.Model;

namespace Library.View
{
    public class BookView : IView
    {
        private readonly List<Book> _books;

        public BookView(List<Book> books, string command)
        {
            _books = books;
            Command = command;
        }

        public string Command { get; set; }

        public void Draw()
        {
            Console.WriteLine("Id".PadRight(3, ' ') + "Name".PadRight(10, ' ') + "Borrowed");
            Console.WriteLine("".PadLeft(20, '='));
            foreach (var book in _books)
            {
                Console.Write(book.Id.ToString().PadRight(3, ' ') + book.Name.PadRight(10, ' '));
                if (book.Borrowed)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                }
                Console.WriteLine(book.Borrowed);
                Console.ForegroundColor = ConsoleColor.Gray;
            }
        }
    }
}