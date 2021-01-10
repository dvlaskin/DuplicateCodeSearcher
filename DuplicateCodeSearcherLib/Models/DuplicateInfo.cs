using System;
using System.Collections;
using System.Collections.Generic;

namespace DuplicateCodeSearcherLib.Models
{
    public class FileWithDuplicates : SourceInfoBase
    {
        public int DupliateItemCount { get; set; }
    }
}
