using System;
using System.Collections.Generic;
using DuplicateCodeSearcherLib.Utilities;
using Xunit;

namespace DuplicateCodeSearcherLib.Tests.Utilites.Tests
{
    public class TextUtilityTest
    {
        [Fact]
        public void TestSplitRealTextToRows()
        {
            //Arrange
            string testStr = "Row1\r\n" +
                "Row2\r\n" +
                "\tRow3";

            var rowsList = new List<string>()
            {
                "Row1",
                "Row2",
                "\tRow3"
            };

            var textUtil = new TextUtility();

            //Act
            var resultList = textUtil.SplitTextToRows(testStr);

            //Assert
            Assert.Equal(rowsList, resultList);
        }

        [Fact]
        public void TestSplitEmptyTextToRows()
        {
            //Arrange
            string testStr = "";

            var rowsList = new List<string>();

            var textUtil = new TextUtility();

            //Act
            var resultList = textUtil.SplitTextToRows(testStr);

            //Assert
            Assert.Equal(rowsList, resultList);
        }

        [Fact]
        public void TestSplitNullTextToRows()
        {
            //Arrange
            string testStr = null;

            var rowsList = new List<string>();

            var textUtil = new TextUtility();

            //Act
            var resultList = textUtil.SplitTextToRows(testStr);

            //Assert
            Assert.Equal(rowsList, resultList);
        }
    }
}
