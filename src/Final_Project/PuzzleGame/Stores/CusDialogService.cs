using PuzzleGame.Core;
using PuzzleGame.MVVM.ViewModels;
using PuzzleGame.MVVM.Views;
using System.Windows;

namespace PuzzleGame.Stores
{
    public class CusDialogService : ICusDialogService
    {
        private static volatile CusDialogService _instance;
        public static CusDialogService Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new CusDialogService();
                }

                return _instance;
            }
        }

        Window currDialog;
        public void CloseDialog()
        {
            currDialog.Close();
        }

        public void MoveDialog() => currDialog.DragMove();

        public  async Task<CustomDialogResult> ShowDialog(string msg, bool btn_type=false)
        {
            var rlt= new TaskCompletionSource<CustomDialogResult>();
            currDialog = new CustomDialog();
            currDialog.DataContext = new CustomDialogViewModel(msg,btn_type,rlt,this);
            currDialog.ShowDialog();

            return  await rlt.Task;
        }
        public void ShowAddPicture(ObservableObject vm)
        {
            var rlt = new TaskCompletionSource<CustomDialogResult>();
            currDialog = new AddPicture();
            currDialog.DataContext = vm;
            currDialog.ShowDialog();
        }
    }
}
