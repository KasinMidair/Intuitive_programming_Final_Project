using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using PuzzleGame.Core;
using PuzzleGame.Core.Helper;
using Prism.Events;

namespace PuzzleGame.MVVM.ViewModels
{
    public class MainMenuViewModel : ObservableObject
    {

        private ObservableObject _currentPage;
        public RelayCommand<object> StartCommand { get; set; }
        public EventAggregator EventAggregator { get; set; } = (EventAggregator)Application.Current.Resources["AppEventAggregator"];

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
        public MainMenuViewModel()
        {

            StartCommand = new RelayCommand<object>((o) =>
            {
                CurrentPage = new UserEnterNameViewModel();

            });
        }



    }
}
