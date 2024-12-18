﻿using System;
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
    class AddPictureViewModel: ObservableObject
    {
        public readonly CusDialogService _cusDialogService = new CusDialogService();
        public readonly GalleryViewModel _galleryViewModel = new GalleryViewModel();
        public readonly LoadPictureListService _loadPictureListService = new LoadPictureListService();

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


        public string path { get; set; }


        public AddPictureViewModel()
        {

            _wndBgr = defaultColornum1;
            NewPicUrl = @"/Assets/Imgs/AddPicBackGround.jpg";

            ChoosePictureCommand = new RelayCommand<object>((o) =>{ ChoosePic(); });

            AddPictureCommand = new RelayCommand<object>((o) =>{ AddPic("HOAI"); });
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

        void AddPic (string PlayerName)
        {
            bool check = false;

            if (NewPicUrl == null || NewPicUrl == @"/Assets/Imgs/AddPicBackGround.jpg")
            {
                MessageBox.Show("Chua chon tranh...");
                check = true;
            }
            else if (NewPicName == null || NewPicName == "")
            {
                MessageBox.Show("Chua nhap ten tranh...");
                check = true;
            }
            else
            {
                foreach (Picture pic in _galleryViewModel.PictureList)
                {
                    if (NewPicName == pic.Name)
                    {
                        MessageBox.Show("ten tranh da ton tai...");
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
                    Url = Path.Combine(Directory.GetCurrentDirectory().Remove(Directory.GetCurrentDirectory().Length - 24, 24) + $"\\Assets\\picture\\{PlayerName}{NewPicName}.png")
                };

                File.Copy(NewPicUrl, NewPic.Url, true);

                /*string projectDirectory = AppDomain.CurrentDomain.BaseDirectory;
                string assetsDirectory = Path.Combine(projectDirectory, "/Assets/picture/");
                string destinationPath = Path.Combine(assetsDirectory, $"{NewPicName}.png"); 
                if (!Directory.Exists(assetsDirectory))
                { 
                    Directory.CreateDirectory(assetsDirectory); 
                }
                File.Copy(NewPicUrl, destinationPath, true);*/

                _loadPictureListService.AddPicture(NewPic, "HOAI");
                _galleryViewModel._loadPicListService.LoadPictureList(_galleryViewModel.PictureList, "HOAI");
                MessageBox.Show("Da luu tranh... chac vay");
            }
        }


    }
}