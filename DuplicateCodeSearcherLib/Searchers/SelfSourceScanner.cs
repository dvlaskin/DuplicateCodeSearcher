using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DuplicateCodeSearcherLib.Utilities;

namespace DuplicateCodeSearcherLib.Searchers
{
    /// <summary>
    /// Sanner duplicate rows in self text
    /// </summary>
    public class SelfSourceScanner : IScanSelfText
    {
        private int _mainIndexRowPosition;
        private List<string> _rowsList;
        private StringBuilder _duplCodeText;

        /// <summary>
        /// Scan and find duplicate rows in self text
        /// </summary>
        /// <param name="rowsList">List rows to scan</param>
        /// <returns></returns>
        public Dictionary<string, int> Scan(List<string> rowsList)
        {
            var result = new Dictionary<string, int>();

            _mainIndexRowPosition = 0;
            _rowsList = rowsList;            

            while (_mainIndexRowPosition < _rowsList.Count)
            {
                string currRow = _rowsList[_mainIndexRowPosition];

                // находим начальные индексы дубл. строки
                List<List<int>> duplRowIndxs = FindStartDuplIndexs(currRow);

                // если дубликатов нет, переходим к следующей строке
                if (duplRowIndxs.Count == 0)
                {
                    _mainIndexRowPosition++;
                    continue;
                }

                _duplCodeText = new StringBuilder();
                _duplCodeText.AppendLine(currRow);          
                
                // пошаговая проверка начальных дубл. строк
                CheckDuplicateNextRows(duplRowIndxs);

                if (_duplCodeText.Length > 1)
                {
                    result.Add(_duplCodeText.ToString(), duplRowIndxs.Count);

                    RemoveDuplicateFromRowsList(duplRowIndxs);
                }
            }

            //Console.WriteLine($"selfIterationCounter = {currIterationCounter}");

            return result;
        }


        /// <summary>
        /// Checking duplicate string sequences to match the original string sequence
        /// </summary>
        /// <param name="duplRowIndxs">A start indexes duplicate sequence</param>
        private void CheckDuplicateNextRows(List<List<int>> duplRowIndxs)
        {
            bool duplRowsExists = true;
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

                    // проверяем чтобы следующая проверяемая строка
                    // не выходила за границы массива строк
                    if (duplRowStartIndex + nextRowStep >= _rowsList.Count)
                        break;

                    // сравниваем следущую строку оригинала
                    // со следующей строкой дубликата
                    if (_rowsList[_mainIndexRowPosition + nextRowStep] == _rowsList[duplRowStartIndex + nextRowStep])
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
                    _duplCodeText.AppendLine(_rowsList[_mainIndexRowPosition + nextRowStep]);
                    nextRowStep++;
                }
            }


            // увеличиваем шаг посточного сравнения
            // на значение последней просмотренной строки
            _mainIndexRowPosition += nextRowStep;
        }

        /// <summary>
        /// Remove founded duplicate lines from the full list of lines
        /// </summary>
        /// <param name="duplRowIndxs">Duplicate rows indexes</param>
        private void RemoveDuplicateFromRowsList(List<List<int>> duplRowIndxs)
        {
            for (int i = duplRowIndxs.Count - 1; i >= 0; i--)
            {
                _rowsList.RemoveRange(duplRowIndxs[i][0], duplRowIndxs[i].Count);
            }
        }

        /// <summary>
        /// Search duplicate of current row in self text
        /// </summary>
        /// <param name="currRow"></param>
        /// <returns></returns>
        private List<List<int>> FindStartDuplIndexs(string currRow)
        {
            return _rowsList
                .FindAllIndexesOf(currRow, _mainIndexRowPosition + 1);
        }
    }
}
