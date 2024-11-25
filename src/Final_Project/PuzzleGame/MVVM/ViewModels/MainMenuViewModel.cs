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
        public RelayCommand<object> StartCommand { get; set; } //command for start button
        public RelayCommand<object> OpenGalleryCommand { get; set; } //command for Gallery button

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
            _wndBgr = defaultColornum1;
            StartCommand = new RelayCommand<object>((o) =>
            {
                CurrentPage = new UserEnterNameViewModel();

            });

            OpenGalleryCommand = new RelayCommand<object>((o) =>
            {
                CurrentPage = new LevelSelectionViewModel();
            });
        }
    }
}
