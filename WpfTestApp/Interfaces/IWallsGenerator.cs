using System.Collections.ObjectModel;
using WpfTestApp.ViewModels;

namespace WpfTestApp.Interfaces
{
    internal interface IWallsGenerator
    {
        ObservableCollection<Block> Generate( int currentLevel);
    }
}
