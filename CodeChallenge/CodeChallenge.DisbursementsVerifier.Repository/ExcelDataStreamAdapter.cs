using System.Data;
using CodeChallenge.DisbursementsVerifier.Repository.Interfaces;
using ExcelDataReader;
using Microsoft.Extensions.Logging;

namespace CodeChallenge.DisbursementsVerifier.Repository;

public class ExcelDataStreamAdapter : IExcelDataStreamAdapter
{
    private readonly ILogger<ExcelDataStreamAdapter> _logger;

    public ExcelDataStreamAdapter(ILogger<ExcelDataStreamAdapter> logger)
    {
        _logger = logger;
    }

    public Task<DataSet> GetData(Stream stream)
    {
        _logger.LogInformation($"Retrieving data from excel stream ");
        
        try
        {
            using (var reader = ExcelReaderFactory.CreateReader(stream))
            {
                var config = new ExcelDataSetConfiguration
                {
                    ConfigureDataTable = _ => new ExcelDataTableConfiguration
                    {
                        UseHeaderRow = true 
                    }
                };
        
                var result =  reader.AsDataSet(config);
                return Task.FromResult(result);
            }
        }
        catch (Exception e)
        {
            _logger.LogError($"Error reading data from stream");
            throw;
        }
    }
}