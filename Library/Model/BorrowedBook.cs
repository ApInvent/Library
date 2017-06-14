using System;

namespace Library.Model
{
    public class BorrowedBook
    {
        public BorrowedBook(Book book, DateTime borrowingDate)
        {
            Book = book;
            BorrowingDate = borrowingDate;
        }

        public int Id { get; set; }

        public Book Book { get; }

        public DateTime BorrowingDate { get; }
    }
}