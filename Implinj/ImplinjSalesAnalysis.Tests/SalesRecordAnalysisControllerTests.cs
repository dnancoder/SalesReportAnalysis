
namespace ImplinjSalesAnalysis.Tests
{
    using ImplinjSalesAnalysis.Controllers;
    using ImplinjSalesAnalysis.Model;
    using ImplinjSalesAnalysis.Processor;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using System.IO;

    namespace SalesRecordsAPI.Tests
    {
        [TestClass]
        public class SalesRecordsControllerTests
        {
            private Mock<ILogger<SalesRecordAnalysisController>> _mockSalesRecordAnalysisController;

            [TestInitialize]
            public void Setup()
            {
                _mockSalesRecordAnalysisController = new Mock<ILogger<SalesRecordAnalysisController>>();
            }

            [TestMethod]
            public void GetSalesRecordSummary_FileName_ReturnsCorrectSummary()
            {
                var mockService = new Mock<ISalesRecordProcessor>();
                var expectedSummary = new SalesRecordSummary
                {
                    MedianUnitCost = 117.11m,
                    MostCommonRegion = "Sub-Saharan Africa",
                    FirstOrderDate = DateTime.Parse("2010-01-01T00:00:00"),
                    LastOrderDate = DateTime.Parse("2017-07-28T00:00:00"),
                    DaysBetween = 2765,
                    TotalRevenue = 133606673066.41m
                };

                var fileName = "SalesRecords.csv";
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Data", fileName);
                mockService.Setup(s => s.GetSalesRecordSummary(filePath)).Returns(expectedSummary);

                var controller = new SalesRecordAnalysisController(mockService.Object, _mockSalesRecordAnalysisController.Object);
                var result = controller.GetSalesRecordSummary(fileName);

               // Assert
                Assert.IsInstanceOfType(result.Result, typeof(OkObjectResult));
                var actualSummary = ((OkObjectResult)result.Result).Value;
                Assert.AreEqual(expectedSummary, actualSummary);
            }

            [TestMethod]
            public void UplpadFileAndGetSalesRecordSummary_File_Upload_ReturnsCorrectSummary()
            {
                var mockService = new Mock<ISalesRecordProcessor>();
                var expectedSummary = new SalesRecordSummary
                {
                    MedianUnitCost = 97.44m,
                    MostCommonRegion = "Middle East and North Africa",
                    FirstOrderDate = DateTime.Parse("2014-10-08T00:00:00"),
                    LastOrderDate = DateTime.Parse("2015-12-09T00:00:00"),
                    DaysBetween = 427,
                    TotalRevenue = 2225376.30m
                };

                mockService.Setup(s => s.GetSalesRecordSummary(It.IsAny<Stream>())).Returns(expectedSummary);

                var controller = new SalesRecordAnalysisController(mockService.Object, _mockSalesRecordAnalysisController.Object);

               // Create a mock data
                var content = "Region,Country,Item Type,Sales Channel,Order Priority,Order Date,Order ID,Ship Date,Units Sold,Unit Price,Unit Cost,Total Revenue,Total Cost,Total Profit\n" +
                            "Europe, Monaco, Office Supplies,Online,M,1/13/2012,982711875,1/17/2012,5137,651.21,524.96,3345265.77,2696719.52,648546.25\n" +
                            "Sub - Saharan Africa,Comoros,Beverages,Offline,L,8/10/2014,580605828,8/20/2014,3022,47.45,31.79,143393.90,96069.38,47324.52\n" +
                            "Europe,Iceland,Meat,Online,L,4/22/2013,899161708,4/28/2013,786,421.89,364.69,331605.54,286646.34,44959.20\n";
                var formFile = new FormFile(new MemoryStream(System.Text.Encoding.UTF8.GetBytes(content)), 0, content.Length, "file", "SalesRecords.csv");

                var result = controller.UploadFileAndGetSalesRecordSummary(formFile);

               // Assert
                Assert.IsInstanceOfType(result.Result, typeof(OkObjectResult));
                var actualSummary = ((OkObjectResult)result.Result).Value;
                Assert.AreEqual(expectedSummary, actualSummary);
            }
        }
    }


}
