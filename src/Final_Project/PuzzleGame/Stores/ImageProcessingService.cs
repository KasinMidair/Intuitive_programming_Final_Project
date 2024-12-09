using PuzzleGame.MVVM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows;
using System.Windows.Media;
using System.Collections.ObjectModel;
using PuzzleGame.MVVM.ViewModels;
using System.Linq;

namespace PuzzleGame.Stores
{
    public class ImageProcessingService
    {

        private static ImageProcessingService? _instance;
        public static ImageProcessingService Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new ImageProcessingService();
                }

                return _instance;
            }
        }
        /// <summary>
        /// split original image into row x col piece
        /// </summary>
        public void SplitIntoPieces(ObservableCollection<CusPieceViewModel> imgPieces)
        {

            int index = 0;
            int i, j;
            imgPieces.Clear();

            for (i = 0; i < GameModel.Instance.row; ++i)
            {
                for (j = 0; j < GameModel.Instance.col; ++j)
                {
                    CroppedBitmap pieceImg;

                    if (index==GameModel.Instance.row*GameModel.Instance.col-1)
                    {
                        pieceImg = new CroppedBitmap(GameModel.Instance.blckBoxImg, new Int32Rect(0, 0, (int)(GameModel.Instance.gamePlayBoxX / GameModel.Instance.col), (GameModel.Instance.gamePlayBoxY / GameModel.Instance.row)));
                        GameModel.Instance.BlackBox_Indx = index ;
                    }
                    else
                    {
                        Int32Rect rct = new Int32Rect((int)(GameModel.Instance.UnitX * j),
                           (int)(GameModel.Instance.UnitY * i), (int)GameModel.Instance.UnitX, (int)GameModel.Instance.UnitY);
                        pieceImg = new CroppedBitmap(GameModel.Instance.SrcImg, rct);

                    }

                    var imgPiece = new CusPiece(i, j, index++);
                    imgPieces.Add(new (imgPiece, pieceImg));
                }
            }

        }



        /// <summary>
        /// using  Fisher-Yates algorithm( Knuth shuffle algorithm ) to shuffle all the pieces.
        /// The Fisher-Yates algorithm is a popular algorithm 
        /// for randomly shuffling a list of elements so that each element has an equal probability 
        /// of appearing in each position.
        /// </summary>
        public int ShufflePieces(ObservableCollection<CusPieceViewModel> imgPieces)
        {
            List<int> ImgIndex = Enumerable.Range(0, GameModel.Instance.row * GameModel.Instance.col).ToList<int>();
            int pieceIndex = 0;
            Random random = new Random();
            for (int i = 0; i < imgPieces.Count-1; ++i)
            {
                int randIndex = random.Next(pieceIndex++, imgPieces.Count-1);
                imgPieces[i].SwapCusVM(imgPieces[randIndex]);
                Swap(ImgIndex, i, randIndex);
            };

            ImgIndex.Remove(GameModel.Instance.row * GameModel.Instance.col - 1);


            return InversionCountingService.CountInversions(ImgIndex);
        }


        public void Swap(List<int>arr, int a,int b)
        {
            int temp = arr[a];
            arr[a]= arr[b];
            arr[b] = temp;
        }

    }
}
