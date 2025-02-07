namespace ImplinjSalesAnalysis.Processor
{
    using ImplinjSalesAnalysis.Model;
    using System.IO;

    public interface ISalesRecordProcessor
    {
        SalesRecordSummary GetSalesRecordSummary(string filePath);

        SalesRecordSummary GetSalesRecordSummary(Stream stream);

    }
}
