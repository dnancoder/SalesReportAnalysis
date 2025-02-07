namespace ImplinjSalesAnalysis.Controllers
{
    using ImplinjSalesAnalysis.Model;
    using ImplinjSalesAnalysis.Processor;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using System.IO;

    [Route("api/[controller]")]
    [ApiController]
    public class SalesRecordAnalysisController : ControllerBase
    {
        private readonly ISalesRecordProcessor _salesRecordProcessor;
        private readonly ILogger<SalesRecordAnalysisController> _logger;

        public SalesRecordAnalysisController(ISalesRecordProcessor salesRecordProcessor, ILogger<SalesRecordAnalysisController> logger)
        {
            _salesRecordProcessor = salesRecordProcessor;
            _logger = logger;
        }

        [HttpPost("PostSummary")]
        public ActionResult<SalesRecordSummary> UploadFileAndGetSalesRecordSummary(IFormFile file)
        {
            try
            {
                if (file == null || file.Length == 0)
                {
                    return BadRequest("No file uploaded or file is empty.");
                }

                this._logger.LogInformation($"Getting SalesRecordSummary for attached file: {file.FileName}");
                using (var stream = file.OpenReadStream())
                {
                    var summary = _salesRecordProcessor.GetSalesRecordSummary(stream);
                    return Ok(summary);
                }
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex.Message, ex);
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("GetSummary")]
        public ActionResult<SalesRecordSummary> GetSalesRecordSummary(string fileName)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(fileName))
                {
                    return BadRequest("File name is not valid.");
                }

                this._logger.LogInformation($"Getting SalesRecordSummary for file: {fileName}");
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Data", fileName);
                var summary = _salesRecordProcessor.GetSalesRecordSummary(filePath);
                return Ok(summary);
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex.Message, ex);
                return StatusCode(500, ex.Message);
            }
        }
    }
}
