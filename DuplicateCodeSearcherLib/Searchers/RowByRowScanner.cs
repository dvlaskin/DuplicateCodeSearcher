using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DuplicateCodeSearcherLib.Models;
using DuplicateCodeSearcherLib.Utilities;


namespace DuplicateCodeSearcherLib.Searchers
{
    /// <summary>
    /// Row-by-row search for duplicate code
    /// </summary>
    public class RowByRowScanner : SearcherBase
    {
        private readonly TextUtility _textUtil = new TextUtility();
        private readonly IScanSelfText _selfTextScanner = new SelfSourceScanner(); 

        /// <summary>
        /// Row-by-row search for duplicate code
        /// </summary>
        /// <param name="textsForScan"></param>
        public RowByRowScanner(Stack<ScanSource> textsForScan) : base(textsForScan)
        {
        }

        protected override List<ScanResult> ScanSelfText(ScanSource textSource)
        {
            var result = new List<ScanResult>();            

            List<string> rowsList = _textUtil.SplitTextToRows(textSource.Text);

            Dictionary<string, int> selfDuplCode = _selfTextScanner.Scan(rowsList);

            foreach (var selfDupl in selfDuplCode)
            {
                var resObj = new ScanResult()
                {
                    DuplicateText = selfDupl.Key,
                    DuplicateFilesInfos = new List<FileWithDuplicates>()
                     {
                        new FileWithDuplicates()
                        {
                            Name = textSource.Name,
                            Path = textSource.Path,
                            DupliateItemCount = selfDupl.Value
                        }
                     }
                };

                result.Add(resObj);
            }

            return result;
        }

        protected override List<ScanResult> ScanSources(ScanSource textSource)
        {
            var result = new List<ScanResult>();            
            
            List<string> rowsList = _textUtil.SplitTextToRows(textSource.Text);

            foreach (var itemScanSource in _textsForScan)
            {
                List<string> itemRowsList = _textUtil.SplitTextToRows(itemScanSource.Text);
                string processedText = "";
                Dictionary<string, int> dupliateTexts = TextRowsCompare(rowsList, itemRowsList, out processedText);
                itemScanSource.Text = processedText;

                foreach (var duplItem in dupliateTexts)
                {
                    if (result.Any(w => w.DuplicateText == duplItem.Key))
                    {
                        ScanResult scanResultItem = result.First(w => w.DuplicateText == duplItem.Key);
                        var duplFileInfo = new FileWithDuplicates()
                        {
                            Name = itemScanSource.Name,
                            Path = itemScanSource.Path,
                            DupliateItemCount = duplItem.Value
                        };

                        scanResultItem.DuplicateFilesInfos.Add(duplFileInfo);
                    }
                    else
                    {
                        var selfSourceInfo = new FileWithDuplicates()
                        {
                            Name = textSource.Name,
                            Path = textSource.Path,
                            DupliateItemCount = 1
                        };

                        var duplSourceInfo = new FileWithDuplicates()
                        {
                            Name = itemScanSource.Name,
                            Path = itemScanSource.Path,
                            DupliateItemCount = duplItem.Value
                        };

                        var scanResultItem = new ScanResult()
                        {
                            DuplicateText = duplItem.Key,
                            DuplicateFilesInfos = new List<FileWithDuplicates>() { selfSourceInfo, duplSourceInfo }
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

                for (int startMainIndex = 0; (lenChunk + startMainIndex) <= rowCount; startMainIndex++)
                {
                    string[] mainRowArr = mainRows.Skip(startMainIndex).Take(lenChunk).ToArray();
                    //Console.WriteLine("mRows: " + string.Join(", ", mainRowArr));

                    for (int startCompareIndex = 0; startCompareIndex < comparableRows.Count(); startCompareIndex++)
                    {
                        currIterationCounter++;

                        string[] compRowArr = comparableRows.Skip(startCompareIndex).Take(lenChunk).ToArray();

                        //Console.WriteLine("cRows: " + string.Join(", ", compRowArr));

                        if (isEnumerableEquals(mainRowArr, compRowArr))
                        {
                            string mainRowStr = string.Join(Environment.NewLine, mainRowArr);
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

            processedText = string.Join(Environment.NewLine, comparableRows);

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