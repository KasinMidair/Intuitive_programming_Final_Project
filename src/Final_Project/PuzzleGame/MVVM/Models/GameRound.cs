using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace PuzzleGame.MVVM.Models
{
    public class GameRound : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        public string GameId { get; set; }
        public string PlayerName { get; set; }
        public string PlayerID { get; set; }
        public string Pieces { get; set; }
        public string Time { get; set; }
        public string Date { get; set; }
    }
}