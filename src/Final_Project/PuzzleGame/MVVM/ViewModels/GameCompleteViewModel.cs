using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Navigation;
using PuzzleGame.Core;
using PuzzleGame.Core.Helper;

namespace PuzzleGame.MVVM.ViewModels
{
    public class GameCompleteViewModel : ObservableObject
    {

        public RelayCommand<object> GoToMainMenuCommand { get; set; }
        public RelayCommand<object> PlayAgainCommand { get; set; }
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
        private string _time;
        public string time
        {
            get => _time;
            set
            {
                if (_time != value) {
                    _time = value;
                    OnPropertyChanged();
                }
            }
        }
        public GameCompleteViewModel()
        {
            _wndBgr = defaultColornum2;
            GoToMainMenuCommand = new RelayCommand<object>((o) => { EventAggregator.GetEvent<PubSubEvent<string>>().Publish("Main menu"); });
            PlayAgainCommand = new RelayCommand<object>((o) => { CurrentPage = new LevelSelectionViewModel(); });
        }
    }
}
