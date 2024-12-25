using PuzzleGame.Core;
using PuzzleGame.MVVM.Models;
using PuzzleGame.Stores;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuzzleGame.MVVM.ViewModels
{
    public class LeaderBoardViewModel : ObservableObject
    {
        public readonly LeaderBoardService _leaderBoardService = new LeaderBoardService();

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

        public ObservableCollection<GameRound> GameRounds;


        public LeaderBoardViewModel()
        {
            GameRounds = new ObservableCollection<GameRound>();

            _leaderBoardService.LoadGameRounds(GameRounds);
        }
    }
}