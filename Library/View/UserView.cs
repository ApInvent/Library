using System;
using System.Collections.Generic;
using Library.Model;

namespace Library.View
{
    public class UserView : IView
    {
        private readonly List<User> _users;

        public UserView(List<User> users, string command)
        {
            _users = users;
            Command = command;
        }

        public string Command { get; set; }

        public void Draw()
        {
            Console.WriteLine("id".PadRight(3, ' ') + "name");
            Console.WriteLine("=".PadRight(20, '='));
            foreach (var user in _users)
            {
                Console.WriteLine(user.Id.ToString().PadRight(3, ' ') + user.Name.PadRight(10,' '));
                Console.WriteLine();
                Console.WriteLine("  Borrowed Books:");
                Console.WriteLine("  " + "id".PadRight(3, ' ') + "name".PadRight(10, ' ') + "borowing date");
                Console.WriteLine("  =".PadRight(20, '='));
                foreach (var borrowedBook in user.BorrowedBooks)
                {
                    Console.WriteLine("  " + 
                                      borrowedBook.Book.Id.ToString().PadRight(3, ' ') + 
                                      borrowedBook.Book.Name.PadRight(10, ' ') + 
                                      borrowedBook.BorrowingDate);
                }
            }
        }
    }
}