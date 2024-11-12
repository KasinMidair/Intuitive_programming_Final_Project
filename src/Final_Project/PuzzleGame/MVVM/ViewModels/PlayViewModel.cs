using PuzzleGame.Class_Cus;
using PuzzleGame.Core;
using PuzzleGame.Core.Helper;
using PuzzleGame.Stores;
using System.ComponentModel;
using System.Security.Cryptography.Xml;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Navigation;
using System.Windows.Threading;




namespace PuzzleGame.MVVM.ViewModels
{
    public class PlayViewModel : ObservableObject
    {

        SolidColorBrush toolBarColor;
        public NavigationService _navigationService;
        public readonly CusDialogService _dialogService;
        public RelayCommand<FrameworkElement> SettingCommand { get; set; }
        public RelayCommand<object> GoBackCommand { get; set; }

        public RelayCommand<object> ShutdownCommand { get; set; }
        public RelayCommand<Window> CloseDialogCommand { get; set; }
        public RelayCommand<Window> MoveWndCommand { get; set; }
        public RelayCommand<object> ShowMSGBoxCommand { get; set; }

        public EventAggregator EventAggregator { get; set; }

       

        bool isSettingVisible;
        DispatcherTimer _countDownClock;
        GameRound player;
        public SolidColorBrush _wndBgr { get; }

        public ObservableObject _currentPage;


        public bool IsSettingVisible
        {
            get => isSettingVisible;
            set
            {
                isSettingVisible = value;
                OnPropertyChanged();
            }
        }

        public SolidColorBrush ToolBarColor
        {
            get => toolBarColor;
            set
            {
                toolBarColor = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangingEventHandler? PropertyChanging;

        public PlayViewModel()
        {
            EventAggregator = (EventAggregator)Application.Current.Resources["AppEventAggregator"];
            EventAggregator.GetEvent<PubSubEvent<ObservableObject>>().Subscribe((o) => OnCurrentPageChanged(o));
            _dialogService = new CusDialogService();
            toolBarColor = new SolidColorBrush(defaultColornum1);
            // player = new GameRound(pnlcontainer, pnlGamePlaySpace);
            _countDownClock = new DispatcherTimer();
            _countDownClock.Interval = TimeSpan.FromSeconds(30);
            _wndBgr = new SolidColorBrush(Color.FromArgb(255, 21, 22, 28));
            //player.StartGame();
            _countDownClock.Start();

            isSettingVisible = false;

            SettingCommand = new RelayCommand<FrameworkElement>((SettingMenu)=>{
                SettingMenuStatus(); 
            });


            ShutdownCommand = new RelayCommand<object>((o) =>
            {
                QuitApp();

            });
            GoBackCommand = new RelayCommand<object>((o) =>
            {
                GoBackPage();
            }); 
            CloseDialogCommand = new RelayCommand<Window>((o) =>
            {
                CloseDialog(o);
            });
            MoveWndCommand = new RelayCommand<Window>((o) =>
            {
                MoveWnd(o);
            });
            ShowMSGBoxCommand = new RelayCommand<object>((o) =>
            {
                _dialogService.ShowDialog("hien roy ne");
            });

        }
        private void OnCurrentPageChanged(ObservableObject page)
        {
            var navPage= new UserEnterNameViewModel();
            _currentPage = navPage;
            _navigationService.Navigate(navPage);
        }
        
        
        public void MoveWnd(Window wnd) => wnd.DragMove();
        public void CloseDialog(Window w) => w.Close();
        public void GoBackPage() => _navigationService?.GoBack();
        private void QuitApp() => Application.Current.Shutdown();
        private void SettingMenuStatus() => IsSettingVisible = !IsSettingVisible;

        /// <summary>
        /// using key press event to mowing blackPiece around
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="keyData"></param>
        /// <returns></returns>
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (GameManager.Instance.Status == GameStatus.PreStart)
                return;
            int moveIndex = -1;
            Piece tmp = (Piece)player.imgPieces[player.BlackBox_Indx];
            switch (e.Key)
            {
                case Key.Down:
                    player.MovingPiece(tmp.XIndex - 1, tmp.YIndex, ref moveIndex);
                    break;
                case Key.Up:
                    player.MovingPiece(tmp.XIndex + 1, tmp.YIndex, ref moveIndex);
                    break;
                case Key.Left:
                    player.MovingPiece(tmp.XIndex, tmp.YIndex + 1, ref moveIndex);
                    break;
                case Key.Right:
                    player.MovingPiece(tmp.XIndex, tmp.YIndex - 1, ref moveIndex);
                    break;
            }
            if (moveIndex == -1)
                return;
            ((Piece)player.imgPieces[player.BlackBox_Indx]).SwapPieces((Piece)player.imgPieces[moveIndex]);
            player.BlackBox_Indx = moveIndex;

            if (GameManager.Instance.UpdateStatus(player.imgPieces, tmp, (Piece)player.imgPieces[moveIndex]))
                GameManager.Instance.IsWin();
            return;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            GameManager.Instance.PlayTime--;
            if (GameManager.Instance.PlayTime == 0)
                GameManager.Instance.IsLose();
        }
        //private void ZoomIn_Out()
        //{
        //    if (!isFullScr)
        //    {
        //        this.WindowState = WindowState.Maximized;
        //        isFullScr = true;
        //    }
        //    else
        //    {
        //        this.WindowState = WindowState.Normal;
        //        isFullScr = false;
        //    }

        //}

    }
}
