using SlideFun.MVVM.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlideFun.Core
{
    public interface ICusDialogService
    {
        public abstract Task<CustomDialogResult> ShowDialog(string msg, bool rslt = false);
        public abstract void CloseDialog(string addPic);
    }
}
