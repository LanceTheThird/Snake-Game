using System.Collections.ObjectModel;

namespace WpfTestApp.Interfaces
{
    internal interface IGuiLoader
    {
        ObservableCollection<string> LoadDescription();
        string GetBack(int currentLevel);
        string GetListBack();
        string GetDetailsBack();
        string LoadTitle();
    }
}
