using System;
using System.Collections.Generic;
using System.Linq;
using Library.Controller.Commands;

namespace Library.Controller
{
    public class LibraryController
    {
        private readonly List<ICommandProcessor> _commandProcessors;

        public LibraryController(List<ICommandProcessor> commandProcessors)
        {
            _commandProcessors = commandProcessors;
        }

        public bool ProcessCommand(string command)
        {
            foreach (var commandProcessor in _commandProcessors)
            {
                if (!commandProcessor.CanProcess(command))
                {
                    continue;
                }
                commandProcessor.Process(command);
                return true;
            }

            return false;
        }

        public string GetHelp()
        {
            return string.Join(Environment.NewLine, _commandProcessors.Select(cp => cp.Description));
        }
    }
}