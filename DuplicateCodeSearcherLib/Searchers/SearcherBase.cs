using System;
using System.Collections.Generic;
using DuplicateCodeSearcherLib.Models;

namespace DuplicateCodeSearcherLib.Searchers
{
    public abstract class SearcherBase
    {
        protected Stack<ScanSource> _textsForScan;

        public SearcherBase(Stack<ScanSource> textsForScan)
        {
            _textsForScan = textsForScan;
        }

        public List<ScanResult> SearchDuplicates()
        {
            var result = new List<ScanResult>();

            while (_textsForScan.Count > 0)
            {
                ScanSource currText = _textsForScan.Pop();

                List<ScanResult> scanResult = ScanSource(currText);

                if (scanResult.Count > 0)
                    result.AddRange(scanResult);
            }

            return result;
        }

        protected abstract List<ScanResult> ScanSource(ScanSource currText);
    }
}
