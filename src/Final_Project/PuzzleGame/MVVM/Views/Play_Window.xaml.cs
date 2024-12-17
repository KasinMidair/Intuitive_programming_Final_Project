using MaterialDesignThemes.Wpf;
using PuzzleGame.MVVM.ViewModels;
using PuzzleGame.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging; 
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using PuzzleGame.MVVM.Models;

namespace PuzzleGame.MVVM.Views
{
    public partial class Play_Window : Window
    {

        public Play_Window()
        {
            var vm = new PlayViewModel();
            this.DataContext = vm;

            InitializeComponent();
            
            //Setup NavigationServices
            ((PlayViewModel)this.DataContext)._navigationService = MainFrame.NavigationService;
            ((PlayViewModel)this.DataContext)._navigationService.Navigate(new MainMenuViewModel());
            MainFrame.Focusable = false;

        }


    }
}
