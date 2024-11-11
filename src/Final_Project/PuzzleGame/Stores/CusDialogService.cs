using DryIoc;
using PuzzleGame.Core;
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
        public void ShowDialog(string msg)
        {
            var cusWnd = new CustomDialog(msg);
            cusWnd.ShowDialog();
        }
    }
}
