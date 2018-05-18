using System.Collections.ObjectModel;
using Telerik.Windows.Controls;
using WpfTestApp.ViewModels;
using WpfTestApp.Model;
using Direction = WpfTestApp.Model.Direction;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using WpfTestApp.Interfaces;
using WpfTestApp.ServiceClasses;

namespace WpfTestApp
{
    internal class ViewModel : ViewModelBase,  INotifyPropertyChanged
    {
        private ObservableCollection<Block> _blocks;
        private ObservableCollection<Block> _walls;
        private RelayCommand _downCommand, _upCommand, _leftCommand, _rightCommand, _shotCommand;
        private readonly Timer _timer = new Timer();
        private int _height = Constants.Height;
        private int _width = Constants.Width;
        private int _windowHeight, _windowWidth;
        private int _switchView;
        private readonly int _difficulty;
        private int _currentLevel;
        private readonly IOManager _manager = new IOManager();
        public Direction Direct { get; set; }
        private string _backImage;
        private string _bodyIcon;

        public ViewModel(int difficulty, ObservableCollection<Block> walls, int currentLevel, bool isRepeat)
        {
            _difficulty = difficulty;
            _walls = walls;
            CurrentLevel = currentLevel;
            IsRepeat = isRepeat;
            BackImage = _manager.GetBack(currentLevel);
            //BodyIcon = _manager.GetBodyIcon(CurrentLevel);
        }

        public ViewModel()
        {
            _difficulty = Constants.Easy;
        }

        public ObservableCollection<Block> Walls
        {
            get
            {
                if (_walls != null) return _walls;
                _walls = CreateWalls(false);              
                return _walls;
            }
        }

        public ObservableCollection<Block> Blocks
        {
            get
            {
                if (_blocks != null) return _blocks;
                _blocks = SnakeCreator.CreateBlocks(CurrentLevel);
                StartTimer();
                return _blocks;
            }
        }

        private void StartTimer()
        {
            _timer.Direction = Direct;
            _timer.Blocks = _blocks;
            _timer.Height = Height;
            _timer.Width = Width;
            _timer.Walls = Walls;
            _timer.IsRepeat = IsRepeat;
            _timer.CurrentLevel = CurrentLevel;
            _timer.GenerateFood(Constants.FoodNumber);

            _timer.Start();
        }

        public void StopTimer()
        {
            _timer.Stop();
        }

        private ObservableCollection<Block> CreateWalls(bool import)
        {
            IWallsGenerator generator;
            if (!import)
            {
                generator = new LevelGenerator(Height, Width, _difficulty);
            }
            else
            {
                generator = new JsonGenerator();
            }          
            return generator.Generate(CurrentLevel);
        }


        public bool IsRepeat { get; set; }


        public RelayCommand DownCommand
        {
            get
            {
                return _downCommand ??
                       (_downCommand = new RelayCommand(obj => { _timer.Direction = Direction.Down; }));
            }
        }

        public RelayCommand UpCommand
        {
            get
            {
                return _upCommand ??
                       (_upCommand = new RelayCommand(obj => { _timer.Direction = Direction.Up; }));
            }
        }

        public RelayCommand LeftCommand
        {
            get
            {
                return _leftCommand ??
                       (_leftCommand = new RelayCommand(obj => { _timer.Direction = Direction.Left; }));
            }
        }

        public RelayCommand RightCommand
        {
            get
            {
                return _rightCommand ??
                       (_rightCommand = new RelayCommand(obj => { _timer.Direction = Direction.Right; }));
            }
        }

        public RelayCommand ShotCommand
        {
            get
            {
                return _shotCommand ??
                       (_shotCommand = new RelayCommand(obj => WindowShot()));
            }
        }

        private void WindowShot()
        {
            Util.SaveWindow(ViewsAccessibility.GetCorresponingWindow(this), 96, _manager.GetShot(CurrentLevel));
        }

        public int Width
        {
            get
            {
                if (_width == 0)
                    _width = Constants.Width;
                return _width;
            }
            set
            {
                while (value % Constants.Step != 0)
                    value--;
                _width = value;
                _windowWidth = value + (int)(Constants.Step / 2);
                _timer.Width = value;
                OnPropertyChanged("Width");
            }
        }

        public int WindowWidth
        {
            get
            {
                if (_windowWidth == 0)
                    _windowWidth = Constants.Width + (int)(Constants.Step/2);
                return _windowWidth;
            }
            set
            {
                while (value % Constants.Step != 0)
                    value--;
                _windowWidth = value;
                Width = value - (int)(Constants.Step / 2);
                OnPropertyChanged("WindowWidth");
            }
        }

        public int Height
        {
            get
            {
                if (_height == 0)
                    _height = Constants.Height;
                return _height;
            }
            set
            {
                while (value % Constants.Step != 0)
                    value--;
                _height = value;
                _windowHeight = value + Constants.Step;
                _timer.Height = value;
                OnPropertyChanged("Height");
            }
        }

        public int WindowHeight
        {
            get
            {
                if (_windowHeight == 0)
                    _windowHeight = Constants.Height + Constants.Step;
                return _windowHeight;
            }
            set
            {
                while (value % Constants.Step != 0)
                    value--;
                _windowHeight = value;
                Height = value - Constants.Step;
                OnPropertyChanged("WindowHeight");
            }
        }

        public int SwitchView
        {
            get => _switchView;

            set
            {
                _switchView = value;
                OnPropertyChanged("SwitchView");
            }
        }

        public string BodyIcon
        {
            get => _bodyIcon;

            set
            {
                _bodyIcon = value;
                OnPropertyChanged("BodyIcon");
            }
        }

        public int CurrentLevel
        {
            get => _currentLevel;

            set
            {
                _currentLevel = value;
                OnPropertyChanged("CurrentLevel");
            }
        }

        public string BackImage
        {
            get => _backImage;

            set
            {
                _backImage = value;
                OnPropertyChanged("BackImage");
            }
        }
      

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

    }
}
