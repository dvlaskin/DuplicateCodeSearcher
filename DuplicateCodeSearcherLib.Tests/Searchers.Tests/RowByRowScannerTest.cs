using System;
using System.Collections.Generic;
using DuplicateCodeSearcherLib.Models;
using DuplicateCodeSearcherLib.Searchers;
using Newtonsoft.Json;
using Xunit;

namespace DuplicateCodeSearcherLib.Tests.Searchers.Tests
{
    public class RowByRowScannerTest
    {
        [Fact]
        public void TestSample_01RowByRowScanner()
        {
            //Arrange
            string text1 =
                "Row1\r\n" +
                "Row2\r\n" +
                "Row3\r\n" +
                "Row4";

            string text2 =
                "Row1\r\n" +
                "Row2\r\n" +
                "Row3\r\n" +
                "Row4.1";


            string text3 =
                "Row0\r\n" +
                "Row1\r\n" +
                "Row2\r\n" +
                "Row3";


            var sourceStack = new Stack<ScanSource>();
            sourceStack.Push(new ScanSource() { Name = "Text1", Text = text1 });
            sourceStack.Push(new ScanSource() { Name = "Text2", Text = text2 });
            sourceStack.Push(new ScanSource() { Name = "Text3", Text = text3 });

            string duplicateText =
                "Row1\r\n" +
                "Row2\r\n" +
                "Row3";

            var text1Info = new FileWithDuplicates()
            {
                Name = "Text1",
                DupliateItemCount = 1,
                Path = ""
            };

            var text2Info = new FileWithDuplicates()
            {
                Name = "Text2",
                DupliateItemCount = 1,
                Path = ""
            };

            var text3Info = new FileWithDuplicates()
            {
                Name = "Text3",
                DupliateItemCount = 1,
                Path = ""
            };

            var duplicateFilesInfos = new List<FileWithDuplicates>()
            {
                text3Info,
                text2Info,
                text1Info
            };

            var expectedResult = new List<ScanResult>()
            {
                new ScanResult()
                {
                    DuplicateText = duplicateText,
                    DuplicateFilesInfos = duplicateFilesInfos
                }
            };

            //Act
            var scanerObj = new RowByRowScanner(sourceStack);
            var resultReal = scanerObj.SearchDuplicates();

            string expJson = JsonConvert.SerializeObject(expectedResult);
            string realJson = JsonConvert.SerializeObject(resultReal);

            //Assert
            Assert.Equal(expJson, realJson);
        }
    }
}
