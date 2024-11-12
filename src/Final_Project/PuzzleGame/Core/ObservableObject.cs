using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Media;


namespace PuzzleGame.Core
{
    public class ObservableObject : INotifyPropertyChanged
    {
        protected Color defaultColornum2 = Color.FromArgb(255, 21, 22, 18);
        protected Color defaultColornum1 = Color.FromArgb(255, 245, 199, 164);
        protected bool isDarkBG;

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

    }
}