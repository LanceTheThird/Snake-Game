using System.Collections.ObjectModel;
using WpfTestApp.ViewModels;

namespace WpfTestApp.Interfaces
{
    internal interface IBlocksLoader
    {
        ObservableCollection<Block> LoadBlocks(int currentLevel);
    }
}
