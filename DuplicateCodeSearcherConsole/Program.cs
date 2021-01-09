using System;
using System.Collections.Generic;
using System.Linq;
using DuplicateCodeSearcherLib.Models;
using DuplicateCodeSearcherLib.Searchers;
using DuplicateCodeSearcherLib.Utilities;

namespace DuplicateCodeSearcherConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            var list1 = new List<string>() { "row1", "row2", "row3", "row4", "row5", };
            var list2 = new List<string>() { "row1", "row2", "row2", "row5", "row5", };

            string text1 = "row1\r\n" +
                "row2\r\n" +
                "row3\r\n" +
                "row4\r\n" +
                "row5\r\n";

            string text2 = "row1.1\r\n" +
                "row2\r\n" +
                "row3\r\n" +
                "row4\r\n";

            var sourceStack = new Stack<ScanSource>();
            sourceStack.Push(new ScanSource() { Name = "Text1", Text = text1 });
            sourceStack.Push(new ScanSource() { Name = "Text2", Text = text2 });

            var test = new RowByRowScaner(sourceStack);
            List<ScanResult> res = test.SearchDuplicates();



            Console.WriteLine("Done!");
            
            Console.ReadKey();
            Console.Clear();
        }

    }
}
