using System;
using System.Collections.Generic;
using Library.Controller;
using Library.Controller.Commands;

namespace Library.View
{
    public class HelpView : IView
    {
        private readonly List<ICommandProcessor> _commandProcessors;
        private readonly List<IView> _views;

        public HelpView(List<ICommandProcessor> commandProcessors, List<IView> views, string command)
        {
            _commandProcessors = commandProcessors;
            _views = views;
            Command = command;
        }

        public string Command { get; set; }

        public void Draw()
        {
            Console.WriteLine("View commands:");
            _views.ForEach(v => Console.WriteLine("  " + v.Command));
            Console.WriteLine("Action commands:");
            _commandProcessors.ForEach(v => Console.WriteLine("  " + v.Description));
        }
    }
}