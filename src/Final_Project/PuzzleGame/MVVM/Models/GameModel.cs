using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Windows.Media;


namespace PuzzleGame.MVVM.Models
{
    public enum GameStatus
    {
        PreStart,
        StartGame,
        Playing,
        EndGame
    }
    public class GameModel
    {
        public double UnitX { get;set;}
        public double UnitY { get; private set; }
        public BitmapSource SrcImg; 
        public BitmapSource blckBoxImg;
        public int BlackBox_Indx;

        int playTime;
        public bool isSetCountDown {  get; set; }
        public int row, col;
        private static volatile GameModel _instance;
        private GameStatus status;
        private static object key = new object();
        public static GameModel Instance
        {
            get
            {
                if (_instance == null || _instance.Status==GameStatus.EndGame)
                {
                    lock (key)
                    {
                        _instance = new GameModel();
                    }
                }

                return _instance;
            }
        }
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

        public int PlayTime
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
        private GameModel()
        {

            SrcImg = new BitmapImage(new Uri("pack://application:,,,/Assets/Imgs/Sprite-0002.png", UriKind.Absolute));
            blckBoxImg = new BitmapImage(new Uri("pack://application:,,,/Assets/Imgs/Sprite-0003.png", UriKind.Absolute));
            row = 50; col = 50;
            status = GameStatus.PreStart;
            playTime = 2 * 60;
            UnitX = SrcImg.Width / col;
            UnitY = SrcImg.Height / row;
            isSetCountDown = false;

        }
        

       
    }
}
