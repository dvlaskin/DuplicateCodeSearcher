using System;
using System.Collections.Generic;
using DuplicateCodeSearcherLib.Models;

namespace DuplicateCodeSearcherLib.Searchers
{
    /// <summary>
    /// Logic searching Duplicate code in ScanSource
    /// </summary>
    public abstract class SearcherBase
    {
        /// <summary>
        /// Text sources for scan on duplicate code
        /// </summary>
        protected Stack<ScanSource> _textsForScan;

        /// <summary>
        /// Base logic searching Duplicate code in ScanSource
        /// </summary>
        /// <param name="textsForScan">Text sources</param>
        public SearcherBase(Stack<ScanSource> textsForScan)
        {
            _textsForScan = textsForScan;
        }

        /// <summary>
        /// Search duplicated code in text sources
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Scan text source on duplicate code in self text and other sources
        /// </summary>
        /// <param name="textSource">Source text for scan</param>
        /// <returns></returns>
        protected abstract List<ScanResult> ScanSource(ScanSource textSource);
    }
}
