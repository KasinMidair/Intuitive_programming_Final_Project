using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuzzleGame.MVVM.Models
{
    public class GameRound
    {
        public string GameId { get; set; }
        public string PlayerName { get; set; }
        public string PlayerID { get; set; }
        public string Pieces { get; set; }
        public string Time { get; set; }
        public string Date { get; set; }
    }
}