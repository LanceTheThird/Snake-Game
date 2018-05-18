using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Threading;
using WpfTestApp.Model;
using WpfTestApp.ServiceClasses;
using Direction = WpfTestApp.Model.Direction;

namespace WpfTestApp.ViewModels
{
    public class Timer :  INotifyPropertyChanged
    {
        private readonly DispatcherTimer _timer = new DispatcherTimer();
        private int _width;
        private int _height;
        private int _tickIterator;
        private int _score;
        
        private bool _canTurn = true;
        public bool IsRepeat;
        // Other Manager uses outer files
        //private readonly IOManager _manager = new IOManager();
        private readonly DBManager _manager = new DBManager();       
        private ObservableCollection<TimeTickData> _tickDatas = new ObservableCollection<TimeTickData>();
        private Direction _direction;
        
        public ObservableCollection<Block> Walls { get; set; }

        public Direction Direction
        {
            get => _direction;
            set
            {
                if ((value == Direction.Right && _direction == Direction.Left) ||
                    (value == Direction.Left && _direction == Direction.Right) ||
                    (value == Direction.Up && _direction == Direction.Down) ||
                    (value == Direction.Down && _direction == Direction.Up)) return;
                if (!_canTurn) return;
                if (IsRepeat) return;
                _direction = value;
                _canTurn = false;
            }
        }

        public void Start()
        {                      
            _timer.Interval = TimeSpan.FromMilliseconds(Constants.Timestep);
            _timer.Tick += timer_Tick;
            _timer.Start();
        }

        public void Stop()
        {
            _timer.Stop();
        }
        private void timer_Tick(object sender, EventArgs e)
        {
            _canTurn = true;
            if (IsRepeat)
            {
                if (_tickIterator == 0)
                _tickDatas = _manager.LoadPath(CurrentLevel);               
            }

            var tickData = new TimeTickData();
            for (var i = Blocks.Count -1; i > 1; i--)
            {
                Blocks[i].Left = Blocks[i - 1].Left;
                Blocks[i].Top = Blocks[i - 1].Top;
                Blocks[i].View = Blocks[i - 1].View;
            }

            if (!IsRepeat)
            {
                Blocks[1] = MoveHead(Blocks[1]);
                tickData.Direction = _direction;
                tickData.Height = Height;
                tickData.Width = Width;
            }
            else
            {
                try
                {
                    _direction = _tickDatas[_tickIterator].Direction;
                    Blocks[1] = MoveHead(Blocks[1]);
                    Height = _tickDatas[_tickIterator].Height;
                    Width = _tickDatas[_tickIterator].Width;
                }
                catch (ArgumentOutOfRangeException)
                {
                    var result = MessageBox.Show(Constants.BourderTouchError);
                    if (result == MessageBoxResult.OK)
                    {
                        Application.Current.Shutdown();
                    }
                    return;
                }
            }

            if (DoNotSelfCross(Blocks[1], Blocks, false) || DoNotSelfCross(Blocks[1], Walls, true))
            {
               _timer.Stop();
                var result = MessageBox.Show(Constants.GameOver, Constants.Confirmation, MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (!IsRepeat)
                {
                    tickData.FoodLeft = Blocks[0].Left;
                    tickData.FoodTop = Blocks[0].Top;

                    _tickDatas.Add(tickData);
                }

                 _manager.UnloadPath(_tickDatas, CurrentLevel);               
                Score--;
                new DBBestScoreLoader().LoadBestScore(new BestScore(System.Security.Principal.WindowsIdentity.GetCurrent().Name, CurrentLevel, Score));
                if (result == MessageBoxResult.Yes)
                {
                    Application.Current.Shutdown();
                }               
                return;
            }

            //Generate food 

            var leftDiff = Math.Abs(Blocks[1].Left - Blocks[0].Left);
            var topDiff = Math.Abs(Blocks[1].Top - Blocks[0].Top);

            if (!IsRepeat)
            {
                tickData.FoodLeft = Blocks[0].Left;
                tickData.FoodTop = Blocks[0].Top;
                _tickDatas.Add(tickData);
            }
            else
            {
                try
                {
                    _tickIterator++;
                    Blocks[0].Left = _tickDatas[_tickIterator].FoodLeft;
                    Blocks[0].Top = _tickDatas[_tickIterator].FoodTop;
                    
                }
                catch (ArgumentOutOfRangeException)
                {
                    _tickIterator--;
                    Blocks[0].Left = _tickDatas[_tickIterator].FoodLeft;
                    Blocks[0].Top = _tickDatas[_tickIterator].FoodTop;
                    _tickIterator++;
                }


            }

            if ((leftDiff >= Constants.Step) || (topDiff >= Constants.Step))
                return;

            if (!IsRepeat)
            {
                GenerateFood(Constants.FoodNumber);
            }

            var block = new Block(Blocks[Blocks.Count - 1].Color, Blocks[Blocks.Count - 1].Left,
                    Blocks[Blocks.Count - 1].Top, Blocks[Blocks.Count - 1].View,
                    Blocks[Blocks.Count - 1].ChainType, Blocks[Blocks.Count - 1].FontSize,
                    Blocks[Blocks.Count - 1].Scale);
                Blocks.Add(block);
            
        }

        public void GenerateFood(int foodNumber)
        {
            var iLeft = Width / Constants.Step;
            var iTop = Height / Constants.Step;
            Score++;
            do
            {
                do
                {
                    Blocks[foodNumber].Left = Constants.Random.Next(0, iLeft) * Constants.Step;
                    Blocks[foodNumber].Top = Constants.Random.Next(0, iTop) * Constants.Step;
                } while (DoNotSelfCross(Blocks[foodNumber], Blocks, false));
            } while (DoNotSelfCross(Blocks[foodNumber], Walls, true));
        }

        private static bool DoNotSelfCross(Block currentBlock, ObservableCollection<Block> blocks, bool isWallsCheck)
        {
            foreach (var block in blocks)
            {
                var leftBase = Math.Abs(block.Left - currentBlock.Left);
                var topBase = Math.Abs(block.Top - currentBlock.Top);
                var step = Constants.Step / 2;


                if (isWallsCheck)
                {
                    if ((leftBase < step) && (topBase < step))
                    {
                        return true;
                    }
                }
                else
                {
                    if ((block != blocks[0]) && (block != blocks[1]) && (leftBase < step) && (topBase < step))
                    {
                        return true;
                    }
                }

            }
            return false;
        }

        private Block MoveHead(Block block)
        {
            switch (Direction)
            {
                case Direction.Down:
                    block.Top += Constants.Step;
                    if (block.Top > Height)
                        block.Top = 0;
                    block.View = Constants.QuaterAngle * 0;
                    return block;
                case Direction.Up:
                    block.Top -= Constants.Step;
                    if (block.Top < 0)
                        block.Top = Height - Constants.Step;
                    block.View = Constants.QuaterAngle * 2;
                    return block;
                case Direction.Right:
                    block.Left += Constants.Step;
                    if (block.Left > Width)
                        block.Left = 0;
                    block.View = Constants.QuaterAngle * 3;
                    return block;
                default:
                    block.Left -= Constants.Step;
                    if (block.Left < 0)
                        block.Left = Width - Constants.Step;
                    block.View = Constants.QuaterAngle * 1;
                    return block;
            }                           
        }

        public ObservableCollection<Block> Blocks { get; set; }


        public int Width
        {
            get => _width;
            set
            {
                _width = value;
                OnPropertyChanged("Width");

            }
        }

        public int Score
        {
            get => _score;
            set
            {
                _score = value;                
                OnPropertyChanged("Score");

            }
        }


        public int Height
        {
            get => _height;
            set
            {                
                _height = value;
                OnPropertyChanged("Height");
            }
        }

        public int CurrentLevel { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

    }
}
