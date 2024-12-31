using SlideFun.Core;
using SlideFun.Core.Helper;
using SlideFun.MVVM.Models;
using SlideFun.MVVM.Views.Pages;
using SlideFun.Stores;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace SlideFun.MVVM.ViewModels
{

    public class GameRoundViewModel:ObservableObject
    {
        public Leaderboard leaderboardserivce = new Leaderboard();
        private ObservableObject? _currentPage;
        public ObservableObject? CurrentPage
        {
            get => _currentPage;
            set
            {
                if (_currentPage != value)
                {
                    _currentPage = value;
                    EventAggregator.GetEvent<PubSubEvent<ObservableObject>>().Publish(CurrentPage);
                }
            }
        }
        private readonly ImageProcessingService _imageProcessingService;

        private ObservableCollection<CusPieceViewModel> imgPieces;      //container store Piece
        public ObservableCollection<CusPieceViewModel> ImgPieces
        {
            get => imgPieces;
            set
            {
                imgPieces = value;
                OnPropertyChanged();
            }
        }

        //info
        public int Col
        {
            get => GameModel.Instance.col;
            set
            {
                GameModel.Instance.col = value;
                OnPropertyChanged();
            }

        }
        public int Row
        {
            get => GameModel.Instance.row;
            set
            {
                GameModel.Instance.row = value;
                OnPropertyChanged();
            }

        }
        public BitmapSource OriginPic
        {
            get=>GameModel.Instance.SrcImg;
        }
        long timeSet;

        public RelayCommand<object> GoToMainMenuCommand { get; set; }
        public RelayCommand<object> GoToLeaderboardCommand { get; set; }
        public RelayCommand<object> PlayAgainCommand { get; set; }
        //Command handling input key
        public RelayCommand<object> HandleKeyCommand { get; set; }
        public RelayCommand<object> MouseControlCommand { get; set; }

        #region Biding Properties
        public string timestatus
        {
            get
            {
                if (GameModel.Instance.isSetCountDown)
                    return "Remaining Time";
                return "Total Time";
            }
        }

        private bool _isFocused;            //set focus 
        public bool IsFocused
        {
            get => _isFocused;
            set
            {
                if (_isFocused != value)
                {
                    _isFocused = value;
                    OnPropertyChanged();
                }
            }
        }
        string endGameText;
        public string EndGameText
        {
            get => endGameText;
            set
            {
                endGameText = value;
                OnPropertyChanged();
            }
        }
        string endGameImmageSource;
        public string EndGameImmageSource
        {
            get => endGameImmageSource;
            set
            {
                endGameImmageSource = value;
                OnPropertyChanged();
            }
        }
        private TimeSpan _completionTime;
        private string _completionTimeStr;
        public string CompletionTimeStr
        {
            get => _completionTimeStr;
            set
            {
                _completionTimeStr = value;
                OnPropertyChanged();
            }
        }
        private TimeSpan _lastGameTime;
        private string _lastGameTimeStr;
        public string LastGameTimeStr
        {
            get => _lastGameTimeStr;
            set
            {
                _lastGameTimeStr = value;
                OnPropertyChanged();
            }
        }
        bool isEndGameVisible;
        public bool IsEndGameVisible
        {
            get => isEndGameVisible;
            set
            {
                isEndGameVisible = value;
                OnPropertyChanged();
            }
        }
        private string _time;
        public string time
        {
            get => _time;
            set
            {
                if (_time != value)
                {
                    _time = value;
                    OnPropertyChanged();
                }
            }
        }
        private int _focusPieceX;
        public int FocusPieceX
        {
            get => _focusPieceX;
            set 
            { 
                _focusPieceX = value;
                OnPropertyChanged();
            }
        }
        private int _focusPieceY;
        public int FocusPieceY
        {
            get => _focusPieceY;
            set
            {
                _focusPieceY = value;
                OnPropertyChanged();
            }
        }

        #endregion

        DispatcherTimer _clock;

        public GameRoundViewModel()
        {
            EventAggregator.GetEvent<PubSubEvent<bool>>().Subscribe((o) => IsTimerCounting(o));
            _imageProcessingService = new ImageProcessingService();
            imgPieces = new ObservableCollection<CusPieceViewModel>();
            _wndBgr = defaultColornum1;
            IsFocused = true;
            timeSet = GameModel.Instance.PlayTime;
            IsEndGameVisible = true;
            GameModel.Instance.Status=GameStatus.StartGame;

            //Create CountDown timer
            _clock = new DispatcherTimer();
            _clock.Tick += _countDownClock_Tick;
            _clock.Interval = TimeSpan.FromSeconds(1);
            _completionTime = new TimeSpan(0, 0, 0);
            _lastGameTime = (GameModel.Instance.isSetCountDown)? TimeSpan.FromSeconds(GameModel.Instance.PlayTime):new TimeSpan(0,0,0);
            LastGameTimeStr = _lastGameTime.ToString(@"hh\:mm\:ss");
            CompletionTimeStr = _lastGameTime.ToString(@"hh\:mm\:ss");

            //command def
            GoToLeaderboardCommand = new RelayCommand<object>((o) => { CurrentPage = new LeaderBoardViewModel(); });
            GoToMainMenuCommand = new RelayCommand<object>((o) => { CurrentPage = new LevelSelectionViewModel(); });
            PlayAgainCommand = new RelayCommand<object>((o) => { CurrentPage = new LevelSelectionViewModel(); });
            HandleKeyCommand = new RelayCommand<object>(o => { Game_Control((string)o) ;});
            MouseControlCommand = new RelayCommand<object> (o => { Mouse_Control((CusPiece)o); });


            StartGame();


        }

        private void IsTimerCounting(bool isCounting)
        {
            if(GameModel.Instance.Status == GameStatus.EndGame && _clock == null) { return; } 
            if (GameModel.Instance.PlayTime==0 && _clock.IsEnabled==true) _clock.Stop();
            else if(GameModel.Instance.Status==GameStatus.StartGame ) _clock.Start();
        }

        /// <summary>
        /// countdown func
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _countDownClock_Tick(object? sender, EventArgs e)
        {
            if (GameModel.Instance.isSetCountDown)
            {
                _lastGameTime = _lastGameTime.Subtract(TimeSpan.FromSeconds(1));
                if (--GameModel.Instance.PlayTime == 0)    IsLose();
            }
            else
            _lastGameTime= _lastGameTime.Add(TimeSpan.FromSeconds(1));
            _completionTime = _completionTime.Add(TimeSpan.FromSeconds(1));
            _completionTime.Add(TimeSpan.FromSeconds(1));
            CompletionTimeStr = _completionTime.ToString(@"hh\:mm\:ss");
            LastGameTimeStr = _lastGameTime.ToString(@"hh\:mm\:ss");
        }



        /// <summary>
        /// Add Piece into Controls of pnlGamePlaySpace and Playing
        /// </summary>
        /// 
        public void StartGame()
        {
            GameModel.Instance.Status = GameStatus.StartGame;
            _imageProcessingService.SplitIntoPieces(ImgPieces);
            int inversion;
            do
            {
                inversion = _imageProcessingService.ShufflePieces(ImgPieces);

                if (!IsSolvable(inversion))                   //check whether it is solvable or not
                {                                             //If not ,make it sovable.
                    MakeItSovable(ref inversion);
                }
            }
            while (inversion == 0); 


            if (imgPieces == null)                          
                throw new Exception("Load Image Error");
           
            MovingFocus();
            _clock.Start();                        //start timer
        }

        void MakeItSovable( ref int inversion)
        {
            for(int i = 0; i < imgPieces.Count-1; ++i)
            {
                if (imgPieces[i].CusPiece.ImgIdx > imgPieces[i + 1].CusPiece.ImgIdx)
                {
                    imgPieces[i].SwapCusVM(imgPieces[i+1]);
                    if (IsSolvable(--inversion)) return;              //check whether it is solvable or not
                }
            }
        }

        /// <summary>
        /// check whether it is solvable or not
        /// </summary>
        /// <param name="inversion"></param>
        /// <returns></returns>
        private bool IsSolvable(int inversion)
        {
            if (GameModel.Instance.col * GameModel.Instance.row % 2 == 0)
            {
                return (GameModel.Instance.row - 1 +inversion ) % 2!= 0;
                
            }
            return inversion % 2 == 0;
        }



        /// <summary>
        ///check constraints and determine blackbox coordinates
        /// </summary>
        /// <param name="mxPos"></param>
        /// <param name="myPos"></param>
        /// <param name="mPos"></param>
        public void MovingPiece(int mxPos, int myPos, ref int mPos)
        {
            if (mxPos > -1 && myPos < GameModel.Instance.col && myPos > -1 && mxPos < GameModel.Instance.row)
            {
                mPos = mxPos * GameModel.Instance.col + myPos;
            }
            else
                mPos = -1;
        }

        /// <summary>
        /// add status of 2 piece after moving
        /// </summary>
        /// <param name="ImgPieces"></param>
        public bool UpdateStatus(ObservableCollection<CusPieceViewModel> ImgPieces)
        {
            for (int i = 0; i < GameModel.Instance.row * GameModel.Instance.col - 1; ++i)
            {
                if (!imgPieces[i].CusPiece.Match())
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// using key press event to mowing blackPiece around
        /// </summary>
        /// <param name="sKey"></param>
        /// <returns></returns>
        private void Game_Control(string sKey)
        {
            if (GameModel.Instance.Status !=GameStatus.StartGame)
                return;
            int moveIndex = -1;
            CusPiece tmp = imgPieces[GameModel.Instance.BlackBox_Indx].CusPiece;
            switch (sKey)
            {
                case "Down":
                    MovingPiece(tmp.XIndex - 1, tmp.YIndex, ref moveIndex);
                    break;
                case "Up":
                    MovingPiece(tmp.XIndex + 1, tmp.YIndex, ref moveIndex);
                    break;
                case "Left":
                   MovingPiece(tmp.XIndex, tmp.YIndex + 1, ref moveIndex);
                    break;
                case "Right":
                  MovingPiece(tmp.XIndex, tmp.YIndex - 1, ref moveIndex);
                    break;
            }
            if (moveIndex == -1)
                return;

            MovingFocus();

            (imgPieces[GameModel.Instance.BlackBox_Indx]).SwapCusVM(imgPieces[moveIndex]);
            GameModel.Instance.BlackBox_Indx = moveIndex;

            if (UpdateStatus(imgPieces))
            {
                IsWin();
            }
            return;
        }

        /// <summary>
        /// using mouse to play game
        /// </summary>
        /// <param name="o"></param>
        private void Mouse_Control(CusPiece o)
        {
            int blackBoxPos = GameModel.Instance.BlackBox_Indx;
            int[] validPos = { blackBoxPos - 1, blackBoxPos + 1, blackBoxPos - GameModel.Instance.col, blackBoxPos + GameModel.Instance.col };

            int currentIndex = o.CurrentImgIndex();

            if (currentIndex == validPos[3])
            {
                Game_Control("Up");
            }
            else if (currentIndex == validPos[2])
            {
                Game_Control("Down");
            }
            else if (currentIndex == validPos[1])
            {
                Game_Control("Left");
            }
            else if (currentIndex == validPos[0])
            {
                Game_Control("Right");
            }
        }


        /// <summary>
        /// moving focus frame follow piece
        /// </summary>
        public void MovingFocus()
        {
            FocusPieceX = GameModel.Instance.BlackBox_Indx % GameModel.Instance.col * (int)(GameModel.Instance.gamePlayBoxX / GameModel.Instance.col);
            FocusPieceY = GameModel.Instance.BlackBox_Indx / GameModel.Instance.row * (int)(GameModel.Instance.gamePlayBoxY/GameModel.Instance.row);
        }

        //private Key ConvertCharToKeyCode(string key)
        //{
        //    return (Key)System.Enum.Parse(typeof(Key), key);
        //}


        public void IsLose()
        {
            MusicSystemService.Instance.EndGame_Sound(0);
            EndGameImmageSource = "pack://application:,,,/Assets/Imgs/Lose.png";
            EndGameText = "\"Amazing try, Don't sad! Winning takes practice, and you're on the right path!\"";
            GameModel.Instance.Status = GameStatus.EndGame;
            IsEndGameVisible = false;
            ReleaseClock();
        }


        public void IsWin()
        {
            MusicSystemService.Instance.EndGame_Sound(1);
            if (GameModel.Instance.isSetCountDown)
            {
                time = "Time: " + TimeSpan.FromSeconds(timeSet - GameModel.Instance.PlayTime).ToString(@"hh\:mm\:ss");
            }
            else
                time = "Time: " + LastGameTimeStr;

            SaveGameRoundData();
            EndGameImmageSource = "pack://application:,,,/Assets/Imgs/Win.png";
            EndGameText = $"\"You completed the round in just {time}!You're a true champion! Keep up the amazing work!\"";
            GameModel.Instance.Status = GameStatus.EndGame;
            IsEndGameVisible = false;
           ReleaseClock();
        }

        public void SaveGameRoundData()
        {
            GameModel.Instance.GameRound.PlayerID = GameModel.Instance.Player.Id;
            GameModel.Instance.GameRound.PlayerName = GameModel.Instance.Player.Name;
            GameModel.Instance.GameRound.Pieces = Row.ToString() + " x " + Col.ToString();
            GameModel.Instance.GameRound.Time = CompletionTimeStr;
            GameModel.Instance.GameRound.Date = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
            GameModel.Instance.LeaderBoardService.AddGameRound(GameModel.Instance.GameRound);
        }
        private void ReleaseClock()
        {
            _clock.Tick -= _countDownClock_Tick;
            _clock = null;
        }
    }
}
