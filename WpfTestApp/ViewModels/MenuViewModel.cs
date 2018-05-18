using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Telerik.Windows.Controls;
using WpfTestApp.Model;
using WpfTestApp.ServiceClasses;

namespace WpfTestApp.ViewModels
{
    internal class MenuViewModel : ViewModelBase, INotifyPropertyChanged
    {
        private RelayCommand _easyCommand, _level1Command, _repeatCommand, _backCommand;
        private string _easyDescr;
        private string _currentLevelTitle;
        private string _listBack, _detailsBack;
        private string _advancedPictureFormat;
        private int _switchView;
        private int _score;
        private int _currentLevel;
        private bool _advancedFormat;
        private string _scoreString;
        private ObservableCollection<Level> _levels;
        private ObservableCollection<Block> _walls;
        private ObservableCollection<Block> _greatWalls;
        private readonly IOManager _manager = new IOManager();
        private readonly DBManager _DBmanager = new DBManager();


        public MenuViewModel()
        {
            Description = _manager.LoadDescription();

            SwitchView = 0;
            ListBack = _manager.GetListBack();
            DetaiilsBack = _manager.GetDetailsBack();
            Title = _manager.LoadTitle();
            

        }

        private static ObservableCollection<Level> CreateLevels()
        {
            var levels = new ObservableCollection<Level>();
            for (var i = 1; i <= Constants.LevelsCount; i++)
            {
                levels.Add(new Level(i, $"Level {i}"));
            }
            return levels;
            
        }

        public ObservableCollection<Block> Walls
        {
            get
            {
                if (_walls != null) return _walls;
                _walls = CreateWalls();
                return _walls;
            }
            set => _walls = value;
        }



        private ObservableCollection<Block> CreateWalls()
        {           
            var walls =  _manager.LoadBlocks(CurrentLevel);
            _greatWalls = GreatWallsCreation(walls);
            foreach (var element in walls)
            {
                element.Left =element.Left / Constants.Scale;
                element.Top =element.Top / Constants.Scale;
            }

            return walls;
        }

        private static ObservableCollection<Block> GreatWallsCreation(IEnumerable<Block> walls)
        {
            var greatWalls = new ObservableCollection<Block>();
            foreach (var item in walls)
            {
                var block = new Block(item.Color, item.Left, item.Top, item.View, item.ChainType, item.FontSize, item.Scale);
                greatWalls.Add(block);
            }

            return greatWalls;
        }

        public RelayCommand LevelCommand => _level1Command ??
                                            (_level1Command = new RelayCommand(ExecuteAttachmentChecked));

        private void ExecuteAttachmentChecked(object param)
        {            
            CurrentLevel = (int)param;
            SwitchView = 1;
            EasyDescr = Description[CurrentLevel-1];
            Score = new DBBestScoreLoader().UnloadBestScore(_currentLevel).Score;
            AdvancedFormat = _DBmanager.IsThereAPath(CurrentLevel);
            AdvancedPictureFormat = _manager.GetShot(CurrentLevel);
        }

        public RelayCommand BackCommand
        {
            get
            {
                return _backCommand ??
                       (_backCommand = new RelayCommand(obj => { SwitchView = 0;
                           Walls = null;
                       }));
            }
        }


        public RelayCommand EasyCommand
        {
            get
            {
                return _easyCommand ??
                       (_easyCommand = new RelayCommand(obj => {
                           MainWindow mainWindow = new MainWindow
                           {
                               DataContext = new ViewModel(Constants.Easy, _greatWalls, CurrentLevel, false)
                           };
                           mainWindow.Show();
                       }));
            }
        }

        public RelayCommand RepeatCommand
        {
            get
            {
                return _repeatCommand ??
                       (_repeatCommand = new RelayCommand(obj => {
                           var mainWindow = new MainWindow
                           {
                               DataContext = new ViewModel(Constants.Easy, _greatWalls, CurrentLevel, true)
                           };
                           mainWindow.Show();

                       }));
            }
        }


        public ObservableCollection<string> Description { get; }

        public string EasyDescr
        {
            get => _easyDescr;
            set
            {
                _easyDescr = value;
                OnPropertyChanged("EasyDescr");
            }
        }

        public ObservableCollection<Level> Levels
        {
            get
            {
                if (_levels != null) return _levels;
                _levels = CreateLevels();
                return _levels;
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

        public int CurrentLevel
        {
            get => _currentLevel;
            set
            {
                _currentLevel = value;
                CurrentLevelTitle = $"Level {value}";
                OnPropertyChanged("CurrentLevel");
            }
        }

        public string CurrentLevelTitle
        {
            get => _currentLevelTitle;
            set
            {
                _currentLevelTitle = value;               
                OnPropertyChanged("CurrentLevelTitle");
            }
        }

        public int Score
        {
            get => _score;
            set
            {
                _score = value;
                
                ScoreString = String.Format(Constants.RecordString, value);
                OnPropertyChanged("Score");

            }
        }

        public string ScoreString
        {
            get => _scoreString;
            set
            {
                _scoreString = value;
                
                OnPropertyChanged("ScoreString");

            }
        }

        public string Title { get; set; }


        public bool AdvancedFormat
        {
            get => _advancedFormat;
            set
            {
                _advancedFormat = value;
                OnPropertyChanged("AdvancedFormat");
            }
        }

        public string AdvancedPictureFormat
        {
            get => _advancedPictureFormat;

            set
            {
                _advancedPictureFormat = value;
                OnPropertyChanged("AdvancedPictureFormat");
            }
        }

        public string ListBack
        {
            get => _listBack;

            set
            {
                _listBack = value;
                OnPropertyChanged("ListBack");
            }
        }

        public string DetaiilsBack
        {
            get => _detailsBack;

            set
            {
                _detailsBack = value;
                OnPropertyChanged("DetaiilsBack");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

    }
}

