
namespace ImplinjSalesAnalysis.Tests
{
    using ImplinjSalesAnalysis.Processor;
    using Microsoft.Extensions.Logging;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using System.IO;
    using System.Text;

    namespace SalesRecordsAPI.Tests
    {
        [TestClass]
        public class SalesRecordServiceTests
        {
            private ISalesRecordProcessor _salesRecordProcessor;
            private Mock<ILogger<SalesRecordProcessor>> _mokSalesRecordProcessor;

            [TestInitialize]
            public void Setup()
            {
                _mokSalesRecordProcessor = new Mock<ILogger<SalesRecordProcessor>>();
                _salesRecordProcessor = new SalesRecordProcessor(_mokSalesRecordProcessor.Object);
            }

            [TestMethod]
            [ExpectedException(typeof(FileNotFoundException))]
            public void Test_GetSalesRecordSummary_InvalidFilePath()
            {
                var testSalesRecordData = Path.Combine(Directory.GetCurrentDirectory(), "TestData", "SalesRecords_InvalidFilePath.csv");
                var salesRecordSummary = _salesRecordProcessor.GetSalesRecordSummary(testSalesRecordData);
            }

            [TestMethod]
            public void Test_GetSalesRecordSummary_FilePath_EmptyFile()
            {
                var testSalesRecordData = Path.Combine(Directory.GetCurrentDirectory(), "TestData", "SalesRecords_Empty.csv");
                var salesRecordSummary = _salesRecordProcessor.GetSalesRecordSummary(testSalesRecordData);

                Assert.AreEqual(0.0m, salesRecordSummary.MedianUnitCost);
                Assert.AreEqual("N/A", salesRecordSummary.MostCommonRegion);
                Assert.AreEqual(DateTime.MinValue, salesRecordSummary.FirstOrderDate);
                Assert.AreEqual(DateTime.MinValue, salesRecordSummary.LastOrderDate);
                Assert.AreEqual(0, salesRecordSummary.DaysBetween);
                Assert.AreEqual(0.0m, salesRecordSummary.TotalRevenue);
            }

            [TestMethod]
            public void Test_GetSalesRecordSummary_FilePath_Full_ImplinjFile()
            {
                var testSalesRecordData = Path.Combine(Directory.GetCurrentDirectory(), "TestData", "SalesRecords.csv");
                var salesRecordSummary = _salesRecordProcessor.GetSalesRecordSummary(testSalesRecordData);

                Assert.AreEqual(117.11m, salesRecordSummary.MedianUnitCost);
                Assert.AreEqual("Sub-Saharan Africa", salesRecordSummary.MostCommonRegion);
                Assert.AreEqual(DateTime.Parse("2010-01-01T00:00:00"), salesRecordSummary.FirstOrderDate);
                Assert.AreEqual(DateTime.Parse("2017-07-28T00:00:00"), salesRecordSummary.LastOrderDate);
                Assert.AreEqual(2765, salesRecordSummary.DaysBetween);
                Assert.AreEqual(133606673066.41m, salesRecordSummary.TotalRevenue);
            }

            [TestMethod]
            public void Test_GetSalesRecordSummary_FilePath_OddRecords_3Records()
            {
                var testSalesRecordData = Path.Combine(Directory.GetCurrentDirectory(), "TestData", "SalesRecords_Odd_3-Record.csv");
                var salesRecordSummary = _salesRecordProcessor.GetSalesRecordSummary(testSalesRecordData);

                Assert.AreEqual(159.42m, salesRecordSummary.MedianUnitCost);
                Assert.AreEqual("Central America and the Caribbean", salesRecordSummary.MostCommonRegion);
                Assert.AreEqual(DateTime.Parse("2012-11-30T00:00:00"), salesRecordSummary.FirstOrderDate);
                Assert.AreEqual(DateTime.Parse("2015-02-22T00:00:00"), salesRecordSummary.LastOrderDate);
                Assert.AreEqual(814, salesRecordSummary.DaysBetween);
                Assert.AreEqual(3111205.72m, salesRecordSummary.TotalRevenue);
            }

            [TestMethod]
            public void Test_GetSalesRecordSummary_FilePath_EvenRecords_4Records()
            {
                var testSalesRecordData = Path.Combine(Directory.GetCurrentDirectory(), "TestData", "SalesRecords_Even_4-Record.csv");
                var salesRecordSummary = _salesRecordProcessor.GetSalesRecordSummary(testSalesRecordData);

                Assert.AreEqual(97.44m, salesRecordSummary.MedianUnitCost);
                Assert.AreEqual("Europe", salesRecordSummary.MostCommonRegion);
                Assert.AreEqual(DateTime.Parse("2012-12-29T00:00:00"), salesRecordSummary.FirstOrderDate);
                Assert.AreEqual(DateTime.Parse("2015-02-22T00:00:00"), salesRecordSummary.LastOrderDate);
                Assert.AreEqual(785, salesRecordSummary.DaysBetween);
                Assert.AreEqual(3511519.50m, salesRecordSummary.TotalRevenue);
            }

            [TestMethod]
            public void Test_GetSalesRecordSummary_Stream_OddRecords_1Records()
            {
                var csvContent = "Region,Country,Item Type,Sales Channel,Order Priority,Order Date,Order ID,Ship Date,Units Sold,Unit Price,Unit Cost,Total Revenue,Total Cost,Total Profit\n" +
                                 "Central America and the Caribbean,Panama,Cosmetics,Offline,L,2/22/2015,874708545,2/27/2015,4551,437.20,263.33,1989697.20,1198414.83,791282.37";
                var byteArray = Encoding.ASCII.GetBytes(csvContent);
                var stream = new MemoryStream(byteArray);

                var salesRecordSummary = _salesRecordProcessor.GetSalesRecordSummary(stream);

                Assert.AreEqual(263.33m, salesRecordSummary.MedianUnitCost);
                Assert.AreEqual("Central America and the Caribbean", salesRecordSummary.MostCommonRegion);
                Assert.AreEqual(DateTime.Parse("2015-02-22T00:00:00"), salesRecordSummary.FirstOrderDate);
                Assert.AreEqual(DateTime.Parse("2015-02-22T00:00:00"), salesRecordSummary.LastOrderDate);
                Assert.AreEqual(0, salesRecordSummary.DaysBetween);
                Assert.AreEqual(1989697.20m, salesRecordSummary.TotalRevenue);
            }

            [TestMethod]
            public void Test_GetSalesRecordSummary_Stream_NoRecordsInUpload()
            {
                var csvContent = "Region,Country,Item Type,Sales Channel,Order Priority,Order Date,Order ID,Ship Date,Units Sold,Unit Price,Unit Cost,Total Revenue,Total Cost,Total Profit";
                var byteArray = Encoding.ASCII.GetBytes(csvContent);
                var stream = new MemoryStream(byteArray);

                var salesRecordSummary = _salesRecordProcessor.GetSalesRecordSummary(stream);

                Assert.AreEqual(0.0m, salesRecordSummary.MedianUnitCost);
                Assert.AreEqual("N/A", salesRecordSummary.MostCommonRegion);
                Assert.AreEqual(DateTime.MinValue, salesRecordSummary.FirstOrderDate);
                Assert.AreEqual(DateTime.MinValue, salesRecordSummary.LastOrderDate);
                Assert.AreEqual(0, salesRecordSummary.DaysBetween);
                Assert.AreEqual(0.0m, salesRecordSummary.TotalRevenue);
            }
        }
    }
}
