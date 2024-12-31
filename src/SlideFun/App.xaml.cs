using SlideFun.MVVM.ViewModels;
using SlideFun.MVVM.Views;
using SlideFun.MVVM.Views.Pages;
using SlideFun.Stores;
using System.Configuration;
using System.Data;
using System.Windows;


namespace SlideFun
{
    public partial class App : Application
    {
        public App()
        {
        }
        protected override void OnStartup(StartupEventArgs e)
        {

            Play_Window play_Window = new Play_Window();
            play_Window.Show();
            base.OnStartup(e);
        }
    }

}
