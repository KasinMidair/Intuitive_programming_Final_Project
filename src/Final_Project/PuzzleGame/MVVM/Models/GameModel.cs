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

        public string srcPath {  get; set; }

        public BitmapSource SrcImg;
        public BitmapSource blckBoxImg;
        public int BlackBox_Indx;
        public int gamePlayBoxX;
        public int gamePlayBoxY;

        string _gameRoundPicUrl;
        public string GameRoundPicUrl
        {
            get
            {
                return _gameRoundPicUrl;
            }
            set
            {
                _gameRoundPicUrl = value;
            }
        }

        int playTime;
        public bool isSetCountDown { get; set; }
        public int row, col;
        private static volatile GameModel _instance;
        private GameStatus status;
        private static object key = new object();

        public static GameModel Instance
        {
            get
            {
                if (_instance == null || _instance.Status == GameStatus.EndGame)
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
            gamePlayBoxX = 488;
            gamePlayBoxY = 488;

            srcPath = System.IO.Path.Combine(Directory.GetCurrentDirectory().Remove(Directory.GetCurrentDirectory().Length - 24, 24) + $"/Assets/picture/HOAInewpic.png");
           
        }

        public void PicDeviding()
        {
            BitmapImage originalImage = new BitmapImage(new Uri(srcPath, UriKind.RelativeOrAbsolute));
            double scaleX = (double)gamePlayBoxX * originalImage.DpiX / (originalImage.PixelWidth * 96);
            double scaleY = (double)gamePlayBoxY * originalImage.DpiY / (originalImage.PixelHeight * 96);
            SrcImg = new TransformedBitmap(originalImage, new ScaleTransform(scaleX, scaleY));
            blckBoxImg = new BitmapImage(new Uri("pack://application:,,,/Assets/Imgs/Sprite-0003.png", UriKind.Absolute));
            
            row = 3; col = 3;
            status = GameStatus.PreStart;
            playTime = 2 * 60;
            UnitX = SrcImg.PixelWidth / col;
            UnitY = SrcImg.PixelHeight / row;
            isSetCountDown = false;
        }

        public BitmapSource ChangeDPI(BitmapSource source, double newDpiX, double newDpiY)
        {
            int width = source.PixelWidth;
            int height = source.PixelHeight;

            // caculate stride:  Bytes in one line ( Height x width(pixel num,direaction:horizontal ))
            int bytesPerPixel = (source.Format.BitsPerPixel + 7) / 8;
            int stride = width * bytesPerPixel;

            // pixel data from source
            byte[] pixelData = new byte[height * stride];
            source.CopyPixels(pixelData, stride, 0);

            // Create new bitmap with new DPI
            BitmapSource newBitmap = BitmapSource.Create(
                width,          
                height,         
                newDpiX,        // DPI X
                newDpiY,        // DPI Y
                source.Format,  // Pixel format
                source.Palette, // Palette (can be null)
                pixelData,      // pixel data
                stride          // Stride
            );

            return newBitmap;
        }
        
    }
}
