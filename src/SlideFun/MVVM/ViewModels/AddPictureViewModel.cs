using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using SlideFun.Core;
using SlideFun.Core.Helper;
using SlideFun.MVVM.Models;
using SlideFun.Stores;

namespace SlideFun.MVVM.ViewModels
{
    class AddPictureViewModel: ObservableObject
    {
        ObservableCollection<Picture> _pictureList;

        Connection conn = new Connection();

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
        public RelayCommand<object> CloseAddWindowCommand { get; set; }
        public RelayCommand<object> MoveWndCommand { get; set; }


        public string path { get; set; }


        public AddPictureViewModel(ObservableCollection<Picture> pictureList)
        {
            _pictureList = pictureList;
            _wndBgr = defaultColornum1;
            NewPicUrl = @"/Assets/Imgs/AddPicBackGround.jpg";

            ChoosePictureCommand = new RelayCommand<object>((o) =>{ ChoosePic(); });
            AddPictureCommand = new RelayCommand<object>((o) =>{ AddPic(GameModel.Instance.Player.Id); });
            CloseAddWindowCommand = new RelayCommand<object>((o) => { CusDialogService.Instance.CloseDialog("AddPic");});
            MoveWndCommand = new RelayCommand<object>((o) => { CusDialogService.Instance.MoveDialog(false); });
        }

        void ChoosePic()
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "All Image Files| *.jpg; *.png; *.jpeg; *.bmp; *.gif;";

            if (dialog.ShowDialog() == true)
            {
                NewPicUrl = dialog.FileName;
                newBitmap = new BitmapImage(new Uri(NewPicUrl, UriKind.Absolute));
            }
        }

        void AddPic (string playerID)
        {
            bool check = false;

            if (NewPicUrl == null || NewPicUrl == @"/Assets/Imgs/AddPicBackGround.jpg")
            {
                _ = CusDialogService.Instance.ShowDialog("You have not selected an image yet..!");
                check = true;
            }
            else if (NewPicName == null || NewPicName == "")
            {
                _ = CusDialogService.Instance.ShowDialog("The Image name not entered yet.!");
                check = true;
            }

            if (check == false)
            {
                NewPic = new Picture
                {
                    Name = NewPicName,
                    Url = Path.Combine(Directory.GetCurrentDirectory().Remove(Directory.GetCurrentDirectory().Length - 24, 24) + $"\\Assets\\picture\\{playerID}{NewPicName}.png")
                };

                File.Copy(NewPicUrl, NewPic.Url, true);

                string projectDirectory = AppDomain.CurrentDomain.BaseDirectory;
                string assetsDirectory = Path.Combine(projectDirectory, "/Assets/picture/");
                string destinationPath = Path.Combine(assetsDirectory, $"{NewPicName}.png"); 
                if (!Directory.Exists(assetsDirectory))
                { 
                    Directory.CreateDirectory(assetsDirectory);
                }
                File.Copy(NewPicUrl, destinationPath, true);

                LoadPictureListService.Instance.AddPicture(NewPic, GameModel.Instance.Player.Id);
                EventAggregator.GetEvent<PubSubEvent<string>>().Publish(""); 
                
                _ = CusDialogService.Instance.ShowDialog("Picture is saved!...I think so:)))");
            }
        }


    }
}
