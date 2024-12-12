using PuzzleGame.Core;
using PuzzleGame.Core.Helper;
using System.Windows;

namespace PuzzleGame.MVVM.ViewModels
{
    public class UserEnterNameViewModel: ObservableObject
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
        public RelayCommand<object> EnterNameCommand { get; set; } 

        
        public UserEnterNameViewModel()
        {
            _wndBgr = defaultColornum2;
            EnterNameCommand = new RelayCommand<object>(o => { CurrentPage = new LevelSelectionViewModel(); });
        }
    }
}
