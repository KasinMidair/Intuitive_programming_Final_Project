using Caliburn.Micro;
using MaterialDesignColors.ColorManipulation;
using PuzzleGame.Core;
using PuzzleGame.Core.Helper;
using PuzzleGame.MVVM.Models;
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
        public RelayCommand<FrameworkElement> SettingCommand { get; set; }
        public RelayCommand<object> GoBackCommand { get; set; }
        public RelayCommand<object> GoForwardCommand { get; set; }
        public RelayCommand<object> ShutdownCommand { get; set; }



        GameRoundViewModel player;
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

            MusicSystemService.Instance.ChangeBackgroundMusic();

            toolBarColor = defaultColornum2;
            _wndBgr = defaultColornum1;
            IsSettingVisible = true;
            IsGoBack = false;
            IsGoForward = false;

            //init command 
            SettingCommand = new RelayCommand<FrameworkElement>((SettingMenu)=> { SettingMenuStatus(); });
            ShutdownCommand = new RelayCommand<object>((o) => {
                Task.Run(() =>
                {
                    var tcs = new TaskCompletionSource<bool>();
                    MusicSystemService.Instance.PlayBTN_ClickSound();
                    tcs.Task.Wait();
                });
                    QuitApp();
            });
            GoBackCommand = new RelayCommand<object>((o) => { GoBackPage(); });
            GoForwardCommand = new RelayCommand<object>((o) => { GoForwardPage(); }); 


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
            IsGoForward= true;
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
