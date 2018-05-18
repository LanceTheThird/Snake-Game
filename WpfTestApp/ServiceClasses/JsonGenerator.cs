using System.Collections.ObjectModel;
using WpfTestApp.Interfaces;
using WpfTestApp.ViewModels;

namespace WpfTestApp.ServiceClasses
{
    internal class JsonGenerator : IWallsGenerator
    {
        public ObservableCollection<Block> Generate(int currentLevel)
        {
            var manager = new IOManager();
            return manager.LoadBlocks(currentLevel);
        }

    }
}
        
            
    

