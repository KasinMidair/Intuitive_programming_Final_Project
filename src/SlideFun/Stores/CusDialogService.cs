﻿using SlideFun.Core;
using SlideFun.MVVM.ViewModels;
using SlideFun.MVVM.Views;
using System.Windows;

namespace SlideFun.Stores
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
        Window currAddPicDialog;
        public void CloseDialog(string addPic="")
        {
            if (addPic == "AddPic")
            {
                currAddPicDialog.Close();
            }
            else currDialog.Close();
        }   

        public void MoveDialog(bool isCusDialog=true)
        {
            if (isCusDialog)
            {
                currDialog.DragMove();
            }
            else
                currAddPicDialog.DragMove();
        }

        public  async Task<CustomDialogResult> ShowDialog(string msg, bool btn_type=false)
        {
            var rlt= new TaskCompletionSource<CustomDialogResult>();
            currDialog = new CustomDialog();
            currDialog.DataContext = new CustomDialogViewModel(msg,btn_type,rlt);
            currDialog.ShowDialog();

            return  await rlt.Task;
        }
        public void ShowAddPicture(ObservableObject vm)
        {
            var rlt = new TaskCompletionSource<CustomDialogResult>();
            currAddPicDialog = new AddPicture();
            currAddPicDialog.DataContext = vm;
            currAddPicDialog.ShowDialog();
        }
    }
}
