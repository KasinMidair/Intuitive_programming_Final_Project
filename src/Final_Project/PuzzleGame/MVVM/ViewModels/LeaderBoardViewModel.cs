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
    public class LeaderBoardViewModel : ObservableObject
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
        Connection connection = new Connection();

        public ObservableCollection<Models.GameRound> gameRoundsList
        {
            get => LeaderBoardService.Instance.GameRoundsList;
            set
            {
                LeaderBoardService.Instance.GameRoundsList = value;
                OnPropertyChanged();
            }
        }

        public LeaderBoardViewModel()
        {
            _wndBgr = defaultColornum2;
            LeaderBoardService.Instance.LoadGameRounds(gameRoundsList);
        }
    }
}