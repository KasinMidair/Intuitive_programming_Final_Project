using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuzzleGame.MVVM.Models
{
    public class Picture
    {
        public string Name { get; set; }
        public string Url { get; set; }
        public string PlayerName { get; set; }
        public bool isDefault { get; set; }
    }
}
