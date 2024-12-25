using PuzzleGame.Core;
using PuzzleGame.Core.Helper;
using PuzzleGame.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;

namespace PuzzleGame.MVVM.ViewModels
{
    public enum CustomDialogResult
    {
        Yes,
        No,
        OK,
        None
    }
    public class CustomDialogViewModel : ObservableObject
    {
        TaskCompletionSource<CustomDialogResult> _rlt;
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
        CustomDialogResult dialogResult;           
        public CustomDialogResult DialogResult
        {
            get => dialogResult;
            set => dialogResult = value;
        }

        //Command
        public RelayCommand<string> CustomDialogResultCommand{ get; set; }
        public RelayCommand<object> MoveWndCommand { get; set; }

        public CustomDialogViewModel(string msg,bool btn_type, TaskCompletionSource<CustomDialogResult> rlt )
        {
            Message = msg;
            IsYsNoIcon = btn_type;
            IsOKIcon = !btn_type;
            this._rlt = rlt;
            CustomDialogResultCommand = new RelayCommand<string>(o => SwitchResult(o));
            MoveWndCommand = new RelayCommand<object>((o) => { CusDialogService.Instance.MoveDialog(); });
        }

        private void SwitchResult(string o)
        {
            switch (o)
            {
                case "Yes":
                    DialogResult=CustomDialogResult.Yes; break;
                case "No":
                    DialogResult=CustomDialogResult.No; break;
                case "OK":
                    DialogResult=CustomDialogResult.OK;break;
                default:
                    DialogResult = CustomDialogResult.None; break;
            }
            MusicSystemService.Instance.PlayBTN_ClickSound();

            _rlt.TrySetResult(DialogResult);
            CusDialogService.Instance.CloseDialog();
        }

        

    }
}
