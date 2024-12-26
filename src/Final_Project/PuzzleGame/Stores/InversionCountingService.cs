using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuzzleGame.Stores
{
    public class InversionCountingService
    {
        public static int CountInversions(List<int> ls)
        {
            List<int> temp = new List<int>(ls.Count);
            return MergeSortAndCount(ls, 0, ls.Count - 1);
        }

        private static int MergeSortAndCount(List<int> ls, int left, int right)
        {
            int mid, inversionCount = 0;
            if (left < right)
            {
                mid = (left + right) / 2;

                inversionCount += MergeSortAndCount(ls, left, mid);
                inversionCount += MergeSortAndCount(ls, mid + 1, right);

                inversionCount += MergeAndCount(ls, left, mid, right);
            }
            return inversionCount;
        }

        private static int MergeAndCount(List<int> ls, int left, int mid, int right)
        {
            int i = left;
            int j = mid + 1;
            int k = left;
            int inversionCount = 0;      //start counting
            List<int> temp = new List<int>();

            while (i <= mid && j <= right)
            {
                if (ls[i] <= ls[j]) temp.Add(ls[i++]);
                else
                {
                    inversionCount += (mid - i + 1);        //counting nummber > ls[j]
                    temp.Add(ls[j++]);
                }
            }
            while (i <= mid) { temp.Add(ls[i++]); }

            while (j <= right) { temp.Add(ls[j++]); }

            for (i = left; i <= right; i++)
            {
                ls[i] = temp[i - left];
            }

            return inversionCount;
        }
    }
}
