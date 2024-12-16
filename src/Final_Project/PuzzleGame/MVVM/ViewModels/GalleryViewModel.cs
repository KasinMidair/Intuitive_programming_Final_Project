using PuzzleGame.MVVM.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PuzzleGame.Core;
using MaterialDesignColors.ColorManipulation;
using PuzzleGame.Core.Helper;
using System.Windows;
using PuzzleGame.Stores;
using System.Security.Cryptography.Xml;
using Prism.Common;
using System.Collections.ObjectModel;
using System.IO;

namespace PuzzleGame.MVVM.ViewModels
{
    class GalleryViewModel : ObservableObject
    {
        public readonly LoadPictureListService _loadPicListService = new LoadPictureListService();
    
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

        //Connection...
        Connection connection = new Connection();
        public string SelectedPicUrl;

        //Pictures...
        ObservableCollection<Picture> _pictureList = new ObservableCollection<Picture>();
        public ObservableCollection<Picture> PictureList
        {
            get => _pictureList;
            set
            {
                _pictureList = value;
                OnPropertyChanged();
            }
        }


        Picture _selectedPicture;
        public Picture SelectedPicture
        {
            get { return _selectedPicture; }
            set
            {
                _selectedPicture = value;
                OnPropertyChanged();
            }
        }


        //Command...
        public RelayCommand<object> AddPicturePageOpenCommand { get; set; }
        public RelayCommand<Object> DeletePictureCommand { get; set; }

        public GalleryViewModel()
        {
            _wndBgr = defaultColornum2;
            PictureList = new ObservableCollection<Picture>();

            _loadPicListService.LoadPictureList(PictureList, "HOAI");

            AddPicturePageOpenCommand = new RelayCommand<object>((o) => 
            {
                CurrentPage = new AddPicturePageViewModel(); 
            });

            DeletePictureCommand = new RelayCommand<object>((o) => { DeleteSelectedPicture(); });
        }

        void DeleteSelectedPicture()
        {
            string n = SelectedPicture.Name;

            if (_loadPicListService.DeletePicture(SelectedPicture) == true)
            {
                string url = SelectedPicture.Url;

                // Đảm bảo SelectedPicture không còn được sử dụng
                SelectedPicture = null;

                for (int i = PictureList.Count - 1; i >= 0; i--)
                {
                    if (PictureList[i].Name == n)
                    {
                        PictureList.RemoveAt(i);
                        if (i == PictureList.Count - 1 && PictureList.Count > 0)
                            SelectedPicture = PictureList[i--];
                        else if (PictureList.Count > 0)
                            SelectedPicture = PictureList[i++];
                        break;
                    }
                }

                // Xóa tệp
                if (File.Exists(url))
                {
                    try
                    {
                        using (FileStream fs = new FileStream(url, FileMode.Open, FileAccess.Read, FileShare.None)) { }
                        File.Delete(url);
                        Console.WriteLine("File đã được xóa thành công.");
                    }
                    catch (IOException ex)
                    {
                        Console.WriteLine($"Lỗi khi xóa tệp: {ex.Message}");
                    }
                }
            }
        }

    }
}
