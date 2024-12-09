using PuzzleGame.Core;
using PuzzleGame.Core.Helper;
using PuzzleGame.MVVM.Models;
using PuzzleGame.Stores;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace PuzzleGame.MVVM.ViewModels
{

    public class GameRoundViewModel:ObservableObject
    {
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

        //Command handling input key
        public RelayCommand<object> HandleKeyCommand { get; set; }

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
        bool isEndGameVissible;
        public bool IsEndGameVissible
        {
            get => isEndGameVissible;
            set
            {
                isEndGameVissible = value;
                OnPropertyChanged();
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

        DispatcherTimer _clock;

        public GameRoundViewModel()
        {
            EventAggregator.GetEvent<PubSubEvent<bool>>().Subscribe((o) => IsTimerCounting(o));
            _imageProcessingService = new ImageProcessingService();
            _wndBgr = defaultColornum1;
            IsFocused = true;
            imgPieces = new ObservableCollection<CusPieceViewModel>();

            //Create CounDown timer
            _clock = new DispatcherTimer();
            _clock.Tick += _countDownClock_Tick;
            _clock.Interval = TimeSpan.FromSeconds(1);
            _lastGameTime = new TimeSpan(0,0,0);
            LastGameTimeStr = _lastGameTime.ToString(@"hh\:mm\:ss");
            GameModel.Instance.Status=GameStatus.StartGame;
            HandleKeyCommand = new RelayCommand<object>(o => { Game_Control((string)o) ;}); 
            StartGame();

        }

        private void IsTimerCounting(bool isCounting)
        {
            if(isCounting==false) _clock.Stop();
            else _clock.Start();
        }

        private void _countDownClock_Tick(object? sender, EventArgs e)
        {
            if (GameModel.Instance.isSetCountDown==true)
            {
                if (--GameModel.Instance.PlayTime == 0)    IsLose();
            }
            _lastGameTime= _lastGameTime.Add(TimeSpan.FromSeconds(1));
            LastGameTimeStr = _lastGameTime.ToString(@"hh\:mm\:ss");
        }



        /// <summary>
        /// Add Piece into Controls of pnlGamePlaySpace and Playing
        /// </summary>
        /// 
        public void StartGame()
        {
            if (GameModel.Instance.Status == GameStatus.StartGame)
            {
                _imageProcessingService.SplitIntoPieces(ImgPieces);
                int inversion;
                do
                {
                    inversion = _imageProcessingService.ShufflePieces(ImgPieces);    // inversion before one time shuffle
                }
                while (inversion == 0); 
                if (!IsSolvable(inversion))                   //check whether it is solvable or not
                {                                             //If not ,make it sovable.
                    MakeItSovable(inversion);
                }
            }

            if (imgPieces == null)                          
                throw new Exception("Load Image Error");
           
            MovingFocus();
            _clock.Start();                        //start timer
        }

        void MakeItSovable(int inversion)
        {
            for(int i = 0; i < imgPieces.Count-1; ++i)
            {
                if (i==GameModel.Instance.BlackBox_Indx) continue;
                else if (imgPieces[i].CusPiece.ImgIdx > imgPieces[i + 1].CusPiece.ImgIdx)
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
            if ((GameModel.Instance.col * GameModel.Instance.row) % 2 == 0)
            {
               if ((GameModel.Instance.row - imgPieces[GameModel.Instance.BlackBox_Indx].CusPiece.YIndex) % 2 == 0)
               {
                    return inversion % 2 != 0;
               }
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
            if (GameModel.Instance.Status == GameStatus.PreStart)
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
                IsWin();
            return;
        }
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
            GameModel.Instance.Status = GameStatus.EndGame;
            ReleaseClock();
            EventAggregator.GetEvent<PubSubEvent<string>>().Publish("Lose!");
        }


        public void IsWin()
        {
            GameModel.Instance.Status = GameStatus.EndGame;
            ReleaseClock();
        }
        private void ReleaseClock()
        {
            _clock.Tick -= _countDownClock_Tick;
            _clock = null;
        }


    }
}
