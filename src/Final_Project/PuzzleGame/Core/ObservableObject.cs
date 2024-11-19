using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Media;
using System.Windows;


namespace PuzzleGame.Core
{
    public class ObservableObject : INotifyPropertyChanged
    {
        protected SolidColorBrush defaultColornum1 = new SolidColorBrush( Color.FromArgb(255, 21, 22, 18));
        protected SolidColorBrush defaultColornum2 = new SolidColorBrush(Color.FromArgb(255, 245, 199, 164)); 
        public SolidColorBrush _wndBgr { get; init;  }

        public EventAggregator EventAggregator { get; set; } = (EventAggregator)Application.Current.Resources["AppEventAggregator"];

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

    }
}