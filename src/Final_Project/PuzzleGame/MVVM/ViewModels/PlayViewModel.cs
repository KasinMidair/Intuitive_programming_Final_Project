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
        public readonly CusDialogService _dialogService;



        //Command
        public RelayCommand<FrameworkElement> SettingCommand { get; set; }
        public RelayCommand<object> GoBackCommand { get; set; }
        public RelayCommand<object> GoForwardCommand { get; set; }
        public RelayCommand<object> ShutdownCommand { get; set; }
        public RelayCommand<Window> CloseDialogCommand { get; set; }
        public RelayCommand<Window> MoveWndCommand { get; set; }
        public RelayCommand<string> ShowMSGBoxCommand { get; set; }


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


        ObservableObject _currentPage;
        public ObservableObject CurrentPage
        {
            get=>_currentPage;
            set
            {
                _currentPage = value;
                ToolBarUpdateColor(_currentPage);
            }
        } //  the page show in Frame in realtime

        bool isSettingVisible;
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
            EventAggregator.GetEvent<PubSubEvent<string>>().Subscribe((o) => ShowCustomDialog(o));


            _dialogService = new CusDialogService();

            toolBarColor = defaultColornum2;
            _wndBgr = defaultColornum1;
            IsSettingVisible = true;
            IsGoBack = false;
            IsGoForward = false;

            //init command 
            SettingCommand = new RelayCommand<FrameworkElement>((SettingMenu)=>{ SettingMenuStatus(); });
            ShutdownCommand = new RelayCommand<object>((o) => {  QuitApp(); });
            GoBackCommand = new RelayCommand<object>((o) =>{ GoBackPage(); });
            GoForwardCommand = new RelayCommand<object>((o) =>{ GoForwardPage(); }); 
            CloseDialogCommand = new RelayCommand<Window>((o) =>{  CloseDialog(o); });
            MoveWndCommand = new RelayCommand<Window>((o) =>{   MoveWnd(o); });
            ShowMSGBoxCommand = new RelayCommand<string>((o) => { ShowCustomDialog(o); });

        }



        // Execute RelayCommand
        private void SettingMenuStatus() => IsSettingVisible = !IsSettingVisible;
        private void QuitApp() => Application.Current.Shutdown();
        public void CloseDialog(Window w) => w.Close();
        public void MoveWnd(Window wnd) => wnd.DragMove();
        private void ShowCustomDialog(string o) => _dialogService.ShowDialog(o);


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
        private void ToolBarUpdateColor(ObservableObject page )=> ToolBarColor = (page._wndBgr.Color.IsDarkColor()) ? defaultColornum2 : defaultColornum1;

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
