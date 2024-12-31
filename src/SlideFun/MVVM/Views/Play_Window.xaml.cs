using SlideFun.MVVM.ViewModels;
using System.Windows;

namespace SlideFun.MVVM.Views
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
