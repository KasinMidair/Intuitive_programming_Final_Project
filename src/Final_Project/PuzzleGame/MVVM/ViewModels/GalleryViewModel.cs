using PuzzleGame.Core;
using PuzzleGame.MVVM.Models;
using System;
using System.Collections.Generic;
using System.DirectoryServices.ActiveDirectory;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace PuzzleGame.MVVM.ViewModels
{
    public class GalleryViewModel : ObservableObject
    {
        ListView _ImageList = new ListView();

        GalleryConnection GalConn = new GalleryConnection();

        void LoadImageList()
        {
            foreach (img image in GalConn.imglist)
            {
                _ImageList.Items.Add(image.imgname);
            }
        }
    }
}
