using PuzzleGame.Core;
using PuzzleGame.Core.Helper;
using PuzzleGame.MVVM.Models;
using PuzzleGame.MVVM.Views.Pages;
using PuzzleGame.Stores;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace PuzzleGame.MVVM.ViewModels
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

        string _numberOfPieces;
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
                    LeaderBoardService.Instance.GameRoundsList.Clear();
                    gameRoundsList.Clear();
                    LeaderBoardService.Instance.LoadGameRoundsWithPieces(gameRoundsList, _numberOfPieces);
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
        }
    }
}