using System;
using DuplicateCodeSearcherLib.Utilities;

namespace DuplicateCodeSearcherConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            string testStr = "Row1\r\n" +
                "Row2\r\n" +
                "\tRow3";

            var textUtil = new TextUtility();
            var rowList = textUtil.SplitTextToRows(testStr);

            Console.WriteLine(string.Join("\r\n", rowList));

            Console.WriteLine("Done!");
            Console.ReadKey();
        }
    }
}
