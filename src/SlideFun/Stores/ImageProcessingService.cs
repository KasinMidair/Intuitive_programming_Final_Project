using SlideFun.MVVM.Models;
using SlideFun.MVVM.ViewModels;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Media.Imaging;

namespace SlideFun.Stores
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
            int index = 0;  //position of piece in original picture 
            int i, j;
            imgPieces.Clear();

            for (i = 0; i < GameModel.Instance.row; ++i)
            {
                for (j = 0; j < GameModel.Instance.col; ++j)
                {
                    CroppedBitmap pieceImg;
                    // assign Image source  of the last PICTURE's piece is a blank box 
                    if (index == GameModel.Instance.row * GameModel.Instance.col - 1)
                    {
                        pieceImg = new CroppedBitmap(
                            GameModel.Instance.blckBoxImg,
                            new Int32Rect(0, 0,
                                          (GameModel.Instance.gamePlayBoxX / GameModel.Instance.col),
                                          (GameModel.Instance.gamePlayBoxY / GameModel.Instance.row)
                            )
                        );
                        //save the blank box  position
                        GameModel.Instance.BlackBox_Indx = index;
                    }
                    else
                    {
                        Int32Rect rct = new Int32Rect((int)(GameModel.Instance.UnitX * j),
                           (int)(GameModel.Instance.UnitY * i), (int)GameModel.Instance.UnitX, (int)GameModel.Instance.UnitY);
                        pieceImg = new CroppedBitmap(GameModel.Instance.SrcImg, rct);

                    }
                    //Create a piece & add it to Game play Grid.
                    var imgPiece = new CusPiece(i, j, index++);
                    imgPieces.Add(new(imgPiece, pieceImg));
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
            // Create a list of indices from 0 to row * col - 1 to track the positions of the pieces.
            // This list will be used later to count inversions after the shuffle.
            List<int> ImgIndex = Enumerable.Range(0, GameModel.Instance.row * GameModel.Instance.col).ToList<int>();

            Random random = new Random();
            for (int i = 0; i < imgPieces.Count - 2; ++i)
            {
                //random a position beetween i and number of pieces -1 
                int randIndex = random.Next(i, imgPieces.Count - 1);

                //swap 2 Piece VM
                imgPieces[i].SwapCusVM(imgPieces[randIndex]);
                //Swap 2 index in ImgIndex
                Swap(ImgIndex, i, randIndex);
            };

            // remove the index of blank box .The bank box position is the last.
            ImgIndex.Remove(GameModel.Instance.BlackBox_Indx);

            return InversionCountingService.CountInversions(ImgIndex); //counting inversions and return it. 
        }

        //swap element
        public void Swap(List<int> arr, int a, int b)
        {
            int temp = arr[a];
            arr[a] = arr[b];
            arr[b] = temp;
        }

    }
}
