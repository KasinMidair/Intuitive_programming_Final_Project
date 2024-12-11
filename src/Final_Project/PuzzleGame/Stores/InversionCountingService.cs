using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuzzleGame.Stores
{
    public class InversionCountingService
    {

        private static InversionCountingService? _instance;
        public static InversionCountingService Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new InversionCountingService();
                }

                return _instance;
            }
        }
        public static int CountInversions(List<int> ls)
        {
            List<int> temp = new List<int>(ls.Count);
            return MergeSortAndCount(ls, temp, 0, ls.Count - 1);
        }

        /// <summary>
        /// Using MergeSort to counting number of Inversions
        /// </summary>
        /// <param name="ls"></param>
        /// <param name="temp"></param>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        private static int MergeSortAndCount(List<int> ls, List<int> temp, int left, int right)
        {
            int mid, inversionCount = 0;
            if (left < right)
            {
                mid = (left + right) / 2;

                inversionCount += MergeSortAndCount(ls, temp, left, mid);         
                inversionCount += MergeSortAndCount(ls, temp, mid + 1, right);

                inversionCount += MergeAndCount(ls, temp, left, mid, right);
            }
            return inversionCount;
        }

        private static int MergeAndCount(List<int> ls, List<int>temp, int left, int mid, int right)
        {
            int i = left;
            int j = mid + 1;
            int k = left;
            int inversionCount = 0;      //start counting


            while (i <= mid && j <= right)
            {
                if (ls[i] <= ls[j]) 
                    temp.Add(ls[i++]);
                else 
                {
                    inversionCount += (mid - i + 1);        //counting nummber > ls[j]
                    temp.Add(ls[j++]);

                }
            }

            while (i <= mid)
            {
                temp.Add(ls[i++]);
            }

            while (j <= right)
            {
                temp.Add(ls[j++]);
            }

            for (i = left; i <= right; i++)
            {
                ls[i] = temp[i-left];
            }

            return inversionCount;
        }
    }
}
