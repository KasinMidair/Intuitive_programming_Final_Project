using SlideFun.Core;
using SlideFun.Core.Helper;
using SlideFun.Stores;
using System.Collections.ObjectModel;
using System.ComponentModel;


namespace SlideFun.MVVM.ViewModels
{
    public class LeaderBoardViewModel : ObservableObject, INotifyPropertyChanged
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
        private string _playerIDToSearch;
        public string PlayerIDToSearch
        {
            get => _playerIDToSearch;
            set
            {
                _playerIDToSearch = value;
                OnPropertyChanged();
            }
        }
        string _numberOfPieces;
        public RelayCommand<object> SearchPlayerIDCommand { get; set; }
        public RelayCommand<object> GoBackCommand { get; set; }
        public string NumberOfPieces
        {
            get => _numberOfPieces;
            set
            {
                value = value.Substring(value.Length - 5);
                if (_numberOfPieces != value)
                {
                    _numberOfPieces = value;
                    OnPropertyChanged();
                    LeaderBoardService.Instance.LoadGameRounds(gameRoundsList, NumberOfPieces);
                }
            }
        }
        public ObservableCollection<Models.GameRound> gameRoundsList
        {
            get => LeaderBoardService.Instance.GameRoundsList;
            set
            {
                LeaderBoardService.Instance.GameRoundsList = value;
                OnPropertyChanged(nameof(gameRoundsList));
            }
        }

        public LeaderBoardViewModel()
        {
            _wndBgr = defaultColornum2;
            SearchPlayerIDCommand = new RelayCommand<object>(o => { LeaderBoardService.Instance.LoadGameRounds(gameRoundsList, NumberOfPieces, PlayerIDToSearch);});
            GoBackCommand = new RelayCommand<object>(o => { EventAggregator.GetEvent<PubSubEvent<string>>().Publish("Go back"); });
        }
    }
}