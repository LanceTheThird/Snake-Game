using System.Windows;
using WpfTestApp.ViewModels;

namespace WpfTestApp
{
    public partial class MenuWindow : Window
    {
        public MenuWindow()
        {
            InitializeComponent();
            DataContext = new MenuViewModel();
        }
    }
}
