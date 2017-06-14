using System.Collections.Generic;
using System.IO;
using Library.Model;
using Newtonsoft.Json;

namespace Library.App
{
    public class FileRepository : IRepository
    {
        private readonly string _fileName;
        private Data _data;

        public FileRepository(string fileName)
        {
            _fileName = fileName;
            Load();
        }

        public void AddBook(Book book)
        {
            Save();
        }

        public void AddUser(User user)
        {
            Save();
        }

        public void UpdateBook(Book book)
        {
            Save();
        }

        public void UpdateUser(User user)
        {
            Save();
        }

        public List<Book> GetBooks()
        {
            return _data.Books;
        }

        public List<User> GetUsers()
        {
            return _data.Users;
        }

        private void Load()
        {
            if (File.Exists(_fileName))
            {
                var content = File.ReadAllText(_fileName);
                _data = JsonConvert.DeserializeObject<Data>(content, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects });
            }
            else
            {
                _data = new Data { Books = new List<Book>(), Users = new List<User>() };
            }
        }

        private void Save()
        {
            File.Delete(_fileName);
            File.AppendAllText(_fileName, JsonConvert.SerializeObject(_data, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects }));
        }

        private class Data
        {
            public List<Book> Books { get; set; }

            public List<User> Users { get; set; }
        }
    }
}