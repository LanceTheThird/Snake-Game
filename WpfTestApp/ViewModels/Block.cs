using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using WpfTestApp.Model;

namespace WpfTestApp.ViewModels
{
    public class Block : INotifyPropertyChanged
    {
        private readonly Link _link = new Link();


        public Block( string color, int left, int top, int view, ChainType chainType, int fontSize, double scale)
        {
            Color = color;
            Left = left;
            Top = top;
            View = view;
            ChainType = chainType;
            FontSize = fontSize;
            Scale = scale;
        }

        public Block()
        {

        }

        [DataMember]
        public string Color
        {
            get => _link.Color;
            set
            {
                _link.Color = value;
                OnPropertyChanged("Color");
            }
        }

        [DataMember]
        public double Scale
        {
            get => _link.Scale;
            set
            {
                _link.Scale = value;
                OnPropertyChanged("Scale");
            }
        }

        [DataMember]
        public int Left
        {
            get => _link.Left; 
            set
            {
                _link.Left = value;
                OnPropertyChanged("Left");
            }
        }

        [DataMember]
        public int FontSize
        {
            get => _link.FontSize;
            set
            {
                _link.FontSize = value;
                OnPropertyChanged("FontSize");
            }
        }

        [DataMember]
        public int Top
        {
            get => _link.Top; 
            set
            {
                _link.Top = value;
                OnPropertyChanged("Top");
            }
        }

        [DataMember]
        public int View
        {
            get => _link.View;
            set
            {
                _link.View = value;
                OnPropertyChanged("View");
            }
        }

        [DataMember]
        public ChainType ChainType
        {
            get => _link.ChainType;
            set
            {
                _link.ChainType = value;
                OnPropertyChanged("ChainType");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
