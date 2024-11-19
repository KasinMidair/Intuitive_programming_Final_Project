using DryIoc;
using PuzzleGame.Core;
using PuzzleGame.MVVM.ViewModels;
using PuzzleGame.MVVM.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuzzleGame.Stores
{
    public class CusDialogService : ICusDialogService
    {
        public void ShowDialog(string msg, bool btn_type=false)
        {
            var cusWnd = new CustomDialog();
            cusWnd.DataContext = new CustomDialogViewModel(msg,btn_type);
            cusWnd.ShowDialog();
        }
    }
}
