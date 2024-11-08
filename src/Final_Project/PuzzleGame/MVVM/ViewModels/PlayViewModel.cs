using PuzzleGame.Class_Cus;
using PuzzleGame.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media.Animation;
using System.Windows.Media;
using System.Windows.Threading;
using System.Windows;
using PuzzleGame.Core.Helper;
using System.Windows.Controls;
using System.ComponentModel;
using PuzzleGame.Stores;
using System.Security.RightsManagement;


namespace PuzzleGame.MVVM.ViewModels
{
    public class PlayViewModel : ObservableObject
    {
        public static int cnt = 0;
        private bool isFullScr;
        bool isLogin;
        UserEnterNameViewModel enterName = new UserEnterNameViewModel();
        private readonly Navigation _navigation;
        public RelayCommand<FrameworkElement> SettingCommand { get; set; }
        public RelayCommand<object> StartCommand { get; set; }
        bool isSettingVisible;
        DispatcherTimer _countDownClock;
        GameRound player;
        public SolidColorBrush _wndBgr { get; }

        public ObservableObject CurrentPage {
            get => _navigation.CurrentViewModel;
            set 
            {
                _navigation.CurrentViewModel = value;
                OnPropertyChanged();
            }
        }


        public bool IsSettingVisible
        {
            get => isSettingVisible;
            set
            {
                isSettingVisible = value;
                OnPropertyChanged();
            }
        }
        bool isTRanss=false;

        public event PropertyChangingEventHandler? PropertyChanging;

        public PlayViewModel()
        {
            cnt++;
            _navigation = new Navigation();
            _navigation.CurrentViewModel = new MainMenuViewModel();
            // player = new GameRound(pnlcontainer, pnlGamePlaySpace);
            _countDownClock = new DispatcherTimer();
            _countDownClock.Interval = TimeSpan.FromSeconds(30);
            _wndBgr = new SolidColorBrush(Color.FromArgb(255, 21, 22, 28));
            //player.StartGame();
            _countDownClock.Start();

            isSettingVisible = false;
            isFullScr = false;
            isLogin = false;
            SettingCommand = new RelayCommand<FrameworkElement>((SettingMenu)=>{
                SettingMenuStatus(); 
            });

            StartCommand = new RelayCommand<object>((o) =>
            {
                UserLogin();
            });



        }
        public void UserLogin()
        {
            if (!isTRanss)
            {
                isTRanss = true;
                CurrentPage = new UserEnterNameViewModel();
            }
            else
            {
                isTRanss = false;
                CurrentPage = new MainMenuViewModel();
            }
        }

        private void SettingMenuStatus()
        {
            IsSettingVisible = !IsSettingVisible;

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

    }
}
