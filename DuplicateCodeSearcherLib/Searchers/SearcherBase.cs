using System;
using System.Collections.Generic;
using System.Linq;
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
        protected Queue<ScanSource> _textsForScan;

        /// <summary>
        /// Base logic searching Duplicate code in ScanSource
        /// </summary>
        /// <param name="textsForScan">Text sources</param>
        public SearcherBase(Queue<ScanSource> textsForScan)
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
                ScanSource currText = _textsForScan.Dequeue();

                List<ScanResult> selfScanResult = ScanSelfText(currText);

                List<ScanResult> sourcesScanResult = ScanSources(currText);

                List<ScanResult> mergedScanResult = MergeScanResults(selfScanResult, sourcesScanResult);

                if (mergedScanResult.Count > 0)
                    result.AddRange(sourcesScanResult);
            }

            return result;
        }


        /// <summary>
        /// Scan text source on duplicate code in self text
        /// </summary>
        /// <param name="textSource">Source text for scan</param>
        /// <returns></returns>
        protected abstract List<ScanResult> ScanSelfText(ScanSource textSource);

        /// <summary>
        /// Scan text source on duplicate code in other sources
        /// </summary>
        /// <param name="textSource">Source text for scan</param>
        /// <returns></returns>
        protected abstract List<ScanResult> ScanSources(ScanSource textSource);

        /// <summary>
        /// Merge scan results
        /// </summary>
        /// <param name="selfScanResult">Left scan result</param>
        /// <param name="sourcesScanResult">Right scan result</param>
        /// <returns></returns>
        private List<ScanResult> MergeScanResults(List<ScanResult> selfScanResult, List<ScanResult> sourcesScanResult)
        {
            foreach (var sr in selfScanResult)
            {
                var sameDuplResult = sourcesScanResult.Where(w => w.DuplicateText == sr.DuplicateText).FirstOrDefault();

                if (sameDuplResult != null)
                {
                    //sameDuplResult.DuplicateFilesInfos.AddRange(sr.DuplicateFilesInfos);
                    var selfFile = sameDuplResult
                        .DuplicateFilesInfos
                        .FirstOrDefault();

                    var existingRecord = sameDuplResult
                        .DuplicateFilesInfos
                        .Where(w => w.Name == selfFile.Name && w.Path == selfFile.Path)
                        .FirstOrDefault();

                    if (existingRecord != null)
                    {
                        existingRecord.DupliateItemCount += selfFile.DupliateItemCount;
                    }
                    else
                    {
                        sameDuplResult.DuplicateFilesInfos.AddRange(sr.DuplicateFilesInfos);
                    }                        
                }
                else
                {
                    sourcesScanResult.Add(sr);
                }
            }

            return sourcesScanResult;
        }
    }
}
