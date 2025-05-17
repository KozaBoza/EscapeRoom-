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

        /// <summary>
        /// Rejestracja widoku pod określoną nazwą
        /// </summary>
        public void Register(string viewName, Func<UserControl> createView)
        {
            _pages[viewName] = createView;
        }

        /// <summary>
        /// Nawigacja do określonego widoku
        /// </summary>
        public bool NavigateTo(string viewName)
        {
            if (_pages.TryGetValue(viewName, out var createView))
            {
                _mainFrame.Content = createView();
                return true;
            }
            return false;
        }

        /// <summary>
        /// Nawigacja wstecz
        /// </summary>
        public bool GoBack()
        {
            if (_mainFrame.CanGoBack)
            {
                _mainFrame.GoBack();
                return true;
            }
            return false;
        }

        /// <summary>
        /// Nawigacja do przodu
        /// </summary>
        public bool GoForward()
        {
            if (_mainFrame.CanGoForward)
            {
                _mainFrame.GoForward();
                return true;
            }
            return false;
        }

        /// <summary>
        /// Bieżąca strona/widok
        /// </summary>
        public object CurrentView => _mainFrame.Content;
    }
}