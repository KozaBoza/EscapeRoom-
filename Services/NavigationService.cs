using System;
using System.Collections.Generic;
using System.Windows.Controls;

namespace EscapeRoom.Services
{
    public class NavigationService
    {
        private readonly Dictionary<string, Func<UserControl>> _pages = new Dictionary<string, Func<UserControl>>();
        private readonly Frame _mainFrame;

        public NavigationService(Frame mainFrame)
        {
            _mainFrame = mainFrame ?? throw new ArgumentNullException(nameof(mainFrame));
        }

        public void Register(string viewName, Func<UserControl> createView)
        {
            _pages[viewName] = createView;
        }

//nawigacja do widoku
        public bool NavigateTo(string viewName)
        {
            if (_pages.TryGetValue(viewName, out var createView))
            {
                _mainFrame.Content = createView();
                return true;
            }
            return false;
        }


        public bool GoBack()
        {
            if (_mainFrame.CanGoBack)
            {
                _mainFrame.GoBack();
                return true;
            }
            return false;
        }

        public bool GoForward()
        {
            if (_mainFrame.CanGoForward)
            {
                _mainFrame.GoForward();
                return true;
            }
            return false;
        }

//biezace widoki
        public object CurrentView => _mainFrame.Content;
    }
}