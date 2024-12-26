using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace PuzzleGame.MVVM.Models
{
    public class CusPiece
    {
        int yIndex;     // position follow y asis  in GameImage map
        int imgIdx;     // nth of img in whole SrcImage
        int xIndex;     // position follow x asis  in GameImage map
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

        public CusPiece(CusPiece _custom)
        {
            this.ImgIdx = _custom.ImgIdx;
            this.XIndex = _custom.XIndex;
            this.YIndex = _custom.YIndex;

        }
        public CusPiece(int xIndex, int yIndex, int imgIdx)
        {
                this.ImgIdx = imgIdx;
                this.XIndex = xIndex;
                this.YIndex = yIndex;
        }

        /// <summary>
        /// swap between 2 CusPiece
        /// </summary>
        /// <param name="p2"></param>
        /// <exception cref="InvalidOperationException"></exception>
        public void SwapPieces(CusPiece p2)
        {
            int tmpImgIdx = this.ImgIdx;
            this.ImgIdx = p2.ImgIdx;
            p2.ImgIdx = tmpImgIdx;
        }

        /// <summary>
        /// Check if this CusPiece is in the correct position
        /// </summary>
        /// <returns></returns>
        public bool Match()
        {
            return xIndex * GameModel.Instance.col + yIndex == imgIdx;
        }

        public int CurrentImgIndex()
        {
            return xIndex * GameModel.Instance.col + yIndex;
        }
    }
}
