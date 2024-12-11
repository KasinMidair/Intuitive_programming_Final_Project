using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PuzzleGame.Core;
using PuzzleGame.MVVM.Models;
using PuzzleGame.Core.Helper;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Caliburn.Micro;
using PuzzleGame.Stores;

namespace PuzzleGame.MVVM.ViewModels
{
    public class LevelSelectionViewModel : ObservableObject
    {
        public readonly LoadPictureListService _loadPicListService = new LoadPictureListService();

        private ObservableObject? _currentPage;
        public ObservableObject? CurrentPage
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

        public RelayCommand<object> OpenGalleryCommand { get; set; } //command for Gallery button
        public RelayCommand<object> SelectPictureCommand { get; set; }
        public RelayCommand<object> PlayCommand { get; set; }


        //Connection connection = new Connection();
        public List<Picture> PictureList { get; set; }
        public int hour, minute, second;

        Picture? _selectedPicture;
        public Picture? SelectedPicture
        {
            get { return _selectedPicture; }
            set
            { 
                _selectedPicture = value;
                OnPropertyChanged("SelectedPicture");
            }
        }

        string _numberOfPieces;
        public string NumberOfPieces
        {
            get => _numberOfPieces;
            set
            {
                _numberOfPieces = value;
                OnPropertyChanged();
            }
        }

        public LevelSelectionViewModel()
        {
            _wndBgr = defaultColornum1;
            PictureList = new List<Picture>();
           // _loadPicListService.LoadPictureList(PictureList);

            OpenGalleryCommand = new RelayCommand<object>((o) => {CurrentPage = new GalleryViewModel();});

            PlayCommand = new RelayCommand<object>((o) => {CurrentPage = new GameRoundViewModel();});
        }
    }
}
