using PuzzleGame.Class_Cus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace PuzzleGame
{
    public partial class Play_Window : Window
    {
        DispatcherTimer _countDownClock;
        GameRound player;
        public Play_Window()
        {
            InitializeComponent();
            player = new GameRound(pnlcontainer, pnlGamePlaySpace);
            _countDownClock= new DispatcherTimer();
            _countDownClock.Interval = TimeSpan.FromSeconds(30);
            player.StartGame();
            _countDownClock.Start();

        }



        /// <summary>
        /// using key press event to mowing blackPiece around
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="keyData"></param>
        /// <returns></returns>
        private void Window_KeyDown(object sender, KeyEventArgs e)
        { 
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
                return ;
            ((Piece)player.imgPieces[player.BlackBox_Indx]).SwapPieces((Piece)player.imgPieces[moveIndex]);
            player.BlackBox_Indx = moveIndex;

            if (GameManager.Instance.UpdateStatus(player.imgPieces, tmp, (Piece)player.imgPieces[moveIndex]))
                GameManager.Instance.IsWin();
            return ;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            GameManager.Instance.PlayTime--;
            if (GameManager.Instance.PlayTime == 0)
                GameManager.Instance.IsLose();
        }
    }
}
