using System;
using System.Collections.Generic;
using Library.App;
using Library.Controller;
using Library.Controller.Commands;
using Library.View;

namespace Library
{
    class Program
    {
        private static LibraryController _libraryController;
        private static LibraryView _libraryView;

        static void Main()
        {
            var repository = SelectRepository();
            Bootstrap(repository);

            while (true)
            {
                var command = Console.ReadLine();
                if (command == "exit")
                {
                    break;
                }

                try
                {
                    if (_libraryController.ProcessCommand(command))
                    {
                        continue;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    continue;
                }

                _libraryView.ProcessCommand(command);
            }
        }

        static IRepository SelectRepository()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Select source:");
                Console.WriteLine("1 - File");
                Console.WriteLine("2 - DataBase");
                var command = Console.ReadLine();
                int options;
                if (int.TryParse(command, out options))
                {
                    if (options == 1)
                    {
                        return new FileRepository("db.txt");
                    }
                    if (options == 2)
                    {
                        return new DbRepository("db.sql");
                    }
                }
            }
        }

        static void Bootstrap(IRepository repository)
        {
            //setup Library
            var library = new Model.Library(repository.GetUsers(), repository.GetBooks()) ;

            //setup controller
            var commandProcessors = new List<ICommandProcessor>
            {
                new AddBookCommandProcessor(library, repository),
                new AddUserCommandProcessor(library, repository),
                new BorrowBookCommandProcessor(library, repository),
                new ReturnBookCommandProcessor(library, repository)
            };
            _libraryController = new LibraryController(commandProcessors);

            //setup view
            var views = new List<IView>();
            var helpView = new HelpView(commandProcessors, views, "show help");
            views.AddRange(
                new IView[]{
                    new BookView(library.Books, "show books"),
                    new UserView(library.Users, "show users"),
                    new UserView(library.BadUsers, "show bad users"),
                    helpView
            });
            _libraryView = new LibraryView(library, views, helpView);
        }
    }
}
