using System;
using System.Collections.Generic;
using System.Linq;
using DuplicateCodeSearcherLib.Models;
using DuplicateCodeSearcherLib.Searchers;
using DuplicateCodeSearcherLib.Utilities;
using Newtonsoft.Json;

namespace DuplicateCodeSearcherConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Clear();
            Console.WriteLine("======================");

            string text1 =
                "row1\r\n" +
                "row1.1\r\n" +
                "row2\r\n" +
                "row3\r\n" +
                "row4\r\n" +
                "row5\r\n" +
                "rowText1\r\n" +
                "rowText1.1\r\n" +
                "rowEmpty\r\n" +
                "rowText1\r\n" +
                "rowText1.1\r\n";

            string text2 =
                "row1.1\r\n" +
                "row1.2\r\n" +
                "row1.3\r\n" +
                "row2\r\n" +
                "row3\r\n" +
                "row4\r\n" +
                "row4.1\r\n" +
                "row2\r\n" +
                "row3\r\n" +
                "row4\r\n";


            string text3 =
                "row1.2\r\n" +
                "row2\r\n" +
                "row3\r\n" +
                "row4\r\n" +
                "row5.1\r\n" +
                "row5.2\r\n" +
                "row5.3\r\n" +
                "row2\r\n" +
                "row3\r\n" +
                "row4\r\n" +
                "row5.1\r\n" +
                "row5.2\r\n" +
                "row5.3\r\n" +
                "row5.1\r\n" +
                "row5.2\r\n" +
                "row5.1\r\n" +
                "row5.2\r\n" +
                "row5.3\r\n";
            /*
                - 2
                row2
                row3
                row4
                - 3
                row5.1
                row5.2
                row5.3
            */

            var sourceStack = new Stack<ScanSource>();
            sourceStack.Push(new ScanSource() { Name = "Text1", Text = text1 });
            sourceStack.Push(new ScanSource() { Name = "Text2", Text = text2 });
            sourceStack.Push(new ScanSource() { Name = "Text3", Text = text3 });

            var stopWatch = System.Diagnostics.Stopwatch.StartNew();

            SearcherBase scanerObj = new RowByRowScaner(sourceStack);
            List<ScanResult> res = scanerObj.SearchDuplicates();

            stopWatch.Stop();
            Console.WriteLine($"Calc time => {stopWatch.ElapsedMilliseconds} mlsec");
            //Console.WriteLine($"TotalIterationCount = {scanerObj.TotalIterationCount}");

            string resJson = JsonConvert.SerializeObject(res, Formatting.Indented);

            Console.WriteLine(resJson);

            Console.WriteLine("Done!");
            
            Console.ReadKey();
            Console.Clear();
        }

    }
}
