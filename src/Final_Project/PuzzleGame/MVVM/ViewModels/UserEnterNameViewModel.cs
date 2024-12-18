using PuzzleGame.Core;
using PuzzleGame.Core.Helper;
using PuzzleGame.MVVM.Models;
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


        string _playerName {  get; set; }
        public string PlayerName
        {
            get => _playerName;
            set
            {
                _playerName = value;
                OnPropertyChanged(nameof(PlayerName));
            }
        }

        string _playerPassword {  get; set; }
        public string PlayerPassword
        {
            get => _playerPassword;
            set
            {
                _playerPassword = value;
                OnPropertyChanged(nameof(PlayerPassword));
            }
        }

        List<Player> PlayerList = new List<Player>();


        public RelayCommand<object> EnterNameCommand { get; set; } 
        public RelayCommand<object> AddPlayerCommand { get; set; }

        
        public UserEnterNameViewModel()
        {
            _wndBgr = defaultColornum2;

            PlayerName = string.Empty;
            PlayerPassword = string.Empty;

            EnterNameCommand = new RelayCommand<object>(o => { CurrentPage = new LevelSelectionViewModel(); });
            AddPlayerCommand = new RelayCommand<object>(o => { });
        }

        bool checkName(string name)
        {
            foreach (Player player in PlayerList) 
                if (player.Name == name) 
                    return true;
            return false;
        }
        
        bool checkPlayer(string name, string password)
        {
            foreach (Player player in PlayerList)
                if (player.Name == name && player.Password == password) 
                    return true;
            return false;
        }

        void addPlayer(string name, string password)
        {
            if (checkName(name) == true) 
            {
                MessageBox.Show("Ten nguoi choi da ton tai");
            }
            else
            {
                //them nguoi choi
            }
        }

        void Enter(string name, string password)
        {
            if (checkPlayer(name, password) == true)
            {
                CurrentPage = new LevelSelectionViewModel();
            }
            else
            {
                MessageBox.Show("Nguoi choi khong ton tai...");
            }
        }
    }
}
