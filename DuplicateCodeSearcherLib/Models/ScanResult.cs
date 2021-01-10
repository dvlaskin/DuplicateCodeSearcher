using System;
using System.Collections.Generic;
using System.Linq;

namespace DuplicateCodeSearcherLib.Models
{
    public class ScanResult
    {
        public string DuplicateText { get; set; }
        public int TotalItems
        {
            get { return DuplicateFilesInfos.Sum(s => s.DupliateItemCount); }
        }
        public List<FileWithDuplicates> DuplicateFilesInfos { get; set; } = new List<FileWithDuplicates>();
    }
}
