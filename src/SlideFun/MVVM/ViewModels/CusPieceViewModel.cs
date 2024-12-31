using SlideFun.Core;
using SlideFun.MVVM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace SlideFun.MVVM.ViewModels
{
    public class CusPieceViewModel:ObservableObject
    {
        CusPiece _cusPiece;         //pos of Piece in Image and GamePlay map 
        public CusPiece CusPiece
        {
            get=>_cusPiece;
            set => _cusPiece = value;
        }
        ImageSource img;
        public ImageSource Img
        {
            get => img;
            set
            {
                img = value;
                OnPropertyChanged();

            }
        }
        public CusPieceViewModel(CusPiece cus,ImageSource _img)
        {
            CusPiece = cus;
            Img = _img;
        }

        public void SwapCusVM(CusPieceViewModel vm2)
        {
            this.CusPiece.SwapPieces(vm2.CusPiece);
            ImageSource img2 = this.Img;
            this.Img=vm2.Img;
            vm2.Img = img2;
        }
    }
}
