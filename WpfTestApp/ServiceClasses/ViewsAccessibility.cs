using System.Linq;
using System.Windows;
using Telerik.Windows.Controls;

namespace WpfTestApp.ServiceClasses
{
    public static class ViewsAccessibility
    {
        public static Window GetCorresponingWindow(ViewModelBase viewModel)
        {
            var windowAccessibility = new WindowAccessibility(viewModel);
            return windowAccessibility.CorrespondanteWindow;
        }

        private class WindowAccessibility
        {
            public Window CorrespondanteWindow { get; private set; }

            public WindowAccessibility(ViewModelBase viewModel)
            {
                var windows = Application.Current.Windows.OfType<Window>();
               // CorrespondanteWindow = (from window in windows
                    //where window.DataContext.Equals(viewModel)
                   // select window).First();
                foreach (var window in windows)
                {
                    if (window.DataContext != null)
                        if (window.DataContext.Equals(viewModel))
                        {
                            CorrespondanteWindow = window;
                            break;
                        }
                }

                if (CorrespondanteWindow == null)
                    CorrespondanteWindow = Application.Current.MainWindow;
            }
        }
    }
}
