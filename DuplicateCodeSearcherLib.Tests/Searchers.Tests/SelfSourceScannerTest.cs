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
        private string envNewLine = Environment.NewLine;

        [Fact]
        public void TestSample_01SelfScanner()
        {
            //Arrange
            var sample_01Text = new List<string>()
            {
                "Row1",
                "Row2",
                "\tRow3",
                "Row1",
                "Row2",
                "Row2.1"
            };

            var duplText = new StringBuilder();
            duplText.AppendLine("Row1");
            duplText.Append("Row2");

            var expectedResult = new Dictionary<string, int>()
            {
                { duplText.ToString(), 1 }
            };

            var testObj = new SelfSourceScanner();


            //Act
            var resultReal = testObj.Scan(sample_01Text);

            //Assert
            Assert.Equal(expectedResult, resultReal);
        }

        [Fact]
        public void TestSample_02SelfScanner()
        {
            //Arrange
            var sample_02Text = new List<string>()
            {
                "row1.1",
                "row1.2",
                "row1.3",

                "row2",
                "row3",
                "row4",

                "row4.1",

                "row1.1",
                "row1.2",
                "row1.3",

                "row2",
                "row3",
                "row4",

                "row1.1",
                "row1.2",
                "row1.3"
            };

            var duplText = new StringBuilder();
            duplText.AppendLine("row1.1");
            duplText.AppendLine("row1.2");
            duplText.AppendLine("row1.3");
            duplText.AppendLine("row2");
            duplText.AppendLine("row3");
            duplText.Append("row4");

            var expectedResult = new Dictionary<string, int>()
            {
                { duplText.ToString(), 1 }
            };

            var testObj = new SelfSourceScanner();


            //Act
            var resultReal = testObj.Scan(sample_02Text);

            //Assert
            Assert.Equal(expectedResult, resultReal);
        }

        [Fact]
        public void TestSample_03SelfScanner()
        {
            //Arrange
            var sample_03Text = new List<string>()
            {
                "Row1",
                "Row2",
                "Row3",

                "Row4",

                "Row1",
                "Row2",

                "Row5",

                "Row1",
                "Row2",
                "Row3"
            };                

            var duplText = new StringBuilder();
            duplText.AppendLine("Row1");
            duplText.AppendLine("Row2");
            duplText.Append("Row3");

            var expectedResult = new Dictionary<string, int>()
            {
                { duplText.ToString(), 1 }
            };

            var testObj = new SelfSourceScanner();


            //Act
            var resultReal = testObj.Scan(sample_03Text);

            //Assert
            Assert.Equal(expectedResult, resultReal);
        }
    }
}
