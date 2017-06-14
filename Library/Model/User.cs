using System.Collections.Generic;

namespace Library.Model
{
    public class User
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public List<BorrowedBook> BorrowedBooks { get; set; }
    }
}