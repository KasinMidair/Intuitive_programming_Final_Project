using MaterialDesignColors.ColorManipulation;
using PuzzleGame.Class_Cus;
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
        public EventAggregator EventAggregator { get; set; }

        //Command
        public RelayCommand<FrameworkElement> SettingCommand { get; set; }
        public RelayCommand<object> GoBackCommand { get; set; }
        public RelayCommand<object> ShutdownCommand { get; set; }
        public RelayCommand<Window> CloseDialogCommand { get; set; }
        public RelayCommand<Window> MoveWndCommand { get; set; }
        public RelayCommand<string> ShowMSGBoxCommand { get; set; }
        public RelayCommand<object> ShowGalleryCommand { get; set; }
        public RelayCommand<object> DeleteImageCommand { get; set; }
      

        //EventHandler
        public event PropertyChangingEventHandler? PropertyChanging;


        DispatcherTimer _countDownClock;
        GameRound player;
        public SolidColorBrush _wndBgr { get; }


        public ObservableObject _currentPage; //  the page show in Frame in realtime

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

        public Connection _connection {  get; set; } = new Connection();

        public PlayViewModel()
        {
            EventAggregator = (EventAggregator)Application.Current.Resources["AppEventAggregator"];
            EventAggregator.GetEvent<PubSubEvent<ObservableObject>>().Subscribe((o) => OnCurrentPageChanged(o));


            _dialogService = new CusDialogService();


            toolBarColor = defaultColornum2;
            _wndBgr = defaultColornum1;
            isSettingVisible = true;


            // player = new GameRound(pnlcontainer, pnlGamePlaySpace);
            _countDownClock = new DispatcherTimer();
            _countDownClock.Interval = TimeSpan.FromSeconds(30);
            //player.StartGame();
            _countDownClock.Start();


            //init command 
            SettingCommand = new RelayCommand<FrameworkElement>((SettingMenu)=>{ SettingMenuStatus(); });
            ShutdownCommand = new RelayCommand<object>((o) => {  QuitApp(); });
            GoBackCommand = new RelayCommand<object>((o) =>{ GoBackPage(); }); 
            CloseDialogCommand = new RelayCommand<Window>((o) =>{ CloseDialog(o); });
            MoveWndCommand = new RelayCommand<Window>((o) => { MoveWnd(o); });
            ShowMSGBoxCommand = new RelayCommand<string>((o) => { ShowCustomDialog(o); });
            ShowGalleryCommand = new RelayCommand<object>((o) => { ShowGallery(); });
            DeleteImageCommand = new RelayCommand<object>((o) => { GoBackPage(); });
        }

        private void ShowGallery() => FrameNavigation(new GalleryViewModel());


        // Execute RelayCommand
        private void SettingMenuStatus() => IsSettingVisible = !IsSettingVisible;
        private void QuitApp() => Application.Current.Shutdown();
        public void GoBackPage()
        {
            _navigationService?.GoBack();
            _currentPage = (ObservableObject)_navigationService.Content;
        }
        public void CloseDialog(Window w) => w.Close();
        public void MoveWnd(Window wnd) => wnd.DragMove();
        private void ShowCustomDialog(string o) => _dialogService.ShowDialog(o);


        /// <summary>
        /// handle msg giving by Aggreator.Navigation to UserNameEnter Page 
        /// </summary>
        /// <param name="page"></param>
        private void OnCurrentPageChanged(ObservableObject page)
        {
            FrameNavigation(page);
        }

        private void ToolBarUpdateColor()=> ToolBarColor = (_wndBgr.Color.IsDarkColor()) ? defaultColornum1 : defaultColornum2;

        private void FrameNavigation(ObservableObject cur)
        {
            _currentPage = cur;
            _navigationService.Navigate(_currentPage);
            ToolBarUpdateColor();
        }

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

        List<Image> _image = new List<Image>();
        public List<Image> ImageList
        {
            get { return _image; }
            set {
                _image = value;
                OnPropertyChanged();
            }
        }
    }
}
