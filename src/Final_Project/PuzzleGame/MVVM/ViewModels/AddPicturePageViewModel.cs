using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PuzzleGame.Core;

namespace PuzzleGame.MVVM.ViewModels
{
    class AddPicturePageViewModel: ObservableObject
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

        string _name = "HaHaHa";
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged("Name");
            }
        }

        public AddPicturePageViewModel()
        {
            _wndBgr = defaultColornum1;
        }
    }
}
