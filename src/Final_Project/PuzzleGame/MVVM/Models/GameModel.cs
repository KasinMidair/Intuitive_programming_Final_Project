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
using DryIoc.ImTools;
using PuzzleGame.MVVM.ViewModels;
using System.ComponentModel;
using System.IO;


namespace PuzzleGame.MVVM.Models
{
    public enum GameStatus
    {
        PreStart,
        StartGame,
        Playing,
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

        public BitmapSource SrcImg, blckBoxImg;
        public int BlackBox_Indx;
        public int gamePlayBoxX, gamePlayBoxY;
        string srcPath;


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

        public void PicDeviding()
        {
            BitmapImage originalImage = new BitmapImage(new Uri(srcPath, UriKind.RelativeOrAbsolute));
            double scaleX = (double)gamePlayBoxX * originalImage.DpiX / (originalImage.PixelWidth * 96);
            double scaleY = (double)gamePlayBoxY * originalImage.DpiY / (originalImage.PixelHeight * 96);
            SrcImg = new TransformedBitmap(originalImage, new ScaleTransform(scaleX, scaleY));
            UnitX = SrcImg.PixelWidth / col;
            UnitY = SrcImg.PixelHeight / row;
        }
        
    }
}
