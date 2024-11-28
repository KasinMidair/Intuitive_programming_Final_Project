using System;
using System.Collections.Generic;
using System.IO.Enumeration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Microsoft.Win32;
using PuzzleGame.Core;
using PuzzleGame.Core.Helper;
using PuzzleGame.MVVM.Models;

namespace PuzzleGame.MVVM.ViewModels
{
    class AddPicturePageViewModel: ObservableObject
    {
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
        public RelayCommand<object> ChangeNewPictureNameCommand { get; set; }

        public AddPicturePageViewModel()
        {
            _wndBgr = defaultColornum1;
            NewPicUrl = @"/Assets/Imgs/AddPicBackGround.jpg";
            NewPicName = new string("Hahaha");

            ChoosePictureCommand = new RelayCommand<object>((o) =>
            {
                OpenFileDialog dialog = new OpenFileDialog();
                dialog.Filter = "All Image Files| *.jpg; *.png; *.jpeg; *.bmp; *.gif;";

                if (dialog.ShowDialog() == true)
                {
                    NewPicUrl = dialog.FileName.ToString();
                }
            });

            ChangeNewPictureNameCommand = new RelayCommand<object>((o) =>
            {



            });
        }
    }
}
