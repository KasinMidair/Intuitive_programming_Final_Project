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

namespace PuzzleGame
{
    /// <summary>
    /// Interaction logic for Gallery_Window.xaml
    /// </summary>
    public partial class Gallery_Window : Window
    {
        public Gallery_Window()
        {
            InitializeComponent();
        }

        class Iamge
        {
            public string path { get; set; }
            public string name { get; set; }
        
        }
    }
}
