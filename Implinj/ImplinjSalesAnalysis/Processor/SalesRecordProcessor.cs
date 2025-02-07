
namespace ImplinjSalesAnalysis.Processor
{
    using ImplinjSalesAnalysis.Model;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;

    public class SalesRecordProcessor : ISalesRecordProcessor
    {
        private readonly ILogger<SalesRecordProcessor> _logger;

        public SalesRecordProcessor(ILogger<SalesRecordProcessor> logger)
        {
            this._logger = logger;
        }

        public SalesRecordSummary GetSalesRecordSummary(string filePath)
        {
            try
            {
                using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    var salesRecordSummary = ReadSalesRecords(stream);
                    return salesRecordSummary;
                }
            }
            catch (FileNotFoundException fex)
            {
                this._logger.LogError(fex.Message, fex);
                throw;
            }
        }

        public SalesRecordSummary GetSalesRecordSummary(Stream stream)
        {
            var salesRecordSummary = ReadSalesRecords(stream);
            return salesRecordSummary;
        }

        private SalesRecordSummary ReadSalesRecords(Stream stream)
        {
            List<SalesRecord> records = new();
            SalesRecordSummary salesRecordSummary = new();
            Dictionary<string, int> regionCount = new();

            using (var reader = new StreamReader(stream))
            {
                // Skip the header line
                var columnHeader = reader.ReadLine();
                if (columnHeader.Split(',').Count() != 14)
                {
                    throw new Exception("Invlid file format!");
                }

                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line.Split(',');

                    var record = new SalesRecord
                    {
                        Region = values[0],
                        Country = values[1],
                        ItemType = values[2],
                        SalesChannel = values[3],
                        OrderPriority = values[4],
                        OrderDate = DateTime.ParseExact(values[5], "M/d/yyyy", CultureInfo.InvariantCulture),
                        OrderID = long.Parse(values[6]),
                        ShipDate = DateTime.ParseExact(values[7], "M/d/yyyy", CultureInfo.InvariantCulture),
                        UnitsSold = int.Parse(values[8]),
                        UnitPrice = decimal.Parse(values[9]),
                        UnitCost = decimal.Parse(values[10]),
                        TotalRevenue = decimal.Parse(values[11]),
                        TotalCost = decimal.Parse(values[12]),
                        TotalProfit = decimal.Parse(values[13])
                    };

                    if (salesRecordSummary.FirstOrderDate == DateTime.MinValue || record.OrderDate < salesRecordSummary.FirstOrderDate)
                    {
                        salesRecordSummary.FirstOrderDate = record.OrderDate;
                    }
                    salesRecordSummary.LastOrderDate = salesRecordSummary.LastOrderDate > record.OrderDate ? salesRecordSummary.LastOrderDate : record.OrderDate;
                    salesRecordSummary.TotalRevenue += record.TotalRevenue;
                    regionCount[record.Region] = regionCount.GetValueOrDefault(record.Region, 0) + 1;

                    records.Add(record);
                }
            }

            salesRecordSummary.MostCommonRegion = regionCount.Count == 0 ? "N/A" : regionCount.Aggregate((l, r) => l.Value > r.Value ? l : r).Key;
            salesRecordSummary.DaysBetween = (salesRecordSummary.LastOrderDate - salesRecordSummary.FirstOrderDate).Days;
            salesRecordSummary.MedianUnitCost = records.Count == 0 ? 0.0m : GetMedian(records.Select(r => r.UnitCost).OrderBy(c => c).ToList());

            this._logger.LogInformation($"Parsed reconds {records.Count} from stream");
            //return records;
            return salesRecordSummary;
        }

        private SalesRecordSummary CalculateSummary(List<SalesRecord> records)
        {
            var medianUnitCost = records.Count == 0 ? 0.0m : GetMedian(records.Select(r => r.UnitCost).OrderBy(c => c).ToList());
            var mostCommonRegion = records.Count == 0 ? "N/A" : records.GroupBy(r => r.Region).OrderByDescending(g => g.Count()).First().Key;
            var firstOrderDate = records.Count == 0 ? DateTime.MinValue : records.Min(r => r.OrderDate);
            var lastOrderDate = records.Count == 0 ? DateTime.MinValue : records.Max(r => r.OrderDate);
            var daysBetween = (lastOrderDate - firstOrderDate).Days;
            var totalRevenue = records.Count == 0 ? 0.0m : records.Sum(r => r.TotalRevenue);

            return new SalesRecordSummary
            {
                MedianUnitCost = medianUnitCost,
                MostCommonRegion = mostCommonRegion,
                FirstOrderDate = firstOrderDate,
                LastOrderDate = lastOrderDate,
                DaysBetween = daysBetween,
                TotalRevenue = totalRevenue
            };
        }

        private decimal GetMedian(List<decimal> sortedList)
        {
            int count = sortedList.Count;
            if (count % 2 == 0)
            {
                return (sortedList[count / 2 - 1] + sortedList[count / 2]) / 2;
            }
            else
            {
                return sortedList[count / 2];
            }
        }
    }

}
