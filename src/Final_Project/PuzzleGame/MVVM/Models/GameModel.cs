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
        public double UnitX { get; set; }
        public double UnitY { get; private set; }
        public BitmapSource SrcImg;
        public BitmapSource blckBoxImg;
        public int BlackBox_Indx;

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
            string srcPath = "pack://application:,,,/Assets/picture/pic1.jpg";
            SrcImg = ChangeDPI(new BitmapImage(new Uri(srcPath, UriKind.Absolute)), 96, 96);
            blckBoxImg = new BitmapImage(new Uri("pack://application:,,,/Assets/Imgs/Sprite-0003.png", UriKind.Absolute));
            row = 5; col = 5;
            status = GameStatus.PreStart;
            playTime = 2 * 60;
            UnitX = 450 / col;
            UnitY = 450 / row;
            isSetCountDown = false;

        }

        public BitmapSource ChangeDPI(BitmapSource source, double newDpiX, double newDpiY)
        {
            int width = source.PixelWidth;
            int height = source.PixelHeight;

            // Tính stride: số byte trên mỗi dòng (chiều rộng ảnh x số byte mỗi pixel)
            int bytesPerPixel = (source.Format.BitsPerPixel + 7) / 8;
            int stride = width * bytesPerPixel;

            // Lấy dữ liệu pixel từ ảnh gốc
            byte[] pixelData = new byte[height * stride];
            source.CopyPixels(pixelData, stride, 0);

            // Tạo ảnh mới với DPI được thay đổi
            BitmapSource newBitmap = BitmapSource.Create(
                width,          // Chiều rộng (pixels)
                height,         // Chiều cao (pixels)
                newDpiX,        // DPI X
                newDpiY,        // DPI Y
                source.Format,  // Pixel format
                source.Palette, // Palette (null nếu không có)
                pixelData,      // Dữ liệu pixel
                stride          // Stride
            );

            return newBitmap;
        }
    }
}
