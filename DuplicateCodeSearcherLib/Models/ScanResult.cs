using System;
using System.Collections.Generic;

namespace DuplicateCodeSearcherLib.Models
{
    public class ScanResult
    {
        public string DuplicateText { get; set; }
        public int TotalItems { get; set; }
        public List<FilesWithDuplicates> DuplicateFilesInfos { get; set; } = new List<FilesWithDuplicates>();
    }
}
