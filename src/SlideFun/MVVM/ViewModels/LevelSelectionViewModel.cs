using Caliburn.Micro;
using SlideFun.Core;
using SlideFun.Core.Helper;
using SlideFun.MVVM.Models;
using SlideFun.MVVM.ViewModels;
using SlideFun.Stores;
using System.Collections.ObjectModel;

namespace SlideFun.MVVM.ViewModels
{
    public class LevelSelectionViewModel : ObservableObject
    {

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
        public ObservableCollection<Picture> pictureList = new ObservableCollection<Picture>();
        public ObservableCollection<Picture> PictureList
        {
            get => pictureList;
            set
            {
                pictureList = value;
                OnPropertyChanged();
            }
        }

        Picture? _selectedPicture;
        public Picture? SelectedPicture
        {
            get { return _selectedPicture; }
            set
            { 
                _selectedPicture = value;
                OnPropertyChanged();
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
        private bool timerChecked;

        public bool TimerChecked
        {
            get => timerChecked; 
            set
            {
                timerChecked = value; 
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
            NumberOfPieces = "3";
            timerChecked = false;
            _hour = _minute = _second = "00";
            _wndBgr = defaultColornum2;
            PictureList = new ObservableCollection<Picture>();
            EventAggregator.GetEvent<PubSubEvent<string>>().Subscribe((o) => LoadPicTureList(o));
            LoadPictureListService.Instance.LoadPictureList(PictureList, GameModel.Instance.Player.Id);
            // _loadPicListService.LoadPictureList(PictureList);

            SelectedPicture = PictureList.ElementAt(0);
            OpenGalleryCommand = new RelayCommand<object>((o) => {CurrentPage = new GalleryViewModel();});
            PlayCommand = new RelayCommand<object>((o) => {   SetGameRoundData((bool)o);    });
        }

        //set the selectionPicture is PictureList[0] if you add/delete pictures
        private void LoadPicTureList(string o)
        {
            LoadPictureListService.Instance.LoadPictureList(PictureList, GameModel.Instance.Player.Id);
            SelectedPicture = PictureList.ElementAt(0);
        }


        /// <summary>
        /// set data for a game round and navigate to it
        /// </summary>
        /// <param name="isCounting"></param>
        public void SetGameRoundData(bool isCounting)
        {
            int piecesNum = int.Parse(NumberOfPieces[NumberOfPieces.Length - 1].ToString());
            //check if countdown timer is on
            long PlayTime = (isCounting)? (long.Parse(_hour)*60*60+ int.Parse(_minute)*60+ int.Parse(_second)):0;
            GameModel.Instance.SetData(piecesNum, piecesNum, SelectedPicture.Url, PlayTime);
            GameModel.Instance.PicResize();
            CurrentPage = new GameRoundViewModel();
        }
    }
}
