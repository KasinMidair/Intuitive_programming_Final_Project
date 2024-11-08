using PuzzleGame.MVVM.ViewModels;
using PuzzleGame.MVVM.Views;
using PuzzleGame.MVVM.Views.Pages;
using PuzzleGame.Stores;
using System.Configuration;
using System.Data;
using System.Windows;


namespace PuzzleGame
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
