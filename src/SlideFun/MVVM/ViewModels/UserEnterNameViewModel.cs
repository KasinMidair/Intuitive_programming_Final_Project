
using SlideFun.Core;
using SlideFun.Core.Helper;
using SlideFun.MVVM.Models;
using SlideFun.Stores;
using System.Net.Http.Headers;
using System.Windows;

namespace SlideFun.MVVM.ViewModels
{
    public class UserEnterNameViewModel : ObservableObject
    {
        public readonly PlayerService _playerService = new PlayerService();

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


        string _playerName { get; set; }
        string _playerPassword { get; set; }

        public string PlayerName
        {
            get => _playerName;
            set
            {
                _playerName = value;
                OnPropertyChanged(nameof(PlayerName));
            }
        }

        public string PlayerPassword
        {
            get => _playerPassword;
            set
            {
                _playerPassword = value;
                OnPropertyChanged(nameof(PlayerPassword));
            }
        }

        Player Player { get; set; }

        public RelayCommand<object> EnterNameCommand { get; set; }
        public RelayCommand<object> AddPlayerCommand { get; set; }


        public UserEnterNameViewModel()
        {
            _wndBgr = defaultColornum2;

            EnterNameCommand = new RelayCommand<object>(o => { Enter(PlayerName, PlayerPassword); });
            AddPlayerCommand = new RelayCommand<object>(o => { AddPlayer(PlayerName, PlayerPassword); });
        }


        void Enter(string name, string password)
        {
            if (name == null || name == string.Empty)
                _ = CusDialogService.Instance.ShowDialog("NAME not entered yet.!");
            else if (password == null || password == string.Empty)
                _ = CusDialogService.Instance.ShowDialog("PASSWORD not entered yet.!");
            else
            {
                if (_playerService.CheckPlayerName(name) != null)
                {
                    Player = _playerService.CheckPlayerName(name);

                    if (password != Player.Password)
                        _ = CusDialogService.Instance.ShowDialog("NAME or PASSWORD is incorrect.!");
                    else
                    {
                        GameModel.Instance.Player = this.Player;
                        CurrentPage = new LevelSelectionViewModel();
                    }
                }
                else
                    _ = CusDialogService.Instance.ShowDialog("NAME or PASSWORD is incorrect.!");
            }
        }

        void AddPlayer(string name, string password)
        {
            if (name == null || name == string.Empty)
                _ = CusDialogService.Instance.ShowDialog("NAME not entered yet.!");
            else if (password == null || password == string.Empty)
                _ = CusDialogService.Instance.ShowDialog("PASSWORD not entered yet.!");
            else
            {
                if (_playerService.CheckPlayerName(name) != null)
                    _ = CusDialogService.Instance.ShowDialog("The name already exists. Please enter a different name.");
                else
                {
                    _playerService.AddPlayer(name, password);
                    _ = CusDialogService.Instance.ShowDialog("New player added successfully.");
                }
                

            }
        }
    }
}