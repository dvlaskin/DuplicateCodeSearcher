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
                "Row1\r\n" +
                "Row2\r\n" +
                "\tRow3\r\n" +
                "Row1\r\n" +
                "Row2\r\n" +
                "Row2.1";

            string text2 =
                "row1.1\r\n" +
                "row1.2\r\n" +
                "row1.3\r\n" +

                "row2\r\n" +
                "row3\r\n" +
                "row4\r\n" +

                "row4.1\r\n" +

                "row1.1\r\n" +
                "row1.2\r\n" +
                "row1.3\r\n" +

                "row2\r\n" +
                "row3\r\n" +
                "row4\r\n" +

                "row1.1\r\n" +
                "row1.2\r\n" +
                "row1.3\r\n";


            string text3 =
                "Row1.1\r\n" +
                "Row1.2\r\n" +
                "Row1.3\r\n" +

                "row2\r\n" +
                "row3\r\n" +
                "row4\r\n" +

                "Row1\r\n" +
                "Row2\r\n" +
                "Row3\r\n";

            var sourceStack = new Stack<ScanSource>();
            sourceStack.Push(new ScanSource() { Name = "Text1", Text = text1 });
            sourceStack.Push(new ScanSource() { Name = "Text2", Text = text2 });
            sourceStack.Push(new ScanSource() { Name = "Text3", Text = text3 });


            var stopWatch = System.Diagnostics.Stopwatch.StartNew();

            SearcherBase scanerObj = new RowByRowScanner(sourceStack);
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
