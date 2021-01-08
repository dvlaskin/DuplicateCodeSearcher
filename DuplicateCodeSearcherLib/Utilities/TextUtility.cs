using System;
using System.Collections.Generic;
using System.IO;

namespace DuplicateCodeSearcherLib.Utilities
{
    public class TextUtility
    {
        public List<string> SplitTextToRows(string text)
        {
            var result = new List<string>();

            int counter = 0;
            using (var reader = new StringReader(text))
            {
                string strRow;
                while ((strRow = reader.ReadLine()) != null )
                {
                    counter++;
                    Console.WriteLine($"Row #{counter}: {strRow}");
                    result.Add(strRow);
                }
            }

            return result;
        }
    }
}
