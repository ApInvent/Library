using System;
using System.Collections.Generic;
using System.Linq;

namespace Library.View
{
    public class LibraryView
    {
        private IView _currentView;
        private readonly IView _defaultView;
        private readonly List<IView> _views;

        public LibraryView(Model.Library library, List<IView> views, IView defaultView)
        {
            _currentView = defaultView;
            _defaultView = defaultView;
            library.LibraryChange += delegate { DrawCurentView(); };
            _views = views;
        }

        public void ProcessCommand(string command)
        {
            var view = _views.FirstOrDefault(v => v.Command == command);
            _currentView = view ?? _defaultView;
            DrawCurentView();
        }

        public void DrawCurentView()
        {
            Console.Clear();
            _currentView?.Draw();
        }
    }
}