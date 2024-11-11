using PuzzleGame.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;

namespace PuzzleGame.Stores
{
    public class Navigation
    {
        private ObservableObject _currentViewModel;

        public ObservableObject CurrentViewModel
        {
            get => _currentViewModel;
            set
            {
                _currentViewModel = value;
            }
        }
        public Navigation(ObservableObject CurrentViewModel=null)
        {
        }
    }
}