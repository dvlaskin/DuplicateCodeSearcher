using System;
namespace DuplicateCodeSearcherLib.Models
{
    public abstract class SourceInfoBase
    {
        public string Name { get; set; } = "";
        public string Path { get; set; } = "";
    }
}
