using System;
using System.Collections.Generic;
using System.Text;
using DuplicateCodeSearcherLib.Models;
using DuplicateCodeSearcherLib.Searchers;
using DuplicateCodeSearcherLib.Utilities;
using Xunit;

namespace DuplicateCodeSearcherLib.Tests.Searchers.Tests
{
    public class SelfSourceScannerTest
    {
        [Fact]
        public void TestSample_01SelfScanner()
        {
            //Arrange
            string sample_01Text =
                "Row1\r\n" +
                "Row2\r\n" +
                "\tRow3\r\n" +
                "Row1\r\n" +
                "Row2\r\n" +
                "Row2.1";

            var textUtil = new TextUtility();
            var sourceToScan = textUtil.SplitTextToRows(sample_01Text);

            var duplText = new StringBuilder();
            duplText.AppendLine("Row1");
            duplText.AppendLine("Row2");

            var expectedResult = new Dictionary<string, int>()
            {
                { duplText.ToString(), 1 }
            };

            var testObj = new SelfSourceScanner();


            //Act
            var resultReal = testObj.Scan(sourceToScan);

            //Assert
            Assert.Equal(expectedResult, resultReal);
        }

        [Fact]
        public void TestSample_02SelfScanner()
        {
            //Arrange
            string sample_02Text =
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

            var textUtil = new TextUtility();
            var sourceToScan = textUtil.SplitTextToRows(sample_02Text);

            var duplText = new StringBuilder();
            duplText.AppendLine("row1.1");
            duplText.AppendLine("row1.2");
            duplText.AppendLine("row1.3");
            duplText.AppendLine("row2");
            duplText.AppendLine("row3");
            duplText.AppendLine("row4");

            var expectedResult = new Dictionary<string, int>()
            {
                { duplText.ToString(), 1 }
            };

            var testObj = new SelfSourceScanner();


            //Act
            var resultReal = testObj.Scan(sourceToScan);

            //Assert
            Assert.Equal(expectedResult, resultReal);
        }

        [Fact]
        public void TestSample_03SelfScanner()
        {
            //Arrange
            string sample_03Text =
                "Row1\r\n" +
                "Row2\r\n" +
                "Row3\r\n" +

                "Row4\r\n" +

                "Row1\r\n" +
                "Row2\r\n" +

                "Row5\r\n" +

                "Row1\r\n" +
                "Row2\r\n" +
                "Row3\r\n";

            var textUtil = new TextUtility();
            var sourceToScan = textUtil.SplitTextToRows(sample_03Text);

            var duplText = new StringBuilder();
            duplText.AppendLine("Row1");
            duplText.AppendLine("Row2");
            duplText.AppendLine("Row3");

            var expectedResult = new Dictionary<string, int>()
            {
                { duplText.ToString(), 1 }
            };

            var testObj = new SelfSourceScanner();


            //Act
            var resultReal = testObj.Scan(sourceToScan);

            //Assert
            Assert.Equal(expectedResult, resultReal);
        }
    }
}
