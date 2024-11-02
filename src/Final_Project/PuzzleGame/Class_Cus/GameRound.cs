using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace PuzzleGame.Class_Cus
{
    internal class GameRound
    {
        public List<Image> imgPieces;
        BitmapSource img;
        BitmapSource blckBoxImg;
        public int BlackBox_Indx;
        Panel pnlContainer;
        Panel pnlGamePlaySpace;
        public GameRound(Panel Container, Panel GamePlaySpace)
        {
            imgPieces = new List<Image>();
            img = new BitmapImage(new Uri("Assets/Imgs/Sprite-0002.png",UriKind.Relative));
            blckBoxImg = new BitmapImage(new Uri("Assets/Imgs/Sprite-0003.png", UriKind.Relative));
            pnlContainer = Container;
            pnlGamePlaySpace = GamePlaySpace;
            BlackBox_Indx = GameManager.Instance.col * GameManager.Instance.row - 1;
        }


        /// <summary>
        /// Add Piece into Controls of pnlGamePlaySpace and Playing
        /// </summary>
        /// 
        public void StartGame()
        {

            pnlContainer.Width = GameManager.Instance.imgsize.Width + 20;
            pnlContainer.Height = GameManager.Instance.imgsize.Width + 200;
            pnlGamePlaySpace.Width = GameManager.Instance.imgsize.Width;
            pnlGamePlaySpace.Height = GameManager.Instance.imgsize.Height;

            if (GameManager.Instance.Status == GameStatus.StartGame)
            {
                SplitIntoPieces();
                Shuffle();
            }
            if (imgPieces == null)
                return;


            pnlGamePlaySpace.Children.Clear();

            foreach (var img in imgPieces)
            {
                pnlGamePlaySpace.Children.Add(img);
            }
            pnlGamePlaySpace.UpdateLayout();

        }


        /// <summary>
        /// split original image into row X col piece
        /// </summary>
        public void SplitIntoPieces()
        {
            int index = 0;
            int i, j;
            imgPieces.Clear();

            for (i = 0; i < GameManager.Instance.row; ++i)
            {
                for (j = 0; j < GameManager.Instance.col; ++j)
                {
                    CroppedBitmap pieceImg;

                    if (BlackBox_Indx == (i * GameManager.Instance.col + j))
                    {
                        pieceImg = new CroppedBitmap(blckBoxImg, new Int32Rect(0, 0, (int)GameManager.Instance.UnitX, (int)GameManager.Instance.UnitY));
                    }
                    else
                    {
                        Int32Rect rct = new Int32Rect( (int)(GameManager.Instance.UnitX * j),
                           (int)(GameManager.Instance.UnitY * i), (int)GameManager.Instance.UnitX, (int)GameManager.Instance.UnitY);
                        pieceImg = new CroppedBitmap(img, rct);
                        
                    }
                    var imgPiece = new Piece(i, j, index++, pieceImg)
                    {
                        Width = GameManager.Instance.UnitX,
                        Height = GameManager.Instance.UnitY,
                    };

                    Canvas.SetLeft(imgPiece, j * GameManager.Instance.UnitX);
                    Canvas.SetTop(imgPiece, i * GameManager.Instance.UnitY);

                    imgPieces.Add(imgPiece);
                }
            }

        }


        /// <summary>
        /// using  Fisher-Yates algorithm( Knuth shuffle algorithm ) to shuffle all the pieces.
        /// The Fisher-Yates algorithm is a popular algorithm 
        /// for randomly shuffling a list of elements so that each element has an equal probability 
        /// of appearing in each position.
        /// </summary>
        public void Shuffle()
        {
            int pieceIndex = 0;
            Random random = new Random();
            imgPieces.ForEach(p =>
            {
                int randIndex = random.Next(pieceIndex++, imgPieces.Count);
                if (randIndex == BlackBox_Indx)
                    BlackBox_Indx = imgPieces.IndexOf(p);
               ((Piece)p).SwapPieces((Piece)imgPieces[randIndex]);
            });
        }


        /// <summary>
        ///check constraints and determine blackbox coordinates
        /// </summary>
        /// <param name="mxPos"></param>
        /// <param name="myPos"></param>
        /// <param name="mPos"></param>
        public void MovingPiece(int mxPos, int myPos, ref int mPos)
        {
            if (mxPos > -1 && myPos < GameManager.Instance.col && myPos > -1 && mxPos < GameManager.Instance.row)
            {
                mPos = mxPos * GameManager.Instance.col + myPos;
            }
            else
                mPos = -1;
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            if (--GameManager.Instance.PlayTime == 0)
            {
                GameManager.Instance.IsLose();
            }
        }
    }
}
