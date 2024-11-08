using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace PuzzleGame.Class_Cus
{
    internal class Piece:Image
    {
        int yIndex;
        int imgIdx;
        int xIndex;
        public int ImgIdx
        {
            set
            {
                imgIdx = value;
            }
            get
            {
                return imgIdx;
            }
        }
        public int XIndex
        {
            set
            {
                xIndex = value;
            }
            get
            {
                return xIndex;
            }
        }
        public int YIndex
        {
            set
            {
                yIndex = value;
            }
            get
            {
                return yIndex;
            }
        }
        public Piece(Piece p)
        {
            this.ImgIdx = p.ImgIdx;
            this.XIndex = p.XIndex;
            this.YIndex = p.YIndex;
            this.Width = p.Width;
            this.Height = p.Height;
            this.Source = p.Source;
        }

        public Piece(int xIndex, int yIndex, int imgIdx, ImageSource img)
        {
            this.ImgIdx = imgIdx;
            this.XIndex = xIndex;
            this.YIndex = yIndex;
            this.Width = img.Width; 
            this.Height = img.Height; 
            this.Source = img;
        }

        public void SwapPieces(Piece p2)
        {
            ImageSource img= this.Source;
            if (img == null)
            {
                throw new InvalidOperationException("No Image!");
            }
            int tmpImgIdx = this.ImgIdx;

            this.Source = p2.Source; 
            this.ImgIdx = p2.ImgIdx;
            p2.Source = img;
            p2.ImgIdx = tmpImgIdx;
        }
        public bool Match()
        {
            return xIndex * GameManager.Instance.col + yIndex == imgIdx;
        }
        public int CurrentImgIndex()
        {
            return xIndex * GameManager.Instance.col + yIndex;
        }
    }
}
