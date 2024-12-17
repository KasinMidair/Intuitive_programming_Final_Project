using Caliburn.Micro;
using MaterialDesignColors.ColorManipulation;
using PuzzleGame.Core;
using PuzzleGame.Core.Helper;
using PuzzleGame.MVVM.Models;
using PuzzleGame.MVVM.Views;
using PuzzleGame.MVVM.Views.Pages;
using PuzzleGame.Stores;
using System.ComponentModel;
using System.Security.Cryptography.Xml;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Navigation;
using System.Windows.Threading;




namespace PuzzleGame.MVVM.ViewModels
{

    public class PlayViewModel : ObservableObject
    {
        //Service and EventAggreator
        public NavigationService _navigationService;

        //Command
        public RelayCommand<object> SettingCommand { get; set; }
        public RelayCommand<object> GoBackCommand { get; set; }
        public RelayCommand<object> GoForwardCommand { get; set; }
        public RelayCommand<object> ShutdownCommand { get; set; }
        public RelayCommand<object> Mute_UnMuteCommand { get; set; }
        public RelayCommand<object> GoToMainMenuCommand { get; set; }
        public RelayCommand<object> BackgroundMusic0Command { get; set; }
        public RelayCommand<object> BackgroundMusic1Command { get; set; }
        public RelayCommand<object> BackgroundMusic2Command { get; set; }

        private double sfxVolume;
        public double SFXVolume
        {
            get => sfxVolume;
            set
            {
                sfxVolume = value;
                OnPropertyChanged();
                MusicSystemService.Instance.SetVolume(AudioType.SFX_MSC, value / 50.0, value / 50.0);
            }
        } 
        
        private double backgroundVolume;
        public double BackgroundVolume
        {
            get => backgroundVolume;
            set
            {
                backgroundVolume = value;
                OnPropertyChanged();
                MusicSystemService.Instance.SetVolume(AudioType.BACKGROUND_MSC, value / 50.0, value / 50.0);
            }
        }

        bool isGoBack;
        public bool IsGoBack 
        {
            get => isGoBack;
            set 
            {
                isGoBack = value;
                OnPropertyChanged();   
            }
        }

        bool isGoForward;
        public bool IsGoForward
        {
            get => isGoForward;
            set
            {
                isGoForward = value;
                OnPropertyChanged();
            }
        }

        string isMusicMute;  
        public string IsMusicMute
        {
            get => isMusicMute;
            set
            {
                isMusicMute = value;
                OnPropertyChanged();
            }
        }

        private bool isSettingVisible;
        public bool IsSettingVisible
        {
            get => isSettingVisible; 
            set
            {
                isSettingVisible = value;
                OnPropertyChanged();
            }
        }
        
        public SolidColorBrush WndBgr
        {
            get => _wndBgr;
            set
            {
                _wndBgr = value;
                OnPropertyChanged();
            }
        }
        ObservableObject _currentPage;
        public ObservableObject CurrentPage
        {
            get=>_currentPage;
            set
            {
                _currentPage = value;
                WndBgr = _currentPage.GetWndBgr();
                ToolBarUpdateColor();
            }
        } //  the page show in Frame in realtime


        SolidColorBrush toolBarColor;
        public SolidColorBrush ToolBarColor
        {
            get => toolBarColor;
            set
            {
                toolBarColor = value;
                OnPropertyChanged();
            }
        }


        public PlayViewModel()
        {
            //Message subcriber
            EventAggregator.GetEvent<PubSubEvent<ObservableObject>>().Subscribe((o) => OnCurrentPageChanged(o));
            EventAggregator.GetEvent<PubSubEvent<GameStatus>>().Subscribe((o) => Appstatus(o));

            MusicSystemService.Instance.ChangeBackgroundMusic(2);

            toolBarColor = defaultColornum2;
            _wndBgr = defaultColornum1;
            IsSettingVisible = true;
            IsGoBack = false;
            IsGoForward = false;
            IsMusicMute = "Mute";
            BackgroundVolume = 25;
            SFXVolume = 25;

            //init command 
            SettingCommand = new RelayCommand<object>((o)=> { SettingMenuStatus(); });
            ShutdownCommand = new RelayCommand<object>((o) => {  QuitApp(); });
            GoBackCommand = new RelayCommand<object>((o) => { GoBackPage(); });
            GoForwardCommand = new RelayCommand<object>((o) => { GoForwardPage(); });
            Mute_UnMuteCommand = new RelayCommand<object>((o) => { Mute_UnMute(); });
            GoToMainMenuCommand = new RelayCommand<object>((o) => { GoToMainMenu(); });
            BackgroundMusic0Command = new RelayCommand<object>((o) => { MusicSystemService.Instance.ChangeBackgroundMusic(0); });
            BackgroundMusic1Command = new RelayCommand<object>((o) => { MusicSystemService.Instance.ChangeBackgroundMusic(1); });
            BackgroundMusic2Command = new RelayCommand<object>((o) => { MusicSystemService.Instance.ChangeBackgroundMusic(2); });
        }
        private void GoToMainMenu()
        {
            SettingMenuStatus();
            while (_navigationService.CanGoBack) { GoBackPage(); }
             AvoidNavigate();
        }
        private void Mute_UnMute()
        {
            if (IsMusicMute=="UnMute")
            {
                MusicSystemService.Instance.SetVolume(AudioType.ALL, 1, 1);
                IsMusicMute = "Mute";
            }
            else 
            {
                MusicSystemService.Instance.SetVolume(AudioType.ALL, 0, 0);
                IsMusicMute = "UnMute";
            } 
            
        }

        // Execute RelayCommand
        private void SettingMenuStatus() 
        { 
            IsSettingVisible = !IsSettingVisible;
            EventAggregator.GetEvent<PubSubEvent<bool>>().Publish(IsSettingVisible);
        }
        private void QuitApp()
        {
            MusicSystemService.Instance.Dispose();
            Application.Current.Shutdown();
        
        }

        public void  GoBackPage()
        {
            _navigationService.GoBack();
            IsGoBack = _navigationService.CanGoBack;
            IsGoForward = true;
           Application.Current.Dispatcher.InvokeAsync(() =>
           {
                CurrentPage = (ObservableObject)_navigationService.Content;
           });
        }

        private void GoForwardPage()
        {
            _navigationService.GoForward();
            IsGoForward = _navigationService.CanGoForward;
            IsGoBack = true;

            Application.Current.Dispatcher.InvokeAsync(() =>
            {
                CurrentPage = (ObservableObject)_navigationService.Content;
            });

        }

        public void AvoidNavigate()
        {
            IsGoForward=false;
            IsGoBack=false;
            _navigationService.RemoveBackEntry();
        }


        /// <summary>
        /// handle msg giving by Aggreator.Navigation to UserNameEnter Page 
        /// </summary>
        /// <param name="page"></param>
        private void OnCurrentPageChanged(ObservableObject page)
        {

            FrameNavigation(page);

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="status"></param>
        private void Appstatus(GameStatus status)
        {
            if (status == GameStatus.QuitApp);
            QuitApp();
        }


        //change toolbar color 
        private void ToolBarUpdateColor()=> ToolBarColor = (WndBgr.Color.IsDarkColor()) ? defaultColornum2 : defaultColornum1;

        /// <summary>
        /// Naviagte to "cur" page
        /// /// </summary>
        /// <param name="cur"></param>
        public void FrameNavigation(ObservableObject cur)
        {
            _navigationService.Navigate(cur);
            CurrentPage = cur;
            if (_currentPage is GameRoundViewModel)
            {
                AvoidNavigate();
            }
            else
            {
                IsGoBack = true;
                IsGoForward = false;
            }
        }
        
    }
}
