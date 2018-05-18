using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;

namespace WpfTestApp.Model
{
    internal class Level : INotifyPropertyChanged
    {
        private string _content;
        private int _level;

        [DataMember]
        public string Str
        {
            get => _content;
            set
            {
                _content = value;
                OnPropertyChanged("Str");
            }
        }

        [DataMember]
        public int LevelNumber
        {
            get => _level;
            set
            {
                _level = value;
                OnPropertyChanged("LevelNumber");
            }
        }


        public Level(int level, string str)
        {
            LevelNumber = level;
            Str = str;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
