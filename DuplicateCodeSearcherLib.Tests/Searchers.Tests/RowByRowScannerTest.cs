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
        private string envNewLine = Environment.NewLine;

        [Fact]
        public void TestSample_01_RowByRowScanner()
        {
            //Arrange
            string[] text1 =
            {
                "Row1",
                "Row2",
                "Row3",
                "Row4"
            };

            string[] text2 =
            {
                "Row1",
                "Row2",
                "Row3",
                "Row4.1"
            };


            string[] text3 =
            {
                "Row0",
                "Row1",
                "Row2",
                "Row3"
            };


            var sourceStack = new Stack<ScanSource>();
            sourceStack.Push(new ScanSource() { Name = "Text1", Text = string.Join(envNewLine, text1) });
            sourceStack.Push(new ScanSource() { Name = "Text2", Text = string.Join(envNewLine, text2) });
            sourceStack.Push(new ScanSource() { Name = "Text3", Text = string.Join(envNewLine, text3) });

            string[] duplicateText =
            {
                "Row1",
                "Row2",
                "Row3"
            };

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
                    DuplicateText = string.Join(envNewLine, duplicateText),
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

        [Fact]
        public void TestSample_02_RowByRowScanner()
        {
            //Arrange
            string[] text1 =
            {
                "Row1",
                "Row2",
                "Row3",
                "Row4",
                "Row1",
                "Row2",
                "Row3",
                "Row4.1"
            };

            string[] text2 =
            {
                "Row1",
                "Row2",
                "Row3",
                "Row4.2"
            };

            string[] text3 =
            {
                "Row0",
                "Row1",
                "Row2",
                "Row3",
                "Row4",
                "Row5",
                "Row1",
                "Row2",
                "Row3",
                "Row4.3"
            };


            var sourceStack = new Stack<ScanSource>();
            sourceStack.Push(new ScanSource() { Name = "Text1", Text = string.Join(envNewLine, text1) });
            sourceStack.Push(new ScanSource() { Name = "Text2", Text = string.Join(envNewLine, text2) });
            sourceStack.Push(new ScanSource() { Name = "Text3", Text = string.Join(envNewLine, text3) });

            string[] duplicateText_01 =
            {
                "Row1",
                "Row2",
                "Row3",
                "Row4"
            };

            var text1Info_01 = new FileWithDuplicates()
            {
                Name = "Text1",
                DupliateItemCount = 1,
                Path = ""
            };

            var text3Info_01 = new FileWithDuplicates()
            {
                Name = "Text3",
                DupliateItemCount = 1,
                Path = ""
            };

            var duplicateFilesInfos_01 = new List<FileWithDuplicates>()
            {
                text3Info_01,
                text1Info_01
            };

            string[] duplicateText_02 =
            {
                "Row1",
                "Row2",
                "Row3"
            };

            var text1Info_02 = new FileWithDuplicates()
            {
                Name = "Text1",
                DupliateItemCount = 1,
                Path = ""
            };

            var text2Info_02 = new FileWithDuplicates()
            {
                Name = "Text2",
                DupliateItemCount = 1,
                Path = ""
            };

            var text3Info_02 = new FileWithDuplicates()
            {
                Name = "Text3",
                DupliateItemCount = 2,
                Path = ""
            };

            var duplicateFilesInfos_02 = new List<FileWithDuplicates>()
            {
                text3Info_02,
                text2Info_02,
                text1Info_02
            };

            var expectedResult = new List<ScanResult>()
            {
                new ScanResult()
                {
                    DuplicateText = string.Join(envNewLine, duplicateText_02),
                    DuplicateFilesInfos = duplicateFilesInfos_02
                },
                new ScanResult()
                {
                    DuplicateText = string.Join(envNewLine, duplicateText_01),
                    DuplicateFilesInfos = duplicateFilesInfos_01
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

        [Fact]
        public void TestSample_03_RowByRowScanner()
        {
            //Arrange
            string[] text1 =
            {
                "Row1",
                "Row2",
                "Row3",
                "Row4",
                "Row1",
                "Row2",
                "Row3",
                "Row4.1",
                "Row1",
                "Row2",
                "Row3",
                "Row4.2"
            };

            string[] text2 =
            {
                "Row1",
                "Row2",
                "Row3",
                "Row4.3",
                "Row1",
                "Row2",
                "Row3"
            };

            string[] text3 =
            {
                "Row0",
                "Row1",
                "Row2",
                "Row3"
            };


            var sourceStack = new Stack<ScanSource>();
            sourceStack.Push(new ScanSource() { Name = "Text1", Text = string.Join(envNewLine, text1) });
            sourceStack.Push(new ScanSource() { Name = "Text2", Text = string.Join(envNewLine, text2) });
            sourceStack.Push(new ScanSource() { Name = "Text3", Text = string.Join(envNewLine, text3) });

            string[] duplicateText =
            {
                "Row1",
                "Row2",
                "Row3"
            };

            var text1Info = new FileWithDuplicates()
            {
                Name = "Text1",
                DupliateItemCount = 3,
                Path = ""
            };

            var text2Info = new FileWithDuplicates()
            {
                Name = "Text2",
                DupliateItemCount = 2,
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
                    DuplicateText = string.Join(envNewLine, duplicateText),
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

        [Fact]
        public void TestSample_04_RowByRowScanner()
        {
            //Arrange
            string[] text1 =
            {
                "Row1",
                "Row2",
                "Row3",
                "Row4",
                "Row5",
                "Row6",
                "Row7",

                "Row1",
                "Row2",
                "Row3",
                "Row4.1"
            };

            string[] text2 =
            {
                "Row1",
                "Row2",
                "Row3",
                "Row4.1",

                "Row1",
                "Row2",
                "Row3",
                "Row4",
                "Row5",
                "Row6",

                "Row1",
                "Row2",
                "Row3",

                "Row1",
                "Row2",
                "Row3",
                "Row4.1",
                "Row5",

                "Row1",
                "Row2",
                "Row3"
            };


            var sourceStack = new Stack<ScanSource>();
            sourceStack.Push(new ScanSource() { Name = "Text1", Text = string.Join(envNewLine, text1) });
            sourceStack.Push(new ScanSource() { Name = "Text2", Text = string.Join(envNewLine, text2) });


            string[] duplicateText_01 =
            {
                "Row1",
                "Row2",
                "Row3",
                "Row4.1"
            };

            var text1Info_01 = new FileWithDuplicates()
            {
                Name = "Text1",
                DupliateItemCount = 1,
                Path = ""
            };

            var text2Info_01 = new FileWithDuplicates()
            {
                Name = "Text2",
                DupliateItemCount = 2,
                Path = ""
            };

            var duplicateFilesInfos_01 = new List<FileWithDuplicates>()
            {
                text2Info_01,
                text1Info_01
            };

            string[] duplicateText_02 =
            {
                "Row1",
                "Row2",
                "Row3",
                "Row4",
                "Row5",
                "Row6"
            };

            var text1Info_02 = new FileWithDuplicates()
            {
                Name = "Text1",
                DupliateItemCount = 1,
                Path = ""
            };

            var text2Info_02 = new FileWithDuplicates()
            {
                Name = "Text2",
                DupliateItemCount = 1,
                Path = ""
            };

            var duplicateFilesInfos_02 = new List<FileWithDuplicates>()
            {
                text2Info_02,
                text1Info_02
            };

            string[] duplicateText_03 =
            {
                "Row1",
                "Row2",
                "Row3"
            };

            var text2Info_03 = new FileWithDuplicates()
            {
                Name = "Text2",
                DupliateItemCount = 2,
                Path = ""
            };

            var duplicateFilesInfos_03 = new List<FileWithDuplicates>()
            {
                text2Info_03
            };

            var expectedResult = new List<ScanResult>()
            {         
                new ScanResult()
                {
                    DuplicateText = string.Join(envNewLine, duplicateText_02),
                    DuplicateFilesInfos = duplicateFilesInfos_02
                },
                new ScanResult()
                {
                    DuplicateText = string.Join(envNewLine, duplicateText_01),
                    DuplicateFilesInfos = duplicateFilesInfos_01
                },
                new ScanResult()
                {
                    DuplicateText = string.Join(envNewLine, duplicateText_03),
                    DuplicateFilesInfos = duplicateFilesInfos_03
                },
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
