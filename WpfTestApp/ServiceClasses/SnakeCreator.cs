using System.Collections.ObjectModel;
using WpfTestApp.Model;
using WpfTestApp.ViewModels;

namespace WpfTestApp.ServiceClasses
{
    public static class SnakeCreator
    {
        public static ObservableCollection<Block> CreateBlocks(int currentLevel)
        {
            var blocks = new ObservableCollection<Block>();

            var block = new Block(Constants.Food, 8 * Constants.Step, 3 * Constants.Step, 0, ChainType.Food, currentLevel, 0.6);
            blocks.Add(block);
            block = new Block(Constants.Head, 15 * Constants.Step, Constants.Step, Constants.QuaterAngle, ChainType.Head, currentLevel, 0.6);
            blocks.Add(block);
            block = new Block(Constants.Body, 16 * Constants.Step, Constants.Step, Constants.QuaterAngle, ChainType.Body, currentLevel, 0.3);
            blocks.Add(block);
            block = new Block(Constants.Body, 17 * Constants.Step, Constants.Step, Constants.QuaterAngle, ChainType.Body, currentLevel, 0.3);
            blocks.Add(block);

            return blocks;
        }
    }
}
