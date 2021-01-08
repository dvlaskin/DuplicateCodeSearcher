using System;
using System.Collections.Generic;
using DuplicateCodeSearcherLib.Models;
using DuplicateCodeSearcherLib.Utilities;

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

            var textUtil = new TextUtility();
            List<string> rowsList = textUtil.SplitTextToRows(currText.Text);


            return result;
        }
    }
}
