using SlideFun.Stores;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;


namespace SlideFun.MVVM.Models
{
    public enum GameStatus
    {
        PreStart,
        StartGame,
        EndGame,
        QuitApp
    }
    public class GameModel
    {
        public double UnitX { get; set; }
        public double UnitY { get; private set; }
        public int row { get; set; }
        public int col { get; set; }
        public Player Player { get; set; }
        public bool isSetCountDown { get; set; }
        public string srcPath { get; set; }


        private GameStatus status;
        public GameStatus Status
        {
            get
            {
                return status;
            }
            set
            {
                status = value;
            }
        }

        public BitmapSource SrcImg;
        public BitmapSource blckBoxImg;
        public int BlackBox_Indx;
        public int gamePlayBoxX;
        public int gamePlayBoxY;
        private GameRound _gameRound;

        public GameRound GameRound
        {
            get
            {
                if (_gameRound == null)
                {
                    _gameRound = new GameRound();
                }
                return _gameRound;
            }
        }
        private LeaderBoardService _leaderBoardService;
        public LeaderBoardService LeaderBoardService
        {
            get
            {
                if (_leaderBoardService == null) { _leaderBoardService = new LeaderBoardService(); }
                return _leaderBoardService;
            }
        }


        long playTime;
        public long PlayTime
        {
            get
            {
                return playTime;
            }
            set
            {
                playTime = value;
            }
        }

        private static object key = new object();
        private static volatile GameModel _instance;
        public static GameModel Instance
        {
            get
            {
                if (_instance == null )
                {
                    lock (key)
                    {
                        _instance = new GameModel();
                    }
                }

                return _instance;
            }
        }


        private GameModel()
        {


            gamePlayBoxX = 488;
            gamePlayBoxY = 488;
            status = GameStatus.PreStart;
            blckBoxImg = new BitmapImage(new Uri("pack://application:,,,/Assets/Imgs/Sprite-0003.png", UriKind.Absolute));

        }
        public void SetData(int r,int c,string imgSrcPath ,long _playTime)
        {
            srcPath = imgSrcPath;
            row =r; col = c;
            playTime = _playTime;
            isSetCountDown = (_playTime==0)? false:true;
        }

        public void PicResize()
        {
            BitmapImage originalImage = new BitmapImage(new Uri(srcPath, UriKind.RelativeOrAbsolute));

            // caculate to crop Image to filltoUniform
            int originPixeldimension= Math.Min(originalImage.PixelWidth,originalImage.PixelHeight);
            int offsetX = (originalImage.PixelWidth - originPixeldimension) / 2;
            int offsetY = (originalImage.PixelHeight - originPixeldimension) / 2;
            CroppedBitmap croppedImage = new CroppedBitmap(originalImage,
                new Int32Rect(offsetX, offsetY, originPixeldimension, originPixeldimension));

            //resize image to fit diff DPI
            double uniformScale = (double)gamePlayBoxX * originalImage.DpiX / (croppedImage.PixelWidth * 96);

            SrcImg = new TransformedBitmap(croppedImage, new ScaleTransform(uniformScale, uniformScale));

            //set dimension of Piece
            UnitX = SrcImg.PixelWidth / col;
            UnitY = SrcImg.PixelHeight / row;
        }
        
    }
}
