using System;
using System.Collections.Generic;

namespace DuplicateCodeSearcherLib.Searchers
{
    /// <summary>
    /// Scanner self source
    /// </summary>
    public interface IScanSelfText
    {
        /// <summary>
        /// Scan duplicate text in self source
        /// </summary>
        /// <param name="rowsList"></param>
        /// <returns></returns>
        Dictionary<string, int> Scan(List<string> rowsList);
    }
}
