using System;
using System.Collections.Generic;
using System.IO;

namespace DuplicateCodeSearcherLib.Utilities
{
    public class TextUtility
    {
        /// <summary>
        /// Split text string to List of rows
        /// </summary>
        /// <param name="text">Text string</param>
        /// <returns></returns>
        public List<string> SplitTextToRows(string text)
        {
            var result = new List<string>();

            if (string.IsNullOrEmpty(text))
                return result;

            using (var reader = new StringReader(text))
            {
                string strRow;
                while ((strRow = reader.ReadLine()) != null )
                {
                    if(string.IsNullOrEmpty(strRow) == false)
                        result.Add(strRow);
                }
            }

            return result;
        }
    }
}
