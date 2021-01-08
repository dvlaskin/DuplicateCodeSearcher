using System;
using System.Collections.Generic;
using DuplicateCodeSearcherLib.Models;

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


         
            return result;
        }
    }
}
