using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DuplicateCodeSearcherLib.Utilities;

namespace DuplicateCodeSearcherLib.Searchers
{
    public class SelfSourceScanner : IScanSelfText
    {
        public Dictionary<string, int> Scan(List<string> rowsList)
        {
            var result = new Dictionary<string, int>();

            int indexMainRow = 0;

            while (indexMainRow < rowsList.Count)
            {
                string currRow = rowsList[indexMainRow];

                // находим начальные индексы дубл. строки
                List<List<int>> duplRowIndxs = rowsList.FindAllIndexesOf(currRow, indexMainRow + 1);

                // признак наличия дублика проверяемой строки
                bool duplRowsExists = duplRowIndxs.Count > 0;

                // если дубликатов нет, переходим к следующей строке
                if (duplRowsExists == false)
                {
                    indexMainRow++;
                    continue;
                }

                var duplCodeText = new StringBuilder();
                duplCodeText.AppendLine(currRow);

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
                        if (duplRowStartIndex + nextRowStep >= rowsList.Count)
                            break;

                        // сравниваем следущую строку оригинала
                        // со следующей строкой дубликата
                        if (rowsList[indexMainRow + nextRowStep] == rowsList[duplRowStartIndex + nextRowStep])
                        {
                            // если строки совпадают
                            // добавляем в перечень строк дубликата
                            duplRowsExists = true;
                            duplRowIndxs[i].Add(duplRowStartIndex + nextRowStep);                            
                        }
                    }

                    // если было найдено совпадение
                    // по сравнению следующих строк
                    if (duplRowsExists)
                    {
                        // удаляем ранее найденые дубликаты, которые
                        // по количеству строк меньше, чем последние найденые
                        duplRowIndxs.RemoveAll(w => w.Count() <= nextRowStep);
                        duplCodeText.AppendLine(rowsList[indexMainRow + nextRowStep]);
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
    }
}
