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
    public class RowByRowScaner : SearcherBase
    {
        /// <summary>
        /// Row-by-row search for duplicate code
        /// </summary>
        /// <param name="textsForScan"></param>
        public RowByRowScaner(Stack<ScanSource> textsForScan) : base(textsForScan)
        {
        }

        protected override List<ScanResult> ScanSource(ScanSource textSource)
        {
            var result = new List<ScanResult>();

            var textUtil = new TextUtility();
            List<string> rowsList = textUtil.SplitTextToRows(textSource.Text);

            Dictionary<string, int> selfDuplCode = SelfSourceScan(rowsList);

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

            foreach (var selfDuplItem in selfDuplCode)
            {
                if (result.Any(w => w.DuplicateText == selfDuplItem.Key))
                {
                    ScanResult existsItem = result.First(w => w.DuplicateText == selfDuplItem.Key);

                    if (existsItem.DuplicateFilesInfos.Any(w => w.Name == textSource.Name && w.Path == textSource.Path))
                    {
                        var sameFileDuplInfo = existsItem.DuplicateFilesInfos
                            .Where(w => w.Name == textSource.Name && w.Path == textSource.Path)
                            .First();

                        sameFileDuplInfo.DupliateItemCount += selfDuplItem.Value;
                    }
                    else
                    {
                        var duplSelfFileInfo = new FileWithDuplicates()
                        {
                            Name = textSource.Name,
                            Path = textSource.Path,
                            DupliateItemCount = selfDuplItem.Value
                        };

                        existsItem.DuplicateFilesInfos.Add(duplSelfFileInfo);
                    }
                }
                else
                {
                    var duplFileInfo = new FileWithDuplicates()
                    {
                        Name = textSource.Name,
                        Path = textSource.Path,
                        DupliateItemCount = selfDuplItem.Value
                    };

                    var scanResultItem = new ScanResult()
                    {
                        DuplicateText = selfDuplItem.Key,
                        DuplicateFilesInfos = new List<FileWithDuplicates>() { duplFileInfo }
                    };

                    result.Add(scanResultItem);
                }
            }


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
                            Name = textSource.Name,
                            Path = textSource.Path,
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

        /// <summary>
        /// Search duplicate code in self text
        /// </summary>
        /// <param name="rowsList">List of rows in text</param>
        /// <returns></returns>
        private Dictionary<string, int> SelfSourceScan(List<string> rowsList)
        {
            // todo: переписать поиск дубликатов кода в собственном тексте
            var result = new Dictionary<string, int>();

            int indexMainRow = 0;

            while (indexMainRow < rowsList.Count)
            {
                string currRow = rowsList[indexMainRow];                

                // находим начальные индексы дубл. строки
                List<List<int>> duplRowIndxs = FindAllIndexof(rowsList, currRow, indexMainRow + 1);

                // признак наличия дублика проверяемой строки
                bool duplRowsExists = duplRowIndxs.Count > 0;

                // если дубликатов нет, переходим к следующей строке
                if (duplRowsExists == false)
                {
                    indexMainRow++;
                    continue;
                }
                    

                var duplCodeText = new StringBuilder(currRow);

                // индекс седующей строки для сравнения на дубликат
                int nextRowStep = 1;

                // пока есть дубликат, проверяем следующую строку оригинала
                // со следующей строкой дубликата
                while (duplRowsExists)
                {
                    duplRowsExists = false;

                    // производим сравнение по всем найденым строкам
                    // начальной строки оригинала
                    for (int i = 0; i < duplRowIndxs.Count; i++)
                    {
                        // индекс начальной строки дублирующегося кода
                        int duplRowStartIndex = duplRowIndxs[i][0];

                        // проверяем что бы следующая проверяемая строка
                        // не выходила за границы массива строк
                        if (duplRowStartIndex + nextRowStep == rowsList.Count)
                            break;

                        // сравниваем следущую строку оригинала
                        // со следующей строкой дубликата
                        if (rowsList[indexMainRow + nextRowStep] == rowsList[duplRowStartIndex + nextRowStep])
                        {
                            // если строки совпадают
                            // добавляем в перечень строк дубликата
                            duplRowsExists = true;
                            duplRowIndxs[i].Add(duplRowStartIndex + nextRowStep);
                            duplCodeText.AppendLine(rowsList[indexMainRow + nextRowStep]);
                        }
                    }

                    // если было найдено совпадение
                    // по сравнению следующих строк
                    if (duplRowsExists)
                    {
                        // удаляем ранее найденые дубликаты, которые
                        // по количеству строк меньше, чем последние найденые
                        duplRowIndxs.RemoveAll(w => duplRowIndxs.Any(w => w.Count() < nextRowStep));
                        nextRowStep++;
                    }
                }

                if (duplCodeText.Length > 1)
                {
                    result.Add(duplCodeText.ToString(), duplRowIndxs.Count);
                    foreach (var duplItem in duplRowIndxs)
                    {
                        rowsList.RemoveRange(duplItem[0], duplItem.Count);                        
                    }
                }
                    

                // увеличиваем шаг посточного сравнения
                // на значение последней просмотренной строки
                indexMainRow += nextRowStep;
            }            

            //Console.WriteLine($"selfIterationCounter = {currIterationCounter}");

            return result;
        }

        private List<List<int>> FindAllIndexof<T>(IEnumerable<T> values, T val, int startIndex)
        {
            return values
                .Select((b, i) => new List<int> { object.Equals(b, val) ? i : -1 })
                .Where(i => i[0] != -1 && i[0] > startIndex)
                .ToList();
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