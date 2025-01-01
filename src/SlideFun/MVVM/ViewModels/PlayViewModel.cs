using Caliburn.Micro;
using MaterialDesignColors.ColorManipulation;
using SlideFun.Core;
using SlideFun.Core.Helper;
using SlideFun.MVVM.Models;
using SlideFun.Stores;
using System.Windows;
using System.Windows.Media;
using System.Windows.Navigation;




namespace SlideFun.MVVM.ViewModels
{

    public class PlayViewModel : ObservableObject
    {
        //Service and EventAggreator
        public NavigationService _navigationService;

        #region Command
        public RelayCommand<object> SettingCommand { get; set; }
        public RelayCommand<object> GoBackCommand { get; set; }
        public RelayCommand<object> GoForwardCommand { get; set; }
        public RelayCommand<object> ShutdownCommand { get; set; }
        public RelayCommand<object> Mute_UnMuteCommand { get; set; }
        public RelayCommand<object> GoToMainMenuCommand { get; set; }
        public RelayCommand<object> BackgroundMusicCommand { get; set; }
        public RelayCommand<object> CopyIdCommand { get; set; }
        public RelayCommand<object> LeaderboardCommand { get; set; }
        #endregion

        private double sfxVolume;
        public double SFXVolume
        {
            get => sfxVolume;
            set
            {
                sfxVolume = value;
                VolumeChanged(value,false);
                OnPropertyChanged();
                
            }
        } 
        
        private double backgroundVolume;
        public double BackgroundVolume
        {
            get => backgroundVolume;
            set
            {
                backgroundVolume = value;
                VolumeChanged(value);
  
                
            }
        }
        #region  Binding Properties

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

        int mscSelectedOption;
        public int MscSelectedOption
        {
            get => mscSelectedOption;
            set
            {
                mscSelectedOption = value;
                OnPropertyChanged();
            }
        }
        string settingUserName;
        public string SettingUserName
        {
            get => settingUserName;
            set 
            {
                settingUserName = value;
                OnPropertyChanged();
            }
        }

        string settingUserId;
        public string SettingUserId
        {
            get => settingUserId;
            set
            {
                settingUserId= value;
                OnPropertyChanged();
            }
        }

        bool isLogin;
        public bool IsLogin
        {
            get => isLogin;
            set
            {
                isLogin = value;
                OnPropertyChanged();
            }
        }
        #endregion

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




        public PlayViewModel()
        {
            //Message subcriber
            EventAggregator.GetEvent<PubSubEvent<ObservableObject>>().Subscribe((o) => OnCurrentPageChanged(o));
            EventAggregator.GetEvent<PubSubEvent<GameStatus>>().Subscribe((o) => Appstatus(o));
            EventAggregator.GetEvent<PubSubEvent<string>>().Subscribe((o) => GoBackFromOtherView(o));
            MusicSystemService.Instance.ChangeBackgroundMusic(2);

            toolBarColor = defaultColornum2;

            _wndBgr = defaultColornum1;
            IsSettingVisible = true;
            IsGoBack = false;
            IsGoForward = false;
            IsMusicMute = "Mute";
            BackgroundVolume = Properties.Settings.Default.BGVolume;  //using application settings to save basic data.
            SFXVolume = Properties.Settings.Default.SFXVolume;
            MscSelectedOption = Properties.Settings.Default.BGMusicSong;
            MusicSystemService.Instance.ChangeBackgroundMusic(MscSelectedOption);

            #region init command 

            SettingCommand = new RelayCommand<object>((o) => { SettingMenuStatus(); });
            ShutdownCommand = new RelayCommand<object>((o) => { QuitApp(); });
            GoBackCommand = new RelayCommand<object>((o) => { GoBackPage(); });
            GoForwardCommand = new RelayCommand<object>((o) => { GoForwardPage(); });
            Mute_UnMuteCommand = new RelayCommand<object>((o) => { Mute_UnMute(); });
            GoToMainMenuCommand = new RelayCommand<object>((o) => { GoToMainMenu(); });
            BackgroundMusicCommand = new RelayCommand<object>((o) => { BackgroundMusic((string)o); });
            CopyIdCommand = new RelayCommand<object>((o) => { CopyId((string)o); });
            LeaderboardCommand = new RelayCommand<object>((o) => {
                FrameNavigation(new LeaderBoardViewModel());
                SettingMenuStatus();    
            });
            #endregion

        }

        #region Execute Command
        private void SettingMenuStatus() 
        { 
            IsSettingVisible = !IsSettingVisible;
            if (_currentPage is GameRoundViewModel)
            {
               EventAggregator.GetEvent<PubSubEvent<bool>>().Publish(IsSettingVisible); //if setting menu is shown,countdown is paused.
            }
        }

        private void QuitApp()
        {
            string msgText = "Are you sure about quitting the game ?";
            if (_currentPage is GameRoundViewModel && GameModel.Instance.Status==GameStatus.StartGame)
            {
                msgText = msgText + " (Your gameplay will not be saved.)";
            }
            CustomDialogResult rlt= CusDialogService.Instance.ShowDialog(msgText, true).Result;
            if (rlt == CustomDialogResult.Yes)
            {
                MusicSystemService.Instance.Dispose();
                Application.Current.Shutdown();
            }
        
        }
        public void GoBackFromOtherView(string subscribedString)
        {
            if (subscribedString == "Go back")
            {
                GoBackPage();
            }
        }
        public void  GoBackPage()
        {
            _navigationService.GoBack();
            
           Application.Current.Dispatcher.InvokeAsync(() =>
           {
                CurrentPage = (ObservableObject)_navigationService.Content;
               if (_currentPage is LevelSelectionViewModel)
               {
                   IsGoBack = false;
               }
               else IsGoBack = _navigationService.CanGoBack;
               IsGoForward = true;
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

        private void Mute_UnMute()
        {
            if (IsMusicMute == "UnMute")
            {

                MusicSystemService.Instance.SetVolume(AudioType.ALL, backgroundVolume / 50.0, sfxVolume / 50.0);

                IsMusicMute = "Mute";
            }
            else
            {
                MusicSystemService.Instance.SetVolume(AudioType.ALL, 0, 0);
                IsMusicMute = "UnMute";
            }

        }

        private void GoToMainMenu()
        {
            if (IsLogin)     //check player login 
            {
                CustomDialogResult rlt = CusDialogService.Instance.ShowDialog("Do you really want to log out?", true).Result;
             
                if (rlt == CustomDialogResult.No)
                {

                    return;
                }
            }
            GameModel.Instance.Player = null;
            FrameNavigation(new MainMenuViewModel());
            AvoidNavigate();
            SettingMenuStatus();

        }
        private void BackgroundMusic(string num)
        {
            int.TryParse(num, out int mscNum);
            Properties.Settings.Default.BGMusicSong=mscNum;
            Properties.Settings.Default.Save();
            MusicSystemService.Instance.ChangeBackgroundMusic(mscNum);
        }

        private void CopyId(string o) 
        {
            Clipboard.SetText(o);
            _= CusDialogService.Instance.ShowDialog("You have copied the ID<3");
        }

        #endregion


        public void VolumeChanged(double value,bool isbgms=true)
        {
            if (isbgms)
            {
                Properties.Settings.Default.BGVolume = value;
                Properties.Settings.Default.Save();
                MusicSystemService.Instance.SetVolume(AudioType.BACKGROUND_MSC, value / 50.0, value / 50.0);
            }
            else 
            {
                Properties.Settings.Default.SFXVolume = value;
                Properties.Settings.Default.Save();
                MusicSystemService.Instance.SetVolume(AudioType.SFX_MSC, value / 50.0, value / 50.0);
            }
        }

        public void AvoidNavigate()
        {
            _navigationService.RemoveBackEntry();
            IsGoForward =false;
            IsGoBack=false;

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


            //user info setting
            if (GameModel.Instance.Player != null)
            {
                SettingUserId = GameModel.Instance.Player.Id;
                SettingUserName = GameModel.Instance.Player.Name;
                IsLogin = true;
            }
            else
            {
                SettingUserId = "??????";
                SettingUserName = "?????????";
                IsLogin = false;
            }


            // back/foward status setting
            _navigationService.Navigate(cur);
            Application.Current.Dispatcher.InvokeAsync(() =>
            {
                CurrentPage = (ObservableObject)_navigationService.Content;
                if (_currentPage is MainMenuViewModel || _currentPage is GameRoundViewModel || _currentPage is LevelSelectionViewModel)
                {
                    AvoidNavigate();
                }
                else IsGoBack = _navigationService.CanGoBack;
                IsGoForward = false;
            });

           


        }
        
    }
}
