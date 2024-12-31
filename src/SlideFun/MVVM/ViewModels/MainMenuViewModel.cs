using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using SlideFun.Core;
using SlideFun.Core.Helper;
using Prism.Events;
using SlideFun.Stores;
using SlideFun.MVVM.Models;

namespace SlideFun.MVVM.ViewModels
{
    public class MainMenuViewModel : ObservableObject
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

        public RelayCommand<object> StartCommand { get; set; } //command for start button
        public RelayCommand<object> ShutdownCommand { get; set; } //command for Exit button
        public RelayCommand<string> LeaderBoardCommand { get; set; }// command for achievement button


        public MainMenuViewModel()
        {
            _wndBgr = defaultColornum1;
            StartCommand = new RelayCommand<object>((o) =>
            {
                CurrentPage = new UserEnterNameViewModel();

            });
            LeaderBoardCommand = new RelayCommand<string>((o) =>
            {
                CurrentPage = new LeaderBoardViewModel();
            });
            ShutdownCommand = new RelayCommand<object>((o) => 
            {
                EventAggregator.GetEvent<PubSubEvent<GameStatus>>().Publish(GameStatus.QuitApp);
            });
        }
    }
}
