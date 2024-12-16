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
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Globalization;
using System.Windows.Controls;

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

        public RelayCommand<object> OpenGalleryCommand { get; set; } //command for Gallery button to open Gallery
        public RelayCommand<object> SelectPictureCommand { get; set; }
        public RelayCommand<object> PlayCommand { get; set; } //command for Play button to start game round


        //Connection connection = new Connection();
        public ObservableCollection<Picture> PictureList { get; set; }

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

        string _hour, _minute, _second;
        public string Hour
        {
            get => _hour;
            set
            {
                _hour = value;
                OnPropertyChanged(nameof(Hour));
            }
        }
        public string Minute
        {
            get => _minute;
            set
            {
                _minute = value;
                OnPropertyChanged(nameof(Minute));
            }
        }
        public string Second
        {
            get => _second;
            set
            {
                _second = value;
                OnPropertyChanged();
            }
        }


        public LevelSelectionViewModel()
        {
            _wndBgr = defaultColornum2;
            PictureList = new ObservableCollection<Picture>();
            _loadPicListService.LoadPictureList(PictureList, "HOAI");
            // _loadPicListService.LoadPictureList(PictureList);

            OpenGalleryCommand = new RelayCommand<object>((o) => {CurrentPage = new GalleryViewModel();});

            PlayCommand = new RelayCommand<object>((o) => 
            {
                CurrentPage = new GameRoundViewModel();
            });
        }
    }
}
