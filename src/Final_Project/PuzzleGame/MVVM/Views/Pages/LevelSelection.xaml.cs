﻿using System;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PuzzleGame.MVVM.Views.Pages
{
    /// <summary>
    /// Interaction logic for LevelSelection.xaml
    /// </summary>
    public partial class LevelSelection : Page
    {
        public LevelSelection()
        {
            InitializeComponent();
        }

        private void PictureSelection_Combobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}