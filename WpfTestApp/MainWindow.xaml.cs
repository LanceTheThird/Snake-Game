using System.ComponentModel;
using System.Windows;

namespace WpfTestApp
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            
            DataContext = new ViewModel();
            Closing += Stop;

        }

        private void Stop(object sender, CancelEventArgs e)
        {
            if(DataContext is ViewModel viewModel)
                viewModel.StopTimer();       
        }
    }
}
