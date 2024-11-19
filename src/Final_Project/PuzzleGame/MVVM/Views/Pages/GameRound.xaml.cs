using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using PuzzleGame.Core;
using PuzzleGame.MVVM.Models;
using PuzzleGame.MVVM.ViewModels;
using PuzzleGame.Stores;

namespace PuzzleGame.MVVM.Views.Pages
{
    /// <summary>
    /// Interaction logic for GameRound.xaml
    /// </summary>
    public partial class GameRound : Page
    {
        public GameRound()
        {
            InitializeComponent();
        }
    }
}
