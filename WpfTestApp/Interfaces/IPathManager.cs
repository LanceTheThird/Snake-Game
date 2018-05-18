using System.Collections.ObjectModel;
using WpfTestApp.ServiceClasses;

namespace WpfTestApp.Interfaces
{
    internal interface IPathManager
    {
        void UnloadPath(ObservableCollection<TimeTickData> tickDatas, int currentLevel);
        ObservableCollection<TimeTickData> LoadPath(int currentLevel);
        bool IsThereAPath(int currentLevel);
    }
}
