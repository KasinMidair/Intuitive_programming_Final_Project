using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Enumeration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using DryIoc.FastExpressionCompiler.LightExpression;
using Microsoft.Win32;
using PuzzleGame.Core;
using PuzzleGame.Core.Helper;
using PuzzleGame.MVVM.Models;
using PuzzleGame.Stores;

namespace PuzzleGame.MVVM.ViewModels
{
    class AddPicturePageViewModel: ObservableObject
    {
        public readonly CusDialogService _cusDialogService = new CusDialogService();
        public readonly GalleryViewModel _galleryViewModel = new GalleryViewModel();

        private ObservableObject _currentPage;
        public ObservableObject CurrentPage
        {
            get => _currentPage;
            set
            {
                if (_currentPage != value)
                {
                    _currentPage = value;
                    EventAggregator.GetEvent<PubSubEvent<ObservableObject>>().Publish(CurrentPage);
                }
            }
        }

        public string _newPicName { get; set; }
        public string NewPicName
        {
            get { return _newPicName; }
            set
            {
                _newPicName = value;
                OnPropertyChanged();
            }
        }

        string _newPicUrl;
        public string NewPicUrl
        {
            get => _newPicUrl;
            set
            {
                _newPicUrl = value;
                OnPropertyChanged();
            }
        }

        Picture _newPic;
        public Picture NewPic
        {
            get => _newPic;
            set
            {
                _newPic = value;
                OnPropertyChanged();
            }
        }

        BitmapImage _newBitmap;
        BitmapImage newBitmap
        {
            get => _newBitmap;
            set
            {
                _newBitmap = value;
                OnPropertyChanged();
            }
        }


        //Command
        public RelayCommand<object> ChoosePictureCommand { get; set; }
        public RelayCommand<object> AddPictureCommand { get; set; }


        public string path { get; set; }


        public AddPicturePageViewModel()
        {



            _wndBgr = defaultColornum1;
            NewPicUrl = @"/Assets/Imgs/AddPicBackGround.jpg";

            ChoosePictureCommand = new RelayCommand<object>((o) =>
            {
                OpenFileDialog dialog = new OpenFileDialog();
                dialog.Filter = "All Image Files| *.jpg; *.png; *.jpeg; *.bmp; *.gif;";

                if (dialog.ShowDialog() == true)
                {
                    NewPicUrl = dialog.FileName;
                    newBitmap = new BitmapImage(new Uri(NewPicUrl, UriKind.Absolute));
                }
            });

            AddPictureCommand = new RelayCommand<object>((o) =>
            {
                bool check = false;
                
                if (NewPicName == null || NewPicName == "")
                {
                    _cusDialogService.ShowDialog("Chua nhap ten tranh", true);
                    check = true;
                }
                else if(
                NewPicUrl == null)
                {
                    _cusDialogService.ShowDialog("Chua chon tranh");
                    check = true;
                }
                else
                {
                     foreach (Picture pic in _galleryViewModel.PictureList)
                    {
                        if (NewPicName == pic.Name)
                        {
                            _cusDialogService.ShowDialog("Ten tranh da ton tai");
                            check = true;
                            break;
                        }
                    }
                }

                if (check == false)
                {
                    NewPic = new Picture
                    {
                        Name = NewPicName,
                        Url = Directory.GetCurrentDirectory().Remove(Directory.GetCurrentDirectory().Length - 24, 24) + @"Assets\picture\" + NewPicName + ".jpg"
                    };
                    FileInfo info = new FileInfo(NewPicUrl);
                    info.CopyTo(NewPic.Url);
                    _cusDialogService.ShowDialog("Ten tranh da ton tai");
                }
            });
        }


    }
}
