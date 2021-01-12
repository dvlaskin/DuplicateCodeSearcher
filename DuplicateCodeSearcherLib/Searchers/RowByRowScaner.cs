using System;
using System.Collections.Generic;
using System.Linq;
using DuplicateCodeSearcherLib.Models;
using DuplicateCodeSearcherLib.Utilities;

namespace DuplicateCodeSearcherLib.Searchers
{
    public class RowByRowScaner : SearcherBase
    {
        public int TotalIterationCount { get; private set; } = 0;

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
                List<string> itemList = textUtil.SplitTextToRows(item.Text);
                string processedText = "";
                Dictionary<string, int> dupliateTexts = TextRowsCompare(rowsList, itemList, out processedText);
                item.Text = processedText;

                foreach (var duplItem in dupliateTexts)
                {
                    if (result.Any(w => w.DuplicateText == duplItem.Key))
                    {
                        ScanResult scanResultItem = result.First(w => w.DuplicateText == duplItem.Key);
                        var duplFileInfo = new FileWithDuplicates()
                        {
                            Name = item.Name,
                            Path = item.Path,
                            DupliateItemCount = duplItem.Value
                        };

                        scanResultItem.DuplicateFilesInfos.Add(duplFileInfo);
                    }
                    else
                    {
                        var duplSelfFileInfo = new FileWithDuplicates()
                        {
                            Name = currText.Name,
                            Path = currText.Path,
                            DupliateItemCount = 1
                        };

                        var duplFileInfo = new FileWithDuplicates()
                        {
                            Name = item.Name,
                            Path = item.Path,
                            DupliateItemCount = duplItem.Value
                        };

                        var scanResultItem = new ScanResult()
                        {
                            DuplicateText = duplItem.Key,
                            DuplicateFilesInfos = new List<FileWithDuplicates>() { duplSelfFileInfo, duplFileInfo }
                        };

                        result.Add(scanResultItem);
                    }
                }
            }            

            return result;
        }


        private Dictionary<string, int> TextRowsCompare(IEnumerable<string> mainRows, IEnumerable<string> comparableRows, out string processedText)
        {
            var result = new Dictionary<string, int>();

            int currIterationCounter = 0;

            int rowCount = mainRows.Count() < comparableRows.Count() ? mainRows.Count() : comparableRows.Count();

            for (int lenChunk = rowCount; lenChunk > 1; lenChunk--)
            {
                //for (int startMainIndex = 0; startMainIndex < rowCount; startMainIndex++)
                //{
                //    if ((lenChunk + startMainIndex) > rowCount)
                //        break;

                for (int startMainIndex = 0; (lenChunk + startMainIndex) <= rowCount; startMainIndex++)
                {
                    string[] mainRowArr = mainRows.Skip(startMainIndex).Take(lenChunk).ToArray();
                    //Console.WriteLine("mRows: " + string.Join(", ", mainRowArr));

                    for (int startCompareIndex = 0; startCompareIndex < comparableRows.Count(); startCompareIndex++)
                    {
                        TotalIterationCount++;
                        currIterationCounter++;

                        string[] compRowArr = comparableRows.Skip(startCompareIndex).Take(lenChunk).ToArray();

                        //Console.WriteLine("cRows: " + string.Join(", ", compRowArr));

                        if (isEnumerableEquals(mainRowArr, compRowArr))
                        {
                            string mainRowStr = string.Join("\r\n", mainRowArr);
                            if (result.ContainsKey(mainRowStr))
                            {
                                result[mainRowStr]++;
                            }
                            else
                            {
                                result.Add(mainRowStr, 1);
                            }

                            for (int r = startCompareIndex; r < startCompareIndex + lenChunk; r++)
                            {
                                comparableRows = comparableRows.Select((x, i) => r == i ? "" : x).ToArray();
                            }
                        }
                    }
                }
            }

            processedText = string.Join("\r\n", comparableRows);

            Console.WriteLine($"currIterationCounter = {currIterationCounter}");

            return result;
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
