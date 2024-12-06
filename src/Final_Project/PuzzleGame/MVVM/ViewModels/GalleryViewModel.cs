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

        Connection connection = new Connection();
        public string SelectedPicUrl;
        public List<Picture> PictureList {  get; set; }

        Picture _selectedPicture;
        public Picture SelectedPicture
        {
            get { return _selectedPicture; }
            set
            {
                _selectedPicture = value;
                OnPropertyChanged("SelectedPicture");
            }
        }

        public RelayCommand<object> AddPicturePageOpenCommand { get; set; }

        public GalleryViewModel()
        {
            _wndBgr = defaultColornum1;
            PictureList = new List<Picture>();

            _loadPicListService.LoadPictureList(PictureList);

            AddPicturePageOpenCommand = new RelayCommand<object>((o) =>
            {
                CurrentPage = new AddPicturePageViewModel();
            });

        }
    }
}
