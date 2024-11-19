using PuzzleGame.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Interop;

namespace PuzzleGame.MVVM.ViewModels
{
    public enum CustomDialogResult
    {
        Yes,
        No,
        OK
    }
    public class CustomDialogViewModel : ObservableObject
    {
        string _message;        //Message showed on Dialog
        public string Message 
        { 
            get=>_message;
            set
            {
               _message = value;
                OnPropertyChanged();
            }
        }
        bool isYsNoIcon;         //Dialog Button Type
        public bool IsYsNoIcon
        {
            get => isYsNoIcon;
            set 
            {
                isYsNoIcon = value;
                OnPropertyChanged();
            }
        }
        public bool IsOKIcon
        {
            get => !isYsNoIcon;
            set
            {
                isYsNoIcon = !value;
                OnPropertyChanged();
            }
        }
        public CustomDialogViewModel(string msg,bool btn_type)
        {
            Message = msg;
            IsYsNoIcon = btn_type;
            IsOKIcon = !btn_type;
        }


    }
}
