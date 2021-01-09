using System;
using System.Collections.Generic;
using System.Linq;
using DuplicateCodeSearcherLib.Models;
using DuplicateCodeSearcherLib.Utilities;

namespace DuplicateCodeSearcherLib.Searchers
{
    public class RowByRowScaner : SearcherBase
    {

        public RowByRowScaner(Stack<ScanSource> textsForScan) : base(textsForScan)
        {
        }

        protected override List<ScanResult> ScanSource(ScanSource currText)
        {
            var result = new List<ScanResult>();

            var textUtil = new TextUtility();
            List<string> rowsList = textUtil.SplitTextToRows(currText.Text);


            foreach (var item in _textsForScan)
            {
                var itemList = textUtil.SplitTextToRows(item.Text);
                TextRowsCompare(rowsList, itemList);
            }


            return result;
        }


        private void TextRowsCompare(IEnumerable<string> mainRows, IEnumerable<string> comparableRows)
        {
            int rowCount = mainRows.Count() < comparableRows.Count() ? mainRows.Count() : comparableRows.Count();

            for (int i = rowCount; i > 1; i--)
            {
                Console.WriteLine($"{i}==========================================");

                for (int l = 0; l < rowCount; l++)
                {
                    if ((i + l) > rowCount)
                        break;

                    Console.WriteLine("mRows: " + string.Join(", ", mainRows.Skip(l).Take(i)));
                    Console.WriteLine("cRows: " + string.Join(", ", comparableRows.Skip(l).Take(i)));
                }
            }
        }

        /// <summary>
        /// Comparing two Enumerable for equality
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source1"></param>
        /// <param name="source2"></param>
        /// <returns></returns>
        private bool isEnumerableEquals<T>(IEnumerable<T> source1, IEnumerable<T> source2)
        {
            var cnt = new Dictionary<T, int>();

            foreach (T s in source1)
            {
                if (cnt.ContainsKey(s))
                {
                    cnt[s]++;
                }
                else
                {
                    cnt.Add(s, 1);
                }
            }

            foreach (T s in source2)
            {
                if (cnt.ContainsKey(s))
                {
                    cnt[s]--;
                }
                else
                {
                    return false;
                }
            }

            return cnt.Values.All(c => c == 0);
        }
    }
}
